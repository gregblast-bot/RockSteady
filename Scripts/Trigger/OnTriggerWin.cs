using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnTriggerWin : MonoBehaviour
{
    public AudioSource TriggerSound;
    public AudioClip TriggerClip;
    public float volume;
    public UnityEvent triggerWinEvent;

    // Trigger Enter
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            triggerWinEvent.Invoke();
        }
    }

    // Trigger Stay
    public void OnTriggerStay(Collider collision)
    {
        // Do nothing
    }

    // Trigger Exit
    public void OnTriggerExit(Collider collision)
    {
        // Do nothing
    }

    public void PlayWinSound()
    {
        TriggerSound.PlayOneShot(TriggerClip, volume);
    }
}
