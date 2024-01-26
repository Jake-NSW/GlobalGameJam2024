using UnityEngine;

namespace Jam
{
    public sealed class MovementController : MonoBehaviour
    {
        [SerializeField] private float m_Acceleration = 10;
        [SerializeField] private float m_MaxSpeed = 3;
        [SerializeField] private float m_Fart = 25;
        [SerializeField] private float m_Drag = 5;

        private Vector3 m_Velocity;

        private void Update()
        {
            var wishDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                wishDir += Vector3.forward;

            if (Input.GetKey(KeyCode.S))
                wishDir += Vector3.back;

            if (Input.GetKey(KeyCode.A))
                wishDir += Vector3.left;

            if (Input.GetKey(KeyCode.D))
                wishDir += Vector3.right;

            if (Input.GetKey(KeyCode.Space))
            {
                m_Velocity.y += m_Fart * Time.deltaTime;
            }


            wishDir.Normalize();

            var wishMove = m_Velocity.WithY(0);

            // calculate wish velocity based on speed and direction
            wishMove += wishDir * (Time.deltaTime * m_Acceleration);

            // Cap wish velocity to max speed
            if (wishMove.magnitude > m_MaxSpeed)
                wishMove = wishMove.normalized * m_MaxSpeed;

            // Add wish velocity to current velocity, by using delta
            m_Velocity += wishMove.WithY(0) - m_Velocity.WithY(0);

            // Apply Gravity
            m_Velocity += Vector3.up * (Physics.gravity.y * Time.deltaTime);
            
            var t = transform;
            var position = t.position;

            t.position = Vector3.MoveTowards(position, position + m_Velocity, m_Velocity.magnitude * Time.deltaTime);
            if (t.position.y < 0)
            {
                t.position = t.position.WithY(0);
                m_Velocity = m_Velocity.WithY(0);
            }

            m_Velocity -= m_Velocity.WithY(0) * (Time.deltaTime * m_Drag);
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 1.1f);
        }
    }
}
