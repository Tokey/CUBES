using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	private Vector3 offSet;
	public Transform player;
	private Vector3 velocity;
	void Start () {
		offSet = transform.position - player.position;
		velocity = new Vector3 (0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.SmoothDamp (transform.position,player.position + offSet,ref velocity,1f); 
		//player.transform.position + offSet;
	}
}
