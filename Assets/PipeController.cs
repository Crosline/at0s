using UnityEngine;

public class PipeController : MonoBehaviour {

    public bool isComplete;

    public Pipe[] pipes;

    public int rotateTime;

    void Start() {
        for (int i = 0; i < rotateTime; i++) {
            RotatePipes();
            /*
         foreach (Pipe pipe in pipes) {
                pipe.RotatePipe();
            }    
         */
        }
    }


    public void RotatePipes() {

        bool temp = true;

        foreach (Pipe pipe in pipes) {
            pipe.RotatePipe();


            if (!pipe.isCorrect && temp) {
                temp = false;
            }
        }

        foreach (Pipe pipe in pipes) {
            if (!pipe.isCorrect) {
                temp = false;
                break;
            }
        }

        if (temp) {
            isComplete = true;
            transform.parent.parent.GetComponent<PipeExe>().CheckComplete();
        }

    }



}
