using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger Instance;

    void Awake()
    {
        Instance = this;
    }


    public Dialogue[] dialogues;

    public void TriggerDialogue(string dialogueName)
    {
        Dialogue sentecesToTell = null;
        foreach(Dialogue dialogue in dialogues)
        {
            if(dialogue.name == dialogueName)
            {
                sentecesToTell = dialogue;
            }
        }

        StoryManager.Instance.StartDialogue(sentecesToTell);
    }
}
