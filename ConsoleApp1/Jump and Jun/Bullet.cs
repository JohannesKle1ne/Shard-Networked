using Shard;
using System;
using System.Drawing;

namespace JumpAndRun
{
    class Bullet : GameObject, CollisionHandler
    {

        int direction = 1;
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


        public override void update()
        {
            Transform.translate(direction*300 * Bootstrap.getDeltaTime(), 0);
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
