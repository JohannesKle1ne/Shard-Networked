using Shard;
using System;

namespace JumpAndRun
{
    class MovingBox : NetworkedObject, CollisionHandler
    {
        private int moveDirX, moveDirY;
        private int maxY, minY;
        private int maxX, minX;
        private int moveDist;
        private int moveSpeed;

        private int origX, origY;
        public int MoveDist { get => moveDist; set => moveDist = value; }
        public int MoveDirX { get => moveDirX; set => moveDirX = value; }
        public int MoveDirY { get => moveDirY; set => moveDirY = value; }
        public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public MovingBox(bool synced)
        {
            Debug.Log("synced: " + synced);
            this.synced = true;
            syncedInitialize();


        }
        public MovingBox()
        {
            Debug.Log("non synced");
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
           // setPosition(0,0)
            this.Transform.SpritePath = "ManicMinerSprites/box.png";

            addTag("MovingBox");

        }

        public override void localInitialize()
        {

        }

        public void setPosition(int x, int y, int dist, int speed) {
            //origX = x;
            //origY = y;

            //MoveDist = dist;

            //minY = origY - MoveDist;
            //maxY = origY;

            //maxX = origX + MoveDist;
            //minX = origX;

            //MoveSpeed = speed;

           // Transform.translate (x, y);
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
            
                if (Transform.Y < 200) {
                    MoveDirY = 1;

                }
            }


            if (moveDirX != 0)
            {
                Transform.translate(moveSpeed * moveDirX * Bootstrap.getDeltaTime(), 0);

                if (Transform.X > maxX)
                {
                    MoveDirX = -1;
                }

                if (Transform.X < minX)
                {
                    MoveDirX = 1;

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
