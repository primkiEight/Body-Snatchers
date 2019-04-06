using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {

    public Transform Target;
    private Transform _nextTarget;
    public Vector2 SpeedMinAndMax;
    private float _speed;
    public Vector2 DistanceToKeepMinAndMax;
    private float _distanceToKeep;

    public List<Transform> Steps = new List<Transform>();
    private int _listStepIndex;

    private Transform _transform;

    private void Awake()
    {
        // ovo ispod je zapravo transform = GetComponent<Transform>
        _transform = transform; //ovo je stvar dobre prakse, mogli smo dolje i direktno mijenjati vlasiti transform.position

        _speed = Vector2RandomExtension.V2Random(SpeedMinAndMax);
        _distanceToKeep = Vector2RandomExtension.V2Random(DistanceToKeepMinAndMax);

        _listStepIndex = 0;
        Target = Steps[_listStepIndex];
    }

    private void Update()
    {
        if (Target == null) {
            //return;
            SetNextStep();            
        }

        _transform.LookAt(new Vector3(Target.position.x, _transform.position.y, Target.position.z));

        float distanceToTarget = Vector3.Distance(Target.position, _transform.position);

        if (distanceToTarget > _distanceToKeep)
            _transform.position += _transform.forward * _speed * Time.deltaTime; //TRANSFORM.forward pomiče u odnosu na lokalnu, vlastitu, koordinatnu os, to nam je jedinični vektor kretanja ne u odnosu na globalne XYZ, već svoje lokalne
        //else
            //_transform.position -= _transform.forward * _speed * Time.deltaTime;
    }

    public void SetNextStep()
    {
        if (_listStepIndex >= Steps.Count - 1)
            _listStepIndex = 0;
        else
            ++_listStepIndex;
        _nextTarget = Steps[_listStepIndex];
        Target = _nextTarget;
    }

    private void OnDestroy()
    {
        GameManager.GM.AddScore(1);
        Destroy(transform.parent); //TRENUTNO NE FUNKCIONIRA, postoji problem da objekt za SpotSphere ostaje na sceni i u hijerarhiji...
    }
}
