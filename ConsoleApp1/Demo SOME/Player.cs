using Shard;
using System.Collections.Generic;
using SDL2;
using Newtonsoft.Json;
using System;

namespace JumpAndRun
{
    class Player : NetworkedObject, InputListener, PositionCollisionHandler
    {
        private bool left, right, jumpUp, fall, canJump, shoot;
        private int spriteCounter, spriteCounterDir;
        private string spriteName;
        private double spriteTimer, jumpCount;
        private double jumpMax = 0.3;

        private double jumpSpeed = 260;
        private double speed = 100;
        public Bullet bullet;
        private int id;
        public string spriteColor = "red";
        public double reload;
        public double reloadTime;

        public int rewardCounter = 0;
        public int killCounter = 0;

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
            spriteTimer = 0;
            jumpCount = 0;
            Transform.translate(50, 480);
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

            double oldX = Transform.X;
            double oldY = Transform.Y;

            if (left)
            {
                this.Transform.translate(-1 * speed * Bootstrap.getDeltaTime(), 0);
                spriteTimer += Bootstrap.getDeltaTime();
            }

            if (right)
            {
                this.Transform.translate(1 * speed * Bootstrap.getDeltaTime(), 0);
                spriteTimer += Bootstrap.getDeltaTime();
            }

            if (jumpUp)
            {

                if (jumpCount < jumpMax)
                {
                    this.Transform.translate(0, -1 * jumpSpeed * Bootstrap.getDeltaTime());
                    jumpCount += Bootstrap.getDeltaTime();
                }
                else
                {
                    jumpCount = 0;
                    jumpUp = false;

                }
            }

            if (shoot)
            {
                if (true)
                {
                    Bullet bullet = new Bullet();
                    bullet.setSpriteName(spriteColor + "bullet");
                    if (spriteName == "right")
                    {
                        bullet.setPosition(Transform.X + Transform.Wid - 10, Transform.Y + 5);
                        bullet.setDirection(1);
                    }
                    else
                    {
                        bullet.setPosition(Transform.X, Transform.Y + 5);
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

            this.Transform.SpritePath = "ManicMinerSprites/" + getFullSpriteName() + ".png";

            if (reload > 0)
            {
                reload -= Bootstrap.getDeltaTime();
            }

            if (Transform.X > 1300 || Transform.X < -100 || Transform.Y > 700 || Transform.Y < -100)
            {
                this.ToBeDestroyed = true;
            }


            Bootstrap.getDisplay().addToDraw(this);

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

            if (direction == "Bottom")
            {
                canJump = true;
                MyBody.UsesGravity = false;
            }
            if (direction == "BottomLeft")
            {
                Transform.translate(0.1, 0);
            }

            if (direction == "BottomRight")
            {
                Transform.translate(-0.1, 0);
            }

        }

        public void onCollisionEnter(PhysicsBody x, string direction)
        {


            if (x.Parent.checkTag("NetworkedBullet"))
            {
                ToBeDestroyed = true;
            }

            if (x.Parent.checkTag("Gun"))
            {
                reloadTime -= 0.5;
                rewardCounter++;


            }

            if (x.Parent.checkTag("Boot"))
            {
                speed += 50;
                rewardCounter++;

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
            if (MyBody != null && Transform != null)
            {
                handleCollistionDirection(direction);

            }

        }
    }
}
