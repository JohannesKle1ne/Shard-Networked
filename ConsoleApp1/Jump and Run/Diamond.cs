using Shard;
using System;
using System.Drawing;

namespace JumpAndRun
{
    class Diamond : NetworkedObject, CollisionHandler
    {

        private int direction = 1;
        private int updateCounter = 0;
        private int speed = 500;
        private string spriteName;
        public bool synced;


        public Diamond(bool synced)
        {
            Debug.Log("synced: " + synced);
            syncedInitialize();
            this.synced = true;

        }
        public Diamond()
        {
            Debug.Log("non synced");
            localInitialize();
            this.synced = false;
        }

        public override bool isSynced()
        {
            return synced;
        }

        public override void syncedInitialize()
        {
            setPhysicsEnabled();

            addTag("NetworkedDiamond");
            MyBody.addRectCollider((int)Transform.X, (int)Transform.Y, 10, 10);
            MyBody.PassThrough = true;
            spriteName = "diamond";
            this.Transform.SpritePath = "ManicMinerSprites/" + spriteName + ".png";
        }

        public override void localInitialize()
        {
            setPhysicsEnabled();

            addTag("Diamond");
            MyBody.addRectCollider((int)Transform.X, (int)Transform.Y, 10, 10);

            MyBody.StopOnCollision = false;
            MyBody.Kinematic = false;
            spriteName = "diamond";
            this.Transform.SpritePath = "ManicMinerSprites/" + spriteName + ".png";
        }




        public void setSpriteName(string name)
        {
            //this.spriteName = name;
            //this.Transform.SpritePath = "ManicMinerSprites/" + name + ".png";
        }

        public override string getFullSpriteName()
        {
            return spriteName;
        }

        public void setPosition(double x, double y)
        {
            Transform.translate(x, y);
        }

        
        

        public override void update()
        {
            
            Bootstrap.getDisplay().addToDraw(this);

        }

        public void onCollisionEnter(PhysicsBody x)
        {
           
            if (x.Parent.checkTag("Player"))
            {
                if (this.synced)
                {
                    this.RemoteDestroy = true;
                }
                else
                {
                    this.ToBeDestroyed = true;
                }


                Debug.Log("collistion found with player");
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
