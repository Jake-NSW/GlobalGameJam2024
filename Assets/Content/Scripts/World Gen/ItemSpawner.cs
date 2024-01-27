using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazingAssets.CurvedWorld;
using AmazingAssets.CurvedWorld.Example;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Jam
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private ChunkSpawner m_chunkSpawner;

        [SerializeField] private CurvedWorldController m_curvedWorldController;

        [SerializeField]
        private PickupType m_pickupType;
        
        [SerializeField] private GameObject[] m_objects;
        [SerializeField] private float m_SpawnFrequencyInSeconds = 0.5f;
        
        [Range(0f, 1f)]
        [SerializeField] private float m_spawnFailureRate = 0.6f;

        [SerializeField] private float m_howManyToSpawnAtATime = 3;
        [SerializeField] private bool m_autoRotateModel = false;

        [Space(10)]
        public Vector3 StartRotation = new Vector3(0, 90, 0);

        [SerializeField] private float m_firstSpawnHeight = 2f;
        
        [SerializeField] private float m_chanceOfSecondHeight = 0.5f;
        [SerializeField] private float m_secondSpawnHeight = 2;
        
        private float m_deltaTime;
        
        [SerializeField] private int[] m_laneDistances = new int[] {-11, -4, 4, 11};

        private void Awake()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            
            foreach(var obj in m_objects)
            {
                obj.gameObject.SetActive(true);
                AddBoxCollider(obj);
            }
        }

        private void AddBoxCollider(GameObject obj)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogError("MeshFilter not found on the GameObject. BoxCollider cannot be adjusted.");
                return;
            }

            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                Debug.LogError("Mesh not found on the MeshFilter. BoxCollider cannot be adjusted.");
                return;
            }

            BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
            boxCollider.center = mesh.bounds.center;
            boxCollider.size = mesh.bounds.size;
        }

        void Update()
        {
            m_deltaTime += Time.deltaTime;

            if (m_deltaTime > m_SpawnFrequencyInSeconds)
            {
                m_deltaTime = 0;

                if (Random.value > m_spawnFailureRate)
                {
                    SpawnObjectsInSequence();
                }
            }
        }
        
        private async void SpawnObjectsInSequence()
        {
            int zPosition = GetRandomSpecificNumber(); // Cache the Z position for all spawns
            
            float selectedHeight;
            
            selectedHeight = Random.value < m_chanceOfSecondHeight ? m_secondSpawnHeight : m_firstSpawnHeight;

            for (int i = 0; i < m_howManyToSpawnAtATime; i++)
            {
                int index = Random.Range(0, m_objects.Length);
                GameObject item = Instantiate(m_objects[index]);
                item.SetActive(true);

                // Use the cached Z position
                item.transform.position = new Vector3(transform.position.x,selectedHeight ,transform.position.z) + new Vector3(0, 0, zPosition); 
                item.transform.rotation = Quaternion.Euler(StartRotation);
                
                var movingItem = item.AddComponent<MovingItem>();
                var pickup = item.AddComponent<Pickup>();
                pickup.Type = m_pickupType;
                var disablecurvedWorld = item.AddComponent<DisableCurvedWorld>();
                disablecurvedWorld.curvedWorldController = m_curvedWorldController;
                disablecurvedWorld.zMin = -11;
                disablecurvedWorld.zMax = 11;

                if (m_autoRotateModel)
                {
                    item.AddComponent<AutoRotate>();
                }
                
                movingItem.spawner = m_chunkSpawner;

                await Task.Delay(TimeSpan.FromSeconds(0.1f));

                if (!Application.isPlaying) return;
            }
        }
        
        private int GetRandomSpecificNumber()
        {
            int index = Random.Range(0, m_laneDistances.Length);
            return m_laneDistances[index];
        }
    }
}