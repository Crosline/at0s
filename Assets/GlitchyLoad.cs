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
        
        FindObjectOfType<PlayerMovement2D>().canJump = true;

        DialogueTrigger.Instance.TriggerDialogue("Free_Roam");
    }
}
