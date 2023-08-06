using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionWally : MonoBehaviour
{
    public AudioSource CollisionSound;
    public List<AudioClip> CollisionClips;
    public float volume;

    // Collision Enter
    public void OnCollisionEnter(Collision collision)
    {
        // Play a random hit clip everytime Sylvester collides with the wall
        int randomCollision = Random.Range(0, CollisionClips.Count);

        if (collision.gameObject.tag.Equals("Player"))
        {
            CollisionSound.PlayOneShot(CollisionClips[randomCollision], volume);
        }
    }

    // Collision Stay
    public void OnCollisionStay(Collision collision)
    {
       // Do nothing
    }

    // Collision Exit
    public void OnCollisionExit(Collision collision)
    {
        // Do nothing
    }
}
