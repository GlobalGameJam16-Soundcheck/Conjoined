using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour {

	private bool debugging;
	private float timer;
	private float nextLevelTime;

	// Use this for initialization
	void Start () {
		debugging = true;
		timer = 0f;
		nextLevelTime = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (debugging) {
			if (Input.GetKeyDown ("2")) {
				goToNextLevel (1);
			} else if (Input.GetKeyDown ("1")) {
				goToNextLevel (-1);
			} else if (Input.GetKeyDown ("r")) {
				goToNextLevel (0);
			}
		}
	}

	//fixme gotta do this in player
//	void OnCollisionEnter2D(Collision2D other){
//		if (other.gameObject.tag == "player") {
//			Debug.Break ();
//			timer = 0f;
//		}
//	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "player")
        {
			timer += Time.deltaTime;
			if (Input.GetAxis("Vertical") < 0 || timer >= nextLevelTime)
            {
				goToNextLevel (1);
            }
        }
		//fixme if debugging is true, pressing number skips to next level
		//stand in exit for 1 second == next level
    }

	private void goToNextLevel(int j){
		int i = SceneManager.GetActiveScene().buildIndex;
		if (0 <= i + j && i + j < SceneManager.sceneCountInBuildSettings) {
			SceneManager.LoadScene (i + j);
		} else {
			Debug.Log ("this is the last level in teh scene");
		}
	}
}
