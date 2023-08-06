using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class NPC
{
    [SerializeField]
    protected string name;
    [SerializeField]
    protected string speed;
    [SerializeField]
    protected float stoppingDistance;

    protected Transform target;


    public virtual void SetTarget(Transform t)
    {
        target = t;
    }

    // Trigger Enter
    public void OnTriggerEnter(Collider focus)
    {
        if (focus.gameObject.tag.Equals("Player"))
        {
            SetTarget(focus.transform);
        }
    }

    protected bool arrivedAtTarget()
    {
        if (Vector3.Distance(target.position, target.position) < stoppingDistance)
        {
            return true;
        }

        return false;
    }
}
