using UnityEngine;
using System.Collections;

public class playerControler : MonoBehaviour {

    Rigidbody2D myRig;
    Collider2D myCollider;
    SpriteRenderer mySprite;
    bool canJump;
    public float jumpPower = 40.0f;
    public float vaultVPower = 50.0f;
    public float vaultHPower = 5.0f;
    public float vaultTollerance = 2.0f;
    public float runSpeed = 2.0f;
    public float maxRunSpeed = 10.0f;
    float k_GroundedRadius = 0.3f;
    Transform m_GroundCheck;
    public float hFirction = 0.02f;
    public Sprite jumpSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;
    public Sprite idleSprite;
    public AudioClip jumpSound;
    public AudioClip fallSound;
    AudioSource myAudio;

	public int health;
	private bool onPlatform;
	private bool debugging;
	public GameObject Cam;
	private Camera CamCamera;
	private float camMoveHi;
	private float camMoveLo;
	public int toggleValue { get; set; }
//	public Color[] toggleItemColors;
	private Color origColor;
	public GameObject[] togglePlatforms;
	private bool lastTouchedPost = false;
	public float origPostMagnitude;

    // Use this for initialization
    void Awake () {
        myRig = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<Collider2D>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        myAudio = gameObject.GetComponent<AudioSource>();
        m_GroundCheck = transform.Find("GroundCheck");
		onPlatform = false;
		debugging = true;
		CamCamera = Cam.GetComponent<Camera> ();
		toggleValue = -1;
//		toggleItemColors [0] = mySprite.color;
		origColor = mySprite.color;
    }
	
