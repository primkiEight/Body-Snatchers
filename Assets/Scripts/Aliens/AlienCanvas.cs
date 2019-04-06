using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienCanvas : MonoBehaviour {

    //public Transform Target; zašto uzimati transform, ako odmah možemo uzeti HealthManager objekta kad nam on ionako treba
    private HealthManager _myHealthManager;
    public Vector3 Offset;
    public Transform LookAtPlayer;
    public float DistanceToPlayer;

    public Slider HealthSlider;

    //HealthManager heatlhManager;

    private void Start()
    {
        //heatlhManager = Target.GetComponent<HealthManager>();
        _myHealthManager = GetComponentInParent<HealthManager>();
        HealthSlider.value = _myHealthManager.ReturnCurentHP() / _myHealthManager.HealthMAX;
        LookAtPlayer = FindObjectOfType<PlayerController>().transform;
        HealthSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (LookAtPlayer)
        {
            float _distance = Vector3.Distance(transform.position, LookAtPlayer.position);

            if (_distance < DistanceToPlayer)
                HealthSlider.gameObject.SetActive(true);
            else
                HealthSlider.gameObject.SetActive(false);

            if (HealthSlider.enabled == true)
            {
                HealthSlider.value = _myHealthManager.ReturnCurentHP() / _myHealthManager.HealthMAX;
                transform.position = _myHealthManager.transform.position + Offset;
                transform.transform.rotation = LookAtPlayer.transform.rotation; //orijentiran kao i player
            }
        }        
    }
}
