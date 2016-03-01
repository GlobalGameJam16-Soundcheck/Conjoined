using UnityEngine;
using System.Collections;

public class platformBehavoir : MonoBehaviour {

    public bool active = true;
    Collider2D myCollider;
    SpriteRenderer mySprite;
    public AudioClip deactivateSound;
    AudioSource myAudio;

	// Use this for initialization
	void Start () {
        myCollider = gameObject.GetComponent<Collider2D>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        myAudio = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
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
}
