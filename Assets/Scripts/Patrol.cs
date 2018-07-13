using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {

	[SerializeField]
	public Transform[] patrolPoints;
	private int currPoint;
	public float moveSpeed;
	public Vector3 target;

	bool moveToNextPoint;

	void Start () {
		transform.position = patrolPoints [0].position;
		currPoint = 0;

		target = patrolPoints [currPoint + 1].position;
		moveToNextPoint = false;

	}
	
	void Update () {

		if(moveToNextPoint)
		{
			currPoint++;

				if(currPoint==patrolPoints.Length)
				{
					currPoint = 0;
				}

			target = patrolPoints [currPoint].position;
			moveToNextPoint = false;
		}

		target = patrolPoints [currPoint].position;
		transform.position = Vector3.MoveTowards(transform.position,target,moveSpeed*Time.deltaTime);
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Points")
		{
			moveToNextPoint = true;

		}
	}
}
