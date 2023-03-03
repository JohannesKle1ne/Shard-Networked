using Shard;
using System;
using System.Drawing;

namespace JumpAndRun
{
    class Bullet : GameObject, CollisionHandler
    {

        private int direction = 1;
        private int updateCounter = 0;
        public override void initialize()
        {
            setPhysicsEnabled();

            addTag ("Bullet");
            MyBody.addRectCollider((int)Transform.X, (int)Transform.Y, 10, 10);

            MyBody.StopOnCollision = false;
            MyBody.Kinematic = false;

        }

        public void setPosition(double x, double y)
        {
            Transform.translate(x, y);
        }

        private void sendPosition()
        {
            Client client = Client.GetInstance();

            if (updateCounter % 50 == 0)
            {
                string message = new Position(client.id, MessageType.BulletPosition, this.Transform.X, this.Transform.Y, "right").ToJson();
                client.Send(message);
            }
        }


        public override void update()
        {
            if (Transform.X > 600 || Transform.X < 0 || Transform.Y > 600 || Transform.Y < 0)
            {
                this.ToBeDestroyed= true;
            }
            else
            {
                Transform.translate(direction * 300 * Bootstrap.getDeltaTime(), 0);
                sendPosition();
                updateCounter++;
            }
           
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag ("Enemy")) {
                this.ToBeDestroyed = true;
                Debug.Log("Collsions");
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
            this.direction= direction;
        }
    }
}
