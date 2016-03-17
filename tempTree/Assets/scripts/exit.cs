using UnityEngine;
using System.Collections;

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
                int i = Application.loadedLevel;
                Application.LoadLevel(i + 1);
            }
        }
    }
}
