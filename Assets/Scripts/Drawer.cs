using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        collision.collider.transform.SetParent(transform);
    }
}
