using AmazingAssets.CurvedWorld;
using UnityEngine;

namespace Jam
{
    [RequireComponent(typeof(CurvedWorldController))]
    public sealed class AnimatedCurveController : MonoBehaviour
    {
        [SerializeField] private CurvedWorldController m_CurvedWorld;

        private void Update()
        {
            m_CurvedWorld.bendHorizontalSize = Mathf.Sin(Time.time) * 2;
            m_CurvedWorld.bendVerticalSize = Mathf.Cos(Time.time) * 0.5f;
            m_CurvedWorld.bendCurvatureSize = (Mathf.Sin(Time.time / 2) * 0.5f + 1) * 3;
            m_CurvedWorld.bendRotationAxis = new Vector3(Mathf.Sin(Time.time / 2), 0, 0.1f);

            // m_CurvedWorld.bendVerticalOffset = Mathf.Sin(Time.time) * 0.5f;
            // m_CurvedWorld.bendHorizontalOffset = Mathf.Cos(Time.time) * 0.5f;
        }
    }
}
