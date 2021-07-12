using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        DialogueTrigger.Instance.TriggerDialogue("AfterIntro");
        StartCoroutine(Tsdfkjg());
    }

    IEnumerator Tsdfkjg()
    {
        yield return new WaitForSeconds(48);
        FindObjectOfType<CrosshairManager>().canInteract = true;
    }
}
