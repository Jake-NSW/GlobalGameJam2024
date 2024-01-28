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
    
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);
        StartCoroutine(ShowDialog());
        
        yield return new WaitForSeconds(1.5f);
        PickDialog();
    }

    private void PickDialog()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Level1":
                StartCoroutine(FillDialog("I should have know 'blue-cheese' sushi wasn't a thing!"));
                break;
            case "Level2":
                StartCoroutine(FillDialog("Why did I have to order the squid and asparagus burrito!?"));
                break;
            case "Level3":
                StartCoroutine(FillDialog("I've seen some messed up public toilets but this is a new level!"));
                break;
        }
    }

    private IEnumerator FillDialog(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            m_text.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(2f);

        if (!Application.isPlaying) yield break;
        
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