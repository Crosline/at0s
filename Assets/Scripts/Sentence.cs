using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    [TextArea(3, 10)]
    public string sentence;

    public float duration;
    public AudioClip clip;
}
