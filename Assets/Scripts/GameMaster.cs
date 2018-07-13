using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
public class GameMaster : MonoBehaviour {

    public EventSystem eventSystem;
	public Transform[] towers;
	private int currTower;
	private Vector3 lastTowerPos;

	public GameObject lineRenderer;
	private float counter;
	private float dist;
	private Vector3 vert;
	private Vector3 hort;
	public PlayerMovement player;
	public Transform origin;
	public Transform destination;
	public float lineDrawSpeed = 200f;

	private bool firstHalf;
	private bool timeForHortizonal;

	private Vector3 jumpPosition;
	private Vector3 xCom;
	private Vector3 yCom;
	private Vector3 pointAlongLine;
	private int flag;

	public Target target;
	public Transform nextPos;

	private Animator targertAnim;
    private AudioManager audioManager;

    public GameObject LePlayer;
 

	public GameObject sun;
	private bool dayTime;
	private int time;

	public GameObject pauseScreen;

    public bool isInUI;
    public GameObject pauseButton;
	private bool gamePaused;
    public GameObject scrTxt;
    public Text pScore;
    public Text pHS;

    public GameObject tutimg;
    public GameObject tuttxt;



    // Use this for initialization
    void Start () {
        isInUI = false;
        AdManager.Instance.ShowBanner();
        gamePaused = false;
		dayTime = true;
		time = 5;

		lineRenderer.SetActive (true);
		lineRenderer.transform.position = origin.position;


		firstHalf = true;
		timeForHortizonal = true;

		jumpPosition = Vector3.zero;
		lastTowerPos = towers [towers.Length - 1].position;
		currTower = 0;
		flag = 0;
		targertAnim = target.gameObject.GetComponent<Animator> ();
		targertAnim.SetBool ("jump",false);

		Time.timeScale = 1.0F;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (player.onGround) {
			lineRenderer.SetActive (true);
			AnimateLine (hort);
		} else
			lineRenderer.SetActive (false);
			
		if(flag == 1)
		{
			
			jumpPosition = new Vector3 (xCom.x,destination.position.y,destination.position.z);
			player.Jump (jumpPosition);
			jumpPosition = Vector3.zero;

			flag = 0;

		}
		if(CrossPlatformInputManager.GetAxis("FUCKINGPAUSE")>0 ||(Input.GetKeyDown(KeyCode.Escape) &&player.Alive==true))
		{
            
			Pause ();
			CrossPlatformInputManager.SetAxis ("FUCKINGPAUSE", 0);
		}

		
		if (flag == 0&& LePlayer.gameObject.GetComponent<Rigidbody>()&& LePlayer.GetComponent<Rigidbody>().velocity.magnitude==0&&!isInUI && !(CrossPlatformInputManager.GetAxis("FUCKINGPAUSE")>0)&&Input.GetMouseButtonDown(0)) {

			timeForHortizonal = !timeForHortizonal;
			xCom = pointAlongLine;

            if (!gamePaused)
                flag = 1;
		}


		if(player.touchedTarget)
		{	
			StartCoroutine (targerAnimation ());
			StartCoroutine (ReLocateTower());	

			int jumTo = currTower + 1;
				
			if (jumTo == towers.Length)
				jumTo = 0;
			target.Jump (towers[jumTo].GetChild(0).position);


		}

	}
	IEnumerator ReLocateTower()
	{	currTower++;
		if (currTower == towers.Length)
			currTower = 0;


		yield return new WaitForSeconds(1.0f);


		if (currTower > 0) {
			towers [currTower - 1].position = new Vector3 (lastTowerPos.x - 4f, towers [currTower - 1].position.y, Random.Range(-2.0f, 2.0f));
			lastTowerPos = new Vector3 (lastTowerPos.x - 4f, towers [currTower - 1].position.y, Random.Range(-2.0f, 2.0f));
		} else 
		{
			towers [towers.Length - 1].position = new Vector3 (lastTowerPos.x - 4f, towers [towers.Length - 1].position.y,Random.Range(-5.0f, 5.0f));
			lastTowerPos = new Vector3 (lastTowerPos.x - 4f, towers [towers.Length - 1].position.y,Random.Range(-5.0f, 5.0f));
		}
	}
	void AnimateLine(Vector3 des)
	{	
		vert = new Vector3 (origin.position.x,destination.position.y,origin.position.z);
		hort = new Vector3 (destination.position.x,origin.position.y,origin.position.z);

		Vector3 midPoint = (vert + des) / 2;

		dist = Vector3.Distance (origin.position,destination.position);
		lineRenderer.transform.position = origin.position;


		
		if (firstHalf) {
				counter += 0.159f / lineDrawSpeed;
				float x = Mathf.Lerp (0, dist, counter);
				Vector3 pointA = origin.position;
				Vector3 pointB = des;

			pointAlongLine = x * Vector3.Normalize (destination.position - pointA) + pointA;
			lineRenderer.transform.position = new Vector3 (pointAlongLine.x, origin.position.y + 2*Mathf.Sin ((x/dist)*Mathf.PI),pointAlongLine.z);
				if (x >= dist)
					firstHalf = false;
			
			} else if (!firstHalf) {
				counter -= 0.159f / lineDrawSpeed;
				float x = Mathf.Lerp (0, dist, counter);
				Vector3 pointA = origin.position;
				Vector3 pointB = des;

			pointAlongLine = x * Vector3.Normalize (destination.position - pointA) + pointA;
			lineRenderer.transform.position = new Vector3 (pointAlongLine.x, origin.position.y + 2*Mathf.Sin ((x/dist)*Mathf.PI),pointAlongLine.z);

				if (x <= 0)
					firstHalf = true;
			}

	}

	IEnumerator targerAnimation()
	{
		targertAnim.SetBool ("jump",true);
		yield return new WaitForSeconds(1.0f);
		targertAnim.SetBool ("jump",false);
	
	}

    public void Quit()
    {
        Application.Quit();
    }
    
	public void Pause()
	{
        IsInUIToggle();
        gamePaused = !gamePaused;
        if (Time.timeScale == 1.0F)
        {
            Time.timeScale = 0.05F;
            pScore.text="SCORE : " +PlayerMovement.instance.getScore().ToString();
            pHS.text = "BEST : "+PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
        else
            Time.timeScale = 1.0F;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;

		pauseScreen.SetActive (!pauseScreen.activeSelf);
        pauseButton.SetActive(!pauseButton.activeSelf);
        scrTxt.SetActive(!scrTxt.activeSelf);
        tutimg.SetActive(!tutimg.activeSelf);
        tuttxt.SetActive(!tuttxt.activeSelf);



    }
    public void IsInUIToggle()
    {
        AdManager.Instance.ShowVideo();
        isInUI = !isInUI;
    }
}
