using UnityEngine;
using System.Collections;

public class platformBehavoir : MonoBehaviour {

    public bool active = true;
    Collider2D myCollider;
    SpriteRenderer mySprite;
    public AudioClip deactivateSound;
    AudioSource myAudio;

	private PlatformEffector2D pe2d;
	private int playerLayer;
	private bool playerFallingThrough;
	private float timer;
	private float fallTime;

	public GameObject toggleOrb;
	public GameObject toggleBlock;


	// Use this for initialization
	void Start () {
        myCollider = gameObject.GetComponent<Collider2D>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        myAudio = gameObject.GetComponent<AudioSource>();
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
        if (Input.GetKeyDown("1"))
        {
            active = true;
        }
        if (active)
        {
            myCollider.isTrigger = false;
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1.0f);
        }
        else
        {
            myCollider.isTrigger = true;
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.2f);
        }
	}

    public void playDeactivateSound()
    {
        myAudio.clip = deactivateSound;
        myAudio.Play();
    }

	public void letPlayerFallThrough(){
		pe2d.colliderMask = pe2d.colliderMask & ~playerLayer; //lets collider ignore player
		playerFallingThrough = true;
	}
}
