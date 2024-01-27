using UnityEngine;
using UnityEngine.Serialization;

namespace Jam
{
    public class ChunkSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_chunks;
        [SerializeField] private GameObject m_endChunk;
        public int initialSpawnCount = 15;
        public float DestroyZone = 300;
        
        [HideInInspector] public Vector3 MoveDirection = new Vector3(-1, 0, 0);
        public float MovingSpeed => m_MovingSpeed * GameManager.Instance.Speed;
        
        public float m_MovingSpeed = 30;
        
        [SerializeField] private float m_chunkSize = 60;        
        private GameObject m_lastChunk;

        private bool m_hasSpawnedFinalChunk = false;
        

        void Awake()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            
            foreach(var obj in m_chunks)
            {
                obj.gameObject.SetActive(true);
            }

            m_endChunk.SetActive(false);
            
            initialSpawnCount = initialSpawnCount > m_chunks.Length ? initialSpawnCount : m_chunks.Length;

            int chunkIndex = 0;
            for (int i = 0; i < initialSpawnCount; i++)
            {
                GameObject chunk = Instantiate(m_chunks[chunkIndex]);
                
                chunk.SetActive(true);

                chunk.GetComponent<RunnerChunk>().spawner = this;

                chunk.transform.localPosition = new Vector3(i * m_chunkSize, -2, transform.position.z);
                MoveDirection = new Vector3(-1, 0, 0);

                m_lastChunk = chunk;

                if (++chunkIndex >= m_chunks.Length)
                    chunkIndex = 0;
            }           
        }
        
        
        public void ResetChunk(RunnerChunk thisChunk)
        {
            MoveDirection = new Vector3(-1, 0, 0);
            
            Vector3 newPos = m_lastChunk.transform.position;
            
            newPos.x += m_chunkSize;

            m_lastChunk = thisChunk.gameObject;
            m_lastChunk.transform.position = newPos;
            
            
            if (GameManager.Instance.Remaining < 5 && !m_hasSpawnedFinalChunk)
            {
                Debug.Log("Spawn Final Chunk");
                m_hasSpawnedFinalChunk = true;
                
                GameObject chunk = chunk = Instantiate(m_endChunk);
                
                chunk.SetActive(true);

                chunk.GetComponent<RunnerChunk>().spawner = this;

                chunk.transform.localPosition = new Vector3(thisChunk.transform.position.x, -2, transform.position.z);

                m_lastChunk = chunk;
                Destroy(thisChunk);
            }
        }
    }
}
