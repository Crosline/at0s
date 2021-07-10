using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public Transform camTransform;

	public float shakeDuration = 1f;
	public float shake;

	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	void Awake() {
		shake = shakeDuration;
		if (camTransform == null)
			camTransform = GetComponent<Transform>();
	}
	
	public void ShakeCam(float shakeDuration = this.shakeDuration, float shakeAmount = this.shakeAmount, float decreaseFactor = this.decreaseFactor){
		StartCoroutine(Shake());
	}


	public IEnumerator Shake() {
		while (true) {
			if (shake > 0) {
				camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere * shakeAmount;

				shake -= Time.deltaTime * decreaseFactor;
			} else {
				shake = shakeDuration;
				break;
			}
			yield return new WaitForFixedUpdate();
		}

	}
}
