using System.Collections;
using UnityEngine;

public class MoveScreen : MonoBehaviour {
	public float screenSize = 13f;
	public float moveSpeed = 13f;



	void Start() {

		PlayerPrefs.DeleteAll();
	}

	public void Activate(ActivateTrigger at1, ActivateTrigger at2, bool isUp = true) {
		StartCoroutine(LerpScreen(at1, at2, isUp));
	}

	IEnumerator LerpScreen(ActivateTrigger at1, ActivateTrigger at2, bool isUp) {
	        PlayerMovement2D pm = FindObjectOfType<PlayerMovement2D>();
	        
	        pm.canAttack = false;	
	        pm.controller.canMove = false;
	        pm.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
	        
		at1.gameObject.SetActive(false);
		at2.gameObject.SetActive(false);
		Vector3 finalPos;
		if (isUp) {
			finalPos = transform.position - new Vector3(0, screenSize, 0);
		} else {
			finalPos = transform.position + new Vector3(0, screenSize, 0);
		}

		while (Vector3.Distance(transform.position, finalPos) > 0.01f) {

			transform.position = Vector3.MoveTowards(transform.position, finalPos, moveSpeed * Time.fixedDeltaTime);


			yield return new WaitForFixedUpdate();
		}

		transform.position = finalPos;


	        pm.canAttack = true;	
	        pm.controller.canMove = true;
		at2.gameObject.SetActive(true);

	}
}
