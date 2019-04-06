using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectDestructor : MonoBehaviour {

    public float Time = 5.0f;

    private void Awake()
    {
        Invoke("DestroyNow", Time);
    }

    private void DestroyNow()
    {
        Destroy(gameObject);
    }
}
