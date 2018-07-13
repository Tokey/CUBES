using UnityEngine;
using System.Collections;

public class TutoDestroyer : MonoBehaviour {

    public float time=7f;
    public GameObject img;
    public GameObject txt;
    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetInt("Tutorial",0)==1)
        {
            Destroy(img);
            Destroy(txt);
        }

    }
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if(time<=0)
        {
            Destroy(img);
            Destroy(txt);
        }
	}
}
