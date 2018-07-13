using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {


	public float moveSpeed;
	public float movePressure;
	private Rigidbody player;
	public int direction = 1;
	public GameObject deathPaticle;
	public bool onGround;
	public bool touchedTarget;



	private Vector3 jumpdir;
	private Vector3 moveDir;

	private Vector3 spawn;



	public Transform Target;
	public float firingAngle = 45.0f;
	public float gravity = 9.8f;

	public Transform Projectile;      
	private Transform myTransform;
	private Animator anim;
    public Text scoreText;
	private int score;
    public Text highScoreText;
    public Text gHighScoreText;
    public Text gScoreText;
    int highScore;
    public GameObject gameOver;
    public GameObject menu;
    public GameMaster GM;
    public Text gemText;
	public GameObject gemParticle;
    int gems;
    public GameObject GOscr;
	public bool Alive;
    public static PlayerMovement instance;


    //UPGRADE AND SHIT



    void Awake()
	{
		myTransform = transform;   
		Alive = true;
	}

	void Start () {

        AdManager.Instance.ShowBanner();
        instance = this;
        GOscr.SetActive(true);
        score = 0;
        gems = PlayerPrefs.GetInt("Gems",0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
		spawn = transform.position;
		onGround = true;
		touchedTarget = false;

		anim = GetComponent<Animator> ();
		//player = GetComponent<Rigidbody> ();

		if(gemParticle==null)
		{
			Debug.Log ("Gems not referenced");
		}
	}
	

	void Update () {
 
        if (transform.position.y<1f)
		{
            if(Alive)
			Die ();
		}


        gemText.text = gems.ToString();
        scoreText.text = score.ToString();
	}

	public void Jump(Vector3 destination)
	{
        AudioManager.instance.PlaySound("Jump");
		StartCoroutine (SimulateProjectile(destination));
	}
	void OnCollisionEnter(Collision other)
	{
		if(other.transform.tag == "Enemy")
		{
			Die ();
		}
		if(other.transform.tag == "Ground")
		{
			onGround = true;
			anim.SetBool ("onGround",onGround);
		}
	}
    
		
	void OnTriggerEnter(Collider other)
  	{
		if(other.transform.tag == "Target")
		{
            if (PlayerPrefs.GetInt("VibrationToggle", 0) == 0)
                Handheld.Vibrate();
            

            touchedTarget = true;
            score++;
            AudioManager.instance.PlaySound("Point");
		}
		else if(other.transform.tag == "Gems")
		{
            if (Alive)
            {

                gems++;
                PlayerPrefs.SetInt("Gems", gems);
                AudioManager.instance.PlaySound("Crystal");
                Instantiate(gemParticle, transform.position, Quaternion.identity);
            }
		}

	}
	void Die()
	{

        PlayerPrefs.SetInt("TimesDied", PlayerPrefs.GetInt("TimesDied", 0) + 1);
        if (PlayerPrefs.GetInt("VibrationToggle", 0) == 0)
            Handheld.Vibrate();
        RemoveRigidBody();
        AudioManager.instance.PlaySound("Death");
        GOscr.SetActive(false);
        Instantiate(deathPaticle, transform.position, Quaternion.identity);
		Alive = false;

        if (score > highScore)
        { PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }

        
        gameOver.SetActive(true);
        gScoreText.text = "SCORE : " + score.ToString();
        gHighScoreText.text = "BEST : " + highScore.ToString();
        AdManager.Instance.RemoveBanner();
        if (PlayerPrefs.GetInt("TimesDied", 0) >= 3)
        {
            if (PlayerPrefs.GetInt("TimesDied", 0)%3==0)
            AdManager.Instance.ShowVideo();
        }

        

    }


	IEnumerator SimulateProjectile(Vector3 destination)
	{	
		RemoveRigidBody ();
		// Short delay added before Projectile is thrown
		yield return new WaitForSeconds(0f);
		onGround = false;
		anim.SetBool ("onGround",onGround);
		// Move projectile to the position of throwing object + add some offset if needed.
		Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

		// Calculate distance to target
		float target_Distance = Vector3.Distance(Projectile.position, destination);

		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
		float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

		// Calculate flight time.
		float flightDuration = target_Distance / Vx;

		// Rotate projectile to face the target.
		Projectile.rotation = Quaternion.LookRotation(destination - Projectile.position);
		//Projectile.rotation = Quaternion.identity;
		float elapse_time = 0;

		while (elapse_time < flightDuration)
		{
			Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

			elapse_time += Time.deltaTime;

			yield return null;
		}
		onGround = true;
		anim.SetBool ("onGround",onGround);
		AddRigidBody ();

	}
	public void RestartGame()
	{	
        
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);

	}

	public void ReturnToHome()
	{
        AdManager.Instance.ShowRewardedVideo();
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex-1,LoadSceneMode.Single);
	}

	void AddRigidBody()
	{
		if (!this.gameObject.GetComponent<Rigidbody> ())
			this.gameObject.AddComponent<Rigidbody> ();

	}

	void RemoveRigidBody()
	{
		if (this.gameObject.GetComponent<Rigidbody> ())
			Destroy(this.gameObject.GetComponent<Rigidbody> ());

	}

	public int getScore()
	{
		return score;
	}

	public void setScore(int _score)
	{
		score = _score;
	}

	public bool GetAlive()
	{
		return Alive;
	}
		

}
