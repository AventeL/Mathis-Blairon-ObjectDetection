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
            ObjectDetection().DetectObjectInScenesAsync(imageScenesData);

        Assert.Equal(
                "[{\"Dimensions\":{\"X\":188.55171,\"Y\":35.398605,\"Height\":295.1788,\"Width\":233.91995},\"Label\":\"chair\",\"Confidence\":0.9593163},{\"Dimensions\":{\"X\":95.14056,\"Y\":31.15651,\"Height\":292.3445,\"Width\":155.24562},\"Label\":\"chair\",\"Confidence\":0.4776929},{\"Dimensions\":{\"X\":1.7406502,\"Y\":43.580902,\"Height\":265.32745,\"Width\":38.775265},\"Label\":\"person\",\"Confidence\":0.45632786},{\"Dimensions\":{\"X\":251.54828,\"Y\":126.99736,\"Height\":278.1666,\"Width\":163.73639},\"Label\":\"chair\",\"Confidence\":0.41722956}]",
                JsonSerializer.Serialize(detectObjectInScenesResults[0].Box))
            ;

        Assert.Equal(
                "[{\"Dimensions\":{\"X\":282.01044,\"Y\":135.18512,\"Height\":207.9703,\"Width\":118.70157},\"Label\":\"chair\",\"Confidence\":0.7953415},{\"Dimensions\":{\"X\":182.22658,\"Y\":47.17695,\"Height\":58.015762,\"Width\":61.69449},\"Label\":\"tvmonitor\",\"Confidence\":0.7125465},{\"Dimensions\":{\"X\":324.02673,\"Y\":93.46053,\"Height\":233.03401,\"Width\":92.72813},\"Label\":\"chair\",\"Confidence\":0.5710958},{\"Dimensions\":{\"X\":12.099663,\"Y\":109.16438,\"Height\":205.19614,\"Width\":136.84425},\"Label\":\"chair\",\"Confidence\":0.49903274},{\"Dimensions\":{\"X\":339.16867,\"Y\":8.67012,\"Height\":93.52301,\"Width\":64.10001},\"Label\":\"person\",\"Confidence\":0.33659077}]",
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