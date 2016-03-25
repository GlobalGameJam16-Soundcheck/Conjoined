using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

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
    public Sprite floatSprite;
    public AudioClip jumpSound;
	public AudioClip doubleJumpSound;
    public AudioClip fallSound;
	public AudioClip landSound;
	public AudioClip floatSound;
	public AudioClip bossDying;
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
	private float origPostMagnitude;
	private bool marioDeathJump;
	private bool canDoubleJump;
	private bool bossLevel;
	private bool hitCP;
	private float camEpsilon;
	private bool bossCam;
	public float bossCamY;
	public float endCamY;
	private bool bossBeat;
	private float bossBeatTimer;
	private bool startCam;

	private int dealDamage;
	private float origGravScale;
	private bool floating;
	private float origFloatTime;
	private float floatTimeLeft;
	private float floatDelay;
	private float origFloatDelay;
	public float camStartLerpDelay;

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
		marioDeathJump = false;
		canDoubleJump = true;
		origPostMagnitude = -1.5f;
		bossLevel = false;
		hitCP = false;
		dealDamage = 1;
		origGravScale = myRig.gravityScale;
		floating = false;
		origFloatTime = 2f;
		floatTimeLeft = origFloatTime;
		origFloatDelay = 0.1f;
		floatDelay = 0f;
		camEpsilon = 0.5f;
		bossCam = false;
		bossBeatTimer = 0f;
		startCam = true;
		bossBeat = false;
