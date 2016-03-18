using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            if (Input.GetAxis("Vertical") < 0)
            {
//                int i = Application.loadedLevel;
//                Application.LoadLevel(i + 1);
				int i = SceneManager.GetActiveScene().buildIndex;
				if (i + 1 < SceneManager.sceneCountInBuildSettings) {
					SceneManager.LoadScene (i + 1);
				} else {
					Debug.Log ("this is the last level in teh scene");
//					Debug.Break();
				}
            }
        }
    }
}
