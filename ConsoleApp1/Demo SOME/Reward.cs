using Shard;
using System;
using System.Drawing;
using System.Xml.Linq;

namespace JumpAndRun
{
    abstract class Reward : NetworkedObject, CollisionHandler
    {
        private string spriteName;

        public override bool isSynced()
        {
            return synced;
        }

        public override void syncedInitialize()
        {
            setPhysicsEnabled();

            MyBody.addRectCollider();


        }

        public override void localInitialize()
        {
            setPhysicsEnabled();

            MyBody.addRectCollider();
            MyBody.UsesGravity = true;
            MyBody.Mass = 0.4f;

        }

        public void setSpriteName(string name)
        {
            this.spriteName = name;
            this.Transform.SpritePath = "ManicMinerSprites/" + name + ".png";
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
            }

        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }


    }
}
