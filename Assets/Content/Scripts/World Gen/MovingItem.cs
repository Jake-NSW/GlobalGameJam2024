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
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(-1, 0, 0), spawner.MovingSpeed * Time.deltaTime);

            if (transform.position.y < -300)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
