using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
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
	private int _currentTargetIndex = 0;
	[SerializeField] Transform playerTransform;
	float _speed = 20f;

	[SerializeField] private bool _aggroToCharacter;
	private const float KILL_DISTANCE = 5f;

	//public bool AggroToCharacter
	//{
	//	get { return _aggroToCharacter; }
	//	set
	//	{
	//		_aggroToCharacter = value;
	//		Debug.Log($"we want to change AggroToCharacter {_aggroToCharacter}");
	//		//if (_aggroToCharacter == true)
	//		//	_patrolling = false;
	//		//else
	//		//	_patrolling = true;
	//	}
	//}

	private bool _patrolling = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

	

    public void SetAggroToCharacter(bool isAggro)
	{
		_aggroToCharacter = isAggro;
	}

	public void GetAggroToCharacter(bool isTrue)
	{
		_aggroToCharacter=isTrue;
	}
    private void OnEnable()
    {
		FindObjectOfType<PlayerStats>().PlayerBelowThresholdEvent += GetAggroToCharacter;
    }

    private void OnDisable()
    {
		FindObjectOfType<PlayerStats>().PlayerBelowThresholdEvent -= GetAggroToCharacter;
	}
	IEnumerator AttackCoroutine(Vector3 target)
	{
		Debug.Log(distanceToTarget + "1");

		_patrolling = false;
		distanceToTarget = (playerTransform.position - transform.position).magnitude;

		while (playerTransform != null)
		{
			distanceToTarget = (playerTransform.position - transform.position).magnitude;
			transform.forward = (playerTransform.position - transform.position).normalized;
			yield return null;

			if (distanceToTarget >= 10)
			{
				transform.forward = (playerTransform.position - transform.position).normalized;
				transform.position += transform.forward * _speed * Time.deltaTime;
			}

			if (distanceToTarget < KILL_DISTANCE && _aggroToCharacter)
			{
				playerTransform.gameObject.GetComponent<PlayerStats>().SendMessage("Die");
				_aggroToCharacter = false;
				yield break;
			}
			else
			{
				_patrolling = true;
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
		Vector3 direction = (playerTransform.position - transform.position).normalized;
		transform.forward = Vector3.Lerp(transform.forward, direction, .05f);

		Move();
		if(!_aggroToCharacter)
        {
			_patrolling = true;
			return;
        }

        if (Vector3.Distance(playerTransform.position,transform.position)< KILL_DISTANCE && _aggroToCharacter)
        {
			FindObjectOfType<PlayerStats>().PlayerBelowThresholdEvent -= GetAggroToCharacter;
			playerTransform.gameObject.GetComponent<PlayerStats>().SendMessage("Die");
			_aggroToCharacter = false;
		}
		
	}

    private void Update()
	{
		if (!_aggroToCharacter && _patrolling)
		{
			Patrol();
		}
		else
		{
			Attack();
		}
	}


	void Move()
    {
		transform.position += transform.forward * _speed * Time.deltaTime;
	}

	void Patrol()
	{
		_patrolling = true;

		if (_aggroToCharacter)
		{
			_patrolling = false;
			return;
		}

		if (Vector3.Distance(transform.position, patrolTargets[_currentTargetIndex]) < 5f)
		{
			_currentTargetIndex = (_currentTargetIndex + 1) % patrolTargets.Length;
		}
		else
		{
			Vector3 direction = (patrolTargets[_currentTargetIndex] - transform.position).normalized;
			transform.forward = Vector3.Lerp(transform.forward, direction, .05f);
		}
		Move();
	}
}
