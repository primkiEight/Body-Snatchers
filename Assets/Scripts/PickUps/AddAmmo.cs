using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : MonoBehaviour {

    public AudioClip PickUpSound;

    public string WeaponName;
    public int Ammo;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "PlayerMasked")
        {
            AudioSource _pickUpAS = other.GetComponent<AudioSource>();
            ShootFromWeapon weapons = other.GetComponent<ShootFromWeapon>();
            for (int i = 0; i < weapons.WeaponsSlots.Count; i++)
            {
                if(weapons.WeaponsSlots[i].WeaponName == WeaponName)
                {
                    if (weapons.WeaponsSlots[i].gameObject.activeSelf == false)
                        weapons.WeaponsSlots[i].gameObject.SetActive(true);

                    weapons.WeaponsSlots[i].AddAmmo(Ammo);
                    //pusti animaciju
                    _pickUpAS.pitch = Random.Range(0.7f, 1.3f);
                    _pickUpAS.PlayOneShot(PickUpSound);
                    Destroy(gameObject);
                }
            }
        }
    }
}
