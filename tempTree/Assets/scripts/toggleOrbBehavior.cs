using UnityEngine;
using System.Collections;

public class toggleOrbBehavior : MonoBehaviour {

	public int value;
	private SpriteRenderer mySprite;
	private Color color;
	public Sprite grabbedSprite;
	private Sprite origSprite;
	private bool grabbed;
	private float timer;
	private float flashTime;
	private int count;

	void Start(){
		mySprite = GetComponent<SpriteRenderer> ();
		color = mySprite.color;
		flashTime = 2f;
		timer = 0f;
		grabbed = false;
		origSprite = mySprite.sprite;
		count = 0;
	}

	void Update(){
		if (grabbed) {
			timer += Time.deltaTime;
			if (timer >= flashTime) {
				mySprite.sprite = origSprite;
				timer = 0f;
				grabbed = false;
				count = 0;
			} else {
				count++;
				if ((count / 5) % 2 == 0) {
					mySprite.sprite = grabbedSprite;
				} else {
					mySprite.sprite = origSprite;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "player") {
			Debug.Log ("player touching");
			other.gameObject.GetComponent<playerControler> ().grabOrb (value, color);
			grabbed = true;
			timer = 0f;
			count = 0;
		}
	}
}
