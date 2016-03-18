using UnityEngine;
using System.Collections;

public class post : MonoBehaviour {

    public bool active = true;
    bool reloading = false;
    public float reloadTime;
    SpriteRenderer mySprite;
    public bool vertical = false;
	public GameObject[] bgSprites;
	private SpriteRenderer[] bgSpriteRenderers;
	private float[] bgSpriteAlphas;

	//for the player falling through
	private PlatformEffector2D pe2d;
	private int playerLayer;
	private bool playerFallingThrough;
	private float timer;
	private float fallTime;

	// Use this for initialization
	void Start () {
        mySprite = gameObject.GetComponent<SpriteRenderer>();
		bgSpriteRenderers = new SpriteRenderer[bgSprites.Length];
		for (int i = 0; i < bgSprites.Length; i++){
			bgSpriteRenderers [i] = bgSprites[i].GetComponent<SpriteRenderer> ();
		}
		bgSpriteAlphas = new float[bgSpriteRenderers.Length];
		for (int j = 0; j < bgSpriteAlphas.Length; j++) {
			bgSpriteAlphas [j] = bgSpriteRenderers [j].color.a;
		}

		//for the player falling through
		timer = 0f;
		fallTime = 0.25f;
		pe2d = GetComponent<PlatformEffector2D> ();
		playerLayer = (1 << LayerMask.NameToLayer ("Player"));
	}
	
	// Update is called once per frame
	void Update () {
		SpriteRenderer solidWhiteSprite;
	    if(active == false && reloading == false)
        {
            Invoke("reload", reloadTime);
            reloading = true;
        }
        if (active)
        {
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b);
			for (int i = 0; i < bgSpriteRenderers.Length; i++) {
				solidWhiteSprite = bgSpriteRenderers [i];
				solidWhiteSprite.color = new Color(solidWhiteSprite.color.r, solidWhiteSprite.color.g, solidWhiteSprite.color.b, bgSpriteAlphas[i]);
			}
        }
        else
        {
			mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.5f);
			for (int i = 0; i < bgSpriteRenderers.Length; i++) {
				solidWhiteSprite = bgSpriteRenderers [i];
				solidWhiteSprite.color = new Color(solidWhiteSprite.color.r, solidWhiteSprite.color.g, solidWhiteSprite.color.b, bgSpriteAlphas[i] * 0.5f);
			}
        }

		//for the player falling through
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

    void reload()
    {
        active = true;
        reloading = false;
    }

	//for the player falling through
	public void letPlayerFallThrough(){
		pe2d.colliderMask = pe2d.colliderMask & ~playerLayer; //lets collider ignore player
		playerFallingThrough = true;
	}

}
