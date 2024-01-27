using Jam;
using UnityEngine;


namespace Jam
{
    public class RunnerChunk : MonoBehaviour
    {
        public ChunkSpawner spawner;
        

        void Update()
        {
            transform.Translate(spawner.MoveDirection * spawner.MovingSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (transform.position.x < -spawner.DestroyZone)
                spawner.DestroyChunk(this);
        }
    }
}