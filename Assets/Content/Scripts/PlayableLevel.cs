using UnityEngine;

namespace Jam
{
    // Execute after everything, so we can be sure that GameManager is initialized

    [DefaultExecutionOrder(500)]
    public sealed class PlayableLevel : MonoBehaviour
    {
        private void Start()
        {
            // Put sequence before this? 
            GameManager.Instance.StartPlaying();
        }
    }
}
