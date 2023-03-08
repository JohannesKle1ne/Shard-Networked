using Shard;
using System;
using System.Drawing;

namespace JumpAndRun
{
    class Bullet : GameObject, CollisionHandler
    {

        private int direction = 1;
        private int updateCounter = 0;
        private int speed = 500;
        private string spriteName;
        public override void initialize()
        {
            setPhysicsEnabled();

            addTag("Bullet");
            MyBody.addRectCollider((int)Transform.X, (int)Transform.Y, 10, 10);

            MyBody.StopOnCollision = false;
            MyBody.Kinematic = false;
            this.Transform.SpritePath = "ManicMinerSprites/" + "bullet" + ".png";

        }

        public void setSpriteName(string name)
        {
            this.spriteName = name;
            this.Transform.SpritePath = "ManicMinerSprites/" + name + ".png";
        }

        public void setPosition(double x, double y)
        {
            Transform.translate(x, y);
        }

        private void sendPosition()
        {
            NetworkClient client = NetworkClient.GetInstance();

            if (updateCounter % 20 == 0)
            {
                string message = new Position(client.id, MessageType.BulletPosition, this.Transform.X, this.Transform.Y, spriteName).ToJson();
                client.Send(message);
            }
        }

        private void sendDestroy()
        {
            NetworkClient client = NetworkClient.GetInstance();


            string message = new Shard.Action(client.id, MessageType.BulletDestroy).ToJson();
            client.Send(message);

        }


        public override void update()
        {
            if (!this.ToBeDestroyed)
            {
                if (Transform.X > 1200 || Transform.X < 0 || Transform.Y > 600 || Transform.Y < 0)
                {
                    this.ToBeDestroyed = true;
                    sendDestroy();
                }
                else
                {
                    Transform.translate(direction * speed * Bootstrap.getDeltaTime(), 0);
                    sendPosition();
                    updateCounter++;
                }
            }
            Bootstrap.getDisplay().addToDraw(this);

        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Box"))
            {
                this.ToBeDestroyed = true;
                sendDestroy();
            
            //Debug.Log("collistion found with Networked player");
            //this.ToBeDestroyed = true;
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

        internal void setDirection(int direction)
        {
            this.direction = direction;
        }
    }
}
