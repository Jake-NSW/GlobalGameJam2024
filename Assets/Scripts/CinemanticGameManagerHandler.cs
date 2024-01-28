using Jam;
using UnityEngine;

public class CinematicGameManagerHandler : MonoBehaviour
{
    public void NextLevel()
    {
        GameManager.Instance.LooseLevel();
    }
}
