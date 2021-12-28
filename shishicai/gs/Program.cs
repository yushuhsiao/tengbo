using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace shishicai
{
    // {"c":0,"p":[0,0,1]};
    // {"c":"0","p":["","","{\"i\":\"20130227-077\",\"b\":\"33576\"}"]}.
    // {"c":0,"p":[1,0,1]}
    // {"c":"0","p":["[{\"n\":\"20130227-079\",\"t\":\"2013-02-27 19:08:30\"},{\"n\":\"20130227-080\",\"t\":\"2013-02-27 19:18:30\"}]","[{\"n\":\"20130227-078\",\"t\":\"2013-02-27 19:00:00\"},{\"n\":\"20130227-079\",\"t\":\"2013-02-27 19:10:00\"}]","{\"i\":\"20130227-077\",\"b\":\"33576\"}"]}.{"c":"0","p":["","","{\"i\":\"20130227-078\",\"b\":\"90595\"}"]}.

    class Program
    {
        static byte[] tmp = new byte[1024];
        static Socket socket;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //corp_result r = JsonConvert.DeserializeObject<corp_result>("{\"c\":\"0\",\"p\":[\"\",\"\",\"{\\\"i\\\":\\\"20130227-077\\\",\\\"b\\\":\\\"33576\\\"}\"]}");
            string str = JsonConvert.SerializeObject(new corp_result_query());
            //Debugger.Break();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("60.28.11.19", 28004);
            Console.WriteLine("Connected.");
            recv(null);
            for (; ; )
            {
                string s = Console.ReadLine();
                if (s == "1")
                    s = str; // @"{""c"":0,""p"":[0,0,1]};";
                else if (s == "exit")
                    break;
                socket.Send(Encoding.ASCII.GetBytes(str));
            }
        }

        static int line = 16;

        static void dump(int cnt)
        {
            for (int n1 = 0; n1 < cnt; n1 += line)
            {
                int n2 = n1 + line;
                for (int n3 = n1; n3 < n2; n3++)
                {
                    Console.Write("{0:X2} ", tmp[n3]);
                }
                Console.Write("\t");
                for (int n3 = n1; n3 < n2; n3++)
                {
                    char c = (char)tmp[n3];
                    if ((c < 32) || (c > 127))
                        c = '.';
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        static void recv(IAsyncResult ar)
        {
            if (ar != null)
            {
                dump(socket.EndReceive(ar));
            }
            socket.BeginReceive(tmp, 0, tmp.Length, SocketFlags.None, recv, null);
        }
    }
}

[JsonObject]
//var k = new $S.JsonCommand();
//k.Command = TK.Command.Enum.Video;
//k.ListParameter = [i.bet ? 1 : 0, i.current ? 1 : 0, i.last ? 1 : 0];
class corp_result_query
{
    [JsonProperty("c")]
    public int Command = 0;

    [JsonProperty("p")]
    public int[] Parameter = new int[] { 0, 0, 1 };
}
[JsonObject]
class corp_result
{
    [JsonProperty("c")]
    public string Command;

    [JsonProperty("p")]
    public string[] Parameter;
}