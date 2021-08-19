using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    Animator ParentAnim;
    public float transitionTime;
    private void Start()
    {
        ParentAnim = GameObject.FindGameObjectWithTag("Transition").GetComponent<Animator>();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(loadlevel(SceneManager.GetActiveScene().buildIndex + 1)); //要先把关卡build进去，不然会error

    }

    IEnumerator loadlevel(int levelIndex)  //1个协程管理语法，怪
    {
        //Play animation
        ParentAnim.SetTrigger("Out");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //LoadScene
        SceneManager.LoadScene(levelIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            LoadNextLevel();

        }
    }
}
