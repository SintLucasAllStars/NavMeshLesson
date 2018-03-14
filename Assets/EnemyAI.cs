using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
	public enum Mode {Patrol, Search, Chase};

	NavMeshAgent nav;
	float fov = 120f;
	Vector3 lastSeen;

	public Transform target;
	public Light light;

	public Mode mode;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
		nav = GetComponent<NavMeshAgent>();
		SetMode(Mode.Patrol);
		StartCoroutine(PatrolBehaviour());
	}
	
	// Update is called once per frame
	void Update () {
		bool canSee = canSeeTarget();

		if(canSee)
		{
			SetMode(Mode.Chase);
			lastSeen = target.position;
		}

		switch (mode)
		{
		case Mode.Patrol:
			break;
		case Mode.Search:
			if(nav.remainingDistance < .5f)
			{
				SetMode(Mode.Patrol);
			}
			break;
		case Mode.Chase:
			if(canSee)
			{
				nav.SetDestination(target.position);
			} else {
				SetMode(Mode.Search);
			}
			break;
		}

	}

	IEnumerator PatrolBehaviour()
	{
		while(true)
		{
			if(mode == Mode.Patrol)
			{
				Vector3 destination = transform.position + new Vector3(Random.Range(-10, 10), 0f, Random.Range(-10, 10));
				nav.SetDestination(destination);
			}
			yield return new WaitForSeconds(Random.Range(3,7));
		}
	}

	void SetMode(Mode m)
	{
		mode = m;
		switch (mode)
		{
		case Mode.Patrol:
			light.color = Color.blue;
			break;
		case Mode.Search:
			nav.SetDestination(lastSeen);
			light.color = Color.yellow;
			break;
		case Mode.Chase:
			light.color = Color.red;
			break;
		}
	}

	bool canSeeTarget()
	{
		RaycastHit hit;

		Vector3 direction = target.position - transform.position;
		if(Physics.Raycast(transform.position, direction, out hit))
		{
			if(hit.collider.gameObject.CompareTag("Player"))
			{
				float angle = Vector3.Angle(transform.forward, direction);
				if(angle < fov/2)
				{
					return true;
				}
				else
				{
					return false;
				}
			} else 
			{
				return false;
			}
		}
		else
		{
			Debug.Log("I See nothing");
			return false;
		}
	}
}
