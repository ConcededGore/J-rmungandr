using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//public float reloadTime; Not sure if this will be implemented

	public string className;

	public float speed;
	public float jumpHeight;
	public float dashLength;
	public float fireRate;

	public int jumps;
	public int health;

	private int curentJumps = 0;

	private float horizontal;
	private float vertical;

	private Rigidbody2D rb;
	private BoxCollider2D bc;
	private JumpDetector jumpDetector;
	private Vector2 jumpDirection;
	private bool canJump;
	private bool justJumped;
	private bool grounded;
	private bool anchored;
	private bool lowFriction;

	private Vector2 maxHorizontal;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		bc = GetComponent<BoxCollider2D> ();
		jumpDetector = GetComponent<JumpDetector> ();
		maxHorizontal = (Vector2.right * 750 * Time.deltaTime);
	}

	void Update () {
		transform.rotation = Quaternion.Euler (Vector3.zero);
		jumpDirection.y += jumpDirection.magnitude;
		jumpDirection.Normalize ();
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
					rb.velocity = new Vector2 (rb.velocity.x * .8f, rb.velocity.y);
				}
			} else {
				if (rb.velocity.magnitude < 1 && !lowFriction) { //replace 1 with something speed related
					rb.velocity = new Vector2 (horizontal * 5, rb.velocity.y); //5 to be replaced by playerspeed*.5
				} else {
					if (lowFriction) {
						if (rb.velocity.magnitude < 20) { //replace 20 with something speed related
							rb.AddForce (new Vector2 (horizontal * ((15 - rb.velocity.magnitude) * rb.mass * .25f), 0), ForceMode2D.Impulse); //replace 20 with something speed related and maybe .35
						}
					} else {
						if (rb.velocity.magnitude < 15) { //replace 15 with something speed related
							rb.AddForce (new Vector2 (horizontal * ((15 - rb.velocity.magnitude) * rb.mass * .5f), 0), ForceMode2D.Impulse); //replace 15 with something speed related and maybe .5
						}
					}
				}
			}
		} else {
			rb.AddForce (new Vector2 (horizontal/5, 0), ForceMode2D.Impulse);
		}
		if (anchored) { //decide if player should slip down wall at all
			rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * (-(vertical - 1)/2.0f));
		}

		//		print (vertical);

		// Could also use the up arrow, might use space later for another function; might want to just make a new axis for this
		if (Input.GetKeyDown("space") && curentJumps < jumps && canJump) {
			justJumped = true;
			canJump = false;
			rb.AddForce (jumpDirection * jumpHeight * 1.5f);
			jumps++;
		}
	}

	void OnCollisionStay2D(Collision2D coloid) {
		//add some kind of "jumpable" tag
		if (!justJumped) {
			canJump = true;
			Vector2 jumpDir = new Vector2 (0, 0);
			foreach (ContactPoint2D cont in coloid.contacts) {
				if (cont.normal.y > .5f) {
					grounded = true;
					if (cont.collider.GetComponent<WorldAttributes>() != null) {
						if (cont.collider.GetComponent<WorldAttributes> ().isSlippery) {
							lowFriction = true;
							print ("WOW");
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
		jumpDirection = Vector2.zero;
		anchored = false;
		canJump = false;
		grounded = false;
	}
}


