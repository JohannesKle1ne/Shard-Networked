/*
*
*   Any game object that is going to react to collisions will need to implement this interface.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;

namespace Shard
{
    abstract class NetworkedObject : GameObject
    {

        public int id;

        public bool RemoteDestroy;

        public bool synced;

        public abstract string getFullSpriteName();

        public abstract bool isSynced();
        public abstract void syncedInitialize();

        public abstract void localInitialize();

        public virtual void syncedUpdate()
        {

        }
        public virtual void localUpdate()
        {

        }

        public void setId(int id)
        {
            this.id = id;
        }

        public override void update()
        {
            if (synced)
            {
                syncedUpdate();
            }
            else
            {
                localUpdate();
            }
        }


        public NetworkedObject()
        {
            Random rd = new Random();
            id = rd.Next(0, 10000);

            RemoteDestroy = false;

        }
    }
}
