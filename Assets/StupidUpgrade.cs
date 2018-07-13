using UnityEngine;
using System.Collections;

public class StupidUpgrade : MonoBehaviour {
    public Material def;
    public Material goku;
    public Material superman;
    public Material aust;
    public Material spiderman;
    // Use this for initialization
    void Start () {
        Renderer rend = GetComponent<Renderer>();
        if (PlayerPrefs.GetInt("SetSuperman") == 100)
            rend.material = superman;
        else if (PlayerPrefs.GetInt("SetGoku") == 100)
            rend.material = goku;
        else if (PlayerPrefs.GetInt("SetSpiderman") == 100)
            rend.material = spiderman;
        else if (PlayerPrefs.GetInt("SetAUST") == 100)
            rend.material = aust;
        else
            rend.material = def;

        //PlayerPrefs.SetInt("Gems", 1000);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
