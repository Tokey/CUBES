using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Splash : MonoBehaviour {

	public float time;
	public GameObject image;

    void Start()
    {
        
    }
	
	void Update () {
		
		if (time <= 0) {
            SceneManager.LoadScene(1);
		}
		else
			time -= Time.deltaTime;
	}
}
