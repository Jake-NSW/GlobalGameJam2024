using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UICinematicPanel : MonoBehaviour
{
    [field:SerializeField] public float PanelLifeTime { get; private set; }= 3f;
    [field: SerializeField] public float TextTimeDelay { get; private set; } = 1f;
    [field:SerializeField] public RectTransform TextContainer { get; private set; }
    [field:SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        if(TextTimeDelay > PanelLifeTime)
            Debug.LogError("TextTimeDelay is greater than PanelLifeTime - very naughty",this);
    }
}


