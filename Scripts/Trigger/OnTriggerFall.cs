using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnTriggerFall : MonoBehaviour
{
    public AudioSource TriggerSound;
    public AudioClip TriggerClip;
    public float volume;

    [SerializeField] 
    private UnityEvent triggerFallEvent;

    // Trigger Enter
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            triggerFallEvent.Invoke();
            Destroy(collision.gameObject);
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

    public void PlayFallSound()
    {
        TriggerSound.PlayOneShot(TriggerClip, volume);
    }
}
