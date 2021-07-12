using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 4f;
    private AudioSource a;

    void Start()
    {
        a = GetComponent<AudioSource>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if((x != 0 || z != 0) && !a.isPlaying)
        {
            a.Play();
        }
        else if (x == 0 && z == 0 && a.isPlaying)
        {
            a.Stop();
        }
        controller.Move(move * speed * Time.deltaTime);
    }
}
