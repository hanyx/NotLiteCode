using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using static NotLiteCode___Server.Helpers;

namespace NotLiteCode___Server
{
  class WebServer
  {
    public WebServer()
    {
      var httpsv = new HttpServer(80);

      httpsv.RootPath = Environment.CurrentDirectory + "\\Public\\";

      httpsv.OnGet += (sender, e) => {
        var req = e.Request;
        var res = e.Response;

        var path = req.RawUrl;
        if (path == "/")
          path += "index.html";

        byte[] contents;
        if (!tryReadFile(path, out contents))
        {
          res.StatusCode = (int)HttpStatusCode.NotFound;
          return;
        }

        if (path.EndsWith(".html"))
        {
          res.ContentType = "text/html";
          res.ContentEncoding = Encoding.UTF8;
        }
        else if (path.EndsWith(".js"))
        {
          res.ContentType = "application/javascript";
          res.ContentEncoding = Encoding.UTF8;
        }

        res.WriteContent(contents);
      };

      httpsv.AddWebSocketService<MonitorService>("/Monitor");
      httpsv.Start();
    }
  }
  public static partial class Helpers
  {
    public static bool tryReadFile(string path, out byte[] contents)
    {
      contents = null;

      if (!File.Exists(path))
        return false;

      try
      {
        contents = File.ReadAllBytes(path);
      }
      catch
      {
        return false;
      }

      return true;
    }
  }
}
