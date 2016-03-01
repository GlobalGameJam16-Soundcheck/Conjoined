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

    // Use this for initialization
    void Awake () {
        targets = GameObject.FindGameObjectsWithTag("targetPlatform");
        player = GameObject.FindGameObjectWithTag("player");
        target = targets[Random.Range(0, targets.Length)];
        myCollider = gameObject.GetComponent<Collider2D>();
        myRig = gameObject.GetComponent<Rigidbody2D>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        myAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > myStart.position.x)
        {
            if (target.GetComponent<platformBehavoir>().active)
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
            if (target.GetComponent<platformBehavoir>().active)
            {
                mySprite.flipX = false;
            }
            else
            {
                mySprite.flipX = true;
            }
        }
        if (target.GetComponent<platformBehavoir>().active)
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
        }
        if (other.name == target.name)
        {
            other.GetComponent<platformBehavoir>().playDeactivateSound();
            other.GetComponent<platformBehavoir>().active = false;
        }
    }
}
