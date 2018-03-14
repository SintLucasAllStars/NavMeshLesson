using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

	public Vector3 targetPosition;
	bool open = false;
	public float timing;

	// Use this for initialization
	void Start () {
		Close();
		StartCoroutine(OpenDoor(timing));
		targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.2f);
	}

	IEnumerator OpenDoor(float sec)
	{
		while(true)
		{
			yield return new WaitForSeconds(sec);
			Open();
			yield return new WaitForSeconds(sec);
			Close();
		}
	}

	void Open()
	{
		if(!open)
		{
			targetPosition = transform.position + transform.right*5;
			open = true;
		}
	}

	void Close()
	{
		if(open)
		{
			targetPosition = transform.position - transform.right*5;
			open = false;
		}
	}
}
