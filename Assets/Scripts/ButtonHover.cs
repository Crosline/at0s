using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{

    public AudioSource audioSource;

    public void OnPointerEnter(PointerEventData ped)
    {
        audioSource.Play();
    }

}