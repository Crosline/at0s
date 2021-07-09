using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649


public class CrosshairManager : MonoBehaviour {

    public string m_InteractTag = "InteractObject";
    public GameObject player;
    private GameObject holder;

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

        if (Input.GetButtonUp("Interact") && holder != null)
        {
            DeInteract(holder);
        }

        if (Input.GetButton("Interact") && newer != null && holder == null) {
            InteractWithUSB(newer.transform);
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

    public void InteractWithUSB(Transform usb)
    {
        holder = usb.gameObject;
        usb.GetComponent<Rigidbody>().isKinematic = true;
        //usb.transform.localPosition = Vector3.zero;
        usb.transform.SetParent(hand);
        usb.gameObject.layer = 7; //7 is holding
    }

    public void DeInteract(GameObject obj)
    {
        Debug.Log("deinteracted");
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.transform.SetParent(null);
        holder = null;
        obj.layer = 6; //6 is interactable
    }

}