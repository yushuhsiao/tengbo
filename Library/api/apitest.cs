using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using Tools;

namespace api
{
    public class apitest
    {
        [STAThread]
        static void Main(string[] args)
        {
            string s; byte[] b;
            Guid g = new Guid(b = Convert.FromBase64String(s = "sEX+78TMZkGT9uLZISQusA=="));
            Debugger.Break();

            //for (decimal d = 0; d < 1; d += .001m)
            //{
                
            //    Console.WriteLine("{0}\t{1}", d, Math.Round(d, 2, MidpointRounding.AwayFromZero));
            //    Console.ReadKey();
            //}
            //return;

            //web.game.GetProc(BU.UserType.Agent, BU.GameID.BBIN);
            //string[] url_list = new string[]
            //{
            //    "1",
            //    "2",
            //    "3",
            //    "4",
            //};

            Tick.OnTick += Tick_OnTick;


            //            string a = "123\r\n456";
            //            string b = @"123
            //456";

            //            byte[] aa = Encoding.ASCII.GetBytes(a);
            //            byte[] bb = Encoding.ASCII.GetBytes(b);
            //            Console.WriteLine(a);
            //            Console.WriteLine(b);
            //            Console.WriteLine(Convert.ToInt32(b));
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Connect("2014lucky.com", 18443);
            //Guid g = new Guid("9c244412-e227-4e6a-975b-05f2d034bc8b");
            //string s = Convert.ToBase64String(g.ToByteArray());
            //Debugger.Break();
            //extAPI.ag.AG.GetInstance(2).test();
            //extAPI.ag.AGIN.GetInstance(2).test();
            //extAPI.ag.DSP.GetInstance(2).test();
        }

        static Queue<string> url_list = new Queue<string>();
        static int i = 0;
        static bool Tick_OnTick()
        {
            string url;
            lock (url_list)
            {
                if (url_list.Count == 0) return true;
                url = url_list.Dequeue();
            }
            Console.Write("{0}\r\n", Interlocked.Increment(ref i));
            return false;
        }
    }
}