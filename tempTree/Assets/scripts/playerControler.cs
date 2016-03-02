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

    // Use this for initialization
    void Awake () {
        myRig = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<Collider2D>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        myAudio = gameObject.GetComponent<AudioSource>();
        m_GroundCheck = transform.Find("GroundCheck");
    }
	
	// Update is called once per frame
	void Update () {
        //print(myRig.velocity.x);
        if(myRig.velocity.x > 0)
        {
            mySprite.sprite = rightSprite;
        }
        if(myRig.velocity.x < 0)
        {
            mySprite.sprite = leftSprite;
        }
        if(myRig.velocity.y > 0)
        {
            mySprite.sprite = jumpSprite;
        }
        if(myRig.velocity.x == 0 && myRig.velocity.y == 0)
        {
            mySprite.sprite = idleSprite;
        }
        myRig.velocity = new Vector2(myRig.velocity.x * hFirction, myRig.velocity.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, LayerMask.NameToLayer("Platform"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }
        if (Input.GetKeyDown("up"))
        {
            if (canJump)
            {
                myAudio.clip = jumpSound;
                myAudio.Play();
                myRig.velocity = new Vector2(myRig.velocity.x, jumpPower);
                canJump = false;
            }
        }

        if (Input.GetKey("left"))
        {
            if (myRig.velocity.x > -maxRunSpeed)
            {
                myRig.velocity = new Vector2(myRig.velocity.x - runSpeed, myRig.velocity.y);
            }
            if (myRig.velocity.x <= -maxRunSpeed)
            {
                myRig.velocity = new Vector2(-maxRunSpeed, myRig.velocity.y);
            }
        }
        if (Input.GetKey("right"))
        {
            if (myRig.velocity.x < maxRunSpeed)
            {
                myRig.velocity = new Vector2(myRig.velocity.x + runSpeed, myRig.velocity.y);
            }
            if (myRig.velocity.x >= maxRunSpeed)
            {
                myRig.velocity = new Vector2(maxRunSpeed, myRig.velocity.y);
            }
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "targetPlatform")
        {
            if (Input.GetKeyDown("space"))
            {
                other.GetComponent<platformBehavoir>().active = true;
            }
        }
        if (other.tag == "post")
        {
            if (other.gameObject.GetComponent<post>().active)
            {
                if (other.gameObject.GetComponent<post>().vertical)
                {
                    myRig.velocity = new Vector2(myRig.velocity.x, myRig.velocity.y * -1.3f);
                }
                else
                {
                    myRig.velocity = new Vector2(myRig.velocity.x *-1.3f, myRig.velocity.y);
                }
                other.gameObject.GetComponent<post>().active = false;
                if (Input.GetKeyDown("space"))
                {
                    if (other.gameObject.GetComponent<post>().vertical)
                    {
                        myRig.velocity = new Vector2(myRig.velocity.x, myRig.velocity.y * -1.2f);
                    }
                    else
                    {
                        myRig.velocity = new Vector2(myRig.velocity.x * -1.2f, myRig.velocity.y);
                    }
                    other.gameObject.GetComponent<post>().active = false;
                }
            }
        }
    }
}