using UnityEngine;
using System.Collections;

public class OrbitObject : MonoBehaviour {

	public Transform target;
	public float orbitDistance = 10.0f;
	public float orbitDegreesPerSec = 180.0f;

	void Orbit()
	{
		if(target != null)
		{
			// Keep us at orbitDistance from target
			transform.position = target.position + (transform.position - target.position).normalized * orbitDistance;
			transform.RotateAround(target.position, Vector3.up, orbitDegreesPerSec * Time.deltaTime);
		}
	}


	void LateUpdate () {

		Orbit();

	}
}