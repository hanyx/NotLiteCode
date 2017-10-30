using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace NotLiteCode___Server
{
  internal class MonitorService : WebSocketBehavior
  {
    protected override void OnMessage(MessageEventArgs e)
    {
      if (e.IsText)
      {
        var msg = e.Data.Deserialize<NLCCarrier>();
        Send((new NLCCarrier() { Identifier = "Response", Content = msg.Content + " | OwO" }).Serialize());
      }
      else
      {
        Send((new NLCCarrier() { Identifier = "Error", Content = "Request not text" }).Serialize());
      }
    }

    protected override void OnOpen()
    {
      base.OnOpen();
    }
  }

  public static partial class Helpers
  {
    public static string Serialize(this object source)
    {
      var js = new DataContractJsonSerializer(source.GetType());

      string ret;

      using (var ms = new MemoryStream())
      using (var sr = new StreamReader(ms))
      {
        js.WriteObject(ms, source);
        ms.Position = 0;
        ret = sr.ReadToEnd();
      }
      return ret;
    }

    public static T Deserialize<T>(this string source)
    {
      T ret;
      using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(source)))
      {
        var js = new DataContractJsonSerializer(typeof(T));
        ret = (T)js.ReadObject(ms);
      }
      return ret;
    }
  }

  [DataContract]
  internal class NLCCarrier
  {
    [DataMember]
    public string Identifier { get; set; }

    [DataMember]
    public string Content { get; set; }
  }
}