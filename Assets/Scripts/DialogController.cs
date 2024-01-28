using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Image m_image;

    [SerializeField] private CanvasGroup m_canvas;
    
    [SerializeField] private float m_FadeTime = 0.4f;
    
    private void Awake()
    {
        m_canvas.alpha = 0;
        m_text.text = "";
    }

    public async void Start()
    {
        await Task.Delay(TimeSpan.FromSeconds(4));
        StartCoroutine(ShowDialog());
        
        await Task.Delay(TimeSpan.FromSeconds(1.5f));
        PickDialog();
    }

    private void PickDialog()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Level1":
                FillDialog("I dreamed this would happen last night. Maybe I'm psychic.");
                break;
            case "Level2":
                FillDialog("Why do I never learn...");
                break;
            case "Level3":
                FillDialog("Time and space will bend to my will.      All roads lead to the golden dunny");
                break;
        }
    }


    private async void FillDialog(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            m_text.text += letter;
            await Task.Delay(TimeSpan.FromSeconds(0.05f));
        }
        
        await Task.Delay(TimeSpan.FromSeconds(2f));

        if (!Application.isPlaying) return;
        
        StartCoroutine(HideDialog());
    }
    
    // Check if the text is overflowing
    private bool IsTextOverflowing()
    {
        bool isOverflowing = m_text.isTextOverflowing;
        return isOverflowing;
    }
    
    
    public IEnumerator ShowDialog()
    {
        var start = Time.time;

        while (Time.time - start < m_FadeTime)
        {

            var time = (Time.time - start) / m_FadeTime;
            m_canvas.alpha = Mathf.Lerp(0, 1, time);
            yield return null;
        }

        m_canvas.alpha = 1;

        m_canvas.blocksRaycasts = true;
    }
    
    public IEnumerator HideDialog()
    {
        var start = Time.time;
        while (Time.time - start < m_FadeTime)
        {
            var time = (Time.time - start) / m_FadeTime;
            m_canvas.alpha = Mathf.Lerp(1, 0, time);
            yield return null;
        }

        m_canvas.alpha = 0;

        m_canvas.blocksRaycasts = false;
    }

}