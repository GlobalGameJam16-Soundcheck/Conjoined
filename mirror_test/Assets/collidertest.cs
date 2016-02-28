using UnityEngine;
using System.Collections;

public class collidertest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Contllider other)
    {
        print("Still colliding with trigger object " + other.name);
    }
}
