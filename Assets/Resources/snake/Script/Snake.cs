using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour {

	private bool ate = false;
	private bool isDead = false;
	private bool canStart = true;

	public float speed = 1f;

	public float stepAmount = 1f;

	public GameObject tailPrefab;

	public Sprite bodySprite;
	public Sprite tailSprite;

	private Vector2 dir = Vector2.right;
	private List<Transform> tail = new List<Transform>();

	private bool firstHit = true;

	private int tutorial = 2;


	public void StartSnake() {
		if (canStart) {
			canStart = false;
			// Move the Snake every 300ms

			StartCoroutine(Move());

		}
	}

	public void UpdateDirection(int direction) {
		if (!isDead) {
			switch (direction) {
				case 1:
					dir = Vector2.down;
					break;
				case 2:
					dir = Vector2.right;
					break;
				case 3:
					dir = Vector2.left;
					break;
				case 4:
					dir = Vector2.up;
					break;
			}

			if(firstHit)
            {
				DialogueTrigger.Instance.TriggerDialogue("Snake_Error");
				firstHit = false;
            }


			StartSnake();


			if (tutorial > 0) {
				GlitchController.Instance.Glitcher(0.3f);
				tutorial--;
			} else {
				int i = Random.RandomRange(1, 100);
				if (i < 5) {
					GlitchController.Instance.Glitcher(0.3f);
				}
			}
		}




	}

	public void FinishSnake() {
		isDead = true;
	}

	void ResetSnake() {
		StopAllCoroutines();

		GlitchController.Instance.Glitcher(0.5f);

		transform.localPosition = Vector3.zero;

		foreach (Transform t in tail)
			Destroy(t.gameObject);

		tail.Clear();

		speed = 1f;

		canStart = true;
	}

	private IEnumerator Move() {


		if (!isDead) {

			Vector2 v = transform.position;
			Quaternion a = transform.rotation;

			transform.Translate(dir * stepAmount, Space.World);


			if (dir == Vector2.up) {
				transform.eulerAngles = (new Vector3(0, 0, 0));
			} else if (dir == Vector2.left) {
				transform.eulerAngles = (new Vector3(0, 0, 90));
			} else if (dir == Vector2.right) {
				transform.eulerAngles = (new Vector3(180, 0, -90));
			} else if (dir == Vector2.down) {
				transform.eulerAngles = (new Vector3(0, 0, 180));
			}



			if (ate) {


				GameObject g = Instantiate(tailPrefab,
			  v,
			  a, transform.parent);




				// Keep track of it in our tail list
				tail.Insert(0, g.transform);

				/*if (tail.Count > 0) {

					Transform temp =
					tail.Last();

					g = Instantiate(bodyPrefab,
									  temp.position, temp.rotation);

					
					tail.RemoveAt(tail.Count-1);
					Destroy(temp);

					tail.Insert(tail.Count - 1, g.transform);
				}*/

				// Reset the flag
				ate = false;
			} else if (tail.Count > 0) {    // Do we have a Tail?
				MoveTail(v, a);

			}

			//List<Transform> tempTail = tail;
			//List<Transform> temp = tail;

		}

		yield return new WaitForSeconds(speed);

		StartCoroutine(Move());
	}

	void MoveTail(Vector2 v, Quaternion a) {

		// Move last Tail Element to where the Head was
		tail.Last().position = v;
		tail.Last().rotation = a;


		// Add to front of list, remove from the back

		tail.Last().GetComponent<SpriteRenderer>().sprite = bodySprite;

		tail.Insert(0, tail.Last());

		tail.RemoveAt(tail.Count - 1);


		tail.Last().GetComponent<SpriteRenderer>().sprite = tailSprite;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.name.Contains("Food")) {

			ate = true;

			speed = speed - 0.06f;

			GetComponent<SpawnFood>().Eat(coll.gameObject);


		} else {
			ResetSnake();

			GetComponent<SpawnFood>().ResetFood();

		}
	}
}
