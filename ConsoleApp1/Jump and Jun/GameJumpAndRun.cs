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
    class GameJumpAndRun : Game, InputListener
    {
        Random rand;
        Player myPlayer;
        NetworkedBullet myBullet;
        Dictionary<int, NetworkedPlayer> nPlayers;

        public override bool isRunning()
        {

            if (myPlayer != null && myPlayer.ToBeDestroyed)
            {
                return false;
            }

            return true;




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

        public void setPlayerStart(double x, double y)
        {
            Debug.Log("called");
            myPlayer = new Player();
            myPlayer.Move(x, y);
        }

        public void MovePlayer(int id, double x, double y)
        {
            NetworkedPlayer player;
            if (nPlayers.ContainsKey(id))
            {
                player = nPlayers[id];
            }
            else
            {
                player = new NetworkedPlayer(id);
                nPlayers.Add(id, player);
            }
            player.Move(x, y);
        }

        public void MoveBullet(int id, double x, double y)
        {
            if (nPlayers.ContainsKey(id))
            {
                NetworkedPlayer player = nPlayers[id];
                player.MoveBullet(x, y);
            }
        }

        public override void initialize()
        {
            Box p;
            Bootstrap.getInput().addListener(this);
            rand = new Random();

            
            myBullet = new NetworkedBullet();
            Client client = Client.GetInstance();
            client.setGame(this);
            nPlayers = new Dictionary<int, NetworkedPlayer>();


            p = new Box();
            p.setPosition(0, 350, 600, 200);

            p = new Box();
            p.setPosition(200, 350, 600, 200);

        }


        public void handleInput(InputEvent inp, string eventType)
        {
        }
    }
}
