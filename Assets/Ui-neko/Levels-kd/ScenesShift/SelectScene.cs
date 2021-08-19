using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//场景跳转要的头文件

public class SelectScene : MonoBehaviour
{
    public void ToScene(int n) {
        SceneManager.LoadScene(n);//把“”内换成关卡场景名字
    }

}
