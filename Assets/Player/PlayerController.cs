﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public string className;

	public float speed;
	public float jumpForce;
	public float dashLength;
	public float fireRate;

	public int jumps;
	public int health;

	public bool eatBullets = true;

	private int curentJumps = 0;

	private float horizontal;
	private float vertical;

	private float jumpTime = 0;
	private float lastJumpCalc = 0;

	private Rigidbody2D rb;
	private BoxCollider2D bc;
	private Vector2 jumpDirection;

	private bool justJumped = false;
	private bool grounded;
	private bool anchored;
	private bool lowFriction;
	private bool named = false;
	private bool updated = false;

	[SyncVar]
	private bool isDead = false;

	private Vector2 maxHorizontal;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		bc = GetComponent<BoxCollider2D> ();
		maxHorizontal = (Vector2.right * 750 * Time.deltaTime);
		jumpForce = 22.5f;
		speed = 5f;
		jumps = 3;

	}

	void Update () {

		if (!isLocalPlayer) {
			return;
		}

		if (isDead) {
			if (!updated) {
				CmdUpdateConstraints();
				updated = true;
			}
			return;
		}

		transform.rotation = Quaternion.Euler (Vector3.zero);

		justJumped = false;
		// Using GetAxisRaw here, may prefer to use GetAxis later; talk to Jackson
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");

		//rb.velocity = new Vector2 (maxHorizontal.x * horizontal, rb.velocity.y);

		if (grounded) {
			if (horizontal == 0) {
				if (lowFriction) {
					rb.velocity = new Vector2 (rb.velocity.x * .95f, rb.velocity.y);
				} else {
					rb.velocity = new Vector2 (rb.velocity.x * .6f, rb.velocity.y);
				}
			} else {
				if (rb.velocity.magnitude < 1 && !lowFriction) { //replace 1 with something speed related
					rb.velocity = new Vector2 (horizontal * (speed), rb.velocity.y); //5 to be replaced by playerspeed*.5
				} else {
					if (lowFriction) {
						if (rb.velocity.magnitude < (speed * 4)) { //replace 20 with something speed related
							rb.AddForce (new Vector2 (horizontal * (((speed * 4) - rb.velocity.magnitude) * rb.mass * .25f), 0), ForceMode2D.Impulse); //replace 20 with something speed related and maybe .35
						} else {
							rb.velocity = new Vector2 (rb.velocity.x * .975f, rb.velocity.y);
						}
					} else {
						if (rb.velocity.magnitude < (speed * 3)) { //replace 15 with something speed related
							rb.AddForce (new Vector2 (horizontal * (((speed * 3) - rb.velocity.magnitude) * rb.mass * .5f), 0), ForceMode2D.Impulse); //replace 15 with something speed related and maybe .5
						} else {
							rb.velocity = new Vector2 (rb.velocity.x * .9f, rb.velocity.y);
						}
					}
				}
			}
		} else {
			rb.AddForce (new Vector2 ((speed)*horizontal/(Mathf.Abs(rb.velocity.x) < 5 ? 5 : Mathf.Abs(rb.velocity.x)), 0), ForceMode2D.Impulse);
		}
		if (anchored) { //decide if player should slip down wall at all
			rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * (-(vertical - 1)/2.0f));
		}

		//		print (vertical);

		// Could also use the up arrow, might use space later for another function; might want to just make a new axis for this
		if (jumpDirection.magnitude == 0f) {
			jumpDirection = rb.velocity;
		}
		if (Input.GetKeyDown("space") && ((curentJumps < jumps) || anchored)) {
			if (curentJumps == 0) {
				print ("new jump chain");
			}
			jumpDirection.y += jumpDirection.magnitude;
			jumpDirection.Normalize ();
			justJumped = true;
			rb.AddForce (jumpDirection * jumpForce, ForceMode2D.Impulse);
			curentJumps++;
			jumpTime = Time.time;
			lastJumpCalc = Time.time;
			print (curentJumps);
		}
		if (Input.GetKey ("space") && lastJumpCalc - jumpTime < .5f) {
			rb.AddForce (jumpDirection * jumpForce * (Time.time - jumpTime < .5f ? Time.time - lastJumpCalc : .5f - (lastJumpCalc - jumpTime)), ForceMode2D.Impulse);
			lastJumpCalc = Time.time;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Bullet" && eatBullets) {
			Destroy (col.gameObject);
			isDead = true;
		}
	}

	void OnCollisionStay2D(Collision2D coloid) {
		//add some kind of "jumpable" tag
		if (!justJumped && jumpTime - Time.time < -.5f) {
			Vector2 jumpDir = new Vector2 (0, 0);
			foreach (ContactPoint2D cont in coloid.contacts) {
				if (cont.normal.y > .5f) {
					curentJumps = 0;
					grounded = true;
					if (cont.collider.GetComponent<WorldAttributes>() != null) {
						if (cont.collider.GetComponent<WorldAttributes> ().isSlippery) {
							lowFriction = true;
						} else {
							lowFriction = false;
						}
					} else {
						print ("WTF");
					}

				}
				if (cont.normal.y >= 0f && cont.normal.y <= .2f) {
					anchored = true;
				}
				jumpDir = jumpDir + cont.normal;
			}
			jumpDir.Normalize ();
			jumpDirection += jumpDir;
		}
	}
	void OnCollisionExit2D(){
		anchored = false;
		grounded = false;
	}

	public bool IsDead {
		get {
			return isDead;
		}
	}

	[Command]
	void CmdUpdateConstraints() {
		rb.constraints = RigidbodyConstraints2D.None;
	}
}


