using Shard;
using System;

namespace JumpAndRun
{
    class MovingBox : NetworkedObject, CollisionHandler
    {
        private int moveDirX, moveDirY;
        private int moveSpeed;

        public int MoveDirX { get => moveDirX; set => moveDirX = value; }
        public int MoveDirY { get => moveDirY; set => moveDirY = value; }
        public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public MovingBox(bool synced)
        {
            this.synced = true;
            syncedInitialize();


        }
        public MovingBox()
        {
            this.synced = false;
            localInitialize();

        }
        public override void syncedInitialize()
        {
            setPhysicsEnabled();
            MyBody.addRectCollider();
            MyBody.Mass = 10;
            MyBody.Kinematic = true;
            moveDirY = 1;
            moveSpeed = 100;
            this.Transform.SpritePath = "ManicMinerSprites/box.png";

            addTag("MovingBox");

        }

        public override void localInitialize()
        {

        }

        public void onCollisionEnter(PhysicsBody x)
        {
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override void syncedUpdate()
        {

            if (moveDirY != 0)
            {
                Transform.translate(0, moveSpeed * moveDirY * Bootstrap.getDeltaTime());

                if (Transform.Y > 600) {
                    MoveDirY = -1;
                }
            
                if (Transform.Y < 100) {
                    MoveDirY = 1;

                }
            }

            Bootstrap.getDisplay().addToDraw(this);
        }

        public override string getFullSpriteName()
        {
            return "box";
        }

        public override bool isSynced()
        {
            return synced;
        }

  
    }
}
