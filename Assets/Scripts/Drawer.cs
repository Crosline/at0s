using UnityEngine;

public class Drawer : MonoBehaviour {

    public bool isOpen = false;

    void OnCollisionEnter(Collision collision) {
        collision.collider.transform.SetParent(transform);
    }

}
