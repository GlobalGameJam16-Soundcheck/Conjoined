using UnityEngine;
using System.Collections;

public class planeDropFireballsBehavior : MonoBehaviour {

	public float fireballDelay;
	public float speed; //how fast it moves between left and right
	private SpriteRenderer mySprite;
	private Vector3 velocity;
	private int direction; //1 if facing right, -1 if facing left

	// Use this for initialization
	void Start () {
		mySprite = gameObject.GetComponent<SpriteRenderer>();
		setDirectionAndVelo ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + velocity * Time.deltaTime;
	}

	private void setDirectionAndVelo(){
		direction = -1;
		if (mySprite.flipX)
			direction = 1;
//		float yVelo = Mathf.Sin (transform.position.x);
		float yVelo = 0f;
		velocity = new Vector3 (speed * direction, yVelo, 0f);
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("collided");
		if (other.gameObject.tag == "sideEdge") {
			Debug.Log ("side edge hit");
			mySprite.flipX = !mySprite.flipX;
			setDirectionAndVelo ();
		}
	}

}
