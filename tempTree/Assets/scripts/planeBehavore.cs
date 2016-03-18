using UnityEngine;
using System.Collections;

public class planeBehavore : MonoBehaviour {

    GameObject[] targets;
    public float speed = 1.0f;
    GameObject target;
    GameObject player;
    Collider2D myCollider;
    public Transform myStart;
    Rigidbody2D myRig;
    SpriteRenderer mySprite;
    public AudioClip explotionSound;
    AudioSource myAudio;
	public int damage; //when fireballs hit player, how much does this hurt the player?
	public bool bossMode;

	private bool targetHit;

    // Use this for initialization
    void Awake () {
        targets = GameObject.FindGameObjectsWithTag("targetPlatform");
        player = GameObject.FindGameObjectWithTag("player");
        target = targets[Random.Range(0, targets.Length)];
        myCollider = gameObject.GetComponent<Collider2D>();
        myRig = gameObject.GetComponent<Rigidbody2D>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        myAudio = gameObject.GetComponent<AudioSource>();
		targetHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > myStart.position.x)
        {
			if (!targetHit)
            {
                mySprite.flipX = true;
            }
            else
            {
                mySprite.flipX = false;
            }
        }
        else
        {
			if (!targetHit)
            {
                mySprite.flipX = false;
            }
            else
            {
                mySprite.flipX = true;
            }
        }
		if (!targetHit)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), target.transform.position, 3 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), myStart.transform.position, 3 * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            myAudio.clip = explotionSound;
            myAudio.Play();
            transform.position = new Vector2(-1000, -1000);
            Destroy(gameObject, 2);
			other.GetComponent<playerControler> ().getHit (damage);
        }
        if (other.name == target.name)
        {
			targetHit = true;
			Debug.Log ("target hit");
//            other.GetComponent<platformBehavoir>().playDeactivateSound();
//            other.GetComponent<platformBehavoir>().active = false;
        }
    }
}
