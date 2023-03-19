// Sprites from https://www.spriters-resource.com/fullview/113060/

using JumpAndRun;
using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Formats.Asn1.AsnWriter;
using System.Xml.Linq;
using System.Numerics;

namespace Shard
{
    class GameDemoSOME : NetworkedGame, InputListener
    {
        public Player myPlayer;
        private double respawnTime = 0;

        public override bool isRunning()
        {
            return true;
        }

        public override void initialize()
        {
            Box b;
            Bootstrap.getInput().addListener(this);
            Bootstrap.getDisplay().setSize(1200, 600);
            Bootstrap.getDisplay().setBackgroundColor(Color.FromArgb(253, 219, 185));

            b = new Box();
            b.setPosition(50, 350, 0, 0);

            b = new Box();
            b.setPosition(100, 375, 0, 0);

            b = new Box();
            b.setPosition(150, 450, 0, 0);

            b = new Box();
            b.setPosition(100, 560, 0, 0);

            b = new Box();
            b.setPosition(150, 590, 0, 0);

            b = new Box();
            b.setPosition(200, 575, 0, 0);

            b = new Box();
            b.setPosition(250, 560, 0, 0);

            b = new Box();
            b.setPosition(350, 590, 0, 0);

            b = new Box();
            b.setPosition(400, 575, 0, 0);

            b = new Box();
            b.setPosition(450, 525, 0, 0);

            b = new Box();
            b.setPosition(700, 525, 0, 0);

            b = new Box();
            b.setPosition(750, 500, 0, 0);

            b = new Box();
            b.setPosition(800, 475, 0, 0);

            b = new Box();
            b.setPosition(900, 425, 0, 0);

            b = new Box();
            b.setPosition(950, 400, 0, 0);

            b = new Box();
            b.setPosition(1000, 375, 0, 0);

            b = new Box();
            b.setPosition(1050, 400, 0, 0);

            b = new Box();
            b.setPosition(1100, 425, 0, 0);

            b = new Box();
            b.setPosition(770, 90, 0, 0);

            b = new Box();
            b.setPosition(750, 140, 0, 0);

            b = new Box();
            b.setPosition(750, 190, 0, 0);

            b = new Box();
            b.setPosition(750, 240, 0, 0);

            b = new Box();
            b.setPosition(800, 260, 0, 0);

            b = new Box();
            b.setPosition(850, 240, 0, 0);

            b = new Box();
            b.setPosition(900, 220, 0, 0);

            b = new Box();
            b.setPosition(950, 240, 0, 0);

            b = new Box();
            b.setPosition(1000, 220, 0, 0);

            b = new Box();
            b.setPosition(1050, 240, 0, 0);

            b = new Box();
            b.setPosition(50, 160, 0, 0);

            b = new Box();
            b.setPosition(100, 140, 0, 0);

            b = new Box();
            b.setPosition(150, 160, 0, 0);

            b = new Box();
            b.setPosition(200, 180, 0, 0);

            b = new Box();
            b.setPosition(300, 190, 0, 0);

            b = new Box();
            b.setPosition(350, 215, 0, 0);

            b = new Box();
            b.setPosition(400, 240, 0, 0);
        }

        public override void update()
        {
            if (myPlayer != null && myPlayer.ToBeDestroyed && respawnTime <= 0)
            {
                respawnTime = 7;
                if (myPlayer.Transform!=null)
                {
                    Random rd = new Random();
                    int type  = rd.Next(0,3);
                    Reward r;
                    if (type == 0)
                    {
                        r = new Boot();
                        r.Transform.X = myPlayer.Transform.X;
                        r.Transform.Y = myPlayer.Transform.Y;
                    }
                    if (type == 1)
                    {
                        r = new Gun();
                        r.Transform.X = myPlayer.Transform.X;
                        r.Transform.Y = myPlayer.Transform.Y;
                    }
                    if (type == 2)
                    {
                        r = new Spring();
                        r.Transform.X = myPlayer.Transform.X;
                        r.Transform.Y = myPlayer.Transform.Y;
                    }

                }
                
            }
           

            if (respawnTime > 0)
            {

                Color col = Color.Black;

                string gameover = "Game over!";
                string kills = "Kills: " + myPlayer.killCounter + ", Items: " + myPlayer.rewardCounter;
                string rewards = "Items: " + myPlayer.rewardCounter;
                string respawn = "Respawn in: " + (((int)respawnTime) + 1);
                string score = "Total Score: " + (myPlayer.killCounter * 2 + myPlayer.rewardCounter);

                Bootstrap.getDisplay().showText(gameover, 10, 10, 40, col);
                Bootstrap.getDisplay().showText(kills, 11, 52, 20, col);
                Bootstrap.getDisplay().showText(score, 11, 75, 20, col);
                Bootstrap.getDisplay().showText(respawn, 950, 10, 40, col);

                respawnTime = respawnTime - Bootstrap.getDeltaTime();

                if (respawnTime <= 0)
                {
                    NetworkClient client = NetworkClient.GetInstance();
                    (int x, int y) sPos = client.GetRandomStartPosition();
                    string oldColor = myPlayer.spriteColor;
                    setPlayerStart(sPos.x, sPos.y);
                    setPlayerColor(oldColor);
                }

            }


        }

        public void setPlayerStart(double x, double y)
        {
            myPlayer = new Player();
            myPlayer.Move(x, y);
        }

        public void setPlayerColor(string color)
        {
            myPlayer.spriteColor = color;
        }

        public void handleInput(InputEvent inp, string eventType)
        {
        }

    }
}
