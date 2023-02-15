using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;
using JumpAndRun;

namespace Shard
{
    class MyVector
    {
        public double x, y;
    }


    public sealed class Client
    {
        private Client() { }

        private static Client _instance;
        private WebSocket ws;
        private Mate per;
        private bool isSet = false;
        public double id;

        public static Client GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Client();
            }
            return _instance;
        }

        public void Start()
        {
            // Create a scoped instance of a WS client that will be properly disposed
            //using (WebSocket ws = new WebSocket("ws://simple-websocket-server-echo.glitch.me/"))
            ws = new WebSocket("ws://secret-island-78427.herokuapp.com");
            Random rd = new Random();

            id =  rd.Next(1000, 9999);

            ws.OnMessage += Ws_OnMessage;

            ws.OnError += Ws_OnError;

            ws.Connect();

            //ws.Send($"{234},{234},{23432}");
            ws.Send($"Hello!");
            Debug.Log("Connected");
            Console.WriteLine("Connected 2");
            //Console.ReadKey();

        }

        public void Send(String message)
        {
            ws.Send(message);
        }

        public void setGameObject(Object p)
        {
            per = (Mate)p;
            isSet = true;
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log("Received from the server: " + e.Data);

            string subString = e.Data[..4];
            string[] strlist = e.Data.Split(";");
            //Debug.Log(strlist[0]);
            //Debug.Log(strlist[1]);
            //Debug.Log(strlist[2]);

            if(strlist.Length > 2 )
            {
                string id = strlist[0];
                double x = Convert.ToDouble(strlist[1]);
                double y = Convert.ToDouble(strlist[2]);


                if (id != this.id.ToString())
                {
                    //Debug.Log("Other Client found!");
                    if (isSet)
                    {
                        per.move(x, y);
                    }

                }
            }

            

            //try
            //{
            //    MyVector pos = JsonConvert.DeserializeObject<MyVector>(e.Data);
            //    //Console.WriteLine("Created a vector: " + pos.x + "," + pos.y);
            //    DrawDot(pos.x, pos.y, 50, 15, 1);
            //}
            //catch (Exception ex)
            //{
            //    //Console.WriteLine(ex);
            //    Debug.Log("I don't know what to do with \"" + e.Data + "\"");
            //}

        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Debug.Log("Received from the server: " + e.Message);
        }

        private void DrawDot(double xpos, double ypos, int width, int height, int borderWidth)
        {
            // Convert from normalized coordinates to "pixel" coordinates
            int x = (int)Math.Round(xpos * width);
            int y = (int)Math.Round(ypos * height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (i == x && j == y)
                    {
                        Console.Write("O");
                    }
                    else if (j < borderWidth || j > height - 1 - borderWidth
                        || i < borderWidth || i > width - 1 - borderWidth)
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
