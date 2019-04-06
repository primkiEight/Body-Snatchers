using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFromWeapon : MonoBehaviour {

    private GameObject _projectileParent;

    public List<WeaponHolder> WeaponsSlots = new List<WeaponHolder>();
    [SerializeField]
    private int _selectedWeaponIndex = 0;
    public GameObject test;

    private void Awake()
    {
        _projectileParent = new GameObject("Projectile Parent"); //stvara u sceni prazan objekt za parenta projektilima
        Debug.Log(WeaponsSlots.Count);
    }

    private void Update()
    {
        if (GameManager.GM.CurrentState == GameManager.GameState.Playing)
        {
            //ActiveWeaponSelection
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                ++_selectedWeaponIndex;

                if (_selectedWeaponIndex >= WeaponsSlots.Count)
                    _selectedWeaponIndex = 0;

                //provjeravam je li aktivan, ako nije uvećavam index, ako je prekidam provjeru
                for (int i = _selectedWeaponIndex; i < WeaponsSlots.Count; i++)
                {
                    if (!WeaponsSlots[_selectedWeaponIndex].gameObject.activeSelf) //kada je gameObject
                                                                                   //if (!WeaponsSlots[_selectedWeaponIndex].isActiveAndEnabled) //kada je skripta
                    {
                        ++_selectedWeaponIndex;
                    }
                    else if (WeaponsSlots[_selectedWeaponIndex].gameObject.activeSelf) //kada je gameObject
                                                                                       //else if (WeaponsSlots[_selectedWeaponIndex].isActiveAndEnabled) //kada je skripta
                        break;
                }

                if (_selectedWeaponIndex >= WeaponsSlots.Count)
                    _selectedWeaponIndex = 0;

            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                --_selectedWeaponIndex;

                if (_selectedWeaponIndex < 0)
                    _selectedWeaponIndex = WeaponsSlots.Count - 1;

                //provjeravam je li aktivan, ako nije umanjujem index, ako je prekidam provjeru
                for (int i = _selectedWeaponIndex; i > 0; i--)
                {
                    if (!WeaponsSlots[_selectedWeaponIndex].gameObject.activeSelf) //kada je gameObject
                                                                                   //if (!WeaponsSlots[_selectedWeaponIndex].isActiveAndEnabled) //kada je skripta
                    {
                        --_selectedWeaponIndex;
                    }
                    else if (WeaponsSlots[i].gameObject.activeSelf) //kada je gameObject
                                                                    //else if (WeaponsSlots[i].isActiveAndEnabled) //kada je skripta
                        break;
                }

                if (_selectedWeaponIndex < 0)
                    _selectedWeaponIndex = WeaponsSlots.Count - 1;
            }
            test = WeaponsSlots[_selectedWeaponIndex].gameObject;

            if (Input.GetButtonDown("Fire2"))
                WeaponsSlots[_selectedWeaponIndex].Reload();

            if (Input.GetButtonDown("Fire1"))
            {
                //pucanje pozivanjem metode iz skripte aktivnog oružja i provjera je li magazin pun, umjesto ovdje

                WeaponsSlots[_selectedWeaponIndex].Fire(_projectileParent);
                //(WeaponsSlots[_selectedWeaponIndex].UseMagazineAmmo();
            }
        }            
    }

    private void LateUpdate()
    {
        GameManager.GM.SetPlayerScreenWeapons(
            WeaponsSlots[_selectedWeaponIndex].WeaponImage,
            WeaponsSlots[_selectedWeaponIndex].WeaponName,
            WeaponsSlots[_selectedWeaponIndex].CurrentAmmo,
            WeaponsSlots[_selectedWeaponIndex].MagazineCapacity,
            WeaponsSlots[_selectedWeaponIndex].RemainingAmmo);
    }


    /*
    public void Fire()
    {
        Vector3 offSetPoint = new Vector3(
            Random.Range(-WeaponsSlots[_selectedWeaponIndex].ShootingAccuracy, WeaponsSlots[_selectedWeaponIndex].ShootingAccuracy),
            Random.Range(-WeaponsSlots[_selectedWeaponIndex].ShootingAccuracy, WeaponsSlots[_selectedWeaponIndex].ShootingAccuracy),
            Random.Range(-WeaponsSlots[_selectedWeaponIndex].ShootingAccuracy, WeaponsSlots[_selectedWeaponIndex].ShootingAccuracy))/10;

        Vector3 _fireingOffset = WeaponsSlots[_selectedWeaponIndex].Muzzle.forward + offSetPoint;

        GameObject projectileClone = Instantiate(WeaponsSlots[_selectedWeaponIndex].ProjectilePrefab,
            WeaponsSlots[_selectedWeaponIndex].Muzzle.position,
            Quaternion.identity,
            _projectileParent.transform);

        Rigidbody projectileRigidbody = projectileClone.GetComponent<Rigidbody>();
        projectileRigidbody.AddForce(_fireingOffset * WeaponsSlots[_selectedWeaponIndex].ShootingPower,
            ForceMode.VelocityChange); //VelocityChange - odmah se kreće tom brzinom bez utjecaja mase i trenja

        //WeaponsSlots[_selectedWeaponIndex].Muzzle.forward
    }*/
}