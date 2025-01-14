namespace Mathis.Blairon.ObjectDetection;

public class ObjectDetection
{
    public async Task<IList<ObjectDetectionResult>>
        DetectObjectInScenesAsync(IList<byte[]> imagesSceneData)
    {
        await Task.Delay(1000);
// TODO implement your code here
        throw new NotImplementedException();
    }

    public async Task<IList<ObjectDetectionResult>> DetectObjectInScenesAsyncMocked(IList<byte[]>
        imagesSceneData)
    {
        IList<ObjectDetectionResult> results = new List<ObjectDetectionResult>();
        results.Add(new ObjectDetectionResult
        {
            ImageData = [0],
            Box = new List<BoundingBox>()
            {
                new()
                {
                    Confidence = 0.5f, Label = "Car", Dimensions = new
                        BoundingBoxDimensions() { Height = 2, Width = 2, Y = 0, X = 0 }
                }
            }
        });
        results.Add(new ObjectDetectionResult
        {
            ImageData = [0],
            Box = new List<BoundingBox>()
            {
                new()
                {
                    Confidence = 0.9f, Label = "Flower", Dimensions = new
                        BoundingBoxDimensions() { Height = 1, Width = 1, Y = 1, X = 1 }
                }
            }
        });
        await Task.Delay(1000);
        return results;
    }


    public static void Main(string[] args)
    {
    }
}