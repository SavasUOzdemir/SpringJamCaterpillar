using UnityEngine;
using System.Collections;

public class lb_Bird_Game : MonoBehaviour
{
	enum birdBehaviors
	{
		sing,
		preen,
		ruffle,
		peck,
		hopForward,
		hopBackward,
		hopLeft,
		hopRight,
	}

	public AudioClip song1;
	public AudioClip song2;
	public AudioClip flyAway1;
	public AudioClip flyAway2;
	[HideInInspector] public bool fleeCrows = true;

	Animator anim;

	float distanceToTarget = 0.0f;
	
	public float AggroDistance;

	[SerializeField] Vector3[] patrolTargets = new Vector3[10];
	private int currentTargetIndex = 0;
	[SerializeField] Transform playerTransform;
	float _speed = 20f;

	private bool _aggroToCharacter;

	public bool AggroToCharacter
	{
		get { return _aggroToCharacter; }
		set
		{
			_aggroToCharacter = value;
			if (_aggroToCharacter == true)
				_patrolling = false;
			else
				_patrolling = true;
		}
	}

	private bool _patrolling = true;

	IEnumerator AttackCoroutine(Vector3 target)
	{
		Debug.Log(distanceToTarget+ "1");

		_patrolling = false;
		distanceToTarget = (playerTransform.position - transform.position).magnitude;
		while (true)
		{
			distanceToTarget = (playerTransform.position - transform.position).magnitude;
			transform.forward = (playerTransform.position - transform.position).normalized;
			Debug.Log(distanceToTarget);

			while (distanceToTarget>=10)
            {
				Debug.Log("buraya girdi");
				transform.forward = (playerTransform.position - transform.position).normalized;
				transform.position += transform.forward * _speed * Time.deltaTime;
			}
			if (distanceToTarget < 10f && AggroToCharacter)
				playerTransform.gameObject.GetComponent<PlayerStats>().SendMessage("Die");
			else
			{
				Patrol();
				yield break;
			}
		}

	}

	private void Start()
	{
		Patrol();
	}
	void Attack()
	{
		Debug.Log("buraya girdi: attack");

		Vector3 target = playerTransform.position;
		StartCoroutine(AttackCoroutine(target));
	}

	private void Update()
	{
		if (!AggroToCharacter && _patrolling)
		{
			Patrol();
		}
		else
			Attack();
	}
	void Patrol()
	{
		Debug.Log("Patrolling");
		_patrolling = true;

		if (AggroToCharacter)
		{
			_patrolling = false;
			return; 
		}

		if (Vector3.Distance(transform.position, patrolTargets[currentTargetIndex]) < 0.1f)
		{
			currentTargetIndex = (currentTargetIndex + 1) % patrolTargets.Length;
		}
		else
		{
			Vector3 direction = (patrolTargets[currentTargetIndex] - transform.position).normalized;
			transform.position += direction * _speed * Time.deltaTime;
			transform.forward = direction.normalized;
		}
	}
}
