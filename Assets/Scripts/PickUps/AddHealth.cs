using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealth : MonoBehaviour {

    public AudioClip PickUpSound;

    public int HealhtUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerMasked")
        {
            AudioSource _pickUpAS = other.GetComponent<AudioSource>();
            HealthManager theHealthManager = other.GetComponent<HealthManager>();            
            theHealthManager.ApplyHealth(HealhtUp);
            //pusti animaciju
            _pickUpAS.pitch = Random.Range(0.7f, 1.3f);
            _pickUpAS.PlayOneShot(PickUpSound);
            Destroy(gameObject);            
        }
    }
}
