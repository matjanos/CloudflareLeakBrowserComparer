using System.Runtime.Serialization;
using Newtonsoft.Json;

public class HistoryElement
{
    public string url { get; set; }
}

public class History
{
    [JsonProperty("Browser History")]
    public HistoryElement[] BrowserHistory { get; set; }
}