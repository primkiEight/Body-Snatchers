using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerNextStep : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if (other.GetComponent<AlienController>() != null)
                other.GetComponent<AlienController>().SetNextStep();
        }
    }
}
