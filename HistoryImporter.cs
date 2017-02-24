using System.IO;
using Newtonsoft.Json;

public class Importer
{
    private string _filePath;

    public Importer(string filePath)
    {
        this._filePath = filePath;
    }

    public History GetCollectionOfDomains()
    {
        var p = Directory.GetCurrentDirectory();
        using (StreamReader file = File.OpenText(_filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            return (History)serializer.Deserialize(file, typeof(History));
        }
    }
}