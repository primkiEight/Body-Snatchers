using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float WaitToStart = 1.0f;
    public GameObject StoryBoard;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        StoryBoard.SetActive(false);
    }
    
    public void StartNewGame()
    {
        StartCoroutine(StartNewGameCo());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Story()
    {
        if(StoryBoard.activeSelf == false)
            StoryBoard.SetActive(true);
        else
            StoryBoard.SetActive(false);
    }

    private IEnumerator StartNewGameCo()
    {
        yield return new WaitForSeconds(WaitToStart);
        SceneManager.LoadScene(1);
    }
}
