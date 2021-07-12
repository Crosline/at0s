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
	        FindObjectOfType<PlayerMovement2D>().canAttack = false;	
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


	        FindObjectOfType<PlayerMovement2D>().canAttack = true;
		at2.gameObject.SetActive(true);

	}
}
