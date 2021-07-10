using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {


	public float shakeDuration = 1f;
	public float shake;

	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	private Vector3 startPos;

	private bool isShaking = false;

	public bool runOnlyOne = false;

	void Awake() {
		startPos = transform.position;
		shake = shakeDuration;
	}
	
	public void ShakeCam(float shakeDuration = 1f, float shakeAmount = 0.7f, float decreaseFactor = 1.0f) {
		this.shakeDuration = shakeDuration;
		this.shakeAmount = shakeAmount;
		this.decreaseFactor = decreaseFactor;
		StartCoroutine(Shake());
	}


	public IEnumerator Shake() {

		if (isShaking)
			yield break;

		isShaking = true;

		while (true) {
			if (shake > 0) {
				transform.position = startPos + Random.insideUnitSphere * shakeAmount;
				transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z);
				shake -= Time.deltaTime * decreaseFactor;
			} else {
				shake = shakeDuration;
				break;
			}
			yield return new WaitForFixedUpdate();
		}

		if (runOnlyOne) {
			this.enabled = false;


			if (TryGetComponent<Animator>(out Animator animator)) {
				animator.enabled = true;
			}
		}
		yield return new WaitForSeconds(1f);
		isShaking = false;
	}
}
