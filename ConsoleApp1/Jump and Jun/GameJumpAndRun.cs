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
        Dictionary<int, NetworkedPlayer> nPlayers;
        int respawnTime  = 0;


        public override bool isRunning()
        {

            if (myPlayer != null && myPlayer.ToBeDestroyed)
            {
                return false;
            }

            return true;




        }

        private void sendPlayerRemove()
        {
            Client client = Client.GetInstance();

            string message = new Action(client.id, MessageType.PlayerDestroy).ToJson();
            client.Send(message);

        }

        public override void update()
        {
            if (myPlayer != null && myPlayer.ToBeDestroyed && respawnTime==0)
            {
                respawnTime = 1000;
                sendPlayerRemove();
            }


            if (respawnTime>0)
            {
                Debug.Log(respawnTime.ToString());
                Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                Bootstrap.getDisplay().showText("GAME OVER!", 300, 300, 128, col);
                


                if (respawnTime == 1)
                {
                    Client client = Client.GetInstance();
                    (int x, int y) sPos = client.GetRandomStartPosition();
                    setPlayerStart(sPos.x, sPos.y);
                    string message = new Position(client.id, MessageType.PlayerPosition, sPos.x, sPos.y, "right").ToJson();
                    client.Send(message);
                }

                respawnTime--;
                return;
            }


        }

        public void setPlayerStart(double x, double y)
        {
            Debug.Log("called");
            myPlayer = new Player();
            myPlayer.Move(x, y);
        }

        public void MoveNetworkedPlayer(int id, double x, double y, string sprite)
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
            player.setSpriteName(sprite);
        }

        public void removeNetworkedPlayer(int id)
        {
            if (nPlayers.ContainsKey(id))
            {
                NetworkedPlayer player = nPlayers[id];
                player.ToBeDestroyed = true;
                nPlayers.Remove(id);
            }
            
        }
        public void removeNetworkedBullet(int id)
        {
            if (nPlayers.ContainsKey(id))
            {
                NetworkedPlayer player = nPlayers[id];
                player.nBullet.ToBeDestroyed = true;
                player.nBullet = null;
            }

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
