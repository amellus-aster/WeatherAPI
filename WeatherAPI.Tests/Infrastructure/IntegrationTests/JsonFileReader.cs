public static class JsonFileReader
{
    public static string Read(string fileName)
    {
        return File.ReadAllText(Path.Combine("SampleData", fileName));
    }
}