using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotThePlayer : MonoBehaviour {

    public Transform MyPosition;
    public GameObject AlienToInform;
    private GameObject _myParent;

    public AudioSource _myAudioSource;
    public AudioClip HumanSpottedAudioClip;

    private void Start()
    {
        _myParent = transform.parent.gameObject;
    }

    public void Update()
    {
        if (MyPosition == null)
        {
            _myParent.GetComponent<CleanTheMess>().CleanTheMessHere();
        } else
        {
            transform.position = MyPosition.position;
        }        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (AlienToInform.GetComponent<AlienController>() != null)
                AlienToInform.GetComponent<AlienController>().Target = other.transform;
            //_myAudioSource.pitch = Random.Range(0.7f, 1.3f);
            _myAudioSource.PlayOneShot(HumanSpottedAudioClip);
        }
        
        if(other.tag == "UFO")
        {
            if (AlienToInform.GetComponent<HealthManager>() != null)
                AlienToInform.GetComponent<HealthManager>().ApplyHealth(AlienToInform.GetComponent<HealthManager>().HealthMAX);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (AlienToInform.GetComponent<AlienController>() != null)
                AlienToInform.GetComponent<AlienController>().SetNextStep();
        }
    }    
}
