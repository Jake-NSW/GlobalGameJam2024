using UnityEngine;

namespace Jam
{
    public sealed class LookAtTarget : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;
        [SerializeField] private Vector3 m_Offset;

        private void LateUpdate()
        {
            var t = transform;
            var targetPosition = m_Target.position + m_Offset;
            t.LookAt(targetPosition);
        }
    }
}
