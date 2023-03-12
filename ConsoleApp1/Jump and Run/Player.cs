using Shard;
using System.Collections.Generic;
using SDL2;
using Newtonsoft.Json;
using System;

namespace JumpAndRun
{
    class Player : NetworkedObject, InputListener, CollisionHandler
    {
        private string sprite;
        private bool left, right, jumpUp, jumpDown, fall, canJump, shoot;
        private int spriteCounter, spriteCounterDir;
        private string spriteName;
        private double spriteTimer, jumpCount;
        
        private double jumpSpeed = 260;
        private double fallCounter;
        private int updateCounter;
        private double speed = 100;
        private bool movingStarted;
        public Bullet bullet;
        private int id;
        public string spriteColor = "red";
        public bool synced;

        private List<int> xPositions = new List<int> { 50, 300 };
        private List<int> yPositions = new List<int> { 330, 330 };

        class Vector
        {

        }

        public Player(bool synced)
        {
            Debug.Log("synced: "+synced);
            syncedInitialize();
            this.synced = true;

        }
        public Player()
        {
            Debug.Log("non synced");
            localInitialize();
            this.synced = false;
        }

        public override bool isSynced()
        {
            return synced;
        }


        public override void localInitialize()
        {
            spriteName = "right";
            spriteCounter = 1;
            updateCounter = 0;
            movingStarted = false;
            setPhysicsEnabled();
            MyBody.addRectCollider();
            addTag("Player");
            spriteTimer = 0;
            jumpCount = 0;
            MyBody.Mass = 1;
            Bootstrap.getInput().addListener(this);

            id = NetworkClient.GetInstance().id;


            Transform.translate(50, 330);
            MyBody.StopOnCollision = false;
            MyBody.Kinematic = false;

            spriteCounterDir = 1;
        }

        public override void syncedInitialize()
        {
            spriteName = "right";
            spriteCounter = 1;
           // setPhysicsEnabled();
            //MyBody.addRectCollider();
            ///addTag("NetworkedPlayer");
            spriteTimer = 0;
            jumpCount = 0;
            //MyBody.Mass = 1;
            //Bootstrap.getInput().addListener(this);


            Transform.translate(50, 480);
            //MyBody.StopOnCollision = false;
            //MyBody.Kinematic = false;
        

            spriteCounterDir = 1;
        }

 



        public void Move(double x, double y)
        {
            if (this.Transform != null)
            {
                this.Transform.Y = y;
                this.Transform.X = x;
            }

        }

        public int GetId()
        {
            return id;
        }


        public void handleInput(InputEvent inp, string eventType)
        {
            if (eventType == "KeyDown")
            {

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = true;
                    spriteName = "right";

                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = true;
                    spriteName = "left";
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W && canJump == true)
                {
                    jumpUp = true;
                    Debug.Log("Jumping up");

                }
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    shoot = true;
                    Debug.Log("Shoot");
                }
            }

            else if (eventType == "KeyUp")
            {

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = false;

                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = false;
                }



            }

        }


        public override string getFullSpriteName()
        {
            return spriteColor + spriteName + spriteCounter;
        }


        private int getRandom(int max)
        {
            Random rand = new Random();
            return rand.Next(max);
        }



        public override void update()
        {

            //Debug.Log("Fallcounter is " + fallCounter);
            double oldX = Transform.X;
            double oldY = Transform.Y;

            if (left)
            {
                this.Transform.translate(-1 * speed * Bootstrap.getDeltaTime(), 0);
                spriteTimer += Bootstrap.getDeltaTime();
                //this.sendPosition();
            }

            if (right)
            {
                this.Transform.translate(1 * speed * Bootstrap.getDeltaTime(), 0);
                spriteTimer += Bootstrap.getDeltaTime();

                //this.sendPosition();
            }

            if (jumpUp)
            {

                fall = false;
                fallCounter = 0;
                if (jumpCount < 0.3f)
                {
                    this.Transform.translate(0, -1 * jumpSpeed * Bootstrap.getDeltaTime());
                    jumpCount += Bootstrap.getDeltaTime();
                }
                else
                {
                    jumpCount = 0;
                    jumpUp = false;
                    fall = true;

                }
                //this.sendPosition();
            }

            if (shoot)
            {
                if (bullet==null || bullet.ToBeDestroyed)
                {
                    bullet = new Bullet();
                    bullet.setSpriteName(spriteColor+"bullet");
                    if (spriteName == "right")
                    {
                        bullet.setPosition(Transform.X + 40, Transform.Y);
                        bullet.setDirection(1);
                    }
                    else
                    {
                        bullet.setPosition(Transform.X - 10, Transform.Y);
                        bullet.setDirection(-1);
                    }
                }
                
                
               
                shoot = false;
            }



            if (spriteTimer > 0.1f)
            {
                spriteTimer -= 0.1f;
                spriteCounter += spriteCounterDir;

                if (spriteCounter >= 4)
                {
                    spriteCounterDir = -1;

                }

                if (spriteCounter <= 1)
                {
                    spriteCounterDir = 1;

                }


            }

            if (fall)
            {
                Transform.translate(0, jumpSpeed * Bootstrap.getDeltaTime());
                fallCounter += Bootstrap.getDeltaTime();

                if (Transform.Y > 900)
                {
                    ToBeDestroyed = true;
                }
                //this.sendPosition();

            }

            this.Transform.SpritePath = "ManicMinerSprites/" +getFullSpriteName()+ ".png";

            //Debug.Log(this.Transform.SpritePath);


            Bootstrap.getDisplay().addToDraw(this);

            updateCounter++;
        }

        public bool shouldReset(PhysicsBody x)
        {
            float[] minAndMaxX = x.getMinAndMax(true);
            float[] minAndMaxY = x.getMinAndMax(false);

            if (Transform != null)
            {
                if (Transform.X + Transform.Wid >= minAndMaxX[0] && Transform.X <= minAndMaxX[1])
                {
                    // We're in the centre, so it's fine.

                    if (Transform.Y + Transform.Ht >= minAndMaxY[0])
                    {
                        return true;
                    }
                }
            }

            

            return false;
        }

        public void onCollisionEnter(PhysicsBody x)
        {

            if (x.Parent.checkTag("NetworkedBullet"))
            {
                ToBeDestroyed = true;
            }



            if (fallCounter > 2)
            {
                ToBeDestroyed = true;
            }

            fallCounter = 0;

            if (shouldReset(x))
            {
                fall = true;
            }
            else
            {
                fall = false;
            }

        }

        public void onCollisionExit(PhysicsBody x)
        {
            //if (x.Parent.checkTag("Collectible"))
            //{
            //    return;
            //}

            canJump = false;
            fall = true;

            if (x.Parent.checkTag("MovingBox"))
            {
                //Debug.Log("MovingBox collide");
                //sendPosition();
            }

        }

        public void onCollisionStay(PhysicsBody x)
        {
            if (x.Parent.checkTag("MovingBox"))
            {
                //Debug.Log("MovingBox collide");
                //sendPosition();
            }

            if (x.Parent.checkTag("Collectible"))
            {
                return;
            }

            if (shouldReset(x))
            {
                fall = false;
                canJump = true;
                fallCounter = 0;
            }
            else
            {
                fall = true;
            }

        }
    }
}
