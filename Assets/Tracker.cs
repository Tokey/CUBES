using UnityEngine;
using System.Collections;

public class Tracker : MonoBehaviour {


	private LineRenderer line;
	public Transform start;
	public Transform end;

	void Start()
	{
		line = GetComponent<LineRenderer> ();
	}
	void FixedUpdate () {
		line.SetPosition (0,start.position);
		line.SetPosition (1,end.position);
	}
}
