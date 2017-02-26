# CloudflareLeakBrowserComparer
Do you want to see which websites from the ones that you visit using Google Chrome browser, were affected by huge Cloudflare data leak? Check it!

## Compilation
You need to get dotnet SDK ([from here](https://www.microsoft.com/net/download/core)) to compile the project using:

```dotnet restore```

```dotnet build .```

## Using
  First you need download your browser history from Google - [HERE](https://takeout.google.com/settings/takeout)
  It's enough to select only Chrome->BrowserHistory.
  
  Extract the zip file, find BrowserHistory.json.
  
  To run the comparrer:
```
  dotnet run <BrowserHistory.json> sorted_unique_cf.txt
```

In result you will see `result.txt` file, which contains all websites that you have ever visited and which are CloudFlare clients.
