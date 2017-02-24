public class HistoryElement
{
    public string url { get; set; }
}

public class History
{
    public HistoryElement[] BrowserHistory { get; set; }
}