using System.Collections;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour {

    public CharacterController2D controller;
    private bool isDying = false;


    /*[SerializeField] private Collider2D frictionCollider;
    [SerializeField] private PhysicsMaterial2D frictionOn;
    [SerializeField] private PhysicsMaterial2D frictionOff;*/

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    // Start is called before the first frame update
    void Start() {
        controller.Move(0, true, false);
        controller.Move(0, false, false);
    }

    // Update is called once per frame
    void Update() {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        Debug.Log(horizontalMove);

        /* if (Mathf.Abs(horizontalMove) > 0.12f) {
             frictionCollider.sharedMaterial = frictionOff;
         } else {
             frictionCollider.sharedMaterial = frictionOn;
         }*/

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }


    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy") && !isDying) {
            isDying = true;
           // StartCoroutine(Die());
        }

    }


    /*private IEnumerator Die() {
        controller.animator.enabled = false;
        controller.animator.enabled = true;
        controller.animator.SetBool("Death", isDying);
        controller.animator.Play("PlayerDeath");
        controller.Move(0, false, false);
        controller.canMove = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);


        //StartCoroutine(GameplayUI.Instance.FadeEffect(true));

        yield return new WaitUntil(Waiting);
        yield return new WaitForSecondsRealtime(0.3f);

        isDying = false;
        controller.animator.SetBool("Death", isDying);
        transform.position = checkPoint;
        controller.canMove = true;
        StartCoroutine(GameplayUI.Instance.FadeEffect(false));
        yield return new WaitUntil(Waiting);

        bool Waiting() => GameplayUI.Instance.isFading;
    }*/
}
