using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAccuse : MonoBehaviour {

    public GameObject BodyToAnimate;
    public GameObject MotherShip;
    private PlayerController _paralizedPlayer = null;

    private AudioSource _myAudioSource;
    public AudioClip JAccuseSound;

    private void Awake()
    {
        _myAudioSource = GetComponentInParent<AudioSource>();
    }

    //private void OnTriggerStay(Collider other)
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Paralize");
            BodyToAnimate.GetComponent<Animator>().SetBool("jAccuse", true);
            _paralizedPlayer = other.GetComponent<PlayerController>();
            _paralizedPlayer.Paralize(true);
            if (MotherShip)
                MotherShip.GetComponent<UFOController>().SetPlayerTarget(other.transform.position);
            //playSFX
            _myAudioSource.pitch = Random.Range(0.5f, 2.5f);
            _myAudioSource.PlayOneShot(JAccuseSound);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("DeParalize");
            BodyToAnimate.GetComponent<Animator>().SetBool("jAccuse", false);            
        }
    }    

    private void OnDestroy()
    {
        if (_paralizedPlayer)
        {
            _paralizedPlayer.Paralize(false);
            if (MotherShip)
                MotherShip.GetComponent<UFOController>().RestartMovement();            
        }
            
        //play SFX
    }
}
