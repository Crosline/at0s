using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable 0649


public class CrosshairManager : MonoBehaviour {

    public string m_InteractTag = "InteractObject";
    public GameObject player;
    private GameObject holder;
    private bool sitting;

    public GameObject interactText;
    public GameObject monitorLight;
    public GameObject pcSandalyeInteract;
    public GameObject fanObject;
    public GameObject pc;

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

    private Camera pxCam;

    [SerializeField]
    private Color outlineColor = new Color(0, 0, 0, 1);

    [SerializeField]
    private Transform hand;

    private Outline outline;

    private bool isPcOn = false;
    private bool isCursor = false;
    private bool isStandable = false;
    private bool isFirstTimeBoot = true;
    private bool isFirstTimeHitScreen = true;
    private bool isFirstTimeFanToggle = true;

    private AudioClip[] clips;

    void Start() {
        clips = StoryManager.Instance.clips;
        camTransform = playerCam.transform;
        chairPos = chair.position;
        pxCam = playerCam.transform.GetChild(2).GetComponent<Camera>();
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
                if (hit.collider.gameObject.name.Contains("USB"))
                    interactText.GetComponent<TextMeshProUGUI>().text = "Disketi tutmak için 'E'ye basılı tut";
                else if(holder != null && hit.collider.gameObject.name.Contains("PC"))
                    interactText.GetComponent<TextMeshProUGUI>().text = "Disketi takmak için 'E' tuşunu bırak";
                else
                    interactText.GetComponent<TextMeshProUGUI>().text = "Etkileşim için 'E'ye bas"; 

                

                if (!isPcOn)
                {
                    

                    interactText.SetActive(true);
                    
                }
                    
            }
        }
            


        if (Input.GetKeyDown(KeyCode.F) && isFirstTimeHitScreen && sitting)
        {
            player.GetComponent<AudioSource>().PlayOneShot(StoryManager.Instance.clips[14]);
            TestGlitch();
            isFirstTimeHitScreen = false;
            HitScreen.Instance.ShakeIt();
            DialogueTrigger.Instance.TriggerDialogue("Glitch_Fix");
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
            if (newer.name.Contains("PC") && sitting) EnterPC(pc);
            if (newer.name.Contains("Arcade")) ArcadeFunc(newer);
            if (newer.name.Contains("Couch")) CouchFunc(newer);
            if (newer.name.Contains("door")) DoorToggle(newer);
            if (newer.name.Contains("yemek")) YemekToggle(newer);
            if (newer.name.Contains("cay")) CayToggle(newer);
            if (newer.name.Contains("fan")) FanToggle(newer);
            if (newer.name.Contains("Gramaphone")) GramaphoneToggle(newer);
            if (newer.name.Contains("PF_Plak")) PinkFloydToggle(newer);
            if (newer.name.Contains("SayborgPlak")) PlakToggle(newer);
            Debug.Log(newer.name);
        }

        if (Input.GetButton("Interact") && holder != null) {

            InteractWithUSB(holder.transform);
        }


        if (old != null && old != newer && outline != null) {
            old.GetComponent<Outline>().OutlineColor = new Color(0, 0, 0, 0);
            interactText.SetActive(false);
            //outline = null;
            crosshairImage.sprite = crosshairs[0];
        }

        old = newer;



        /*if (isPcOn) {
            //Debug.Log("pc is on");

            if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit, rayLength, layerMaskComputerScreen)) {
                //Debug.Log(hit.collider.name);
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

        }*/

    }

    public IEnumerator RestartOs() {
        AudioSource a = pc.GetComponent<AudioSource>();
        a.Stop();
        a.loop = false;
        a.PlayOneShot(StoryManager.Instance.clips[4]);
        yield return new WaitForSeconds(3f);
        playerChar.SetActive(false);
        Debug.Log("test 1");
        yield return new WaitForSeconds(12f);
        Debug.Log("test 2");
        playerChar.SetActive(true);
        a.PlayOneShot(StoryManager.Instance.clips[2]);
        StartCoroutine(PCStartLoop(a, StoryManager.Instance.clips[2], StoryManager.Instance.clips[3]));
        StartCoroutine(RestartProcess());
    }

    IEnumerator RestartProcess()
    {
        OSScreen.gameObject.SetActive(false);
        Debug.Log("test 3");
        yield return new WaitForEndOfFrame();
        OSScreen.gameObject.SetActive(true);
        DialogueTrigger.Instance.TriggerDialogue("Restart_Complete");
        //playerChar.SetActive(true);
    }

    void MergeObjects(GameObject inserting, GameObject receiving) {

        if (inserting.name.Contains("USB") && receiving.name.Contains("PC")) {
            DialogueTrigger.Instance.TriggerDialogue("Insert_Disk");
            AudioSource a = pc.GetComponent<AudioSource>();
            a.PlayOneShot(StoryManager.Instance.clips[5]);
            inserting.transform.SetParent(null);
            inserting.layer = 6; //6 is interactable

            // BU KISIMLARI DA ÞÝMDÝLÝK ARÇÝN KAPATTI : insert animasyonu çalýþýyor düzgün
            //inserting.transform.SetParent(receiving.transform);
            //inserting.transform.localPosition = new Vector3(0.5f, 0.5f, -1.5f);
            //inserting.transform.localEulerAngles = Vector3.zero;

            inserting.GetComponent<Animator>().enabled = true;
            inserting.GetComponent<Animator>().Play("insert", -1, 0f);

            playerChar.SetActive(true);
            //playerChar.transform.localPosition = playerInitialPos;
            holder = null;
        } else {
            DeInteract(inserting);
        }

    }

    // ARÇÝN EKLÝYORDU BURAYI - Floppy Diski Çýkart
    public void TakeoutFloppy()
    {
        // DiskButton'a týklandýðýnda takeout animasyonunu oynat
        // Disk'i eline al
    }

    public void InteractWithUSB(Transform usb) {
        playerChar.SetActive(false);
        holder = usb.gameObject;
        usb.GetComponent<Rigidbody>().isKinematic = true;
        //usb.transform.localPosition = Vector3.zero;
        usb.transform.SetParent(hand);
        usb.transform.localPosition = Vector3.zero;
        //usb.transform.rotation = Quaternion.identity;
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

        //Open UI
        pcSandalyeInteract.SetActive(true);
        pcSandalyeInteract.transform.GetChild(1).gameObject.SetActive(true);
        pcSandalyeInteract.transform.GetChild(0).gameObject.SetActive(false);


        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<AudioSource>().Stop();
        StartCoroutine(Sit());
    }

    void Stand() {

        //Close UI
        pcSandalyeInteract.SetActive(false);

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

    void EnterPC(GameObject pc) {
        StartCoroutine(SettingCamera(pc));
    }

    void ExitPC() {
        StartCoroutine(ExitingPCCamera());
    }

    private bool isOnTransition = false;

    IEnumerator SettingCamera(GameObject pc) {
        Cursor.SetCursor(inPcCrosshairs[0], Vector2.one * 16, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursor = false;

        crosshairImage.enabled = false;

        isOnTransition = true;
        isPcOn = true;


        //playerCam.targetTexture = null;


        camTransform.localEulerAngles = new Vector3(0, 0, 0);
        player.transform.localEulerAngles = new Vector3(0, 90, 0);
        playerCam.GetComponent<MouseLook>().enabled = false;

        //Open UI
        pcSandalyeInteract.transform.GetChild(1).gameObject.SetActive(false);
        pcSandalyeInteract.transform.GetChild(0).gameObject.SetActive(true);

        Color greenColor = new Color(36f / 255.0f, 191f / 255.0f, 0f / 255.0f);
        monitorLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", greenColor);
        monitorLight.GetComponent<Renderer>().material.SetFloat("_EmissionScaleUI", 1.5f);
        monitorLight.GetComponent<Renderer>().UpdateGIMaterials();

        interactText.SetActive(false);

        outline.enabled = false;
        while (true) {
            
            playerCam.fieldOfView = Mathf.MoveTowards(playerCam.fieldOfView, 23.5f, 40f * Time.deltaTime);
            pxCam.fieldOfView = Mathf.MoveTowards(playerCam.fieldOfView, 23.5f, 40f * Time.deltaTime);
            
            if (camTransform.GetComponent<Camera>().fieldOfView == 23.5f) {
                break;
            }
            yield return null;
        }
        if(pc.TryGetComponent<AudioSource>(out AudioSource a))
        {
            if(!a.isPlaying)
            {
                a.PlayOneShot(StoryManager.Instance.clips[2]);
                StartCoroutine(PCStartLoop(a, StoryManager.Instance.clips[2], StoryManager.Instance.clips[3]));
            }
        }
        monitorButton.EnableKeyword("_EMISSION");
        OSScreen.SetActive(true);
        playerChar.GetComponent<PlayerMovement2D>().enabled = true;
        
        isOnTransition = false;
        if (isFirstTimeBoot) {
            DialogueTrigger.Instance.TriggerDialogue("Glitch_Error");
            isFirstTimeBoot = false;
        }
    }

    IEnumerator PCStartLoop(AudioSource a, AudioClip clip1, AudioClip clip2)
    {
        yield return new WaitForSeconds(clip1.length);
        a.clip = clip2;
        a.loop = true;
        a.Stop();
        a.Play();
    }

    //public RenderTexture outTexture;

    IEnumerator ExitingPCCamera() {

        //open UI
        pcSandalyeInteract.transform.GetChild(1).gameObject.SetActive(true);
        pcSandalyeInteract.transform.GetChild(0).gameObject.SetActive(false);

        playerChar.GetComponent<PlayerMovement2D>().enabled = false;
        Cursor.SetCursor(inPcCrosshairs[1], Vector2.one, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursor = true;
        crosshairImage.enabled = true;
        outline.enabled = true;
        isPcOn = false;

        /*if (outTexture != null)
            playerCam.targetTexture = outTexture;*/



        playerCam.GetComponent<MouseLook>().enabled = true;
        while (true) {
            playerCam.fieldOfView = Mathf.MoveTowards(playerCam.fieldOfView, 60f, 80f * Time.deltaTime);
            pxCam.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(playerCam.fieldOfView, 60f, 80f * Time.deltaTime);
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
            if (drawer.GetComponent<AudioSource>())
            {
                drawer.GetComponent<AudioSource>().PlayOneShot(StoryManager.Instance.clips[1]);
            }
            return;
        }
        if(drawer.GetComponent<AudioSource>())
        {
            drawer.GetComponent<AudioSource>().PlayOneShot(StoryManager.Instance.clips[0]);
        }
        anim.Play("open", -1, 0f);
        drawer.GetComponent<Drawer>().isOpen = true;
    }

    void TestGlitch()
    {
        GlitchController.Instance.ToggleGlitch();
    }

    void ArcadeFunc(GameObject arcade)
    {
        DialogueTrigger.Instance.TriggerDialogue("Interact_Arcade");
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
            door.GetComponent<AudioSource>().PlayOneShot(StoryManager.Instance.clips[6]);
        } else {
            anim.Play("close", -1, 0f);
            door.GetComponent<Door>().isOpen = !toggler;
            door.GetComponent<AudioSource>().PlayOneShot(StoryManager.Instance.clips[7]);
        }
    }

    void YemekToggle(GameObject yemek)
    {
        yemek.layer = 0;
        DialogueTrigger.Instance.TriggerDialogue("Interact_Yemek");
    }

    void CayToggle(GameObject caydanlik)
    {
        caydanlik.layer = 0;
        DialogueTrigger.Instance.TriggerDialogue("Interact_Cay");
    }

    void PinkFloydToggle(GameObject pfPlak)
    {
        pfPlak.layer = 0;
        DialogueTrigger.Instance.TriggerDialogue("Interact_PF");
    }

    void PlakToggle(GameObject plak)
    {
        plak.layer = 0;
        DialogueTrigger.Instance.TriggerDialogue("Interact_Plak2");
    }

    void enableFanInteract()
    {
        fanObject.layer = 6;
    }

    void FanToggle(GameObject fan)
    {
        if(isFirstTimeFanToggle)
        {
            DialogueTrigger.Instance.TriggerDialogue("Heat_Fix");
            isFirstTimeFanToggle = false;
        }
        Animator anim;
        fan.GetComponent<Animator>().enabled = true;
        anim = fan.GetComponent<Animator>();
        bool toggler = fan.GetComponent<Fan>().isOpen;
        if (!toggler)
        {
            anim.Play("FanStart");
            fan.GetComponent<Fan>().isOpen = !toggler;
        }
        else
        {
            anim.Play("FanStop");
            fan.GetComponent<Fan>().isOpen = !toggler;
        }
        fan.GetComponent<AudioSource>().PlayOneShot(StoryManager.Instance.clips[8]);
    }

    void GramaphoneToggle(GameObject gram)
    {
        bool toggler = gram.GetComponent<Gramaphone>().isOpen;
        if (!toggler)
        {
            gram.GetComponent<Gramaphone>().playGramaphone();
        }
        else
        {
            gram.GetComponent<Gramaphone>().stopGramaphone();
        }
    }
}