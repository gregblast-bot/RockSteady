using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnterEventProvider : MonoBehaviour
{
    [SerializeField] private List<string> tagList;
    [SerializeField] private UnityEvent triggerEnterEventPorvider;

    private void OnTriggerEvent(Collider other)
    {
        for (int i = 0; i < tagList.Count; i++)
        {
            if (other.gameObject.tag.Equals(tagList[i]))
            {
                Debug.Log("Hitting on trigger enter.");
                triggerEnterEventPorvider.Invoke();
                break;
            }
        }
    }

    //public void OnTriggerEnter(Collider collision)
    //{
    //    destroyObject(gameObject);
    //}
}
