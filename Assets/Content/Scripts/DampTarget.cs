using UnityEngine;

namespace Jam
{
    public sealed class DampTarget : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;
        [SerializeField] private Vector3 m_Offset;

        [SerializeField] private float m_DampTime = 0.15f;

        private Vector3 m_Velocity = Vector3.zero;

        private void Update()
        {
            var t = transform;
            var position = t.position;
            var targetPosition = m_Target.position + m_Offset;
            t.position = Vector3.SmoothDamp(position, targetPosition, ref m_Velocity, m_DampTime);
        }
    }
}
