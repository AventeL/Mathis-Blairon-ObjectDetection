using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Mathis.Blairon.ObjectDetection;

namespace Mathis.Blairon.ObjectDetection.Console;

internal class Program
{
    private static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            System.Console.WriteLine("Please provide the path to the scenes directory.");
            return;
        }

        var scenesDirectory = args[0];
        if (!Directory.Exists(scenesDirectory))
        {
            System.Console.WriteLine("The provided directory does not exist.");
            return;
        }

        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(scenesDirectory))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var objectDetection = new ObjectDetection();
        var detectObjectInScenesResults = await objectDetection.DetectObjectInScenesAsyncMocked(imageScenesData);

        foreach (var objectDetectionResult in detectObjectInScenesResults)
            System.Console.WriteLine($"Box: {JsonSerializer.Serialize(objectDetectionResult.Box)}");
    }
}