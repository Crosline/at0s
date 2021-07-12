using UnityEngine;

public class PipeExe : MonoBehaviour {


    public PipeController[] pipes;

    public GameObject gizli3;


    public void CheckComplete() {
        bool temp = true;

        foreach (PipeController pipe in pipes)
        {

            if (!pipe.isComplete)
            {
                temp = false;
                break;
            }
        }

        if (temp) {

            GlitchController.Instance.Glitcher(0.3f);
            DialogueTrigger.Instance.TriggerDialogue("Pipe_End");
            gizli3.SetActive(true);
            PlayerPrefs.SetInt("gizli3", 1);

            GetComponent<SecretFileController>().EnableAll();

            //gameObject.SetActive(false);
            Destroy(gameObject, 0.1f);
        }

    }
}
