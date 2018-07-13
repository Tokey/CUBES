using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Text musicText;
    public Text vibrationText;

    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    private Transform myTransform;

    public int gokuCash;
    public int supermanCash;
    public int spidermanCash;
    public int austCash;

    public Text gokuText;
    public Text supermanText;
    public Text spidermanText;
    public Text austText;
    public Text defaultText;
    public Text gemtText;

    public Text tut;

    public void Quit()
    {
        AdManager.Instance.ShowRewardedVideo();
        Application.Quit();
    }

    void Start()
    {

        AdManager.Instance.RemoveBanner();
        Time.timeScale = 0.05F;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        myTransform = Projectile;
        StartCoroutine(SimulateProjectile(Target.position));

        if(PlayerPrefs.GetInt("SetDefault")!=100 && PlayerPrefs.GetInt("SetGoku") != 100&& PlayerPrefs.GetInt("SetSuperman") != 100 && PlayerPrefs.GetInt("SetAUST") != 100 && PlayerPrefs.GetInt("SetSpiderman") != 100 )
        {
            UpgradeMenu(0);
            PlayerPrefs.SetInt("SetDefault", 100);
        }

         if(PlayerPrefs.GetInt("SetSuperman") == 100)
            UpgradeMenu(3);
        else if (PlayerPrefs.GetInt("SetGoku") == 100)
            UpgradeMenu(4);
        else if (PlayerPrefs.GetInt("SetSpiderman") == 100)
            UpgradeMenu(2);
        else if (PlayerPrefs.GetInt("SetAUST") == 100)
            UpgradeMenu(1);
         else
            UpgradeMenu(0);


        if (PlayerPrefs.GetInt("MusicToggle", 0) == 0)
            musicText.text = "ON";
        else
            musicText.text = "OFF";

        if (PlayerPrefs.GetInt("VibrationToggle", 0) == 0)
            vibrationText.text = "ON";
        else
            vibrationText.text = "OFF";

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
            tut.text = "ON";
        else
            tut.text = "OFF";



    }

    void Update()
    {
       
        

    }
    public void MusicToggle()
    {
        if (PlayerPrefs.GetInt("MusicToggle", 0) == 0)
        {
            PlayerPrefs.SetInt("MusicToggle", 1);
            AudioManager.instance.StopSound("MenuMusic");
        }
        else
        {
            PlayerPrefs.SetInt("MusicToggle", 0);
            AudioManager.instance.PlaySound("MenuMusic");
        }

        if (PlayerPrefs.GetInt("MusicToggle", 0) == 0)
            musicText.text = "ON";
        else
            musicText.text = "OFF";

    }

    public void VibrationToggle()
    {
        if (PlayerPrefs.GetInt("VibrationToggle", 0) == 0)
            PlayerPrefs.SetInt("VibrationToggle", 1);
        else
            PlayerPrefs.SetInt("VibrationToggle", 0);

        if (PlayerPrefs.GetInt("VibrationToggle", 0) == 0)
            vibrationText.text = "ON";
        else
            vibrationText.text = "OFF";

    }


    IEnumerator SimulateProjectile(Vector3 destination)
    {
        RemoveRigidBody();
        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(0f);

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

        AddRigidBody();

    }
    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }

    public void ReturnToHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }

    void AddRigidBody()
    {
        if (!Projectile.gameObject.GetComponent<Rigidbody>())
            myTransform.gameObject.AddComponent<Rigidbody>();

    }

    void RemoveRigidBody()
    {
        if (Projectile.gameObject.GetComponent<Rigidbody>())
            Destroy(this.gameObject.GetComponent<Rigidbody>());

    }

    public void BuyGoku()
    {
        if (PlayerPrefs.GetInt("BoughtGoku", 0) != 100)
        {
            if (PlayerPrefs.GetInt("Gems", 0) >= gokuCash)
            {
                PlayerPrefs.SetInt("Gems", (PlayerPrefs.GetInt("Gems", 0) - gokuCash));
                PlayerPrefs.SetInt("BoughtGoku", 100);
                AudioManager.instance.PlaySound("Cash");

                UpgradeMenu(4);
            }
            else
                AudioManager.instance.PlaySound("Denied");
        }
        else
        {
            AudioManager.instance.PlaySound("Select");
            PlayerPrefs.SetInt("SetAUST", 0);
            PlayerPrefs.SetInt("SetDefault", 0);
            PlayerPrefs.SetInt("SetGoku", 100);
            PlayerPrefs.SetInt("SetSuperman", 0);
            PlayerPrefs.SetInt("SetSpiderman", 0);
            UpgradeMenu(4);
        }

    }
    public void BuySuperman()
    {
        if (PlayerPrefs.GetInt("BoughtSuperman", 0) != 100)
        {
            if (PlayerPrefs.GetInt("Gems", 0) >= supermanCash)
            {
                PlayerPrefs.SetInt("Gems", (PlayerPrefs.GetInt("Gems", 0) - supermanCash));
                PlayerPrefs.SetInt("BoughtSuperman", 100);
                AudioManager.instance.PlaySound("Cash");

                UpgradeMenu(3);
            }
            else
                AudioManager.instance.PlaySound("Denied");
            
        }
        else
        {
            AudioManager.instance.PlaySound("Select");
            PlayerPrefs.SetInt("SetAUST", 0);
            PlayerPrefs.SetInt("SetDefault", 0);
            PlayerPrefs.SetInt("SetGoku", 0);
            PlayerPrefs.SetInt("SetSuperman", 100);
            PlayerPrefs.SetInt("SetSpiderman", 0);
            UpgradeMenu(3);
        }
    }
    public void BuySpiderman()
    {
        if (PlayerPrefs.GetInt("BoughtSpiderman", 0) != 100)
        {
            if (PlayerPrefs.GetInt("Gems", 0) >= spidermanCash)
            {
                PlayerPrefs.SetInt("Gems", (PlayerPrefs.GetInt("Gems", 0) - spidermanCash));
                PlayerPrefs.SetInt("BoughtSpiderman", 100);
                UpgradeMenu(2);
                AudioManager.instance.PlaySound("Cash");
            }
            else
                AudioManager.instance.PlaySound("Denied");
        }
        else
        {
            AudioManager.instance.PlaySound("Select");
            PlayerPrefs.SetInt("SetAUST", 0);
            PlayerPrefs.SetInt("SetDefault", 0);
            PlayerPrefs.SetInt("SetGoku", 0);
            PlayerPrefs.SetInt("SetSuperman", 0);
            PlayerPrefs.SetInt("SetSpiderman", 100);
            UpgradeMenu(2);
        }

    }
    public void BuyAUST()
    {
        if (PlayerPrefs.GetInt("BoughtAUST", 0) != 100)
        {
            if (PlayerPrefs.GetInt("Gems", 0) >= austCash)
            {
                PlayerPrefs.SetInt("Gems", (PlayerPrefs.GetInt("Gems", 0) - austCash));
                PlayerPrefs.SetInt("BoughtAUST", 100);
                UpgradeMenu(1);
                AudioManager.instance.PlaySound("Cash");
            }
            else
                AudioManager.instance.PlaySound("Denied");
        }
        else
        {
            AudioManager.instance.PlaySound("Select");
            PlayerPrefs.SetInt("SetAUST", 100);
            PlayerPrefs.SetInt("SetDefault", 0);
            PlayerPrefs.SetInt("SetGoku", 0);
            PlayerPrefs.SetInt("SetSuperman", 0);
            PlayerPrefs.SetInt("SetSpiderman", 0);
            UpgradeMenu(1);
        }
    }

    public void buyDefault()
    {
        AudioManager.instance.PlaySound("Select");
        PlayerPrefs.SetInt("SetAUST", 0);
        PlayerPrefs.SetInt("SetDefault", 100);
        PlayerPrefs.SetInt("SetGoku", 0);
        PlayerPrefs.SetInt("SetSuperman", 0);
        PlayerPrefs.SetInt("SetSpiderman", 0);
        UpgradeMenu(0);
    }

    public void UpgradeMenu(int Select)
    {
        if (Select == 0)
        {
            defaultText.text = "SELECTED";
            if (PlayerPrefs.GetInt("BoughtSpiderman", 0) != 100)
                spidermanText.text = spidermanCash.ToString();
            else
                spidermanText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtSuperman", 0) != 100)
                supermanText.text = supermanCash.ToString();
            else
                supermanText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtGoku", 0) != 100)
                gokuText.text = gokuCash.ToString();
            else
                gokuText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtAUST", 0) != 100)
                austText.text = austCash.ToString();
            else
                austText.text = "DESELECTED";
        }
        else if (Select == 1)
        {
            austText.text = "SELECTED";
            defaultText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtSpiderman", 0) != 100)
                spidermanText.text = spidermanCash.ToString();
            else
                spidermanText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtSuperman", 0) != 100)
                supermanText.text = supermanCash.ToString();
            else
                supermanText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtGoku", 0) != 100)
                gokuText.text = gokuCash.ToString();
            else
                gokuText.text = "DESELECTED";


        }
        else if (Select == 2)
        {
            spidermanText.text = "SELECTED";
            defaultText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtSuperman", 0) != 100)
                supermanText.text = supermanCash.ToString();
            else
                supermanText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtGoku", 0) != 100)
                gokuText.text = gokuCash.ToString();
            else
                gokuText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtAUST", 0) != 100)
                austText.text = austCash.ToString();
            else
                austText.text = "DESELECTED";

           

        }
        else if (Select == 3)
        {
            supermanText.text = "SELECTED";
            defaultText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtAUST", 0) != 100)
                austText.text = austCash.ToString();
            else
                austText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtGoku", 0) != 100)
                gokuText.text = gokuCash.ToString();
            else
                gokuText.text = "DESELECTED";

            if (PlayerPrefs.GetInt("BoughtSpiderman", 0) != 100)
                spidermanText.text = spidermanCash.ToString();
            else
                spidermanText.text = "DESELECTED";


        }
        else if (Select == 4)
        {
            gokuText.text = "SELECTED";
            defaultText.text = "DESELECTED";
            if (PlayerPrefs.GetInt("BoughtSpiderman", 0) != 100)
                spidermanText.text = spidermanCash.ToString();
            else
                spidermanText.text = "DESELECTED";
            if (PlayerPrefs.GetInt("BoughtAUST", 0) != 100)
                austText.text = austCash.ToString();
            else
                austText.text = "DESELECTED";


            if (PlayerPrefs.GetInt("BoughtSuperman", 0) != 100)
                supermanText.text = supermanCash.ToString();
            else
                supermanText.text = "DESELECTED";


        }
        gemtText.text = PlayerPrefs.GetInt("Gems", 0).ToString();
}
    public void ShowAd()
    {
        AdManager.Instance.ShowRewardedVideo();
        AdManager.Instance.ShowVideo();

    }

    public void TutToggle()
    {
        if(PlayerPrefs.GetInt("Tutorial",0)==0)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
            tut.text = "OFF";
        }
        else
        {
            PlayerPrefs.SetInt("Tutorial", 0);
            tut.text = "ON";
        }
    }
}
