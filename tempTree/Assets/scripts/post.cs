using UnityEngine;
using System.Collections;

public class post : MonoBehaviour {

    public bool active = true;
    bool reloading = false;
    public float reloadTime;
    SpriteRenderer mySprite;
    public bool vertical = false;
	public GameObject solidWhite;
	private SpriteRenderer solidWhiteSprite;
	private float solidWhiteAlpha;

	// Use this for initialization
	void Start () {
        mySprite = gameObject.GetComponent<SpriteRenderer>();
		solidWhiteSprite = solidWhite.GetComponent<SpriteRenderer> ();
		solidWhiteAlpha = solidWhiteSprite.color.a;
	}
	
	// Update is called once per frame
	void Update () {
	    if(active == false && reloading == false)
        {
            Invoke("reload", reloadTime);
            reloading = true;
        }
        if (active)
        {
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b);
			solidWhiteSprite.color = new Color(solidWhiteSprite.color.r, solidWhiteSprite.color.g, solidWhiteSprite.color.b, solidWhiteAlpha);
        }
        else
        {
			mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.5f);
			solidWhiteSprite.color = new Color(solidWhiteSprite.color.r, solidWhiteSprite.color.g, solidWhiteSprite.color.b, solidWhiteAlpha * 0.5f);

        }
	}

    void reload()
    {
        active = true;
        reloading = false;
    }

}
