using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public TextMesh baslik;
    public TextMesh info;

    public String[] basliklar;
    public String[] infolar;

    public BoxCollider2D loading;

    void Start() {
        StartCoroutine(TutorialSequence());
    }


    IEnumerator TutorialSequence() {

        baslik.text = basliklar[0];
        info.text = infolar[0];

        yield return new WaitUntil(Walking);

        baslik.text = basliklar[1];
        info.text = infolar[1];

        yield return new WaitUntil(Attacking);

        loading.enabled = true;
        loading.GetComponent<Animator>().Play("buggy");
        
        
        DialogueTrigger.Instance.TriggerDialogue("Tutorial_Bug");

        baslik.text = basliklar[2];
        info.text = infolar[2];

        bool Walking() => Input.GetButton("Horizontal");
        bool Attacking() => Input.GetButton("Interact");
    }




}
