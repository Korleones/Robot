using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialougue dialougue;
    

    public void TriggerDialouge()
    {
        FindObjectOfType<DialougueManager>().StartDialogue(dialougue);
    }
    
}
