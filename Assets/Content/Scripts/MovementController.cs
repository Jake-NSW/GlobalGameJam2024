using UnityEngine;

namespace Jam
{
    public sealed class MovementController : MonoBehaviour
    {
        /// <summary>
        /// Is the movement controller farting? 
        /// </summary>
        public bool IsFarting => m_IsFarting;

        /// <summary>
        /// How much fart have we done so far (normalised)
        /// </summary>
        public float FartNormal => Mathf.Clamp01(m_SinceFartStart / m_MaxFartDuration);

        /// <summary>
        /// How long do we have to wait to fart again
        /// </summary>
        public float FartWait => Mathf.Clamp01(m_SinceLastFart / m_PostFartWait);

        [SerializeField] private float m_Acceleration = 10;
        [SerializeField] private float m_MaxSpeed = 3;
        [SerializeField] private float m_Drag = 5;

        [SerializeField] private float m_Fart = 25;

        [SerializeField] private float m_MaxFartDuration = 0.6f;
        [SerializeField] private float m_PostFartWait = 5;

        private Vector3 m_Velocity;

        private bool m_IsFarting;

        private TimeSince m_SinceFartStart;
        private TimeSince m_SinceLastFart;

        private void Start()
        {
            // Make sure we can fart straight away
            m_SinceLastFart = 50;
        }

        private void HandleFartPower()
        {
            // Fart Power
            if (Input.GetKey(KeyCode.Space))
            {
                // We only want to start farting after two seconds
                if (!m_IsFarting && m_SinceLastFart < m_PostFartWait)
                    return;

                // start fart
                if (!m_IsFarting)
                {
                    m_SinceFartStart = 0;
                    m_SinceLastFart = 0;
                    m_IsFarting = true;
                }

                m_Velocity += Vector3.up * (m_Fart * Time.deltaTime);

                if (m_SinceFartStart > m_MaxFartDuration)
                {
                    m_IsFarting = false;
                    m_SinceLastFart = 0;
                }
            }
            else
            {
                if (m_IsFarting)
                    m_SinceLastFart = 0;

                m_IsFarting = false;
            }
        }

        private void Update()
        {
            var wishDir = Vector3.zero;

            // Always going forward
            wishDir += Vector3.forward;

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

            m_Velocity -= m_Velocity.WithY(0) * (Time.deltaTime * m_Drag);
        }
    }
}
