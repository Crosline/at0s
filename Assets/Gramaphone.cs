using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gramaphone : MonoBehaviour
{

    public bool isOpen;
    public AudioSource audioSource;
    public Animator gramAnim;

    public AudioClip loopClip;


    private bool startClipEnd;


    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        gramAnim.speed = 0f;
    }

    private void Update()
    {
        if (audioSource.time >= audioSource.clip.length && !startClipEnd)
            changeToLoop();
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

    private void changeToLoop()
    {
        if (!startClipEnd)
        {
            startClipEnd = true;
            audioSource.clip = loopClip;
            audioSource.Play();
            audioSource.loop = true;
        }
    }

}
