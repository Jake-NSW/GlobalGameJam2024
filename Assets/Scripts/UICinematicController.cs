using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UICinematicController : MonoBehaviour
{
    [SerializeField] private float storyStartDelay = 1f;
    [SerializeField] private float fadeTime = 0.3f;
    [SerializeField] private float textSlideTime = 0.3f;

    
    public List<UICinematicPanel> panels;
    public UnityEvent onCinematicFinished;

    private int _currentPanelIndex = 0;

    private void Start()
    {
        InitializePanels();
        StartCoroutine(StartCinematicAfterDelay());
    }

    private IEnumerator StartCinematicAfterDelay()
    {
        yield return new WaitForSeconds(storyStartDelay);
        StartCinematic();
    }

    public void StartCinematic()
    {
        StopAllCoroutines();
        _currentPanelIndex = 0;
        InitializePanels();
        if (panels.Count == 0)
        {
            onCinematicFinished.Invoke();
        }
        else
        {
            StartCoroutine(PlayPanelSequence());
        }
    }

    private void InitializePanels()
    {
        foreach (var panel in panels)
        {
            panel.CanvasGroup.alpha = 0;
            panel.TextContainer.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }
    }

    IEnumerator PlayPanelSequence()
    {
        while (_currentPanelIndex < panels.Count)
        {
            var panel = panels[_currentPanelIndex];
            panel.gameObject.SetActive(true);
            yield return StartCoroutine(FadeInPanel(panel.CanvasGroup, panel.TextContainer));
            yield return new WaitForSeconds(panel.PanelLifeTime);
            yield return StartCoroutine(FadeOutPanel(panel.CanvasGroup));
            panel.gameObject.SetActive(false);

            _currentPanelIndex++;
            if (_currentPanelIndex >= panels.Count)
            {
                onCinematicFinished.Invoke();
            }
        }
    }

    IEnumerator FadeInPanel(CanvasGroup canvasGroup, RectTransform textContainer)
    {
        float time = 0;
        while (time < fadeTime)
        {
            canvasGroup.alpha = time / fadeTime;
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(panels[_currentPanelIndex].TextTimeDelay);
        StartCoroutine(SlideInText(textContainer));
    }

    IEnumerator FadeOutPanel(CanvasGroup canvasGroup)
    {
        float time = 0;
        while (time < fadeTime)
        {
            canvasGroup.alpha = 1 - (time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    IEnumerator SlideInText(RectTransform textContainer)
    {
        textContainer.gameObject.SetActive(true);
        var endPosition = textContainer.localPosition; // Original position set in the editor
        var startPosition = endPosition - (textContainer.rect.width * Vector3.right); // Off-screen start position

        float time = 0;
        while (time < textSlideTime)
        {
            textContainer.localPosition = Vector3.Lerp(startPosition, endPosition, time / textSlideTime);
            time += Time.deltaTime;
            yield return null;
        }
        textContainer.localPosition = endPosition;
    }


    public void SkipToNextPanel()
    {
        if (_currentPanelIndex < panels.Count)
        {
            StopAllCoroutines();
            panels[_currentPanelIndex].gameObject.SetActive(false);
            if (_currentPanelIndex < panels.Count - 1)
            {
                _currentPanelIndex++;
            }
            else
            {
                onCinematicFinished.Invoke();
                return;
            }
            StartCoroutine(PlayPanelSequence());
        }
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(UICinematicController))]
public class UICinematicControllerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var cinematicController = (UICinematicController)target;
        
        if (GUILayout.Button("Play Cinematic"))
        {
            cinematicController.StartCinematic();
        }

        if (GUILayout.Button("Skip To Next Panel"))
        {
            cinematicController.SkipToNextPanel();
        }
    }
}
#endif
