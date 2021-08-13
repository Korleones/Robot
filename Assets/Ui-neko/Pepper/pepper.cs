using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pepper : MonoBehaviour
{
    public Dialougue dialougue;
    public Animator anim;
   
    private bool a,b,c;
   // private bool isfirstt;
    // Start is called before the first frame update
    void Start()
    {
        //isfirstt = anim.GetComponent<DialougueManager>().isfirst;
    }

    // Update is called once per frame
    void Update()
    {
        if(!a)
        { 
            FindObjectOfType<DialougueManager>().StartDialogue(dialougue);
            a = true;

        }
        if (c == true)
        {
            anim.SetBool("IsOpen", false);
        }

       // Debug.Log(anim.GetBool("IsOpen"));
    }
    void set()
    {
        b = true;
        Debug.Log(b);

    }
    void set2()
    {
        if(b==true)
        {
            c = true;
        }
    }
    public void change()
    {
        c = false;
        if (c == false)
        {
            anim.SetBool("IsOpen", true);
        }
    }
}

        


       
 
    

