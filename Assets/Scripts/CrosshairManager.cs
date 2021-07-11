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
    [SerializeField] private Transform chair;
    [SerializeField] private Material monitorButton;
    private Vector3 chairPos;
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
        chairPos = chair.position;
    }

    GameObject old = null;

    void Update() {

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

        if(Input.GetKeyDown(KeyCode.F))
        {
            TestGlitch();
        }

        if (Input.GetKeyDown(KeyCode.Q) && sitting && !isPcOn) {
            Stand();
        }

        if (Input.GetKeyDown(KeyCode.Q) && sitting && !isOnTransition) {
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
            if (newer.name.Contains("Drawer") || newer.name.Contains("Handler")) ToggleDrawer(newer);
            if (newer.name.Contains("PC") && sitting) EnterPC();
            if (newer.name.Contains("Arcade")) ArcadeFunc(newer);
            if (newer.name.Contains("Couch")) CouchFunc(newer);
            if (newer.name.Contains("door")) DoorToggle(newer);
            if (newer.name.Contains("yemek")) YemekToggle(newer);
            if (newer.name.Contains("cay")) CayToggle(newer);
            Debug.Log(newer.name);
        }

        if (Input.GetButton("Interact") && holder != null) {

            InteractWithUSB(holder.transform);
        }


        if (old != null && old != newer && outline != null) {
            old.GetComponent<Outline>().OutlineColor = new Color(0, 0, 0, 0);
            //outline = null;
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

        if (inserting.name.Contains("USB") && receiving.name.Contains("PC")) {
            inserting.transform.SetParent(null);
            inserting.layer = 6; //6 is interactable

            // BU KISIMLARI DA ��MD�L�K AR��N KAPATTI : insert animasyonu �al���yor d�zg�n
            //inserting.transform.SetParent(receiving.transform);
            //inserting.transform.localPosition = new Vector3(0.5f, 0.5f, -1.5f);
            //inserting.transform.localEulerAngles = Vector3.zero;

            inserting.GetComponent<Animator>().enabled = true;
            inserting.GetComponent<Animator>().Play("insert", -1, 0f);

            playerChar.SetActive(true);
            playerChar.transform.localPosition = playerInitialPos;
            holder = null;
        } else {
            DeInteract(inserting);
        }

    }

    // AR��N EKL�YORDU BURAYI - Floppy Diski ��kart
    public void TakeoutFloppy()
    {
        // DiskButton'a t�kland���nda takeout animasyonunu oynat
        // Disk'i eline al
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
        chairPos = new Vector3(chairPos.x, 2, chairPos.z);
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

    private bool isOnTransition = false;

    IEnumerator SettingCamera() {
        Cursor.SetCursor(inPcCrosshairs[0], Vector2.one / 2f, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursor = false;

        crosshairImage.enabled = false;

        isOnTransition = true;
        isPcOn = true;
        camTransform.localEulerAngles = new Vector3(0, 0, 0);
        player.transform.localEulerAngles = new Vector3(0, 90, 0);
        playerCam.GetComponent<MouseLook>().enabled = false;
        outline.enabled = false;
        while (true) {
            camTransform.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(camTransform.GetComponent<Camera>().fieldOfView, 25f, 40f * Time.deltaTime);
            if (camTransform.GetComponent<Camera>().fieldOfView == 25f) {
                break;
            }
            yield return null;
        }
        monitorButton.EnableKeyword("_EMISSION");
        OSScreen.SetActive(true);
        playerChar.GetComponent<PlayerMovement2D>().enabled = true;
        isOnTransition = false;
    }

    IEnumerator ExitingPCCamera() {
        playerChar.GetComponent<PlayerMovement2D>().enabled = false;
        Cursor.SetCursor(inPcCrosshairs[0], Vector2.one / 2f, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursor = true;
        crosshairImage.enabled = true;
        outline.enabled = true;
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
        Vector3 standPos = new Vector3(chairPos.x - 0.5f, 2, chairPos.z +1.75f);
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

    void ToggleDrawer(GameObject drawer) {
        if (drawer.name.Contains("Handler")) 
        {
            drawer = drawer.transform.parent.gameObject;
        }
        drawer.GetComponent<Animator>().enabled = true;
        Animator anim = drawer.GetComponent<Animator>();
        if(drawer.GetComponent<Drawer>().isOpen)
        {
            anim.Play("close", -1, 0f);
            drawer.GetComponent<Drawer>().isOpen = false;
            return;
        }
        anim.Play("open", -1, 0f);
        drawer.GetComponent<Drawer>().isOpen = true;
    }

    void CloserDrawer() {
        drawer.Play("close", -1, 0f);
        drawerOpen = false;
    }

    void TestGlitch()
    {
        GlitchController.Instance.ToggleGlitch();
    }

    void ArcadeFunc(GameObject arcade)
    {
        Debug.Log("i think i need to fix this machine");
        arcade.layer = 0;
    }

    void CouchFunc(GameObject couch)
    {
        Debug.Log("there is no time to rest");
        couch.layer = 0;
    }


    void DoorToggle(GameObject door) 
    {
        Animator anim;
        door.GetComponent<Animator>().enabled = true;
        anim = door.GetComponent<Animator>();
        bool toggler = door.GetComponent<Door>().isOpen;
        if (!toggler)
        {
            anim.Play("open", -1, 0f);
            door.GetComponent<Door>().isOpen = !toggler;
        } else {
            anim.Play("close", -1, 0f);
            door.GetComponent<Door>().isOpen = !toggler;
        }
    }

    void YemekToggle(GameObject yemek)
    {
        yemek.layer = 0;
        Debug.Log("kendimi ac hissetmiyorum");
    }

    void CayToggle(GameObject caydanlik)
    {
        caydanlik.layer = 0;
        Debug.Log("cay icecek vakit yok");
    }

}