using UnityEngine;
using UnityEngine.Serialization;

namespace Jam
{
    public class ChunkSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_chunks;
        public int initialSpawnCount = 15;
        public float DestroyZone = 300;
        
        [HideInInspector] public Vector3 MoveDirection = new Vector3(-1, 0, 0);
        public float MovingSpeed => MovingSpeed * GameManager.Instance.Speed;
        
        public float m_MovingSpeed = 30;
        
        [SerializeField] private float m_chunkSize = 60;        
        private GameObject m_lastChunk;


        void Awake()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            
            foreach(var obj in m_chunks)
            {
                obj.gameObject.SetActive(true);
            }
            
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
        
        
        public void DestroyChunk(RunnerChunk thisChunk)
        {
            Vector3 newPos = m_lastChunk.transform.position;
            
            newPos.x += m_chunkSize;

            m_lastChunk = thisChunk.gameObject;
            m_lastChunk.transform.position = newPos;
        }
    }
}
