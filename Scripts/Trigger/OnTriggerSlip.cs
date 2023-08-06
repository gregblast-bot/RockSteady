using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityRandom = UnityEngine.Random;

public class OnTriggerSlip : MonoBehaviour
{
    public AudioSource TriggerSound;
    public AudioClip TriggerClip;
    public float volume;

    [SerializeField] 
    private float slipSpeed;
    [SerializeField] 
    private UnityEvent triggerSlipEvent;

    // Trigger Enter
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            int randForce = UnityRandom.Range(0, 3);
            var rb = collision.gameObject.GetComponent<Rigidbody>();

            if (randForce == 0)
            {
                rb.AddForce(transform.forward * slipSpeed, ForceMode.Acceleration);
            }
            else if (randForce == 1)
            {
                rb.AddForce(transform.up * slipSpeed, ForceMode.Acceleration);
            }
            else if (randForce == 2)
            {
                rb.AddForce(-transform.forward * slipSpeed, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(-transform.up * slipSpeed, ForceMode.Acceleration);
            }

            triggerSlipEvent.Invoke();
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

    public void PlaySlipSound()
    {
        TriggerSound.PlayOneShot(TriggerClip, volume);
    }
}
