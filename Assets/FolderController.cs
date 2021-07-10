using System.Collections;
using UnityEngine;

public class FolderController : MonoBehaviour {
    [SerializeField]
    private Transform player;

    private BoxCollider2D collider2D;
    private SpriteRenderer spriteRenderer;

    public Vector3 posOffset;

    private bool isOnTop = true;

    private bool canRender = true;

    private bool inTrigger = false;

    // Start is called before the first frame update
    void Awake() {
        if (collider2D == null) {
            collider2D = gameObject.GetComponent<BoxCollider2D>();
        }
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("2DPlayer").transform;
        }
        if (spriteRenderer == null) {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (canRender) {
            if (player.transform.position.y > transform.position.y && isOnTop) {
                spriteRenderer.sortingOrder = 3;
                isOnTop = false;


            } else if (player.transform.position.y < transform.position.y && !isOnTop) {
                spriteRenderer.sortingOrder = 1;
                isOnTop = true;
            }
        }

    }

    void Update() {
        if (Input.GetButtonDown("Crouch")) {

            if (inTrigger) {

                Debug.Log("PLAYERPOS");

                if (player.TryGetComponent<Animator>(out Animator anim)) {
                    player.GetComponent<CharacterController2D>().canMove = false;
                    player.GetComponent<BoxCollider2D>().enabled = false;
                    player.GetComponent<Rigidbody2D>().isKinematic = true;
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    anim.SetBool("EnterFolder", true);
                    anim.Play("EnterFolder");
                    canRender = false;
                    StartCoroutine(EnterFolder());
                }
            }
        }
    }

    IEnumerator EnterFolder() {
        player.position = transform.position + posOffset;

        while (player.position.y > transform.position.y - 0.5f) {

            player.position -= new Vector3(0, 0.15f, 0);

            if (player.position.y < transform.position.y - 0.5f) {
                player.position = transform.position + new Vector3(posOffset.x, -0.5f, 0);
            }

            yield return new WaitForSeconds(0.3f);
        }

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && !inTrigger) {
            inTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && !inTrigger) {
            inTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && inTrigger) {
            inTrigger = false;
        }
    }
}
