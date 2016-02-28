using UnityEngine;
using System.Collections;

public class playerControler : MonoBehaviour {

    Rigidbody2D myRig;
    Collider2D myCollider;
    SpriteRenderer mySprite;
    bool canJump;
    public float jumpPower = 40.0f;
    public float runSpeed = 2.0f;
    public float maxRunSpeed = 10.0f;
    float k_GroundedRadius = 0.3f;
    Transform m_GroundCheck;
    public float hFirction = 0.02f;

    // Use this for initialization
    void Awake () {
        myRig = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<Collider2D>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        m_GroundCheck = transform.Find("GroundCheck");
    }
	
	// Update is called once per frame
	void Update () {
        if(myRig.velocity.x > 0)
        {
            mySprite.flipX = true;
        }
        if(myRig.velocity.x < 0)
        {
            mySprite.flipX = false;
        }
        myRig.velocity = new Vector2(myRig.velocity.x * hFirction, myRig.velocity.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, LayerMask.NameToLayer("Platform"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                canJump = true;
        }
        if (Input.GetKeyDown("up"))
        {
            if (canJump)
            {
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
        }
        if (Input.GetKey("right"))
        {
            if (myRig.velocity.x < maxRunSpeed)
            {
                myRig.velocity = new Vector2(myRig.velocity.x + runSpeed, myRig.velocity.y);
            }
        }
    }
}
