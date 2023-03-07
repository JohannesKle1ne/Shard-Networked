using Shard;
using System;
using System.Drawing;
using WebSocketSharp;

namespace JumpAndRun
{
    class NetworkedBullet : GameObject, CollisionHandler
    {
        private int id;

        public NetworkedBullet(int id)
        {
            this.id = id;
        }
        public override void initialize()
        {
            id= 1;

            setPhysicsEnabled();
            
            addTag ("Enemy");
            MyBody.addRectCollider((int)Transform.X, (int)Transform.Y, 10, 10);
            MyBody.PassThrough = true;

        }
        public void setSpriteName(string name)
        {
            //Debug.Log("ManicMinerSprites/" + name + ".png");
            if (this.Transform != null)
            {
                this.Transform.SpritePath = "ManicMinerSprites/" + name + ".png";
            }
            
        }


        public void Move(double x, double y)
        {
            if (this.Transform != null)
            {
                this.Transform.Y = y;
                this.Transform.X = x;
            }

        }

        public override void update()
        {
            //Random r = new Random();
            //Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            //Bootstrap.getDisplay().drawLine(
            //             (int)Transform.X,
            //             (int)Transform.Y,
            //             (int)Transform.X + 10,
            //             (int)Transform.Y + 10,
            //             col);

            //Bootstrap.getDisplay().drawLine(
            //    (int)Transform.X + 10,
            //    (int)Transform.Y,
            //    (int)Transform.X,
            //    (int)Transform.Y + 10,
            //    col);


            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag ("Player")) {
                this.ToBeDestroyed = true;
                Client client = Client.GetInstance();
                Shard.Action a = new Shard.Action(client.id, MessageType.BulletCollision);
                a.bulletId = id;
                client.Send(a.ToJson());
            }

        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "City: [" + Transform.X + ", " + Transform.Y + "]";
        }


    }
}
