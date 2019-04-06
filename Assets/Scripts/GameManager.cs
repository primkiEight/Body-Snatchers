using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager GM;

    public enum GameState
    {
        Playing,
        GameOver,
        Paused,
        Win
    }

    private AudioSource _gameMusicSource;
    private AudioClip _tempMusic;
    public List<AudioClip> MusicAudioClips = new List<AudioClip>();
    public AudioClip GamePausedMusic;
    public AudioClip GameOverMusic;
    public AudioClip GameWinMusic;

    public float WaitToRestart = 1.0f;
    public float WaitToRespawnPlayer = 1.0f;

    public Canvas GameOverCanvas;
    public Text GameOverPausedText;
    public Button NextLevelButton;
    public Button ResumeGameButton;
    public Canvas GamePlayCanvas;

    private bool _gamePause = false;

    public Text Score;
    private int _scoreTotal;
    private AlienController[] _alienCount;

    public Slider TimerSlider;
    public float MaxTime = 60.0f;

    public Slider HealthSlider;
    public Text LifeText;

    public Image WeaponImage;
    public Text WeaponName;
    public Text AvailableAmmo;    
    public Text RemainingAmmo;

    private GameObject thePlayer = null;
    private Camera playerCamera = null;

    private MouseLooker _mouseLooker;

    public GameObject GameOverTriggerText;

    [Header("Read-only")]

    public GameState CurrentState = GameState.Playing;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private float _timer = 0.0f;
    
    private void Awake()
    {
        GM = this;

        Time.timeScale = 1.0f;
        CurrentState = GameState.Playing;

        _timer = MaxTime;
    }

    private void Start()
    {
        //Score and score screen setup
        //GameOverTriggerText.SetActive(false);
        GameOverCanvas.gameObject.SetActive(false);
        GamePlayCanvas.gameObject.SetActive(true);
        ResumeGameButton.gameObject.SetActive(true);
        NextLevelButton.gameObject.SetActive(false);
        _alienCount = FindObjectsOfType<AlienController>();
        //dohvaćam LockCurstor skriptu jer mi je potrebno omogućiti
        //kursor kad otvaram GaveOver i Pause canvase
        _mouseLooker = FindObjectOfType<MouseLooker>();
        _gameMusicSource = GetComponent<AudioSource>();
        _scoreTotal = _alienCount.Length;
        SetScoreScreen(_score);
        //ResetMusic();
    }
    

    private void Update()
    {
        if(CurrentState == GameState.Playing)
        {
            TimerSlider.value = _timer / MaxTime;
            _timer -= Time.deltaTime;

            if (_timer <= 0.0f) //kada vrijeme istekne, gameover
            {
                _timer = 0.0f;
                //GamePlayCanvas.gameObject.SetActive(true);
                //GameOverCanvas.gameObject.SetActive(true);
                //CurrentState = GameState.GameOver;
                GameOver();
            }
        }

        if(CurrentState == GameState.Playing || CurrentState == GameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_gamePause)
                {
                        _gamePause = true;
                        GamePause();
                }/*
                else
                {
                    _gamePause = false;
                    GamePause();
                    //i dalje mi kursor ostaje vidljiv...
                }*/
            }
        }        
    }

    private void LateUpdate()
    {
        if (!_gameMusicSource.isPlaying)
        {
            ResetMusic();
        }
    }

    public void AddScore(int amount)
    {
        _score += amount;
        SetScoreScreen(_score);
        CheckScore();
    }

    public void SetScoreScreen(int score)
    {
        Score.text = score.ToString() + "/" + _scoreTotal.ToString();
    }

    public void CheckScore()
    {
        if(_score == _scoreTotal)
        {
            CurrentState = GameState.Win;
            GameWin();
        }
    }

    public void AddTime(float amount)
    {
        MaxTime += amount;
        _timer += amount;
        TimerSlider.value = _timer / MaxTime;        
    }

    public void RestartScene()
    {
        StartCoroutine(WaitToRestartCo());
        //Time.timeScale = 0.0f;
    }

    private IEnumerator WaitToRestartCo()
    {
        yield return new WaitForSeconds(WaitToRestart);
        //Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        CurrentState = GameState.GameOver;
       
        ChangeMusic(GameOverMusic);

        //moram prikazati kursor jer mi treba
        if (_mouseLooker)
        {
            _mouseLooker.LockCursor(false);
            _mouseLooker.enabled = false;
        }
        ResumeGameButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(false);
        GamePlayCanvas.gameObject.SetActive(true);
        GameOverCanvas.gameObject.SetActive(true);
    }

    public void GameWin()
    {
        CurrentState = GameState.Win;        
        ChangeMusic(GameWinMusic);

        //Time.timeScale = 0.0f;

        //moram prikazati kursor jer mi treba
        if (_mouseLooker)
        {
            _mouseLooker.LockCursor(false);
            _mouseLooker.enabled = false;
        }
        GameOverPausedText.text = "You Have Won";
        ResumeGameButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(true);
        GamePlayCanvas.gameObject.SetActive(true);
        GameOverCanvas.gameObject.SetActive(true);
    }

    public void GamePause()
    {
        if(_gamePause)
        {
            CurrentState = GameState.Paused;           
            ChangeMusic(GamePausedMusic);
            if (_mouseLooker)
            {
                //moram prikazati kursor jer mi treba
                _mouseLooker.LockCursor(false);
                //moram onemogućiti MouseLookeer skriptu jer ću koristiti kursor za svoj canvas
                _mouseLooker.enabled = false;
            }
            Time.timeScale = 0.0f;
            GamePlayCanvas.gameObject.SetActive(true);
            GameOverPausedText.text = "You Have Paused";
            GameOverCanvas.gameObject.SetActive(true);
        } else
        {
            CurrentState = GameState.Playing;
            ResetMusic();
            if (_mouseLooker)
            {
                //moram omogućiti MouseLookeer skriptu jer više ne koristim kursor za svoj canvas
                _mouseLooker.enabled = true;
                //moram zaključati (sakriti) kursor jer mi više ne treba
                _mouseLooker.LockCursor(true);
            }

            Time.timeScale = 1.0f;
            GameOverCanvas.gameObject.SetActive(false);
            GameOverPausedText.text = "You Have Died";
            GamePlayCanvas.gameObject.SetActive(true);
        }
    }

    public void NextLevel(int nextSceneBuildIndex)
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(nextSceneBuildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void StartNewGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UnPause()
    {
        if (_mouseLooker)
        {
            //moram omogućiti MouseLookeer skriptu jer više ne koristim kursor za svoj canvas
            _mouseLooker.enabled = true;
            //moram zaključati (sakriti) kursor jer mi više ne treba
            _mouseLooker.LockCursor(true);
        }
        _gamePause = false;
        Time.timeScale = 1.0f;
        GamePause();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RespawnPlayer(GameObject playerToRespawn, Camera cameraToReset)
    {
        thePlayer = playerToRespawn;
        playerCamera = cameraToReset;
        StartCoroutine(RespawnPlayerCo());
    }

    public IEnumerator RespawnPlayerCo()
    {        
        playerCamera.transform.SetParent(null);
        thePlayer.SetActive(false);
        Debug.Log("Script was desabled");
        yield return new WaitForSeconds(WaitToRespawnPlayer);
        thePlayer.SetActive(true);        
        thePlayer.GetComponent<PlayerController>().IsParalized = false;
        thePlayer.GetComponent<PlayerController>().ResetGravity();        
        playerCamera.transform.SetParent(thePlayer.transform);
        thePlayer.GetComponent<PlayerController>().ResetTransform();
    }

    public void SetPlayerScreenHealth(float health, float healthMAX, int lives)
    {
        //HealthSlider.maxValue = healthMAX;
        HealthSlider.value = health/healthMAX;
        //HealthScreen.text = health.ToString();
        LifeText.text = lives.ToString();
    }

    public void SetPlayerScreenWeapons(Sprite weaponImage, string weaponName, int current, int magazine, int total)
    {
        WeaponImage.sprite = weaponImage;
        WeaponName.text = weaponName;
        AvailableAmmo.text = current.ToString() + "/" + magazine.ToString();
        RemainingAmmo.text = total.ToString();
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        _gameMusicSource.loop = true;
        _gameMusicSource.clip = newMusic;
        _gameMusicSource.Play();
    }

    public void ResetMusic()
    {
        _gameMusicSource.loop = false;
        int index = Random.Range(0, MusicAudioClips.Count - 1);
        _gameMusicSource.clip = MusicAudioClips[index];
        _gameMusicSource.Play();        
    }
}
