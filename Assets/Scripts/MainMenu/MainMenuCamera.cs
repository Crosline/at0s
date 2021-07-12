using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public int camBehav = 0; // indicates the  camera behaviors
    // camBehav == 0, initial cam behav
    // camBehav == 1, graphichs
    // camBehav == 2, audio
    // camBehav == 3, credits

    public float speed = 5f;
    public float height = 2f;
    public float circleRadius = 3.0f;
    public Transform targetObject;

    private Vector3 initialPos;
    private Vector3 initialRotat;

    private void Awake()
    {
        initialPos = transform.position;
        initialRotat = transform.eulerAngles;
    }


    void Update()
    {
        camBehaviour();

    }

    

    void camBehaviour()
    {
        
        Vector3 finalPos;
        Quaternion finalRotation;
        switch (camBehav)
        {

            case 0:     // initial
                initialBehavior();
                break;

            case 1:     // pan to video
                finalPos = new Vector3(9.8f, 3.5f, -19.2f);
                finalRotation = Quaternion.Euler(-2.93f, -0.77f, -51.06f);
                panToSetting(finalPos, finalRotation, 1.5f);
                break;

            case 2:      // pan to audio
                finalPos = new Vector3(8.41f, 1.66f, -23.38f);
                finalRotation = Quaternion.Euler(0.5f, 23.36f, -7.3f);
                panToSetting(finalPos, finalRotation, 1.5f);
                break;

            case 3:     // pan to credits
                finalPos = new Vector3(5.36f, 1.69f, 17.21f);
                finalRotation = Quaternion.Euler(0.3f, -181.66f, 0f);
                panToSetting(finalPos, finalRotation, 2.5f);
                break;

            case 4:      // pan to initial
                panBackToInitial();
                break;

            case 5:     // pan to how to play
                finalPos = transform.position;
                finalRotation = Quaternion.Euler(-80f, transform.eulerAngles.y, transform.eulerAngles.z);
                panToSetting(finalPos, finalRotation, 2.5f);
                break;
        }
    }

    void initialBehavior()
    {
        transform.position = initialPos;
        transform.eulerAngles = initialRotat;
    }
    


    void panToSetting(Vector3 finalPos, Quaternion finalRotation, float panSpeed)
    {
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * panSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * panSpeed);
    }

    void panBackToInitial()
    {
        if (Vector3.Distance(transform.position, initialPos) > 0.003f)
        {
            transform.position = Vector3.Lerp(transform.position, initialPos, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(initialRotat), Time.deltaTime * 5f);
        }
        else
        {
            camBehav = 0;
        }
            
    }
    
}
