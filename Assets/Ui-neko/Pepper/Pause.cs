using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                pause();
            }
        }
    }

    public void Resume()
    {
        //pauseMenuUI.GetComponent<Canvas>().enabled = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("startmenu");
    }
    public void QuitGame() //其实是返回选关界面
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectGame");

    }

    public void Retry1()
    {
        SceneManager.LoadScene("video");
        Time.timeScale = 1f;
        Debug.Log("restart");
    }
    public void Retry2()
    {
        SceneManager.LoadScene("Stage1");
        Time.timeScale = 1f;
        Debug.Log("restart");
    }
    public void Retry3()
    {

        SceneManager.LoadScene("Stage2");
        Time.timeScale = 1f;
        Debug.Log("restart");
    }
    public void Retry4()
    {
        SceneManager.LoadScene("Stage4");
        Time.timeScale = 1f;
        Debug.Log("restart");
    }
}
