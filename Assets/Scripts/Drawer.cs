using UnityEngine;

public class Drawer : MonoBehaviour {
    void OnCollisionEnter(Collision collision) {
        collision.collider.transform.SetParent(transform);
    }

}