	// Update is called once per frame
	void Update () {
        //checkCamPos ();
        CamCamera.transform.position = new Vector3(CamCamera.transform.position.x, Mathf.Lerp(CamCamera.transform.position.y, transform.position.y + 5.0f, 0.75f), CamCamera.transform.position.z);
		if (health > 0 || debugging) {
			//print(myRig.velocity.x);
			changeSprite();
			myRig.velocity = new Vector2 (myRig.velocity.x * hFirction, myRig.velocity.y);
			Collider2D[] colliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, 
				                               k_GroundedRadius, 1 << LayerMask.NameToLayer ("Platforms"));
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject) {
					canJump = true;
				} else {
					canJump = false;
				}
			}
			if (Input.GetKeyDown ("up") || Input.GetKeyDown ("w") || Input.GetKeyDown("space")) {
				if (canJump) {
					onPlatform = false;
					myAudio.clip = jumpSound;
					myAudio.Play ();
					myRig.velocity = new Vector2 (myRig.velocity.x, jumpPower);
					canJump = false;
				}
			} else if (Input.GetKey ("left") || Input.GetKey ("a")) {
				if (myRig.velocity.x > -maxRunSpeed) {
					myRig.velocity = new Vector2 (myRig.velocity.x - runSpeed, myRig.velocity.y);
				}
				if (myRig.velocity.x <= -maxRunSpeed) {
					myRig.velocity = new Vector2 (-maxRunSpeed, myRig.velocity.y);
				}
			} else if (Input.GetKey ("right") || Input.GetKey ("d")) {
				if (myRig.velocity.x < maxRunSpeed) {
					myRig.velocity = new Vector2 (myRig.velocity.x + runSpeed, myRig.velocity.y);
				}
				if (myRig.velocity.x >= maxRunSpeed) {
					myRig.velocity = new Vector2 (maxRunSpeed, myRig.velocity.y);
				}
			}
		}
    }

	private void checkCamPos(){
		camMoveHi = Screen.height / 3f;
		camMoveLo = Screen.height / 5f;
		//the camera moves up or down as the player does
		Vector3 playerScreenPos3 = CamCamera.WorldToScreenPoint(transform.position);
		Vector2 playerScreenPos = new Vector2 (playerScreenPos3.x, playerScreenPos3.y);
//		Debug.Log (camMoveHi + " " + camMoveLo + " " + playerScreenPos);
		float whichCamMove = 0f;
		float camSpeed = 0f;
		Vector3 camPos = CamCamera.transform.position;
		if (playerScreenPos.y > camMoveHi) {
			whichCamMove = camMoveHi;
			camSpeed = 3.5f;
			Debug.Log ("higher than camMoveHi, move cam up");

		} else if (playerScreenPos.y < camMoveLo) {
			whichCamMove = camMoveLo;
			camSpeed = 24f;
			Debug.Log ("lower than camMoveLo, move cam down");
		}
		if (whichCamMove > 0f) {
			CamCamera.transform.position = Vector3.MoveTowards (camPos, 
				new Vector3 (camPos.x, camPos.y + (playerScreenPos.y - whichCamMove), camPos.z),
				camSpeed * Time.deltaTime);
		}
	}

	private void changeSprite(){
		//fixme only do this if damage animation is not playing
		float epsilon = 0.0005f;
		if (myRig.velocity.x > epsilon) {
			mySprite.sprite = rightSprite;
		} else if (myRig.velocity.x < -1f * epsilon) {
			mySprite.sprite = leftSprite;
		}
		if (myRig.velocity.y > epsilon) {
			mySprite.sprite = jumpSprite;
		} else if (myRig.velocity.y < -1f * epsilon && Mathf.Abs (myRig.velocity.x) <= epsilon) {
			mySprite.sprite = jumpSprite; //fixme fall sprite?
		}
		if (Mathf.Abs (myRig.velocity.x) <= epsilon && Mathf.Abs (myRig.velocity.y) <= epsilon){
			mySprite.sprite = idleSprite;
		}


//		if (myRig.velocity.y > 0) {
//			mySprite.sprite = jumpSprite;
//		} else if (myRig.velocity.y < 0 && myRig.velocity.x == 0) {
//			mySprite.sprite = jumpSprite;
//		}
//		if (myRig.velocity.x == 0 && myRig.velocity.y == 0) {
//			mySprite.sprite = idleSprite;
//		}
	}

	void OnCollisionStay2D(Collision2D other){
//		Debug.Log ("collStay");
		if (other.gameObject.tag == "targetPlatform") {
			lastTouchedPost = false;
//			platformBehavoir platScript = other.gameObject.GetComponent<platformBehavoir> ();
			togglePlatformBehavior platScript = other.gameObject.GetComponent<togglePlatformBehavior> ();
			if (Input.GetKey ("down") || Input.GetKey ("s")) {
				platScript.letPlayerFallThrough ();
				//fixme play fall through animation?
			}
		} else if (other.gameObject.tag == "bottomEdge") {
			lastTouchedPost = false;
		}
	}


    void OnTriggerStay2D(Collider2D other)
    {
//        if (other.tag == "targetPlatform")
//        {
//            if (Input.GetKeyDown("space"))
//            {
//                other.GetComponent<platformBehavoir>().active = true;
//            }
//			if (Input.GetKeyDown("down") || Input.GetKeyDown("s")){
//				Debug.Log ("fall through");
//			}
//        }
        if (other.tag == "post")
        {
			post postScript = other.gameObject.GetComponent<post> ();
			float magnitude = origPostMagnitude;
			if (postScript.active)
            {
				if (!lastTouchedPost) {
					magnitude = origPostMagnitude;
				} else {
					magnitude = -1f;
				}
				if (postScript.vertical) {
					myRig.velocity = new Vector2 (myRig.velocity.x, myRig.velocity.y * magnitude);
				} else {
					myRig.velocity = new Vector2 (myRig.velocity.x * magnitude, myRig.velocity.y);
				}
				postScript.active = false;
				lastTouchedPost = true;
            }
        }
    }

	//used by planes/fireballs/enemies
	public void getHit(int damage){
		Debug.Log("player got hit with " + damage);
		health -= damage;
		if (health <= 0) {
			health = 0; //potentially for display purposes
			Debug.Log("player dies");
			//fixme play death animation
		} else {
			//fixmeplay get hit animation
		}
	}

	//used by toggleOrbs
	public void grabOrb(int val, Color color){ //fixme should there be a timer for all toggleValues?
		toggleValue = val;
//		mySprite.color = toggleItemColors [toggleValue];
		mySprite.color = color;
		for (int i = 0; i < togglePlatforms.Length; i++) {
			if (i == val) {
				Debug.Log ("i is val: " + i);
				foreach (Transform togglePlat in togglePlatforms[i].transform) {
					togglePlat.GetComponent<togglePlatformBehavior> ().setActive ();
				}
			} else {
				Debug.Log ("i is not val: " + i);
				foreach (Transform togglePlat in togglePlatforms[i].transform) {
					togglePlat.GetComponent<togglePlatformBehavior> ().setInactive ();
				}
			}
		}
	}

	public void resetToggleValue(){
		toggleValue = -1;
//		mySprite.color = toggleItemColors [toggleValue];
		mySprite.color = origColor;
	}

}