//		camStartLerpDelay = 2.3f;
    }
	
	// Update is called once per frame
	void Update () {
		if (debugging) {
			if (Input.GetKeyDown ("]")) {
				//turn blue
				grabOrb (0, Color.blue);
			} else if (Input.GetKeyDown ("[")) {
				grabOrb (1, Color.red);
			}
		}
		if (health > 0) {
			moveCameraHeight ();
			if (!startCam && !bossBeat) {
				
				//print(myRig.velocity.x);
				changeSprite ();
				myRig.velocity = new Vector2 (myRig.velocity.x * hFirction, myRig.velocity.y);
				Collider2D[] colliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, 
					                        k_GroundedRadius, 1 << LayerMask.NameToLayer ("Platforms"));
				if (colliders.Length < 1) {
					canJump = false; //colliding with nothing
					onPlatform = false;
				}
				bool canJumpOnOne = false;
				for (int i = 0; i < colliders.Length; i++) {
					try {
						togglePlatformBehavior platScript = colliders [i].GetComponent<togglePlatformBehavior> ();
						if (!platScript.inactive) {
							canJumpOnOne = true;
							break;
						}
					} catch (Exception e) {
						if (colliders [i].gameObject != gameObject) {
							canJumpOnOne = true;
							break;
						}
					}
				}
				if (canJumpOnOne) {
					canJump = true;
				}
				if (Input.GetKeyDown ("up") || Input.GetKeyDown ("w")) {
					if (canJump || canDoubleJump) {
						onPlatform = false;
						if (!bossBeat) {
							myAudio.clip = jumpSound;
						}
//					float yForce = jumpPower * 50f;
						myRig.velocity = new Vector2 (myRig.velocity.x, getJumpPower ());
						if (canDoubleJump && !canJump) {
							canDoubleJump = false;
							if (!bossBeat) {
								myAudio.clip = doubleJumpSound;
							}
//						yForce *= 0.75f;
						}
//					myRig.AddForce (new Vector2 (0f, yForce));
						if (!bossBeat) {
							myAudio.Play ();
						}
						canJump = false;
					}
				} else if ((Input.GetKey ("up") || Input.GetKey ("w")) && myRig.velocity.y < -5f && !canJump) {
					floatDown ();
				} else if ((Input.GetKeyUp ("up") || Input.GetKeyUp ("w") && floating)) {
					unfloatDown ();
				}
				if (Input.GetKey ("left") || Input.GetKey ("a")) {
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
		} else {
			if (!marioDeathJump) {
				marioDeathJump = true;
				myCollider.enabled = false;
				myRig.velocity = new Vector2 (0f, 45f);
				mySprite.sprite = jumpSprite;
				mySprite.flipY = true;
				Invoke ("respawn", 2f);
			}
		}
    }

	private void floatDown(){
		floatDelay += Time.deltaTime;
		if (floatDelay >= origFloatDelay) {
			floatTimeLeft -= Time.deltaTime;
			if (floatTimeLeft > 0) {
				Debug.Log ("floaty");
//		myRig.gravityScale = origGravScale / origGravScale;
				myRig.AddForce (new Vector2 (0f, -1f * 7f * myRig.velocity.y));
				if (!bossBeat) {
					myAudio.clip = floatSound;
					if (!myAudio.isPlaying) {
						myAudio.Play ();
					}
				}
				floating = true;
			} else {
				Debug.Log ("no more float time"); 
				floating = false;
				floatDelay = 0f;
			}
		}
	}

	private void unfloatDown(){
//		myRig.gravityScale = origGravScale;
		floatDelay = 0f;
		floating = false;
	}

	private void moveCameraHeight(){
		float offset = 0f;
		float speed = 0.75f;
		if (!bossBeat) {
			if (startCam) {
				speed = 0.5f;
				if (Input.GetKeyDown ("space") || Input.GetKeyDown("d") || Input.GetKeyDown("w") || Input.GetKeyDown("up") 
					|| Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("down") || Input.GetKeyDown("left")
					|| Input.GetKeyDown("right")) {
					startCam = false;
					foreach (Transform startText in CamCamera.transform) {
						if (startText.gameObject.activeInHierarchy) {
							startText.gameObject.SetActive (false);
						}
					}
				}
			}
			if (bossLevel && hitCP) {
				offset = 8f;
				speed = 0.1f;
				camEpsilon = 0.5f;// + offset;
				bossCam = true;
			}
			if (bossCam) {
				if (Mathf.Abs (bossCamY - CamCamera.transform.position.y) > camEpsilon) {
					CamCamera.transform.position = new Vector3 (CamCamera.transform.position.x, 
						Mathf.Lerp (CamCamera.transform.position.y, bossCamY, speed), 
						CamCamera.transform.position.z);
				}
			} else {
				if (startCam) {
					camStartLerpDelay -= Time.deltaTime;
					if (camStartLerpDelay <= 0) {
						foreach (Transform startText in CamCamera.transform) {
							if (!startText.gameObject.activeInHierarchy) {
								startText.gameObject.SetActive (true);
							}
						}
						Vector3 newPos = Vector3.MoveTowards (CamCamera.transform.position, transform.position, speed);
						CamCamera.transform.position = new Vector3 (CamCamera.transform.position.x, newPos.y, CamCamera.transform.position.z);
						if (Mathf.Abs (CamCamera.transform.position.y - transform.position.y) < camEpsilon) {
							foreach (Transform startText in CamCamera.transform) {
								if (startText.gameObject.activeInHierarchy) {
									startText.gameObject.SetActive (false);
								}
							}
							startCam = false;
						}
					}
				} else {
					CamCamera.transform.position = new Vector3 (CamCamera.transform.position.x, 
						Mathf.Lerp (CamCamera.transform.position.y, 
							transform.position.y + offset, speed), 
						CamCamera.transform.position.z);
				}
			}
			if (!(SceneManager.GetActiveScene ().buildIndex < SceneManager.sceneCountInBuildSettings - 1)) {
				bossLevel = true;
			}
		} else {
			bossBeatTimer += Time.deltaTime;
			if (bossBeatTimer >= 3.5f) {
				speed = 0.005f;
				CamCamera.transform.position = new Vector3 (CamCamera.transform.position.x, 
					Mathf.Lerp (CamCamera.transform.position.y, 
						endCamY, speed), 
					CamCamera.transform.position.z);
			}
		}
	}

	private float getJumpPower(){
		if (canJump)
			return jumpPower;
		float yVelo = jumpPower * 0.75f;
//		if ( //fixme if the player is going up faster, yVelo is going to be slower and using the double jump will slow them down
		if (canDoubleJump && lastTouchedPost && myRig.velocity.y > 15f){
			Debug.Log ("***************: " + myRig.velocity.y);
			myRig.AddForce (new Vector2 (0f, jumpPower * 350f * myRig.mass / myRig.velocity.y));
			yVelo = myRig.velocity.y;
		}
		return yVelo;
	}

	void respawn(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
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
		if (myRig.velocity.y > epsilon && !canJump) {
			mySprite.sprite = jumpSprite;
		} else if (myRig.velocity.y < -1f * epsilon && Mathf.Abs (myRig.velocity.x) <= epsilon) {
			mySprite.sprite = jumpSprite; //fixme fall sprite?
		}
		if (Mathf.Abs (myRig.velocity.x) <= epsilon && Mathf.Abs (myRig.velocity.y) <= epsilon){
			mySprite.sprite = idleSprite;
		}
		if (floating) {
			mySprite.sprite = floatSprite;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
//		if (other.gameObject.tag == "plane") {
//			other.GetComponent<planeDropFireballsBehavior> ().getHit (dealDamage);
//		}
	}

	void OnCollisionStay2D(Collision2D other){
//		Debug.Log ("collStay");
		bool hitACP = other.gameObject.tag == "checkPointPlat";
		if (hitACP) {
			hitCP = true;
		}
		if (other.gameObject.tag == "targetPlatform" || hitACP) {
//			platformBehavoir platScript = other.gameObject.GetComponent<platformBehavoir> ();
			togglePlatformBehavior platScript = other.gameObject.GetComponent<togglePlatformBehavior> ();
			if (!platScript.inactive) {
				lastTouchedPost = false;
				if (Input.GetKeyDown ("down") || Input.GetKeyDown ("s") && !hitACP) {
					platScript.letPlayerFallThrough ();
					//fixme play fall through animation?
				}
				onPlatform = true;
			}
		} else if (other.gameObject.tag == "bottomEdge") {
			lastTouchedPost = false;
			onPlatform = true;
		} else if (other.gameObject.tag == "post") {
			post postScript = other.gameObject.GetComponent<post> ();
			if (!postScript.inactive) {
				if (Input.GetKeyDown ("down") || Input.GetKeyDown ("s")) {
					postScript.letPlayerFallThrough ();
				}
				onPlatform = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "post") {
			canDoubleJump = true;
			onPlatform = true;
			float postEpsilon = -30f;
//			Debug.Log ("****************   " + myRig.velocity.y);
			if (myRig.velocity.y > postEpsilon || Input.GetKey ("down") || Input.GetKey ("s")) {
				Debug.Log ("too slow, treat as a regular platform");
				lastTouchedPost = false;
			} else {
				postJump (other.gameObject.GetComponent<post> ());
			}
			floatTimeLeft = origFloatTime;
		} else if (other.gameObject.tag == "targetPlatform" || other.gameObject.tag == "checkPointPlat") {
			if (!other.gameObject.GetComponent<togglePlatformBehavior> ().inactive) {
				canDoubleJump = true;
				onPlatform = true;
				if (!bossBeat) {
					myAudio.clip = landSound;
					myAudio.Play ();
				}
				floatTimeLeft = origFloatTime;
			}
		} else if (other.gameObject.tag == "bottomEdge") {
			canDoubleJump = true;
			onPlatform = true;
			if (bossLevel && !bossBeat) {
				getHit (health);
			}
			if (!bossBeat) {
				myAudio.clip = landSound;
				myAudio.Play ();
			}
			floatTimeLeft = origFloatTime;
		}
	}

	private void postJump(post postScript){
		float magnitude = origPostMagnitude;
		if (postScript.active && !postScript.inactive)
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
			postScript.playSound ();
			//fixme set a max velo?
		}
	}

	//used by planes/fireballs/enemies
	public void getHit(int damage){
		if (!bossBeat) {
			Debug.Log ("player got hit with " + damage);
			health -= damage;
			if (health <= 0) {
				health = 0; //potentially for display purposes
				Debug.Log ("player dies");
				//fixme play death animation
			} else {
				//fixmeplay get hit animation
			}
		}
	}

	//used by toggleOrbs
	public void grabOrb(int val, Color color){ //fixme should there be a timer for all toggleValues?
		toggleValue = val;
//		mySprite.color = toggleItemColors [toggleValue];
		if (val != 2) { 
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
						if (togglePlat.gameObject.tag == "targetPlatform") {
							togglePlat.GetComponent<togglePlatformBehavior> ().setInactive ();
						} else {
							togglePlat.GetComponent<post> ().setInactive ();
						}
					}
				}
			}
		} else {
			//fixme is for the boss level stuff
			mySprite.color = origColor;
			for (int i = 0; i < togglePlatforms.Length; i++) {
				if (i == val) {
					Debug.Log ("i is val: " + i);
					foreach (Transform togglePost in togglePlatforms[i].transform) {
						togglePost.GetComponent<post> ().setActive ();
					}
				} else {
					Debug.Log ("i is not val: " + i);
					foreach (Transform togglePlat in togglePlatforms[i].transform) {
						togglePlat.GetComponent<togglePlatformBehavior> ().setInactive ();
					}
				}
			}
		}
	}

	public void resetToggleValue(){
		toggleValue = -1;
//		mySprite.color = toggleItemColors [toggleValue];
		mySprite.color = origColor;
	}

	public void bossDead(){
		bossBeat = true;
		CamCamera.GetComponent<AudioSource> ().Stop ();
		myAudio.clip = bossDying;
		myAudio.Play ();
	}

}