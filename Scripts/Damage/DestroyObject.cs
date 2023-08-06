using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}

//public class DestroyObject : MonoBehaviour
//{
//    [SerializeField] private float time2Destory;

//    private void Awake()
//    {
//        StartCoroutine((string)DestroyAfterDelay());
//    }

//    public IEnumerable DestroyAfterDelay()
//    {
//        yield return new WaitForSeconds(time2Destory);
//        Destroy(gameObject);
//    }
//}