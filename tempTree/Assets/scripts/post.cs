using UnityEngine;
using System.Collections;

public class post : MonoBehaviour {

    public bool active = true;
    bool reloading = false;
    public float reloadTime = 1;
    SpriteRenderer mySprite;
    public bool vertical = false;

	// Use this for initialization
	void Start () {
        mySprite = gameObject.GetComponent<SpriteRenderer>();
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
        }
        else
        {
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.5f);
        }
	}

    void reload()
    {
        active = true;
        reloading = false;
    }
}
