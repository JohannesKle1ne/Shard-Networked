using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;
using JumpAndRun;

namespace Shard
{


    class NetworkManager
    {

        NetworkedGame game;
        private int updateCounter;
        public NetworkManager(NetworkedGame game)
        {

            this.game = game;
            updateCounter = 0;
        }

        public void update()
        {
            if (updateCounter % 20 == 0)
            {
                List<GameObject> objects = GameObjectManager.getInstance().getAllGameObjects();
                foreach (GameObject obj in objects.ToList())
                {
                    Debug.Log(obj.ToString());
                }
            }

            updateCounter++;

        }

    };



}
