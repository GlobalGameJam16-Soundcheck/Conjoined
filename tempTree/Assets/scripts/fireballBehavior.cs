using UnityEngine;
using System.Collections;

public class fireballBehavior : MonoBehaviour {

	public int damage;
	private bool lastTouchedPost;
	private Rigidbody2D myRig;
	public float origPostMagnitude;

	// Use this for initialization
	public void justSpawned () {
		lastTouchedPost = false;
		myRig = GetComponent<Rigidbody2D> ();
		addXForce ();
	}

	void OnCollisionEnter2D(Collision2D other){
		//fixme maybe need a destroying animation or particle effects
		float destroyDelay = 0.1f;
		if (other.gameObject.tag == "player") {
			Debug.Log ("hitting player");
			other.gameObject.GetComponent<playerControler> ().getHit (damage);
			Debug.Log ("get destroyed");
			Destroy(gameObject, destroyDelay);
			lastTouchedPost = false;
		} else if (other.gameObject.tag == "bottomEdge") {
//				Debug.Log ("hit the bottom get destroyed");
			lastTouchedPost = false;
			Destroy(gameObject, destroyDelay);
		} else if (other.gameObject.tag == "targetPlatform") {
			Debug.Log ("hitting platform");
			float yForce = Random.Range (20f, 30f);
			if (myRig.velocity.y > 0f) {
				yForce = 0f;
			}
			Vector2 forceVector = new Vector2 (Random.Range (-20f, 20f), yForce);
			myRig.AddForce (forceVector, ForceMode2D.Impulse);
			lastTouchedPost = false;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "post") {
			Debug.Log ("hitting post");
			post postScript = other.gameObject.GetComponent<post> ();
			float magnitude = origPostMagnitude;
			if (other.gameObject.GetComponent<post> ().active) {
				if (!lastTouchedPost) {
					magnitude = origPostMagnitude;
				} else {
					magnitude = -0.75f;
					Debug.Log ("lower magnitude");
				}
				if (other.gameObject.GetComponent<post> ().vertical) {
					myRig.velocity = new Vector2 (myRig.velocity.x, myRig.velocity.y * magnitude);
				} else {
					myRig.velocity = new Vector2 (myRig.velocity.x * magnitude, myRig.velocity.y);
				}
				postScript.active = false;
				lastTouchedPost = true;
				addXForce ();
			}
		}
	}

	public void addXForce(){
		Vector2 forceVector = new Vector2 (Random.Range (-20f, 20f), 0f);
		myRig.AddForce (forceVector, ForceMode2D.Impulse);
	}

}
