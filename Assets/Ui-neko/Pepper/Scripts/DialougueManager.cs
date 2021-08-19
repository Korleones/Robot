using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public bool isfirst;
    private Queue<string> sentences;


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        isfirst = true;
    }
    
    public void StartDialogue (Dialougue dialougue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialougue.name;

        sentences.Clear();

        foreach(string sentence in dialougue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        Debug.Log(sentences.Count);
        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialougue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialougue()
    {
        animator.SetBool("IsOpen", false);
        isfirst = false;
        Debug.Log("end");
    }

}
