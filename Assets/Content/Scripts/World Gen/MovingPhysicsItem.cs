using UnityEngine;
using UnityEngine.Serialization;


namespace Jam
{
    public class MovingPhysicsItem : MonoBehaviour
    {                
        public Vector3 moveDirection = new Vector3(1, 0, 0);    //Set by spawner after instantiating
        public float movingSpeed = 1;   //Set by spawner after instantiating
        public float ExtraYGravity = -100f;
        
        Rigidbody rigidBody;


        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            rigidBody.MovePosition(transform.position + moveDirection * movingSpeed * Time.deltaTime * movingSpeed);
            rigidBody.AddForce(Vector3.up * ExtraYGravity, ForceMode.Force);

            if (transform.position.y < -300)
            {
                Destroy(this.gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody)
            {
                Vector3 force = (Vector3.up * 2 + Random.insideUnitSphere).normalized * Random.Range(100, 150);
                collision.rigidbody.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
