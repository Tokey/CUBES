using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
	public PlayerMovement player;
	public float firingAngle = 45.0f;
	public float gravity = 9.8f;

	public Transform Projectile;  
	public Transform gem;
	public Transform obstacle;
	public Transform obstacle2;

	private Vector3 gemSpawn;
	private Vector3 obstacleSpawn;

	// Use this for initialization
	void Start () {
		gemSpawn = gem.localPosition;
		obstacleSpawn = obstacle.localPosition;
	}


	public void Jump(Vector3 destination)
	{	player.touchedTarget = false;
		StartCoroutine (SimulateProjectile(destination));
	}

	IEnumerator SimulateProjectile(Vector3 destination)
	{	
		gem.gameObject.SetActive (false);
		obstacle.gameObject.SetActive (false);
		obstacle2.gameObject.SetActive (false);
		// Short delay added before Projectile is thrown
		yield return new WaitForSeconds(0f);

		// Move projectile to the position of throwing object + add some offset if needed.
		Projectile.position = transform.position + new Vector3(0.1f, 0.0f, 0);

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


		Projectile.rotation = Quaternion.identity;
		gem.gameObject.SetActive (true);
		obstacle.gameObject.SetActive (true);
		obstacle2.gameObject.SetActive (true);

		gem.position = Projectile.position;
		obstacle.position = Projectile.position;
		obstacle2.position = Projectile.position;

	}


}
