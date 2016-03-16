using UnityEngine;
using System.Collections;

public class togglePlatformBehavior : MonoBehaviour {

	private Collider2D myCollider;
	private SpriteRenderer mySprite;
	private PlatformEffector2D pe2d;
	private int playerLayer;
	private bool playerFallingThrough;
	private float timer;
	private float fallTime;
//	public int value;

	// Use this for initialization
	void Start () {
		mySprite = GetComponent<SpriteRenderer> ();
		playerFallingThrough = false;
		timer = 0f;
		fallTime = 0.25f;
		pe2d = GetComponent<PlatformEffector2D> ();
		playerLayer = (1 << LayerMask.NameToLayer ("Player"));
	}
	
	// Update is called once per frame
	void Update () {
		if (playerFallingThrough) {
			timer += Time.deltaTime;
			if (timer >= fallTime) {
				timer = 0f;
				playerFallingThrough = false;
				pe2d.colliderMask = pe2d.colliderMask | playerLayer;
				Debug.Log ("no more fall through");
			}
		}
	}

	public void setActive(){
		mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1.0f);
		pe2d.colliderMask = ~pe2d.colliderMask;
	}

	public void setInactive(){
		mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.2f);
		pe2d.colliderMask = ~pe2d.colliderMask;
	}

	public void letPlayerFallThrough(){
		pe2d.colliderMask = pe2d.colliderMask & ~playerLayer; //lets collider ignore player
		playerFallingThrough = true;
	}
}
