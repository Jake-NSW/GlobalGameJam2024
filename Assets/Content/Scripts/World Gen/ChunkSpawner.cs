using UnityEngine;


namespace AmazingAssets.CurvedWorld.Example
{
    public class ChunkSpawner : MonoBehaviour
    {
        public enum AXIS { XPositive, XNegative, ZPositive, ZNegative }

        public GameObject[] chunks;
        public int initialSpawnCount = 5;
        public float destoryZone = 300;
        

        [HideInInspector]
        public Vector3 moveDirection = new Vector3(-1, 0, 0);
        public float movingSpeed = 1;


        public float chunkSize = 60;        
        GameObject lastChunk;


        void Awake()
        {
            initialSpawnCount = initialSpawnCount > chunks.Length ? initialSpawnCount : chunks.Length;

            int chunkIndex = 0;
            for (int i = 0; i < initialSpawnCount; i++)
            {
                GameObject chunk = Instantiate(chunks[chunkIndex]);
                chunk.SetActive(true);

                chunk.GetComponent<RunnerChunk>().spawner = this;

                chunk.transform.localPosition = new Vector3(i * chunkSize, 0, transform.position.z);
                moveDirection = new Vector3(-1, 0, 0);
                

                lastChunk = chunk;

                if (++chunkIndex >= chunks.Length)
                    chunkIndex = 0;
            }           
        }
        
        public void DestroyChunk(RunnerChunk thisChunk)
        {
            Vector3 newPos = lastChunk.transform.position;
            
            newPos.x += chunkSize;

            lastChunk = thisChunk.gameObject;
            lastChunk.transform.position = newPos;
        }
    }
}
