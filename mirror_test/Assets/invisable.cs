using UnityEngine;
using System.Collections;

public class invisable : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().color = new Color (0,0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
