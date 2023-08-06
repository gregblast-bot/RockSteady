using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationNPC : NPC
{
    private void Update()
    {
        if (target != null)
        {
            //transform.LookAt(target);
            //transform.Translate((target.transform.position - transform.position).normalized * speed * Time.deltaTime);

            if (arrivedAtTarget())
            {
                target = null;
            }
        }
    }
}
