// Sprites from https://www.spriters-resource.com/fullview/113060/

using JumpAndRun;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shard
{
    class GameJumpAndRun : Game, InputListener
    {
        Random rand;
        Player player;
        Mate mate;
        List<Enemy> enemies;

        public override bool isRunning() {       

            if (player == null || player.ToBeDestroyed) {
                return false;
            }

            return true;

            foreach (Enemy c in enemies)
            {
                if (c != null && !c.ToBeDestroyed)
                {
                    return true;
                }
            }

            return false;

        }

        public override void update()
        {

            if (isRunning() == false)
            {
                Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                Bootstrap.getDisplay().showText("GAME OVER!", 300, 300, 128, col);
                return;
            }


        }

        public override void initialize()
        {
            Box p;
            Bootstrap.getInput().addListener(this);
            rand = new Random();
        
            player = new Player();
            mate = new Mate();
            Client client = Client.GetInstance();
            client.setMate(mate);
            enemies = new List<Enemy>();
           

            p = new Box();
            p.setPosition(0, 350, 600, 200);

            p = new Box();
            p.setPosition(200, 350, 600, 200);


            Enemy e = new Enemy();
            e.Transform.translate(50, 50);
            enemies.Add(e);
            client.setEnemy(e);

        }


        public void handleInput(InputEvent inp, string eventType)
        {            
        }
    }
}
