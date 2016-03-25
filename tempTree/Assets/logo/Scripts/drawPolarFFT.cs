using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class drawPolarFFT : MonoBehaviour
{
    AudioSource audio;
    float[] spectrum = new float[128];
    public GameObject mySprite;
    public float xScale = 25.0f;
    public float yScale = 6.0f;
    public float xOffset = 6.0f;
    public float yOffset = 6.0f;
	public float beforeDelay = 2f;
	public float afterDelay = 2f;
    private float clipLength;
	private float timer;
	private bool started;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        clipLength = audio.clip.length;
		timer = 0f;
		started = false;
    }

    void Update()
    {
		if (Input.GetKeyDown("space"))
		{
			goToNextLevel (1);
		}
		if (!started) {
			timer += Time.deltaTime;
		}
		if (timer >= beforeDelay && !started){
			timer = 0f;
			started = true;
			audio.Play ();
		} else {
			if (audio.isPlaying) {
				audio.GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);
				int i = 1;
				while (i < spectrum.Length - 1) {
					//transform.Rotate(new Vector3(0, 0, 1), i / xScale);
					GameObject point = (GameObject)Instantiate (mySprite, new Vector3 (0 + xOffset, Mathf.Log (spectrum [i - 1]) / yScale + yOffset, 0), new Quaternion (0, 0, 0, 0));
					point.transform.RotateAround (transform.position, new Vector3 (0, 0, 1), map (i, 0, 128, 0, 360));
					//SpriteRenderer pointRend = point.gameObject.GetComponent<SpriteRenderer>();
					//pointRend.color = new Color(0.0f, 0.5f, map(audio.time, 0, clipLength, 0.0f, 1.0f), 1.0f);
					//Debug.Log(clipLength);
					//pointColor = new Color(0,0,map(audio.time, 0, clipLength, 0, 255));
					//Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
					i++;
				}
			} else {
				if (started) {
					timer += Time.deltaTime;
					if (timer >= afterDelay) {
						goToNextLevel (1);
					}
				}
			}
		}
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
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