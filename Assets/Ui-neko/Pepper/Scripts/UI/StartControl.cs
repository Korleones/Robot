using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartControl : MonoBehaviour
{
    public AudioSource click;
    public void PlayGame()
    {
        StartCoroutine(wait());
            }

    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator wait()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
