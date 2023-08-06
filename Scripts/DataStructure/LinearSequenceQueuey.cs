using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinearSequenceQueuey : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> events;

    private Queue<UnityEvent> eventQueue = new Queue<UnityEvent>();

    private void Awake()
    {
        foreach (var evt in events)
        {
            eventQueue.Enqueue(evt);
        }

        DequeueNextEvent();
    }

    public void DequeueNextEvent()
    {
        if (eventQueue.Count > 0)
        {
            eventQueue.Dequeue()?.Invoke();
        }
    }
}
