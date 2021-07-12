using UnityEngine;

public class ActivateAttack : MonoBehaviour {

    //private bool inTrigger = false;

    public GameObject snekExe;

    private bool firstHitOnInternetExplorer = true;



    // Update is called once per frame
    public void Activate() {
        if (name.Contains("Hour")) {
            GetComponent<Timer>().IncreaseTimeOffset();
        }
        else if (name.Contains("Controller")) {
            Snake snake = transform.parent.parent.GetChild(2).GetComponent<Snake>();
            if (name.Contains("bottom")) {
                snake.UpdateDirection(1);
            } else if (name.Contains("right")) {
                snake.UpdateDirection(2);
            } else if (name.Contains("left")) {
                snake.UpdateDirection(3);
            } else if (name.Contains("top")) {
                snake.UpdateDirection(4);
            }
        } else if (name.Contains("read")) {
            DialogueTrigger.Instance.TriggerDialogue("Open_Readme");
            PopOS.Instance.PopUp(0, 25);
        } else if (name.Contains("mypc")) {
            GlitchController.Instance.Glitcher(0.2f);
        } else if (name.Contains("trash")) {
            StartCoroutine(GetComponent<CameraShake>().Shake());
        } else if (name.Contains("internet")) {
            if(firstHitOnInternetExplorer)
            {
                DialogueTrigger.Instance.TriggerDialogue("Internet_Explorer");
                firstHitOnInternetExplorer = false;
            }
            GetComponent<InternetExplorer>().IncreaseError();
        } else if (name.Contains("loading")) {
            GetComponent<GlitchyLoad>().FixGlitch();
        } else if (name.Contains("snake")) {
            GlitchController.Instance.Glitcher(0.3f);
            DialogueTrigger.Instance.TriggerDialogue("Snake_Intro");
            snekExe.SetActive(true);
            FindObjectOfType<PlayerMovement2D>().ResetAll(transform.position + new Vector3(1f, .7f, 0f));
        } else if (name.Contains("gizli1")) {
            PopOS.Instance.PopUp(3, 20);
        } else if (name.Contains("gizli2")) {
            FindObjectOfType<PlayerMovement2D>().ResetAll(transform.position + new Vector3(-8f, -7f, 0f));
            PopOS.Instance.PopUp(4, 15);
        } else if (name.Contains("gizli3")) {
            PopOS.Instance.PopUp(6, 25);
            } else if (name.Contains("gizli4")) {
            if (PlayerPrefs.GetInt("gizli1", 0) == 1 && PlayerPrefs.GetInt("gizli2", 0) == 1 && PlayerPrefs.GetInt("gizli3", 0) == 1) {
                //telefon çalması burayaaaaaa
                //OYUNU BİTİR.
                Debug.Log("Oyunu bitir. // dosyayı okurken telefon çalar");
                PopOS.Instance.PopUp(5, 25);
            } else {
                GlitchController.Instance.Glitcher(0.1f);
                PopOS.Instance.PopUp(8, 2);
                FindObjectOfType<PlayerMovement2D>().ResetAll(transform.position + new Vector3(-8f, -7f, 0f));
            }
        } else if (name.Contains("no")) {
            GlitchController.Instance.Glitcher(0.1f);
            PopOS.Instance.PopUp(2, 1);
        } else if (name.Contains("pipcc")) {
            GetComponent<PipeController>().RotatePipes();
        } else if (name.Contains("credits")) {
            PopOS.Instance.PopUp(7, 5);
        } else if (name.Contains("mayin")) {
            GetComponent<Animator>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GlitchController.Instance.Invoke(
            "Glitcher", 0.5f);
            Destroy(this.gameObject, 0.7f);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && !inTrigger) {
            inTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && !inTrigger) {
            inTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && inTrigger) {
            inTrigger = false;
        }
    }*/
}
