using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerManager : MonoBehaviour
{


    public Animator boxAnimator;
    public Animator fanAnimator;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fanStart());
        StartCoroutine(openBox());
    }

    IEnumerator openBox()
    {
        yield return new WaitForSeconds(14f);
        boxAnimator.Play("boxOpen");
    }

    IEnumerator fanStart()
    {
        yield return new WaitForSeconds(1f);
        fanAnimator.Play("FanStart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
