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
        public MessageType type;
        public Position(int id, MessageType type, double x, double y) {
            clientId = id;
            this.x = x;
            this.y = y;
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
        
    }
}
