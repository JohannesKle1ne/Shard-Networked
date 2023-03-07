﻿using Shard;
using System.Collections.Generic;
using SDL2;

namespace JumpAndRun
{
    class NetworkedPlayer : GameObject, InputListener, CollisionHandler
    {
        private string sprite;
        private bool left, right, jumpUp, jumpDown, fall, canJump;
        private int spriteCounter, spriteCounterDir;
        private string spriteName;
        private string fullSpriteName;
        private double spriteTimer, jumpCount;
        private double speed = 100, jumpSpeed = 260;
        private double fallCounter;
        private int id;
        public NetworkedBullet nBullet;

        public NetworkedPlayer(int id)
        {
            this.id = id;
        }

        public void setSpriteName(string sprite)
        {
            fullSpriteName = sprite;
        }
        public override void initialize()
        {
            spriteName = "right";
            spriteCounter = 1;
            //setPhysicsEnabled();
            //MyBody.addRectCollider();
            addTag("NetworkedPlayer");
            spriteTimer = 0;
            jumpCount = 0;
            //MyBody.Mass = 1;
            //Bootstrap.getInput().addListener(this);


            Transform.translate (50, 480);
            //MyBody.StopOnCollision = false;
            //MyBody.Kinematic = false;

            spriteCounterDir = 1;
        }

        public NetworkedBullet GetBullet()
        {
            return nBullet;
        }

        public void MoveBullet(double x, double y, string sprite)
        {
           // Debug.Log(sprite);
            if (nBullet == null)
            {
                nBullet = new NetworkedBullet(id);
                
            }
            nBullet.Move(x, y);
            nBullet.setSpriteName(sprite);
        }

        public void Move(double x, double y)
        {
            spriteTimer += Bootstrap.getDeltaTime()*4;

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
            //if (eventType == "KeyDown")
            //{

            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
            //    {
            //        right = true;
            //        spriteName = "right";

            //    }

            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
            //    {
            //        left = true;
            //        spriteName = "left";
            //    }

            //    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE && canJump == true)
            //    {
            //        jumpUp = true;
            //        Debug.Log ("Jumping up");
            //    }

            //}

            //else if (eventType == "KeyUp")
            //    {

            //        if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
            //        {
            //            right = false;

            //        }

            //        if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
            //        {
            //            left = false;
            //        }

                   

            //}

        }

        public override void update()
        {

            //Debug.Log("Fallcounter is " + fallCounter);


            //if (left)
            //{
            //    this.Transform.translate(-1 * speed * Bootstrap.getDeltaTime(), 0);
            //    spriteTimer += Bootstrap.getDeltaTime();
            //    Debug.Log($"x:{this.Transform.X}, y:{this.Transform.Y}");
            //    Client client = Client.GetInstance();
            //    client.Send($"{client.id};{this.Transform.X};{this.Transform.Y}");
            //}

            //if (right)
            //{
            //    this.Transform.translate(1 * speed * Bootstrap.getDeltaTime(), 0);
            //    spriteTimer += Bootstrap.getDeltaTime();
            //    Client client = Client.GetInstance();
            //    client.Send($"{client.id};{this.Transform.X};{this.Transform.Y}");
            //}

            //if (jumpUp) {
            //    fall = false;
            //    fallCounter = 0;
            //    if (jumpCount < 0.3f) {
            //        this.Transform.translate(0, -1 * jumpSpeed * Bootstrap.getDeltaTime());
            //        jumpCount += Bootstrap.getDeltaTime();
            //    }
            //    else {
            //        jumpCount = 0;
            //        jumpUp = false;
            //        fall = true;

            //    }
            //}



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

            //if (fall) {
            //    Transform.translate(0, jumpSpeed * Bootstrap.getDeltaTime());
            //    fallCounter += Bootstrap.getDeltaTime();

            //    if (Transform.Y > 900) {
            //        ToBeDestroyed = true;
            //    }

            //}

            this.Transform.SpritePath = "ManicMinerSprites/" + fullSpriteName + ".png";


            Bootstrap.getDisplay().addToDraw(this);
        }

        public bool shouldReset(PhysicsBody x)
        {
            float[] minAndMaxX = x.getMinAndMax(true);
            float[] minAndMaxY = x.getMinAndMax(false);

            if (Transform.X + Transform.Wid >= minAndMaxX[0] && Transform.X <= minAndMaxX[1]) {
                // We're in the centre, so it's fine.

                if (Transform.Y + Transform.Ht >= minAndMaxY[0]) {
                    return true;
                }
            }

            return false;
        }

        public void onCollisionEnter(PhysicsBody x)
        {

            if (x.Parent.checkTag("Enemy"))
            {
                //Debug.Log("collistion found with Enemy");
                //this.ToBeDestroyed = true;
            }


            //if (x.Parent.checkTag ("Collectible")) {
            //    return;
            //}

            //if (fallCounter > 2) {
            //    ToBeDestroyed = true;
            //}
            
            //fallCounter = 0;
 
            //if (shouldReset(x))
            //{
            //    fall = true;
            //}
            //else
            //{
            //    fall = false;
            //}

        }

        public void onCollisionExit(PhysicsBody x)
        {
            if (x.Parent.checkTag("Collectible"))
            {
                return;
            }

            canJump = false;
            fall = true;

        }

        public void onCollisionStay(PhysicsBody x)
        {
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
