using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public int LifeMax = 1;
    public float HealthMAX = 1.0f;
    private float _currentHP;
    private int _currentLifes;

    public GameObject ParticleDeathPrefab;
    public AudioClip PlayerDeathScream;

    private bool _isRespawning = false;
    //public float WaitToRespawn = 1.0f;
    public float RespawnInvinsiblityDuration = 1.0f;
    private bool _isInvincible = false;

    private Camera theCamera = null;

    /* trenutno dok nemam vanjski audio source, ne vrijedi mi
    private AudioSource _myAudioSource;
    public AudioClip DeathSound;
    */

    public enum OnDeathAction
    {
        Nothing,
        RestartScene,
        GameOver,
        KillObject
    }
    public OnDeathAction OnAllLivesGone = OnDeathAction.Nothing;

    private void Awake()
    {
        _currentHP = HealthMAX;
        _currentLifes = LifeMax;
        _isRespawning = false;
        _isInvincible = false;
        //_myAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _currentHP = HealthMAX;
        _isRespawning = false;
        Debug.Log("Script was enabled");
        StartCoroutine(RespawnInvinsibility());
    }

    private void Start()
    {
        if (transform.tag == "Player")
        {
            SetPlayerCanvas();
            theCamera = FindObjectOfType<Camera>();
        }
    }
        
    private void Update()
    {
        if(_currentHP <= 0.0f && !_isRespawning){
            //DEATH
            //pusti sfx
            --_currentLifes;

            if (ParticleDeathPrefab)
            {
                Instantiate(ParticleDeathPrefab, transform.position, Quaternion.identity);
            }

            if (_currentLifes > 0)
            {
                if(transform.tag == "Player")
                {
                    //pokreni Respawn playera
                    _isRespawning = true;
                    GameManager.GM.RespawnPlayer(transform.gameObject, theCamera);
                }
                else
                {
                    //pokreni Respawn objekta
                }
            }
            else
            {
                _isRespawning = false;
                switch (OnAllLivesGone)
                {
                    case OnDeathAction.Nothing:
                        break;
                    case OnDeathAction.RestartScene:

                        if (transform.GetComponent<PlayerController>() == true)
                        {
                            _currentLifes = 0;
                            _currentHP = 0.0f;
                            GetComponent<PlayerController>().enabled = false;
                        }


                        if (transform.tag == "Player")
                        {
                            theCamera.transform.SetParent(null);
                            theCamera.transform.position = transform.root.position;
                            theCamera.transform.rotation = transform.root.rotation;
                        }
                        

                        GameManager.GM.RestartScene();
                        Destroy(gameObject);
                        break;
                    case OnDeathAction.GameOver:
                        if(transform.tag == "Player")
                        {
                            theCamera.transform.SetParent(null);
                            theCamera.transform.position = transform.root.position;
                            theCamera.transform.rotation = transform.root.rotation;
                        }
                        GameManager.GM.GameOver();
                        Destroy(gameObject);
                        break;
                    case OnDeathAction.KillObject:
                        if (ParticleDeathPrefab)
                        {
                            Instantiate(ParticleDeathPrefab, transform.position, Quaternion.identity);
                        }
                        if(transform.tag == "Player")
                        {
                            GameManager.GM.ChangeMusic(PlayerDeathScream);
                        }
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }

            }

            if (transform.tag == "Player")
            {
                SetPlayerCanvas();
            }
        }
    }

    private void LateUpdate()
    {
        if (transform.tag == "Player")
        {
            SetPlayerCanvas();
        }
    }

    public void ApplyDamage(float amount)
    {
        if (!_isInvincible)
        {
            _currentHP -= amount;
            //ažuriraj HP canvas
            //pusti SFX
            //pusti visual
        }
        if (transform.tag == "Player")
        {
            SetPlayerCanvas();
        }
    }

    public void ApplyHealth(float amount)
    {
        if (_currentHP < HealthMAX)
            _currentHP += amount;
        if (_currentHP >= HealthMAX)
            _currentHP = HealthMAX;        
        //pusti SFX
        //pusti visual
        if (transform.tag == "Player")
        {
            SetPlayerCanvas();
        }
    }

    public IEnumerator RespawnInvinsibility()
    {
        _isInvincible = true;
        //pokaži da je invinsible
        yield return new WaitForSeconds(RespawnInvinsiblityDuration);
        _isInvincible = false;
        //pokaži da više nije invinsible
    }

    public void SetPlayerCanvas()
    {
        GameManager.GM.SetPlayerScreenHealth(_currentHP, HealthMAX, _currentLifes);
    }

    public float ReturnCurentHP()
    {
        return _currentHP;
    }
}
