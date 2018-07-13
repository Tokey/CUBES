using UnityEngine;
using System.Collections;

public class OrbitObject2 : MonoBehaviour {

	public Transform target;
	public float orbitDistance = 10.0f;
	public float orbitDegreesPerSec = 6.0f;


	private float angle  = 0f;
	public bool XYPlane;

	void Orbit()
	{
		angle += orbitDegreesPerSec * Time.deltaTime;

		if(target != null)
		{
			
			
			transform.RotateAround(target.position, Vector3.up, orbitDegreesPerSec * Time.deltaTime);

			float x = target.position.x + Mathf.Cos (angle)*orbitDistance;
			float y = target.position.y + Mathf.Sin (angle)*orbitDistance;

			float z = target.position.z + Mathf.Sin (angle) * orbitDistance;

			if(XYPlane)	
				transform.position = new Vector3 (x, y, transform.position.z);
			else
				transform.position = new Vector3 (x,target.position.y,z);
		}
	}


	void LateUpdate () {

		Orbit();

	}
}