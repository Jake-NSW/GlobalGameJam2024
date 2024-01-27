using System;
using UnityEngine;

namespace Jam
{
    public sealed class MovementController : MonoBehaviour
    {
        /// <summary>
        /// Is the movement controller farting? 
        /// </summary>
        public bool IsFarting => m_IsFarting;

        [SerializeField] private float m_Acceleration = 10;
        [SerializeField] private float m_MaxSpeed = 3;
        [SerializeField] private float m_Drag = 5;

        [SerializeField] private float m_Fart = 25;

        [SerializeField] private float m_FartRegeneration;
        [SerializeField] private float m_FartRegenerationDelay;

        [SerializeField] private float m_FartDepletion;

        [SerializeField] private float m_MaxFartCapacity = 0.6f;

        private Vector3 m_Velocity;

        private bool m_IsFarting;
        private float m_FartCapacity;
        private TimeSince m_SinceLastFart;

        private void Start()
        {
            // Make sure we can fart straight away
            m_SinceLastFart = 50;
        }

        private void OnGUI()
        {
            GUILayout.Label("Fart Capacity: " + m_FartCapacity);
            GUILayout.Label("Farting: " + m_IsFarting);
            GUILayout.Label("Since Last Fart: " + m_SinceLastFart);
            GUILayout.Label("Velocity: " + m_Velocity);
        }

        private void HandleFartPower()
        {
            // Fart Power
            if (Input.GetKey(KeyCode.Space))
            {
                // We only want to start farting if we got the power to do so 
                if (m_FartCapacity <= 0)
                    return;

                // start fart
                if (!m_IsFarting)
                {
                    m_SinceLastFart = 0;
                    m_IsFarting = true;
                }

                // use velocity to go up
                m_Velocity += Vector3.up * (m_Fart * Time.deltaTime);
                m_FartCapacity -= Time.deltaTime * m_FartDepletion;

                // To much fart, we cant fart anymore
                if (m_FartCapacity > m_MaxFartCapacity)
                {
                    m_IsFarting = false;
                    m_SinceLastFart = 0;
                }
            }
            else
            {
                if (m_IsFarting)
                    m_SinceLastFart = 0;

                // regen fart after time
                if (m_SinceLastFart > m_FartRegenerationDelay)
                    m_FartCapacity += Time.deltaTime * m_FartRegeneration;

                m_IsFarting = false;
            }

            // Clamp to max fart
            m_FartCapacity = Math.Min(m_MaxFartCapacity, m_FartCapacity);
        }

        private void Update()
        {
            var wishDir = Vector3.zero;

            if (Input.GetKey(KeyCode.A))
                wishDir += Vector3.left;

            if (Input.GetKey(KeyCode.D))
                wishDir += Vector3.right;

            wishDir.Normalize();

            HandleFartPower();

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

            // cant go left or right by an extent
            if (t.position.x < -6)
            {
                t.position = t.position.WithX(-6);
                
                var velocity = -(m_Velocity.x / 1.5f);
                m_Velocity.x = velocity;
            }
            else if (t.position.x > 6)
            {
                t.position = t.position.WithX(6);
                
                var velocity = -(m_Velocity.x / 1.5f);
                m_Velocity.x = velocity;
            }

            m_Velocity -= m_Velocity.WithY(0) * (Time.deltaTime * m_Drag);
        }

        private void LateUpdate()
        {
            // Get direction of velocity
            var dir = m_Velocity.normalized;
            dir += Vector3.forward;

            // Rotate to direction
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
