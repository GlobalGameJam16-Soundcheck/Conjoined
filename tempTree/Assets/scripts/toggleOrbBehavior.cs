using UnityEngine;
using System.Collections;

public class toggleOrbBehavior : MonoBehaviour {

	public int value;
	private Color color;

	void Start(){
		color = GetComponent<SpriteRenderer> ().color;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "player") {
			Debug.Log ("player touching");
			other.gameObject.GetComponent<playerControler> ().grabOrb (value, color);
		}
	}
}
