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
        public MessageType type;
        public MatePosition( double x, double y) {
            this.x = x;
            this.y = y;
            type = MessageType.MatePosition;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    class EnemyPosition
    {
        public double x;
        public double y;
        public int enemyId;
        public MessageType type;
        public EnemyPosition(int id, double x, double y)
        {
            this.x = x;
            this.y = y;
            this.enemyId = id;
            type = MessageType.EnemyPosition;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }




    enum MessageType
    {
        Unknown,
        MatePosition,
        EnemyPosition,
        
    }
}
