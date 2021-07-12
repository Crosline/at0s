using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Text subtitle;
    public AudioSource subtitleAs;

    public GameObject phone;


    public static StoryManager Instance;
    private Queue<Sentence> sentences;

    public AudioClip[] clips;

    void Awake()
    {
        Instance = this;
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
        subtitleAs.clip = sentence.clip;
        subtitleAs.Play();
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

    public void EnablePhone()
    {
        phone.layer = 6;
    }

    public void EndGame()
    {
        StartCoroutine(EndingWait());
    }

    IEnumerator EndingWait()
    {
        yield return new WaitForSeconds(10);
        //phone.GetComponent<AudioSource>().Play();
        DialogueTrigger.Instance.TriggerDialogue("Phone_Ring");
    }
}
