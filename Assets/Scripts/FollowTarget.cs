using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform Target;
    public Vector2 SpeedMinAndMax;
    private float _speed;
    public Vector2 DistanceToKeepMinAndMax;
    private float _distanceToKeep;

    private Transform _transform;

    private void Awake()
    {
        // ovo ispod je zapravo transform = GetComponent<Transform>
        _transform = transform; //ovo je stvar dobre prakse, mogli smo dolje i direktno mijenjati vlasiti transform.position

        _speed = Vector2RandomExtension.V2Random(SpeedMinAndMax);
        _distanceToKeep = Vector2RandomExtension.V2Random(DistanceToKeepMinAndMax);

    }

    private void Update()
    {
        if (Target == null)
            return;

        _transform.LookAt(Target);

        float distanceToTarget = Vector3.Distance(Target.position, _transform.position);

        if (distanceToTarget > _distanceToKeep)
            _transform.position += _transform.forward * _speed * Time.deltaTime; //TRANSFORM.forward pomiče u odnosu na lokalnu, vlastitu, koordinatnu os, to nam je jedinični vektor kretanja ne u odnosu na globalne XYZ, već svoje lokalne
        else
            _transform.position -= _transform.forward * _speed * Time.deltaTime;
    }

}