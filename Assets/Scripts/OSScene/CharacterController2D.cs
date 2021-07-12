using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 0649
public class CharacterController2D : MonoBehaviour {
	[Header("Movement Settings")]
	[SerializeField] private float runSpeed = 40f;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement

	[Header("Configuration")]
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_AttackCheck;                           // A position marking where to check if the player is grounded.

	[Header("Utilities")]

	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	public bool canMove = true;

	[SerializeField] private float jumpCooldown = 0.1f;

	private float jumpCD = 0f;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	private CameraShake cameraShake;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public bool GetGrounded() {
		return m_Grounded;
	}
	public void ResetAll(Vector3 newPos) {
		canMove = true;
		animator.Play("Idle");

		m_Rigidbody2D.isKinematic = false;
		GetComponent<BoxCollider2D>().enabled = true;

		transform.position = newPos;

	}

	public void ConcludeAttack() {
		List<Collider2D> enemiesToDamage = Physics2D.OverlapCircleAll(m_AttackCheck.position, 0.6f).ToList<Collider2D>();

		List<Collider2D> temp = new List<Collider2D>();

		foreach (Collider2D collider in enemiesToDamage) {
			if (collider.TryGetComponent<ActivateAttack>(out ActivateAttack at)) {
				at.Activate();
			}
			if (collider.gameObject.name.Contains("Player")) {
				temp.Add(collider);
			}
		}

		foreach (Collider2D collider in temp) {
			enemiesToDamage.Remove(collider);
		}

		if (enemiesToDamage.Count != 0 && cameraShake != null) {
			cameraShake.StopAllCoroutines();
			cameraShake.Res();
			StartCoroutine(cameraShake.Shake());

			if (!m_Grounded) {
				if (m_Rigidbody2D.velocity.y < -1) {
					m_Rigidbody2D.velocity = new Vector3(0, 0.5f, 0);
				} else {
					m_Rigidbody2D.AddForce(new Vector2(0f, 110));
				}
			}
		}


	}

	private void Awake() {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null) {
			OnLandEvent = new UnityEvent();
		}

		if (OnCrouchEvent == null) {
			OnCrouchEvent = new BoolEvent();
		}
	}

	public GameObject tutorial;
	private void Start() {
		tutorial.SetActive(true);

		if (transform.parent.TryGetComponent<CameraShake>(out CameraShake cs)) {
			cameraShake = cs;
		}

		jumpCD = jumpCooldown;

		if (animator == null) {
			animator = GetComponent<Animator>();
		}
	}

	void Update() {
		//Debug.Log(transform.localPosition);
	}

	private void FixedUpdate() {
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				m_Grounded = true;
				if (!wasGrounded) {
					OnLandEvent.Invoke();
				}
			}
		}
	}

	public Animator animator;

	public void Move(float move, bool crouch, bool jump) {
		if (!canMove) {
			return;
		}

		move *= runSpeed;

		animator.SetBool("crouch", m_wasCrouching);
		animator.SetBool("jump", jump);
		animator.SetBool("grounded", m_Grounded);
		animator.SetFloat("fallSpeed", m_Rigidbody2D.velocity.y);
		animator.SetFloat("speed", Mathf.Abs(move));

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl) {

			// If crouching
			if (crouch) {
				if (!m_wasCrouching) {
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching

				if (m_wasCrouching) {
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}

			}
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight) {
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight) {
				// ... flip the player.
				Flip();
			}
			if ((move > 0 || move < 0) && m_Grounded)
			{
				AudioSource a = GetComponent<AudioSource>();
				if (!a.isPlaying)
                {
					a.PlayOneShot(StoryManager.Instance.clips[10]);
                }
			}
		}
		// If the player should jump...
		if (m_Grounded && jump && jumpCD <= 0) {
			AudioSource a = GetComponent<AudioSource>();
			a.PlayOneShot(StoryManager.Instance.clips[9]);
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -50f, 3f));
		} else if (jumpCD > 0) {
			jumpCD -= Time.deltaTime;
		}
	}


	private void Flip() {
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
