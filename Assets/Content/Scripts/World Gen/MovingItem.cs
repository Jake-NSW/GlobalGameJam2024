using UnityEngine;
using UnityEngine.Serialization;


namespace Jam
{
    public class MovingItem : MonoBehaviour
    {                
        public float ExtraYGravity = -100f;
        public ChunkSpawner spawner;
        

        void Update()
        {
            transform.Translate(new Vector3(0, 0, -1) * spawner.movingSpeed * Time.deltaTime);

            if (transform.position.y < -300)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
