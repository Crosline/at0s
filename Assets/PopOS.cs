using System.Collections;
using UnityEngine;

public class PopOS : MonoBehaviour {

    public static PopOS Instance;

    public GameObject[] triggerControllers;

    int lastUsed = 0;

    bool isOn = false;

    /*
     * 0 : ReadMe
     * 1 : OverHeat
     * 2 : SistemHatasi
     * 3 : MaviEkran
     */

    // Start is called before the first frame update
    void Start() {
        Instance = this;
    }

    void Update() {
        if (Input.GetButtonDown("Interact") && isOn) {
            ClosePopUps();
        }
    }


    public void PopUp(int popUpChild = 0, float screenDuration = 5f) {

        lastUsed = popUpChild;

        triggerControllers[0].SetActive(true);
        triggerControllers[1].SetActive(false);

        StartCoroutine(OpenPopUp(popUpChild, screenDuration));


    }


    private IEnumerator OpenPopUp(int popUpChild = 0, float screenDuration = 5f) {
        isOn = true;
        PlayerMovement2D[] controller = FindObjectsOfType<PlayerMovement2D>();

        for (int i = 0; i < controller.Length; i++) {
            controller[i].controller.canMove = false;
            controller[i].controller.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            controller[i].GetComponent<Animator>().Play("Idle");
            controller[i].StopAllCoroutines();

        }

        GameObject a =
        transform.GetChild(popUpChild).gameObject;

        a.SetActive(true);

        yield return new WaitForSeconds(screenDuration);

        ClosePopUps();
    }

    private void ClosePopUps() {


        GameObject a =
        transform.GetChild(lastUsed).gameObject;
        a.SetActive(false);

        StopAllCoroutines();

        StartCoroutine(StartController());


        isOn = false;

    }

    private IEnumerator StartController() {

        yield return new WaitForSeconds(0.2f);

        PlayerMovement2D[] controller = FindObjectsOfType<PlayerMovement2D>();

        for (int i = 0; i < controller.Length; i++) {
            controller[i].controller.canMove = true;
            controller[i].controller.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            controller[i].GetComponent<Animator>().Play("Idle");
        }
    }



}
