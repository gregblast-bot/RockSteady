using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour
{

    // Obbject tracking (focus on and look at)
    [SerializeField] Transform target;

    void start()
    {

    }

    void update()
    {
        transform.LookAt(target.position);
    }
}
