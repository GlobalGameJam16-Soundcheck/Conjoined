using UnityEngine;
using System.Collections;

public class planeDropFireballsBehavior : MonoBehaviour {

	public float fireballSpawnTime;
	public float delay;
	public float speed; //how fast it moves between left and right
	private SpriteRenderer mySprite;
	private Vector3 velocity;
	private int direction; //1 if facing right, -1 if facing left
	public GameObject fireballPrefab;
	public int health;
	private bool gotHit;
	private float timer;
	private float flashTime;
	private int count;

	// Use this for initialization
	void Start () {
		mySprite = gameObject.GetComponent<SpriteRenderer>();
		setDirectionAndVelo ();
		InvokeRepeating("spawnFireballWrapper", delay, fireballSpawnTime);
		gotHit = false;
		count = 0;
		flashTime = 2f;
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + velocity * Time.deltaTime;
		if (gotHit) {
			timer += Time.deltaTime;
			if (timer >= flashTime) {
				mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b);
				timer = 0f;
				gotHit = false;
				count = 0;
			} else {
				count++;
				if ((count / 5) % 2 == 0) {
					mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.5f);
				} else {
					mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b);
				}
			}
		}
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
		if (other.gameObject.tag == "sideEdge") {
			mySprite.flipX = !mySprite.flipX;
			setDirectionAndVelo ();
		}
	}

	private void spawnFireballWrapper(){
		int numToSpawn = Random.Range (1, 3);
		int numSpawned = 0;
		while (numSpawned < numToSpawn) {
			Invoke ("spawnFireball", (numSpawned + 1));
			numSpawned++;
		}
	}

	private void spawnFireball(){
		float xPos = transform.position.x;
		float yPos = transform.position.y - transform.localScale.y - 2 * fireballPrefab.transform.localScale.y;
		GameObject fireball = Instantiate(fireballPrefab, new Vector2(xPos,yPos), Quaternion.identity) as GameObject;
		fireball.GetComponent<fireballBehavior> ().justSpawned ();
	}

	public void getHit(int damage){
		health -= damage;
		//fixme get hit animation
		gotHit = true;
		if (health <= 0) {
			getDestroyed ();
		}
	}

	private void getDestroyed(){
		float destroyDelay = 2f;
		Destroy(gameObject, destroyDelay);
	}

}
