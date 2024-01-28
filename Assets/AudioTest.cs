using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioTest : MonoBehaviour
{
    [SerializeField]private AudioClip clip;
    
    [ContextMenu("Play Audio Clip")]
    public void PlayAudioClip()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
