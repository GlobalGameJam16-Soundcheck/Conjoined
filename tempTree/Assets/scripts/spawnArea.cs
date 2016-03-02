using UnityEngine;
using System.Collections;

public class spawnArea : MonoBehaviour {

    public GameObject thingToSpawn;
    public float delay = 0.0f;
    public float time = 1.0f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("spawn", delay, time);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("`"))
        {
            time = 1.0f;
        }
    }

    void spawn()
    {
        float xPos = Random.Range(transform.position.x - transform.localScale.x / 4, transform.position.x + transform.localScale.x / 4);
        float yPos = Random.Range(transform.position.y - transform.localScale.y / 4, transform.position.y + transform.localScale.y / 4);
        GameObject thingIJustSpawned = Instantiate(thingToSpawn, new Vector2(xPos,yPos), Quaternion.identity) as GameObject;
        thingIJustSpawned.GetComponent<planeBehavore>().myStart = transform;
        time += 0.005f;
    }
}