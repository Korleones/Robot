using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class counterpart : MonoBehaviour
{
    public int steps_left;
    public bool islongerbox, iscloud;
    public Scene scene;
    public Text num;
       
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if(scene.name == "Stage4")
        {
            steps_left = 9;
        }
        num.text = steps_left.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (iscloud)
        {
            steps_left--;

            num.text = steps_left.ToString();
            iscloud = false;
        }
        else if (!iscloud && islongerbox)
        {
            steps_left += 3;
            num.text = steps_left.ToString();
            islongerbox = false;
        }

        
    }
}
