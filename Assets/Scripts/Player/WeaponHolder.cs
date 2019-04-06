using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour{

    public string WeaponName;
    public Sprite WeaponImage;
    public float ShootingPower; //how fast to projectile will be shooted
    public float ShootingFrequency = 0.5f; //how fast
    [Range(0f, 1f)]
    public float ShootingAccuracy = 0f; //how accurate

    public GameObject ProjectilePrefab; //which projectile will be shooted
    
    public int MagazineCapacity = 1; //how many projectiles before reloading
    public int CurrentAmmo;
    public int RemainingAmmo;
    private int ostatak = 0;
    private bool _magazineIsEmpty;

    public AudioSource GunAS;
    public AudioClip FireSound;
    public AudioClip ReloadSound;
    public AudioClip EmptyWeaponSound;

    private Vector3 _offSetPoint;
    private Vector3 _fireingOffset;
    private GameObject projectileClone;
    private Rigidbody _projectileRigidbody;

    public enum WeaponType
    {
        OneBarrelGun,
        TwoBarrelGun,
        Raygun,
        MiniGun,
        Grenade
    }

    public WeaponType WeaponFireType = WeaponType.OneBarrelGun;

    private Transform _muzzle;
    public Transform MuzzleOneBarrel;
    public Transform MuzzleTwoBarrels01;
    public Transform MuzzleTwoBarrels02;

    public void Awake()
    {
        if (RemainingAmmo > MagazineCapacity)
        {
            CurrentAmmo = MagazineCapacity;
            RemainingAmmo = RemainingAmmo - MagazineCapacity;
        }            
        else
            CurrentAmmo = RemainingAmmo;

        if(RemainingAmmo == 0)
            _magazineIsEmpty = true;
    }

    public void AddAmmo(int ammo)
    {
        RemainingAmmo += ammo;
        if (_magazineIsEmpty)
        {
            _magazineIsEmpty = false;
            if (RemainingAmmo > MagazineCapacity)
            {
                CurrentAmmo = MagazineCapacity;
                RemainingAmmo = RemainingAmmo - MagazineCapacity;
            }
            else
                CurrentAmmo = RemainingAmmo;
        }

        if (RemainingAmmo > 999)
            RemainingAmmo = 999;
    }

    public void UseMagazineAmmo()
    {
        if (CurrentAmmo > 0)
            --CurrentAmmo;

        if (CurrentAmmo <= 0)
        {
            _magazineIsEmpty = true;
            CurrentAmmo = 0;
        }
    }

    public void Reload()
    {
        if (RemainingAmmo > 0 && CurrentAmmo != MagazineCapacity)
        {
            GunAS.pitch = Random.Range(0.7f, 1.3f);
            GunAS.PlayOneShot(ReloadSound);
            if (RemainingAmmo >= (MagazineCapacity - CurrentAmmo))
                ostatak = MagazineCapacity - CurrentAmmo;
            else if (RemainingAmmo < (MagazineCapacity - CurrentAmmo))
                ostatak = RemainingAmmo;
            CurrentAmmo += ostatak;
            RemainingAmmo -= ostatak;
            _magazineIsEmpty = false;
            if (RemainingAmmo < 0)
                RemainingAmmo = 0;
        }
    }

    public void Fire(GameObject projectileParent)
    {
        if (_magazineIsEmpty) {
            GunAS.pitch = Random.Range(0.7f, 1.3f);
            GunAS.PlayOneShot(EmptyWeaponSound);
            }


        if (!_magazineIsEmpty)
        {
            GunAS.pitch = Random.Range(0.7f, 1.3f);
            GunAS.PlayOneShot(FireSound);
            switch (WeaponFireType)
            {
                //OneBarrelGun
                case WeaponType.OneBarrelGun:

                    _muzzle = MuzzleOneBarrel;

                    _offSetPoint = new Vector3(
                Random.Range(-ShootingAccuracy, ShootingAccuracy),
                Random.Range(-ShootingAccuracy, ShootingAccuracy),
                Random.Range(-ShootingAccuracy, ShootingAccuracy)) / 10;

                    _fireingOffset = _muzzle.forward + _offSetPoint;

                    projectileClone = Instantiate(ProjectilePrefab,
                        _muzzle.position,
                        Quaternion.identity,
                        projectileParent.transform);

                    UseMagazineAmmo();

                    _projectileRigidbody = projectileClone.GetComponent<Rigidbody>();
                    _projectileRigidbody.AddForce(_fireingOffset * ShootingPower,
                        ForceMode.VelocityChange); //VelocityChange - odmah se kreće tom brzinom bez utjecaja mase i trenja

                    break;

                //TwoBarrelGun
                case WeaponType.TwoBarrelGun:

                    _muzzle = MuzzleTwoBarrels01;

                    for (int i = 0; i < 2; i++)
                    {
                        if (_magazineIsEmpty == true)
                            return;
                        
                        _offSetPoint = new Vector3(
                Random.Range(-ShootingAccuracy, ShootingAccuracy),
                Random.Range(-ShootingAccuracy, ShootingAccuracy),
                Random.Range(-ShootingAccuracy, ShootingAccuracy)) / 10;

                        _fireingOffset = _muzzle.forward + _offSetPoint;

                        projectileClone = Instantiate(ProjectilePrefab,
                            _muzzle.position,
                            Quaternion.identity,
                            projectileParent.transform);

                        UseMagazineAmmo();

                        _projectileRigidbody = projectileClone.GetComponent<Rigidbody>();
                        _projectileRigidbody.AddForce(_fireingOffset * ShootingPower,
                            ForceMode.VelocityChange); //VelocityChange - odmah se kreće tom brzinom bez utjecaja mase i trenja

                        _muzzle = MuzzleTwoBarrels02;
                    }

                    break;

                //MiniGun
                case WeaponType.MiniGun:

                    _muzzle = MuzzleOneBarrel;

                    //UVESTI PUCANJE S DRŽANJEM TIPKE MIŠA

                    //StartCoroutine(MiniGunCo(projectileParent));

                    /*for (int i = 0; i < CurrentAmmo; i++)
                    {
                        _offSetPoint = new Vector3(
                            Random.Range(-ShootingAccuracy, ShootingAccuracy),
                            Random.Range(-ShootingAccuracy, ShootingAccuracy),
                            Random.Range(-ShootingAccuracy, ShootingAccuracy)) / 10;

                        _fireingOffset = _muzzle.forward + _offSetPoint;

                        projectileClone = Instantiate(ProjectilePrefab,
                            _muzzle.position,
                            Quaternion.identity,
                            projectileParent.transform);

                        UseMagazineAmmo();

                        _projectileRigidbody = projectileClone.GetComponent<Rigidbody>();
                        _projectileRigidbody.AddForce(_fireingOffset * ShootingPower,
                            ForceMode.VelocityChange); //VelocityChange - odmah se kreće tom brzinom bez utjecaja mase i trenja
                    }*/

                    break;

                //Grenade
                case WeaponType.Grenade:

                    break;

                default:
                    break;
            }
        }
    }

    //pokušaj za minigun
    private IEnumerator MiniGunCo(GameObject projectileParent)
    {
        for (int i = 0; i < CurrentAmmo; i++)
        {
            _offSetPoint = new Vector3(
                Random.Range(-ShootingAccuracy, ShootingAccuracy),
                Random.Range(-ShootingAccuracy, ShootingAccuracy),
                Random.Range(-ShootingAccuracy, ShootingAccuracy)) / 10;

            _fireingOffset = _muzzle.forward + _offSetPoint;

            projectileClone = Instantiate(ProjectilePrefab,
                _muzzle.position,
                Quaternion.identity,
                projectileParent.transform);

            UseMagazineAmmo();

            _projectileRigidbody = projectileClone.GetComponent<Rigidbody>();
            _projectileRigidbody.AddForce(_fireingOffset * ShootingPower,
                ForceMode.VelocityChange); //VelocityChange - odmah se kreće tom brzinom bez utjecaja mase i trenja

            yield return new WaitForSeconds(ShootingFrequency);
        }
    }
}

