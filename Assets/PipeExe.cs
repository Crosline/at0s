using UnityEngine;

public class PipeExe : MonoBehaviour {


    public PipeController[] pipes;

    public GameObject gizli3;


    public void CheckComplete() {
        bool temp = true;

        foreach (PipeController pipe in pipes) {

            if (!pipe.isComplete && temp) {
                temp = false;
                break;
            }
        }

        if (temp) {

            GlitchController.Instance.Glitcher(0.3f);

            gizli3.SetActive(true);
            PlayerPrefs.SetInt("gizli3", 1);

            GetComponent<SecretFileController>().EnableAll();

            gameObject.SetActive(false);

        }

    }
}
