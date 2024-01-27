﻿using UnityEngine;
using UnityEngine.Serialization;


namespace Jam
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_objects;
        [SerializeField] private float m_spawnRate = 0.2f;
        
        [Range(0f, 1f)]
        [SerializeField] private float m_spawnRandomizer = 0.6f;

        [Space(10)]
        public Vector3 positionRandomizer = new Vector3(0, 0, 0);
        public Vector3 rotation = new Vector3(0, 90, 0);


        [Space(10)]
        public Vector3 moveDirection = new Vector3(1, 0, 0);
        public Vector2 movingSpeed = new Vector2(3, 5);
        
        private float m_deltaTime;
        
        void Update()
        {
            m_deltaTime += Time.deltaTime;

            if(m_deltaTime > m_spawnRate)
            {
                m_deltaTime = 0;

                if(Random.value > m_spawnRandomizer)
                {
                    int index = Random.Range(0, m_objects.Length);

                    GameObject carObject = Instantiate(m_objects[index]);
                    carObject.SetActive(true);

                    carObject.transform.position = transform.position + Vector3.Scale(Random.insideUnitSphere, positionRandomizer);
                    carObject.transform.rotation = Quaternion.Euler(rotation);

                    MovingItem objectScipt = carObject.GetComponent<MovingItem>();
                    objectScipt.moveDirection = moveDirection;
                    objectScipt.movingSpeed = Random.Range(movingSpeed.x, movingSpeed.y);
                }
            }
        }
    }
}