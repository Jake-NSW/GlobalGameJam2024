using UnityEngine;


namespace AmazingAssets.CurvedWorld.Example
{
    public class RunnerChunk : MonoBehaviour
    {
        public ChunkSpawner spawner;
        

        void Update()
        {
            transform.Translate(spawner.moveDirection * spawner.movingSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (transform.position.x < -spawner.destoryZone)
                spawner.DestroyChunk(this);
        }
    }
}