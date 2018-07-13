using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayAnim : MonoBehaviour {

    // Use this for initialization
    public Animator playAnim;
    bool playAnimFlag;
    public float time = 1f;
	public GameObject mainMenu;
    public Text highScore; 

    void Start () {
        playAnim.enabled = false;
        playAnimFlag = false;
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        time = 1f;


    }
	
	// Update is called once per frame
	void Update () {
        if (playAnimFlag)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                mainMenu.SetActive(false);
                Start();
            }
        }
	}
    public void TriggerPlayAnim()
    {
        playAnimFlag = true;
        playAnim.enabled = true;
		StartCoroutine(Play ());
    }

	IEnumerator Play()
	{
		Time.timeScale = 1.0F;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;

		yield return new WaitForSeconds (1f);

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex+1,LoadSceneMode.Single);
	}
}
