using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Jam
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private ChunkSpawner m_chunkSpawner;
        
        [SerializeField] private GameObject[] m_objects;
        [SerializeField] private float m_spawnRate = 0.2f;
        [SerializeField] private bool m_SpawnPhysicsItems = false;
        
        [Range(0f, 1f)]
        [SerializeField] private float m_spawnRandomizer = 0.6f;

        [Space(10)]
        //public Vector3 positionRandomizer = new Vector3(0, 0, 0);
        public Vector3 rotation = new Vector3(0, 90, 0);


        [Space(10)]
        public Vector3 moveDirection = new Vector3(1, 0, 0);
        public Vector2 movingSpeed = new Vector2(3, 5);
        
        private float m_deltaTime;
        
        void Update()
        {
            m_deltaTime += Time.deltaTime;

            if (m_deltaTime > m_spawnRate)
            {
                m_deltaTime = 0;

                if (Random.value > m_spawnRandomizer)
                {
                    SpawnObjectsInSequence();
                }
            }
        }
        
        private async void SpawnObjectsInSequence()
        {
            int zPosition = GetRandomSpecificNumber(); // Cache the Z position for all spawns

            for (int i = 0; i < 3; i++) // Spawn 3 objects
            {
                int index = Random.Range(0, m_objects.Length);
                GameObject item = Instantiate(m_objects[index]);
                item.SetActive(true);

                // Use the cached Z position
                item.transform.position = transform.position + new Vector3(0, 0, zPosition); 
                item.transform.rotation = Quaternion.Euler(rotation);

                if (m_SpawnPhysicsItems)
                {
                    var movingItem = item.AddComponent<MovingPhysicsItem>();
                    movingItem.moveDirection = moveDirection;
                    movingItem.movingSpeed = Random.Range(movingSpeed.x, movingSpeed.y);
                }
                else
                {
                    var movingItem = item.AddComponent<MovingItem>();
                    movingItem.spawner = m_chunkSpawner;
                }

                await Task.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }
        
        int GetRandomSpecificNumber()
        {
            int[] numbers = new int[] {-10, -5, 5, 10};
            int index = Random.Range(0, numbers.Length);
            return numbers[index];
        }
    }
}