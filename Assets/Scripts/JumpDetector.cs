using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDetector : MonoBehaviour {

	private bool check = false;
	private bool grounded = false;
	private bool wallGrounded = false;
	private bool rightGrounded = false;

	private List <GameObject> currentCollisions = new List <GameObject> ();
	private BoxCollider2D boxCollider;

	void Start() {
		boxCollider = GetComponent<BoxCollider2D> ();
	}

	void Update() {
		BoxCollider2D objectCollider;
		float objectBottom = 1;
		float objectTop = 0;
		float playerTop = 0;
		float playerBottom = 1;
		float playerLeft = 1;
		float objectRight = 0;
		wallGrounded = false;
		grounded = false;
		rightGrounded = false;
		foreach (GameObject gameObject in currentCollisions) {
			
			objectCollider = gameObject.GetComponent<BoxCollider2D> ();

			objectBottom = gameObject.transform.position.y - (objectCollider.size.y / 2);
			playerTop = transform.position.y + (boxCollider.size.y / 2);

			playerBottom = playerTop - boxCollider.size.y;
			objectTop = objectBottom + objectCollider.size.y;

			playerLeft = transform.position.x - (boxCollider.size.x / 2);
			objectRight = gameObject.transform.position.x + (objectCollider.size.x / 2);

			if (playerTop > objectBottom) {
				wallGrounded = true;
			}

			if (playerBottom > objectTop) {
				grounded = true;
			}

			if (objectRight < playerLeft) {
				rightGrounded = true;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		currentCollisions.Add (col.gameObject);
	}

	void OnCollisionExit2D (Collision2D col) {
		currentCollisions.Remove (col.gameObject);
	}

	public bool isWallGrounded {
		get {
			return wallGrounded;
		}
	}

	public bool isGrounded {
		get {
			return grounded;
		}
	}

	public bool isRightGrounded {
		get {
			return rightGrounded;
		}
	}
}
