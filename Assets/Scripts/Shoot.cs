using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject WeaponPrefab;
    public GameObject ProjectilePrefab;
    public Transform FiringPoint;
    public float Power;

    private GameObject _projectileParent;

    private void Awake()
    {
        _projectileParent = new GameObject("Projectile Parent"); //stvara u sceni prazan objekt za parenta projektilima
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject projectileClone = Instantiate(ProjectilePrefab, FiringPoint.position, Quaternion.identity, _projectileParent.transform); //zadnji parametar je parent

            Rigidbody projectileRigidbody = projectileClone.GetComponent<Rigidbody>();
            projectileRigidbody.AddForce(FiringPoint.forward * Power, ForceMode.VelocityChange); //VelocityChange - odmah se kreće tom brzinom bez utjecaja mase i trenja
        }
    }
}