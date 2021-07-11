using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gramaphone : MonoBehaviour
{

    public bool isOpen;
    public AudioSource audioSource;
    public Animator gramAnim;


    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        gramAnim.speed = 0f;
    }

    public void playGramaphone()
    {
        isOpen = true;
        audioSource.Play();
        gramAnim.speed = 1f;

    }

    public void stopGramaphone()
    {
        isOpen = false;
        audioSource.Pause();
        gramAnim.speed = 0f;
    }


}
