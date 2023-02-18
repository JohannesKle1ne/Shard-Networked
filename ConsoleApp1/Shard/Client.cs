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
        internal double x, y;
    }


    internal sealed class Client
    {
        private Client() { }

        private static Client _instance;
        private WebSocket ws;
        private Mate per;
        private Enemy enemy;
        private bool isSet = false;
        internal int id;

        internal static Client GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Client();
            }
            return _instance;
        }

        internal void Start()
        {
            // Create a scoped instance of a WS client that will be properly disposed
            //using (WebSocket ws = new WebSocket("ws://simple-websocket-server-echo.glitch.me/"))
            //ws = new WebSocket("ws://secret-island-78427.herokuapp.com");
            ws = new WebSocket("ws://localhost:3000");
            Random rd = new Random();

            id =  rd.Next(1000, 9999);

            ws.OnMessage += Ws_OnMessage;

            ws.OnError += Ws_OnError;

            ws.Connect();

            //ws.Send($"{234},{234},{23432}");
            ws.Send("Hello!");
            Debug.Log("Connected");
            Console.WriteLine("Connected 2");
            //Console.ReadKey();

        }

        internal void Send(String message)
        {
            ws.Send(message);
        }

        internal void setMate(Mate m)
        {
            per = m;
            isSet = true;
        }

        internal void setEnemy(Enemy e)
        {
            enemy = e;
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            //Debug.Log("Received from the server: " + e.Data);

           

            //if (strlist.Length > 2 )
            //{
            //    string id = strlist[0];
            //    double x = Convert.ToDouble(strlist[1]);
            //    double y = Convert.ToDouble(strlist[2]);


            //    if (id != this.id.ToString())
            //    {
            //        //Debug.Log("Other Client found!");
            //        if (isSet)
            //        {
            //            per.move(x, y);
            //        }

            //    }
            //}


            MessageType type = getMessageType(e.Data);
            //Debug.Log(type.ToString());
            if (type == MessageType.MatePosition)
            {
                MatePosition mPos = JsonConvert.DeserializeObject<MatePosition>(e.Data);
                if (isSet)
                {
                    per.Move(mPos.x, mPos.y);
                }
            }
            if (type == MessageType.EnemyPosition)
            {
                EnemyPosition ePos = JsonConvert.DeserializeObject<EnemyPosition>(e.Data);
                if (isSet)
                {
                    enemy.Move(ePos.x, ePos.y);
                }
            }




            //try
            //{
            //    Message message = JsonConvert.DeserializeObject<Message>(e.Data);
            //    if (message.type == MessageType.MatePosition)
            //    {
            //        MatePosition mPos = (MatePosition) message.content;
            //        if (isSet && mPos.id != this.id)
            //        {
            //            per.move(mPos.x, mPos.y);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.Log("I don't know what to do with \"" + e.Data + "\"");
            //    Debug.Log(ex.Message);
            //}

        }

        private MessageType getMessageType(String messageString)
        {
            try
            {
                MatePosition mPos  = JsonConvert.DeserializeObject<MatePosition>(messageString);
                if (mPos != null)
                {
                    return mPos.type;
                }
            }
            catch(Exception ex)
            {
                return MessageType.Unknown;
            }
            return MessageType.Unknown;
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
