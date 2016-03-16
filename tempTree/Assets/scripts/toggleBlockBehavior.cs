using UnityEngine;
using System.Collections;

public class toggleBlockBehavior : MonoBehaviour {

	public int value;
	public bool active { get; set; }//if active == true then this block can stop the player, else player passes through
	private int playerLayer;
	private PlatformEffector2D pe2d;

	// Use this for initialization
	void Start () {
		active = true;
		playerLayer = (1 << LayerMask.NameToLayer ("Player"));
		pe2d = GetComponent<PlatformEffector2D> ();
	}

	public bool attemptToUnblock(int tryVal){
		Debug.Log ("attempt to unblock");
		if (tryVal == value) {
			Debug.Log ("unblocked");
			active = false;
			pe2d.colliderMask = pe2d.colliderMask & ~playerLayer;
			return true;
		} else {
			return false;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "player") {
			playerControler controller = other.gameObject.GetComponent<playerControler> ();
			if (attemptToUnblock (controller.toggleValue)) {
				controller.resetToggleValue ();
			}
		}
	}

}
