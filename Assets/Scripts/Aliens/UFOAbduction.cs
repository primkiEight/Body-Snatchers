using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAbduction : MonoBehaviour {

    public Transform MyPosition;
    public GameObject UFOToInform;
    private GameObject _myParent;

    public float PositiveGravity = 1;
    private PlayerController _thePlayer = null;
    private HealthManager _thePlayerHP = null;
    private UFOController _theUFO;
    public float DamageToDeal = 10f;
    public float DealDamageFreq = 0.5f;
    private bool _canDealDamage = false;

    public AudioSource _myAudioSource;
    public AudioClip AbductionSound;
    private AudioClip _previousTrack;

    private void Start()
    {
        _theUFO = UFOToInform.GetComponent<UFOController>();
        _myParent = transform.parent.gameObject;
    }

    private void Update()
    {
        if (MyPosition == null)
        {
            _myParent.GetComponent<CleanTheMess>().CleanTheMessHere();
        }
        else
        {
            transform.position = MyPosition.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _theUFO.CanMove(false);
            _thePlayer = other.GetComponent<PlayerController>();
            _thePlayer.Paralize(true);
            _thePlayer.ChangeGravity(PositiveGravity);
            _canDealDamage = true;
            //playSFX
            if (_myAudioSource)
            {
                _previousTrack = _myAudioSource.clip;
                _myAudioSource.Stop();                
                _myAudioSource.PlayOneShot(AbductionSound);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _theUFO.CanMove(false);
            _thePlayerHP = other.GetComponent<HealthManager>();
            while (_canDealDamage)
            {
                _thePlayerHP.ApplyDamage(DamageToDeal);
                StartCoroutine(DealDamageCo());
                if(_thePlayerHP.ReturnCurentHP() <= 0.0f)
                    _theUFO.CanMove(true);
            }
            //playSFX                     
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _thePlayer = other.GetComponent<PlayerController>();
            _thePlayer.Paralize(false);
            _thePlayer.ResetGravity();
            _canDealDamage = true;
            _theUFO.CanMove(true);
            //playSFX
            if (_myAudioSource)
            {
                _myAudioSource.Stop();
                _myAudioSource.clip = _previousTrack;
                _myAudioSource.Play();
            }
        }
    }

    private void OnDestroy()
    {
        if (_thePlayer)
        {
            _thePlayer.Paralize(false);
            _thePlayer.ResetGravity();
        }            
        //play SFX
    }

    private IEnumerator DealDamageCo()
    {
        _canDealDamage = false;
        yield return new WaitForSeconds(DealDamageFreq);
        _canDealDamage = true;
    }
}
