using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649


public class CrosshairManager : MonoBehaviour {

    public string m_InteractTag = "InteractObject";
    public GameObject player;
    private GameObject holder;
    private bool sitting;

    [Header("Raycast Length/Layer")]
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private LayerMask layerMaskHolding;
    [SerializeField] private LayerMask layerMaskComputerScreen;
    [SerializeField] private LayerMask layerMaskExclude;

    [Header("Settings")]
    [SerializeField] private Camera playerCam;
    private Transform camTransform;
    [SerializeField] private float rayLength = 5f;

    [Header("Crosshair Reference")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Sprite[] crosshairs;
    [SerializeField] private Texture2D[] inPcCrosshairs;
    /*
     * 0 for normal
     * 1 for interact crosshair
     */


    [Header("PC Screen References and Settings")]
    [SerializeField] private GameObject OSScreen;
    [SerializeField] private GameObject playerChar;
    //Every time usb sticks to pc, playerChar teleport to the playerInitialPos
    public Vector3 playerInitialPos;
    [SerializeField] private Animator drawer;
    private bool drawerOpen = false;

    [SerializeField]
    private Color outlineColor = new Color(0, 0, 0, 1);

    [SerializeField]
    private Transform hand;

    private Outline outline;

    private bool isPcOn = false;
    private bool isCursor = false;
    private bool isStandable = false;

    void Start() {
        camTransform = playerCam.transform;
    }
    void Update() {

        GameObject old = null;
        GameObject newer = null;

        RaycastHit hit;

        //int mask = 1 << layerMaskExclude | layerMaskInteract.value;

        if (Physics.Raycast(camTransform.position, camTransform.TransformDirection(Vector3.forward), out hit, rayLength, layerMaskExclude)) {
            if (layerMaskInteract == (layerMaskInteract | 1 << hit.collider.gameObject.layer)) {
                newer = hit.collider.gameObject;
                crosshairImage.sprite = crosshairs[1];
                outline = newer.GetComponent<Outline>();
                outline.OutlineColor = outlineColor;
            }

        }

        if (Input.GetKeyDown(KeyCode.Q) && sitting && !isPcOn) {
            Stand();
        }

        if (Input.GetKeyDown(KeyCode.Q) && sitting) {
            ExitPC();
        }

        if (Input.GetButtonUp("Interact") && holder != null && newer == null) {
            DeInteract(holder);
        }

        if (Input.GetButtonUp("Interact") && holder != null && newer != null) {
            MergeObjects(holder, newer);
        }

        if (Input.GetButtonDown("Interact") && newer != null && holder == null) {
            if (newer.name.Contains("USB")) InteractWithUSB(newer.transform);
            if (newer.name.Contains("Chair")) SitChair();
            if (newer.name.Contains("Drawer") || newer.name.Contains("Handler")) OpenDrawer();
            if (newer.name.Contains("PC") && sitting) EnterPC();
            Debug.Log(newer.name);
        }

        if (Input.GetButton("Interact") && holder != null) {

            InteractWithUSB(holder.transform);
        }


        if (old == newer && outline != null) {
            outline.OutlineColor = new Color(0, 0, 0, 0);
            outline = null;
            crosshairImage.sprite = crosshairs[0];
        }

        old = newer;



        if (isPcOn) {
            Debug.Log("pc is on");

            if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit, rayLength, layerMaskComputerScreen)) {
                Debug.Log(hit.collider.name);
                if (!isCursor) {
                    Cursor.SetCursor(inPcCrosshairs[1], Vector2.zero, CursorMode.Auto);
                    isCursor = true;
                }
            } else {
                if (isCursor) {
                    Cursor.SetCursor(inPcCrosshairs[0], Vector2.one * 16, CursorMode.Auto);
                    isCursor = false;
                }
            }

        }

    }

    void MergeObjects(GameObject inserting, GameObject receiving) {
        inserting.transform.SetParent(null);
        inserting.layer = 6; //6 is interactable
        if (inserting.name.Contains("USB") && receiving.name.Contains("PC")) {
            inserting.transform.SetParent(receiving.transform);
            inserting.transform.localPosition = new Vector3(0.5f, 0.5f, -1.5f);
            inserting.transform.localEulerAngles = Vector3.zero;
            inserting.GetComponent<Animator>().enabled = true;
            inserting.GetComponent<Animator>().Play("insert", -1, 0f);
        }
        playerChar.SetActive(true);
        playerChar.transform.localPosition = playerInitialPos;
        holder = null;
    }

    public void InteractWithUSB(Transform usb) {
        playerChar.SetActive(false);
        holder = usb.gameObject;
        usb.GetComponent<Rigidbody>().isKinematic = true;
        //usb.transform.localPosition = Vector3.zero;
        usb.transform.SetParent(hand);
        usb.gameObject.layer = 7; //7 is holding
        usb.GetComponent<BoxCollider>().isTrigger = true;
    }

    public void DeInteract(GameObject obj) {
        Debug.Log("deinteracted");
        obj.GetComponent<Rigidbody>().isKinematic = false;
        if (obj.GetComponent<Animator>()) obj.GetComponent<Animator>().enabled = false;
        obj.transform.SetParent(null);
        holder = null;
        obj.layer = 6; //6 is interactable
        obj.GetComponent<BoxCollider>().isTrigger = false;
    }

    void SitChair() {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        StartCoroutine(Sit());
    }

    void Stand() {
        playerChar.GetComponent<PlayerMovement2D>().enabled = false;
        Cursor.SetCursor(inPcCrosshairs[0], Vector2.one / 2f, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        crosshairImage.enabled = true;
        StartCoroutine(Standing());
    }

    IEnumerator Sit() {
        Vector3 chairPos = new Vector3(12f, 2, -4);
        while (true) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, chairPos, 4f * Time.deltaTime);
            if (player.transform.position == chairPos) {
                break;
            }
            yield return null;
        }
        sitting = true;
    }

    void EnterPC() {
        StartCoroutine(SettingCamera());
    }

    void ExitPC() {
        StartCoroutine(ExitingPCCamera());
    }

    IEnumerator SettingCamera() {
        Cursor.SetCursor(inPcCrosshairs[0], Vector2.one / 2f, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        crosshairImage.enabled = false;


        isPcOn = true;
        camTransform.localEulerAngles = new Vector3(10, 0, 0);
        player.transform.localEulerAngles = new Vector3(0, 90, 0);
        playerCam.GetComponent<MouseLook>().enabled = false;
        while (true) {
            camTransform.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(camTransform.GetComponent<Camera>().fieldOfView, 30f, 40f * Time.deltaTime);
            if (camTransform.GetComponent<Camera>().fieldOfView == 30f) {
                break;
            }
            yield return null;
        }
        OSScreen.SetActive(true);
        playerChar.GetComponent<PlayerMovement2D>().enabled = true;
    }

    IEnumerator ExitingPCCamera() {
        playerChar.GetComponent<PlayerMovement2D>().enabled = false;
        Cursor.SetCursor(inPcCrosshairs[0], Vector2.one / 2f, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        crosshairImage.enabled = true;

        isPcOn = false;
        playerCam.GetComponent<MouseLook>().enabled = true;
        while (true) {
            camTransform.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(camTransform.GetComponent<Camera>().fieldOfView, 60f, 80f * Time.deltaTime);
            if (camTransform.GetComponent<Camera>().fieldOfView == 60f) {
                break;
            }
            yield return null;
        }
    }

    IEnumerator Standing() {
        Vector3 standPos = new Vector3(11.5f, 2, -2.75f);
        while (true) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, standPos, 4f * Time.deltaTime);
            if (player.transform.position == standPos) {
                break;
            }
            yield return null;
        }
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        sitting = false;
    }

    void OpenDrawer() {
        if (drawerOpen) {
            CloserDrawer();
            return;
        }
        drawerOpen = true;
        drawer.enabled = true;
        drawer.Play("open", -1, 0f);
    }

    void CloserDrawer() {
        drawer.Play("close", -1, 0f);
        drawerOpen = false;
    }

}