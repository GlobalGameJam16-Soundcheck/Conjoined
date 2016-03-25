using UnityEngine;
using System.Collections;

public class fireballBehavior : MonoBehaviour {

	public int damage;
	private bool lastTouchedPost;
	private Rigidbody2D myRig;
	public float origPostMagnitude;
	public float slowerPostMag;
	public bool floorFire;
	private AudioSource thud;
	public bool starterBall;
	private float timer;
	private float time;

	void Start(){
		if (starterBall)
			justSpawned ();
	}

	// Use this for initialization
	public void justSpawned () {
		lastTouchedPost = false;
		myRig = GetComponent<Rigidbody2D> ();
		thud = GetComponent<AudioSource> ();
		timer = 0.5f;
		time = 0.0f;
//		addXForce (30f);
	}

	void OnCollisionEnter2D(Collision2D other){
		//fixme maybe need a destroying animation or particle effects
		if (!floorFire) {
			if (other.gameObject.tag == "player") {
				Debug.Log ("hitting player");
				other.gameObject.GetComponent<playerControler> ().getHit (damage);
				Debug.Log ("get destroyed");
				thud.Play ();
				getDestroyed ();
				lastTouchedPost = false;
			} else if (other.gameObject.tag == "bottomEdge") {
//				Debug.Log ("hit the bottom get destroyed");
				lastTouchedPost = false;
				thud.Play ();
				getDestroyed ();
			} else if (other.gameObject.tag == "targetPlatform") {
				Debug.Log ("hitting platform");
				thud.Play ();
//				float yForce = 15f;
//				if (myRig.velocity.y > 0f) {
//					yForce = 0f;
//				}
//			Vector2 forceVector = new Vector2 (Random.Range (-20f, 20f), yForce);
//			myRig.AddForce (forceVector, ForceMode2D.Impulse);
//				addXYForce (15f, yForce);
				lastTouchedPost = false;
			} 
			else if (other.gameObject.tag == "post") {
				thud.Play ();
				Debug.Log ("hitting post");
				post postScript = other.gameObject.GetComponent<post> ();
//				float magnitude = origPostMagnitude;
				if (postScript.togglePost && !postScript.inactive) {
					if (!lastTouchedPost) {
						postScript.active = false;
						lastTouchedPost = true;
						Debug.Log (myRig.velocity.y);
						addXYForce (0f, - 600f * myRig.velocity.y);
						Destroy (gameObject, 2f);
						postScript.playSound ();
					}
				}
			} 
			else if (other.gameObject.tag == "fireball") {
				lastTouchedPost = false;
//				getDestroyed ();
			}
		}
	}

	private void getDestroyed(){
		float destroyDelay = 2f;
		Destroy(gameObject, destroyDelay);
		transform.position = new Vector2 ((transform.position.x + 1f) * 10000000f, transform.position.y);
	}

	void OnTriggerStay2D(Collider2D other){
//		if (other.gameObject.tag == "post") {
//			Debug.Log ("hitting post");
//			post postScript = other.gameObject.GetComponent<post> ();
//			float magnitude = origPostMagnitude;
//			if (postScript.togglePost && !postScript.inactive) {
//				if (!lastTouchedPost) {
//					magnitude = origPostMagnitude;
//				} else {
//					magnitude = slowerPostMag;
//					Debug.Log ("lower magnitude");
//				}
//				if (other.gameObject.GetComponent<post> ().vertical) {
//					myRig.velocity = new Vector2 (myRig.velocity.x, myRig.velocity.y * magnitude);
//				} else {
//					myRig.velocity = new Vector2 (myRig.velocity.x * magnitude, myRig.velocity.y);
//				}
//				postScript.active = false;
//				lastTouchedPost = true;
//				addXForce (20f);
//			}
//		}
		time += Time.deltaTime;
		if (other.gameObject.tag == "plane" && time > timer){
			other.gameObject.GetComponent<planeDropFireballsBehavior> ().getHit (damage);
			getDestroyed ();
		}
	}

	public void addXForce(float xForce){
//		Vector2 forceVector = new Vector2 (Random.Range (-1 * xForce, xForce), 0f);
//		myRig.AddForce (forceVector, ForceMode2D.Impulse);
	}

	private void addXYForce(float xForce, float yForce){
//		Vector2 forceVector = new Vector2 (Random.Range (-1 * xForce, xForce), Random.Range (-1 * yForce, yForce));
		Vector2 forceVector = new Vector2(xForce, yForce);
		myRig.AddForce (forceVector, ForceMode2D.Impulse);
	}

}
