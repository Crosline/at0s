using System.Collections;
using UnityEngine;

public class FolderController : MonoBehaviour {
    [SerializeField]
    private Transform player;

    private BoxCollider2D collider2D;
    private SpriteRenderer spriteRenderer;

    public Vector3 posOffset;

    public Sprite closedFolder;

    private bool isOnTop = true;

    public bool canRender = true;

    private bool inTrigger = false;

    public GameObject folderExe;
    public GameObject pipeExe;

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
                    collider2D.enabled = false;
                    player.GetComponent<Rigidbody2D>().isKinematic = true;
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    canRender = false;
                    StartCoroutine(EnterFolder());
                }
            }
        }
    }

    IEnumerator EnterFolder() {
        player.position = transform.position + posOffset;
        player.GetComponent<Animator>().Play("Slide");
        player.GetComponent<PlayerMovement2D>().StopAllCoroutines();

        while (player.position.y > transform.position.y - 0.5f) {

            player.position -= new Vector3(0, 0.15f, 0);

            if (player.position.y < transform.position.y - 0.5f) {
                player.position = transform.position + new Vector3(posOffset.x, -0.5f, 0);
            }

            yield return new WaitForSeconds(0.3f);
        }

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (this.name.Contains("kapak")) {
            spriteRenderer.sprite = closedFolder;
            if (transform.parent.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)) {
                sr.enabled = false;
            }


            yield return new WaitForSeconds(0.1f);

            player.GetComponent<PlayerMovement2D>().ResetAll(transform.position + new Vector3(6f, 4.5f, 0f));

            folderExe.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            canRender = true;

            yield return new WaitForSeconds(11f);
            folderExe.SetActive(false);
            this.enabled = false;
        }
        else if (this.name.Contains("Pipe")) {


            player.GetComponent<PlayerMovement2D>().ResetAll(transform.position + new Vector3(2.45f, -7.6f, 0f));

            pipeExe.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            canRender = true;
        }

    }

    IEnumerator EnterTrash() {
        Vector3 temp = player.position;
        player.position = transform.position + posOffset;
        player.GetComponent<Animator>().Play("Slide");
        player.GetComponent<PlayerMovement2D>().StopAllCoroutines();

        while (player.position.x < transform.position.x + 0.5f) {

            player.position += new Vector3(0.15f, 0, 0);

            if (player.position.x > transform.position.x + 0.5f) {
                player.position = transform.position + new Vector3(0.5f, posOffset.y, 0);
            }

            yield return new WaitForSeconds(0.3f);
        }


        GlitchController.Instance.Glitcher(0.3f);

        player.SetParent(transform.parent.parent);
        player.GetComponent<PlayerMovement2D>().ResetAll(transform.position + new Vector3(-1f, 1f, 0));
        //player.position = temp + new Vector3(-1f,0,0);

        //play trash

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && !inTrigger) {
            if (this.name.Contains("Trash")) {
                if (player.TryGetComponent<Animator>(out Animator anim)) {
                    player.GetComponent<CharacterController2D>().canMove = false;
                    player.GetComponent<BoxCollider2D>().enabled = false;
                    collider2D.enabled = false;
                    player.GetComponent<Rigidbody2D>().isKinematic = true;
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    canRender = false;
                    player.rotation = transform.rotation;
                    player.parent = this.transform;
                    StartCoroutine(EnterTrash());
                }
            }
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
