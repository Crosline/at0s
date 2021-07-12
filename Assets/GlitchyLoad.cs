using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchyLoad : MonoBehaviour
{
    public GameObject readme;
    public void FixGlitch() {

        GlitchController.Instance.Glitcher(0.3f);

        readme.SetActive(true);

        transform.parent.parent.gameObject.SetActive(false);

        DialogueTrigger.Instance.TriggerDialogue("Free_Roam");
    }
}
