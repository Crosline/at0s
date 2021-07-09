using System;
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

    [Header("Settings")]
    [SerializeField] private Camera playerCam;
    private Transform camTransform;
    [SerializeField] private float rayLength = 5f;

    [Header("Crosshair Reference")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Sprite[] crosshairs;
    /*
     * 0 for normal
     * 1 for interact crosshair
     */

    [SerializeField]
    private Color outlineColor = new Color(0, 0, 0, 1);

    [SerializeField]
    private Transform hand;

    private Outline outline;

    void Start() {
        camTransform = playerCam.transform;
    }
    void Update() {

        GameObject old = null;
        GameObject newer = null;

        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.TransformDirection(Vector3.forward), out hit, rayLength, layerMaskInteract)) {
            newer = hit.collider.gameObject;
            crosshairImage.sprite = crosshairs[1];
            outline = newer.GetComponent<Outline>();
            outline.OutlineColor = outlineColor;

        }

        if (Input.GetKeyDown(KeyCode.T) && sitting)
        {
            Stand();
        }

        if (Input.GetKeyDown(KeyCode.Q) && sitting)
        {
            ExitPC();
        }

        if (Input.GetButtonUp("Interact") && holder != null && newer == null)
        {
            DeInteract(holder);
        }

        if (Input.GetButtonUp("Interact") && holder != null && newer != null)
        {
            MergeObjects(holder, newer);
        }

        if (Input.GetButtonDown("Interact") && newer != null && holder == null) {
            if(newer.name.Contains("USB")) InteractWithUSB(newer.transform);
            if (newer.name.Contains("Chair")) SitChair();
            if (newer.name.Contains("PC") && sitting) EnterPC();
            Debug.Log(newer.name);
        }

        if(Input.GetButton("Interact") && holder != null)
        {

            InteractWithUSB(holder.transform);
        }


        if (old == newer && outline != null) {
            outline.OutlineColor = new Color(0, 0, 0, 0);
            outline = null;
            crosshairImage.sprite = crosshairs[0];
        }

        old = newer;
    }

    void MergeObjects(GameObject inserting, GameObject receiving)
    {
        inserting.transform.SetParent(null);
        inserting.layer = 6; //6 is interactable
        if(inserting.name.Contains("USB") && receiving.name.Contains("PC"))
        {
            inserting.transform.SetParent(receiving.transform);
            inserting.transform.localPosition = new Vector3(0.5f, 0.5f, -1.5f);
            inserting.transform.localEulerAngles = Vector3.zero;
            inserting.GetComponent<Animator>().enabled = true;
            inserting.GetComponent<Animator>().Play("insert", -1, 0f);
        }
        holder = null;
    }

    public void InteractWithUSB(Transform usb)
    {
        holder = usb.gameObject;
        usb.GetComponent<Rigidbody>().isKinematic = true;
        //usb.transform.localPosition = Vector3.zero;
        usb.transform.SetParent(hand);
        usb.gameObject.layer = 7; //7 is holding
        usb.GetComponent<BoxCollider>().isTrigger = true;
    }

    public void DeInteract(GameObject obj)
    {
        Debug.Log("deinteracted");
        obj.GetComponent<Rigidbody>().isKinematic = false;
        if(obj.GetComponent<Animator>()) obj.GetComponent<Animator>().enabled = false;
        obj.transform.SetParent(null);
        holder = null;
        obj.layer = 6; //6 is interactable
        obj.GetComponent<BoxCollider>().isTrigger = false;
    }

    void SitChair()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        StartCoroutine(Sit());
    }

    void Stand()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        sitting = false;
    }

    IEnumerator Sit()
    {
        Vector3 chairPos = new Vector3(12f, 2, -4);
        while (true)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, chairPos, 4f * Time.deltaTime);
            if(player.transform.position == chairPos)
            {
                break;
            }
            yield return null;
        }
        sitting = true;
    }

    void EnterPC()
    {

        //player.SetActive(false);
        StartCoroutine(SettingCamera());
    }

    void ExitPC()
    {
        StartCoroutine(ExitingPCCamera());
    }

    IEnumerator SettingCamera()
    {
        camTransform.localEulerAngles = new Vector3(10, 0, 0);
        player.transform.localEulerAngles = new Vector3(0, 90, 0);
        playerCam.GetComponent<MouseLook>().enabled = false;
        while (true)
        {
            camTransform.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(camTransform.GetComponent<Camera>().fieldOfView, 30f, 40f * Time.deltaTime);
            if (camTransform.GetComponent<Camera>().fieldOfView == 30f)
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator ExitingPCCamera()
    {
        playerCam.GetComponent<MouseLook>().enabled = true;
        while (true)
        {
            camTransform.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(camTransform.GetComponent<Camera>().fieldOfView, 60f, 80f * Time.deltaTime);
            if (camTransform.GetComponent<Camera>().fieldOfView == 60f)
            {
                break;
            }
            yield return null;
        }
    }

}