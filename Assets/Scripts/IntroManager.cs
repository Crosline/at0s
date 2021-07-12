using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogueTrigger.Instance.TriggerDialogue("BeginGame");
        StartCoroutine(endCutscene(53f));
    }


    IEnumerator endCutscene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(2);
    }
}
