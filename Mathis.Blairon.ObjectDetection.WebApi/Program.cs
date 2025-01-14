// Mathis.Blairon.ObjectDetection.WebApi/Program.cs

using System.Drawing;
using System.Drawing.Imaging;
using Mathis.Blairon.ObjectDetection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
    sceneSourceStream.CopyTo(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();

    var objectDetection = new ObjectDetection();
    var detectObjectInScenesResults =
        await objectDetection.DetectObjectInScenesAsync(new List<byte[]> { imageSceneData });

    using var image = Image.FromStream(new MemoryStream(imageSceneData));
    using var graphics = Graphics.FromImage(image);
    var pen = new Pen(Color.Red, 2);
    var font = new Font("Arial", 12);
    var brush = new SolidBrush(Color.Red);


    foreach (var box in detectObjectInScenesResults[0].Box)
    {
        var ratioH = image.Height / 416f;
        var ratioW = image.Width / 416f;

        var rect = new Rectangle((int)(box.Dimensions.X * ratioW), (int)(box.Dimensions.Y * ratioH),
            (int)(box.Dimensions.Width * ratioW),
            (int)(box.Dimensions.Height * ratioH));

        graphics.DrawRectangle(pen, rect);
        graphics.DrawString($"{box.Label} {box.Confidence * 100}%", font, brush, rect.Location);
    }

    using var outputMemoryStream = new MemoryStream();
    image.Save(outputMemoryStream, ImageFormat.Jpeg);
    var outputImageData = outputMemoryStream.ToArray();

    return Results.File(outputImageData, "image/jpg");
}).DisableAntiforgery();

app.Run();