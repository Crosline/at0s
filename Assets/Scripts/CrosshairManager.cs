using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public Image crosshair;
    public Sprite originalCrosshair;
    public Sprite handCrosshair;


    

    
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 5f))
        {
            var selection = hit.transform;
            if(selection.GetComponent<Test>())
            {
                crosshair.sprite = handCrosshair;
            }
            else
            {
                crosshair.sprite = originalCrosshair;
            }
        }
        else
        {
            crosshair.sprite = originalCrosshair;
        }
    }
}
