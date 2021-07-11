using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Text subtitle;


    public static StoryManager Instance;

    void Awake()
    {
        Instance = this;
    }

    private Queue<Sentence> sentences;

    void Start()
    {
        sentences = new Queue<Sentence>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if(dialogue == null)
        {
            return;
        }
        Debug.Log("starting dialogue" + dialogue.name);

        StopAllCoroutines();
        sentences.Clear();


        foreach(Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            //Debug.Log(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }
        Sentence sentence = sentences.Dequeue();
        subtitle.text = sentence.sentence;
        StartCoroutine(WaitForSentence(sentence.duration));
    }

    IEnumerator WaitForSentence(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DisplayNextSentence();
    }

    void EndDialogue()
    {
        subtitle.text = "";
        Debug.Log("end of conversation");
    }


}
