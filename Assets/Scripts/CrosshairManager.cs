using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649


public class CrosshairManager : MonoBehaviour {

    public string m_InteractTag = "InteractObject";

    [Header("Raycast Length/Layer")]
    [SerializeField] private LayerMask layerMaskInteract;

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

        if (Input.GetButton("Interact")) {
            Debug.Log(newer.name);
        }


        if (old == newer && outline != null) {
            outline.OutlineColor = new Color(0, 0, 0, 0);
            outline = null;
            crosshairImage.sprite = crosshairs[0];
        }

        old = newer;


    }

}