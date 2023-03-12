using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;
using JumpAndRun;
using System.Reflection;
using System.Collections;
using System.Xml.Linq;

namespace Shard
{


    class NetworkManager
    {

        NetworkedGame game;
        int updateCounter;
        Dictionary<NetworkedObject, NetworkedObjectState> outgoingStates;

        Dictionary<int, NetworkedObject> incomingObjects;
        public NetworkManager(NetworkedGame game)
        {

            this.game = game;
            updateCounter = 0;
            outgoingStates = new Dictionary<NetworkedObject, NetworkedObjectState>();
            incomingObjects = new Dictionary<int, NetworkedObject>();
        }

        private void sendObjectPosition(NetworkedObject obj)
        {
            Debug.Log("Send object position");
            NetworkClient client = NetworkClient.GetInstance();
            Position position = new Position(client.id, MessageType.Position, obj.GetType().ToString(), obj.id, obj.Transform.X, obj.Transform.Y, obj.getFullSpriteName());
            client.Send(position.ToJson());
        }

        private void sendObjectDestroyed(NetworkedObject obj)
        {
            NetworkClient client = NetworkClient.GetInstance();
            Destroy destroy = new Destroy(client.id, MessageType.Destroy, obj.id);
            client.Send(destroy.ToJson());
        }

        public void handleObjectPosition(Position pos)
        {
            if (!incomingObjects.ContainsKey(pos.objectId))
            {
                Debug.Log(pos.objectType);
                Type t = Type.GetType(pos.objectType);
                object[] constructorArgs = new object[] { true };
                NetworkedObject ob = (NetworkedObject)Activator.CreateInstance(t, constructorArgs);
                ob.syncedInitialize();
                incomingObjects.Add(pos.objectId, ob);
            }
            NetworkedObject obj = incomingObjects[pos.objectId];

            if (obj.Transform != null)
            {
                obj.Transform.SpritePath = "ManicMinerSprites/" + pos.sprite + ".png";
                obj.Transform.X = pos.x;
                obj.Transform.Y = pos.y;
            }
        }

        public void handleObjectDestroy(Destroy d)
        {
            Debug.Log("handle destroy");
            if (incomingObjects.ContainsKey(d.objectId))
            {
                NetworkedObject obj = incomingObjects[d.objectId];
                obj.ToBeDestroyed = true;
                incomingObjects.Remove(d.objectId);
            }
        }


        public void update()
        {
            List<GameObject> objects = GameObjectManager.getInstance().getAllGameObjects();

            List<NetworkedObject> foundObjects = new List<NetworkedObject>();

            foreach (GameObject obj in objects.ToList())
            {
                //if (obj.ToBeDestroyed)
                //{
                //    Debug.Log("found destroy");

                   
                //}
                if (obj is NetworkedObject)
                {
                    NetworkedObject nObj = (NetworkedObject)obj;
                    if (!nObj.isSynced())
                    {

                        if (updateCounter % 50 == 0)
                        {
                            if (!outgoingStates.ContainsKey(nObj))
                            {
                                outgoingStates.Add(nObj, new NetworkedObjectState());
                            }
                            NetworkedObjectState state = outgoingStates[nObj];
                            double newX = nObj.Transform.X;
                            double newY = nObj.Transform.Y;
                            double oldX = state.x;
                            double oldY = state.y;
                            if (oldX != newX || oldY != newY)
                            {
                                sendObjectPosition(nObj);
                            }
                            state.x = newX;
                            state.y = newY;
                            
                        }
                        foundObjects.Add(nObj);

                        //if (nObj.ToBeDestroyed)
                        //{
                        //    Debug.Log("found destroy");

                        //    sendObjectDestroyed(nObj);
                        //}
                    }

                }
            }

            Dictionary<NetworkedObject, NetworkedObjectState> copiedDict = new Dictionary<NetworkedObject, NetworkedObjectState>(outgoingStates);

            foreach (KeyValuePair<NetworkedObject, NetworkedObjectState> kvp in copiedDict)
            {
                if (!foundObjects.Contains(kvp.Key))
                {
                    Debug.Log("remove this");
                    sendObjectDestroyed(kvp.Key);
                    outgoingStates.Remove(kvp.Key);
                }
            }

            updateCounter++;

        }

    };

    class NetworkedObjectState
    {
        public double x, y;
    }



}
