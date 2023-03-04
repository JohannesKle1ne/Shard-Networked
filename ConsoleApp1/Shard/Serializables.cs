using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shard
{
    class Position
    {
        public int clientId;
        public double x; 
        public double y;
        public string sprite;
        public MessageType type;
        public Position(int id, MessageType type, double x, double y, string sprite)
        {
            clientId = id;
            this.x = x;
            this.y = y;
            this.type = type;
            this.sprite = sprite;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    class Action
    {
        public int clientId;
        public MessageType type;
        public string color;
        public Action(int id, MessageType type)
        {
            clientId = id;
            this.type = type;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }




    enum MessageType
    {
        Unknown,
        PlayerPosition,
        PlayerStartPosition,
        BulletPosition,
        PlayerDestroy,
        BulletDestroy,
        Color
        
    }
}
