using System;
using UnityEngine;

namespace Jam
{
    public sealed class MovementController : MonoBehaviour
    {
        private Vector3 m_Velocity;

        private void Update()
        {
            var wish = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                wish += Vector3.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                wish += Vector3.back;
            }

            if (Input.GetKey(KeyCode.A))
            {
                wish += Vector3.left;
            }

            if (Input.GetKey(KeyCode.D))
            {
                wish += Vector3.right;
            }

            wish.Normalize();

            m_Velocity += wish * (Time.deltaTime * 10);
            if (m_Velocity.magnitude > 3)
            {
                m_Velocity = m_Velocity.normalized * 3;
            }

            var t = transform;
            var position = t.position;
            t.position = Vector3.MoveTowards(position, position + m_Velocity, m_Velocity.magnitude * Time.deltaTime);

            m_Velocity -= m_Velocity * (Time.deltaTime * 5);
        }
    }
}
