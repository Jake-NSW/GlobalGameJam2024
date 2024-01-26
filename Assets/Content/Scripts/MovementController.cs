using System;
using UnityEngine;

namespace Jam
{
    public sealed class MovementController : MonoBehaviour
    {
        [SerializeField] private float m_Acceleration = 10;
        [SerializeField] private float m_MaxSpeed = 3;
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

            wishDir.Normalize();

            m_Velocity += wishDir * (Time.deltaTime * m_Acceleration);
            if (m_Velocity.magnitude > m_MaxSpeed)
            {
                m_Velocity = m_Velocity.normalized * m_MaxSpeed;
            }

            var t = transform;
            var position = t.position;

            t.position = Vector3.MoveTowards(position, position + m_Velocity, m_Velocity.magnitude * Time.deltaTime);
            m_Velocity = Vector3.MoveTowards(m_Velocity, Vector3.zero, Time.deltaTime * m_Drag);
        }
    }
}
