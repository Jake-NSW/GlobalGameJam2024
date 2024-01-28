using UnityEngine;

namespace Jam
{
    public sealed class MoveUpOverTime : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 1;

        private void Update()
        {
            transform.position += Vector3.up * (Time.deltaTime * m_Speed);
        }
    }

}
