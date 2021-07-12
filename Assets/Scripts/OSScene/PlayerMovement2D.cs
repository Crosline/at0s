using System.Collections;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour {

    public CharacterController2D controller;
    private bool isDying = false;


    /*[SerializeField] private Collider2D frictionCollider;
    [SerializeField] private PhysicsMaterial2D frictionOn;
    [SerializeField] private PhysicsMaterial2D frictionOff;*/

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    public bool canJump = false;
    
    public bool canAttack = true;
    public bool canCrouch = true;

    void Init() {

        isDying = false;
        jump = false;
        crouch = false;

        if (controller == null) {
            controller = GetComponent<CharacterController2D>();
        }

        controller.Move(0, true, false);
        controller.Move(0, false, false);
    }


    // Start is called before the first frame update
    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update() {

        horizontalMove = Input.GetAxisRaw("Horizontal");

        /* if (Mathf.Abs(horizontalMove) > 0.12f) {
             frictionCollider.sharedMaterial = frictionOff;
         } else {
             frictionCollider.sharedMaterial = frictionOn;
         }*/

        if (Input.GetButtonDown("Jump") && canJump) {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch") && canCrouch) {
            crouch = true;
        }

        if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }

        if (Input.GetButtonDown("Interact") && controller.canMove && canAttack) {
            AudioSource a = GetComponent<AudioSource>();
            a.PlayOneShot(StoryManager.Instance.clips[12]);
            controller.canMove = false;
            controller.animator.Play("Attack");
            StartCoroutine(RestartMove());
            if (controller.GetGrounded())
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }


    }

    public void ResetAll() {

        Init();
        transform.rotation = Quaternion.identity;
        controller.ResetAll(transform.position);
    }
    public void ResetAll(Vector3 newPos) {

        Init();
        transform.rotation = Quaternion.identity;
        controller.ResetAll(newPos);
    }


    private IEnumerator RestartMove() {
        yield return new WaitForSeconds(0.83f);
        controller.canMove = true;
        controller.animator.Play("Idle");
    }

    public void StopAttack() {
        controller.canMove = true;
    }

    void FixedUpdate() {

        if (transform.localPosition.x > 10.5f) {
            transform.localPosition = new Vector3(-10f, transform.localPosition.y, transform.localPosition.z);
        } else if (transform.localPosition.x < -10.5f) {
            transform.localPosition = new Vector3(10f, transform.localPosition.y, transform.localPosition.z);
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy") && !isDying) {
            isDying = true;
            // StartCoroutine(Die());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Application")) {
            //if (collision.collider.transform.position.y < transform.position.y) {
                if (collision.collider.gameObject.TryGetComponent<CameraShake>(out CameraShake shake)) {
                    StartCoroutine(shake.Shake());
                }
                if (collision.collider.name.Contains("mayin")) {
        Debug.Log("MAYINA BASTIK ADAMIM");
            //if (collision.collider.transform.position.y < transform.position.y) {
                if (collision.collider.gameObject.TryGetComponent<ActivateAttack>(out ActivateAttack at)) {
                at.Activate();
                }
            //}
        }
            //}
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
