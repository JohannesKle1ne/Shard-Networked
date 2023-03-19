using Shard;
using System.Collections.Generic;
using SDL2;
using Newtonsoft.Json;
using System;

namespace JumpAndRun
{
    class Player : NetworkedObject, InputListener, PositionCollisionHandler
    {
        private string sprite;
        private bool left, right, jumpUp, jumpDown, fall, canJump, shoot;
        private int spriteCounter, spriteCounterDir;
        private string spriteName;
        private double spriteTimer, jumpCount;
        private double jumpMax = 0.3;

        private double jumpSpeed = 260;
        private double fallCounter;
        private double speed = 100;
        private bool movingStarted;
        public Bullet bullet;
        private int id;
        public string spriteColor = "red";
        public double reload;
        public double reloadTime;

        public int rewardCounter = 0;
        public int killCounter = 0;

        class Vector
        {

        }

        public Player(bool synced)
        {
            this.synced = true;
            syncedInitialize();
        }
        public Player()
        {
            this.synced = false;
            localInitialize();
        }



        public override bool isSynced()
        {
            return synced;
        }


        public override void localInitialize()
        {
            spriteName = "right";
            spriteCounter = 1;
            movingStarted = false;
            setPhysicsEnabled();
            MyBody.addRectCollider();
            MyBody.UsesGravity = true;
            MyBody.Mass = 0.4f;
            addTag("Player");
            spriteTimer = 0;
            jumpCount = 0;
            reloadTime = 2;
            reload = 0;
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
                    if (reload <= 0)
                    {
                        shoot = true;
                        reload = reloadTime;
                        Debug.Log("Shoot with time: " + reload);
                    }

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
        public override void syncedUpdate()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }
        public override void localUpdate()
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

                //fall = false;
                //fallCounter = 0;
                if (jumpCount < jumpMax)
                {
                    this.Transform.translate(0, -1 * jumpSpeed * Bootstrap.getDeltaTime());
                    jumpCount += Bootstrap.getDeltaTime();
                }
                else
                {
                    jumpCount = 0;
                    jumpUp = false;
                   // fall = true;

                }
                //this.sendPosition();
            }

            if (shoot)
            {
                if (true)
                {
                    Bullet bullet = new Bullet();
                    bullet.setSpriteName(spriteColor + "bullet");
                    if (spriteName == "right")
                    {
                        bullet.setPosition(Transform.X + Transform.Wid-10, Transform.Y+5);
                        bullet.setDirection(1);
                    }
                    else
                    {
                        bullet.setPosition(Transform.X, Transform.Y+5);
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
                //Transform.translate(0, jumpSpeed * Bootstrap.getDeltaTime());
                //fallCounter += Bootstrap.getDeltaTime();

                //if (Transform.Y > 900)
                //{
                //    ToBeDestroyed = true;
                //}
                //this.sendPosition();

            }

            this.Transform.SpritePath = "ManicMinerSprites/" + getFullSpriteName() + ".png";

            //Debug.Log(this.Transform.SpritePath);
            if (reload > 0)
            {
                reload -= Bootstrap.getDeltaTime();
            }

            if (Transform.X > 1300 || Transform.X < -100 || Transform.Y > 700 || Transform.Y < -100)
            {
                this.ToBeDestroyed = true;
                Debug.Log("set to be destroyd");
            }


            Bootstrap.getDisplay().addToDraw(this);

        }

        public bool isCenterX(PhysicsBody x)
        {
            
            float[] minAndMaxX = x.getMinAndMax(true);
            float[] minAndMaxY = x.getMinAndMax(false);

            if (Transform != null)
            {
                if (Transform.X + Transform.Wid >= minAndMaxX[0] && Transform.X <= minAndMaxX[1])
                {
                    // We're in the centre, so it's fine.
                    return true;

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
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public void handleCollistionDirection(string direction)
        {
            //if (direction == "Top")
            //{
            //    Transform.translate(0, 0.1);
            //}
            if (direction == "Bottom")
            {
                canJump = true;
                MyBody.UsesGravity = false;
            }
            if (direction == "BottomLeft")
            {
                Transform.translate(0.1, 0);
            }
            //if (direction == "Left")
            //{
            //    Transform.translate(0.1, 0.1);
            //}
            if (direction == "BottomRight")
            {
                Transform.translate(-0.1, 0);
            }
            //if (direction == "Right")
            //{
            //    Transform.translate(-0.1, 0.1);
            //}
            //if (direction == "TopLeft")
            //{
            //    Transform.translate(0.1, 0.1);
            //}
            //if (direction == "TopRight")
            //{
            //    Transform.translate(-0.1, 0.1);
            //}
        }

        public void onCollisionEnter(PhysicsBody x, string direction)
        {
            

            Debug.Log("direction: " + direction);
            if (x.Parent.checkTag("NetworkedBullet"))
            {
                ToBeDestroyed = true;
            }

            if (x.Parent.checkTag("Gun"))
            {
                reloadTime -= 0.5;
                rewardCounter++;

                Debug.Log("collistion found with gun");
            }

            if (x.Parent.checkTag("Boot"))
            {
                speed += 50;
                rewardCounter++;
                Debug.Log("collistion found with boot");
            }

            if (x.Parent.checkTag("Spring"))
            {
                jumpMax += 0.1;
                rewardCounter++;
            }

            if (MyBody != null && Transform != null)
            {
                handleCollistionDirection(direction);

            }
        }

        public void onCollisionExit(PhysicsBody x, string direction)
        {
            if (MyBody != null && Transform != null)
            {
                MyBody.UsesGravity = true;

            }

            canJump = false;

        }

        public void onCollisionStay(PhysicsBody x, string direction)
        {
            if(MyBody!=null && Transform != null)
            {
                handleCollistionDirection(direction);

            }

        }
    }
}
