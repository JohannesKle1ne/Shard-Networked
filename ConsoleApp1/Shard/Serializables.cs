using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shard
{
    class MatePosition
    {
        public double x; 
        public double y;
        public int id;
        public MatePosition(int id, double x, double y) {
            this.x = x;
            this.y = y;
            this.id = id;
        }

        public string getJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    


    class Message
    {
        public MessageType type;
        public MatePosition content;
        public Message(MessageType type, MatePosition content)
        {

            this.type = type;
            this.content = content;
        }

        public string getJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    enum MessageType
    {
        MatePosition = 1,    
    }
}
