using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;

namespace Shard
{
    class MyVector
    {
        public double x, y;
    }


    class Client
    {

        public void Start()
        {
            // Create a scoped instance of a WS client that will be properly disposed
            //using (WebSocket ws = new WebSocket("ws://simple-websocket-server-echo.glitch.me/"))
            WebSocket ws = new WebSocket("ws://127.0.0.1:7890/EchoAll");


            ws.OnMessage += Ws_OnMessage;

            ws.Connect();
            ws.Send("Hello from Client 1!");
            Debug.Log("Connected");
            Console.WriteLine("Connected 2");

            //Console.ReadKey();

        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log("Received from the server: " + e.Data);
            Console.WriteLine("Created a vector: ");

            try
            {
                MyVector pos = JsonConvert.DeserializeObject<MyVector>(e.Data);
                //Console.WriteLine("Created a vector: " + pos.x + "," + pos.y);
                DrawDot(pos.x, pos.y, 50, 15, 1);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                Debug.Log("I don't know what to do with \"" + e.Data + "\"");
            }

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
