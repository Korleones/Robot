using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//场景跳转要的头文件


public class success : MonoBehaviour
{
    public string NextStage;
    public GameObject SnakeHead;
    public Animator pepper;
    public AudioSource bgm;
    public int hatx;
    public int haty;
    bool a = false;
    private void Update()
    {
            if (SnakeHead.transform.position.x == hatx && SnakeHead.transform.position.y == haty && !a)
                {
                             StartCoroutine(wait());
            a = true;
                 }
    }

    IEnumerator wait()
    {
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        pepper.SetBool("isWin", true);
        bgm.Stop();
        yield return new WaitForSeconds(6.5f);

        //pepper.SetBool("isWin", false);
        SceneManager.LoadScene(NextStage);
    }
}
