using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Mathis.Blairon.ObjectDetection.Tests;

public class ObjectDetectionUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath,
                     "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var detectObjectInScenesResults = await new
            ObjectDetection().DetectObjectInScenesAsyncMocked(imageScenesData);

        Assert.Equal(
                "[{\"Dimensions\":{\"X\":0,\"Y\":0,\"Height\":2,\"Width\":2},\"Label\":\"Car\",\"Confidence\":0.5}]",
                JsonSerializer.Serialize(detectObjectInScenesResults[0].Box))
            ;

        Assert.Equal(
                "[{\"Dimensions\":{\"X\":1,\"Y\":1,\"Height\":1,\"Width\":1},\"Label\":\"Flower\",\"Confidence\":0.9}]",
                JsonSerializer.Serialize(detectObjectInScenesResults[1].Box))
            ;
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}