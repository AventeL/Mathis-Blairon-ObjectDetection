using System.Drawing;
using System.Drawing.Imaging;
using Mathis.Blairon.ObjectDetection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/ObjectDetection", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count < 1)
        return Results.BadRequest();

    using var sceneSourceStream = files[0].OpenReadStream();
    using var sceneMemoryStream = new MemoryStream();
    await sceneSourceStream.CopyToAsync(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();

    var objectDetection = new ObjectDetection();
    var detectObjectInScenesResults =
        await objectDetection.DetectObjectInScenesAsyncMocked(new List<byte[]> { imageSceneData });

    if (detectObjectInScenesResults.Count == 0)
        return Results.BadRequest("No objects detected.");

    using var image = Image.FromStream(new MemoryStream(imageSceneData));
    using var graphics = Graphics.FromImage(image);

    foreach (var result in detectObjectInScenesResults)
    foreach (var box in result.Box)
    {
        var rect = new Rectangle((int)box.Dimensions.X, (int)box.Dimensions.Y, (int)box.Dimensions.Width,
            (int)box.Dimensions.Height);
        graphics.DrawRectangle(Pens.Red, rect);
    }

    using var outputMemoryStream = new MemoryStream();
    image.Save(outputMemoryStream, ImageFormat.Jpeg);
    var outputImageData = outputMemoryStream.ToArray();

    return Results.File(outputImageData, "image/jpg");
}).DisableAntiforgery();


app.Run();