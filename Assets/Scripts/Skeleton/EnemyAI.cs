using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    [SerializeField] private bool isChasingEnemy = false;
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeedMultiplier = 2f;

    [SerializeField] private bool isAttackingEnemy = false;
    [SerializeField] private float attackingDistance = 2f;
    [SerializeField] private float attackRate = 2f;
    
    public event EventHandler OnEnemyAttack;
    
    public State currentState;
    
    private float _nextAttackTime = 0f;
    private float _roamingTimer;
    private float _roamingSpeed;
    private float _chasingSpeed;
    
    private NavMeshAgent _navMeshAgent;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;
    
    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;
    
    public bool IsRunning => _navMeshAgent.velocity != Vector3.zero;

    public enum State
    {
        Roaming,
        Idle,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        currentState = startingState;

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _roamingSpeed * chasingSpeedMultiplier;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    private void StateHandler()
    {
        switch (currentState)
        {

            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = roamingTimerMax;
                }
                CheckCurrentState();
                break;

            case State.Chasing:
                ChassinTarget();
                CheckCurrentState();
                break;

            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;

            case State.Death:
                break;

            default:
            case State.Idle:
                break;

        }
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        currentState = State.Death;
    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (isChasingEnemy && Player.Instance.IsAlive())
        {
            if (distanceToPlayer <= chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (isAttackingEnemy)
        {
            if (distanceToPlayer <= attackingDistance)
            {
                newState = Player.Instance.IsAlive() ? State.Attacking : State.Roaming;
            }
        }

        if (newState != currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _roamingTimer = 0;
                _navMeshAgent.speed = _roamingSpeed;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }

            currentState = newState;
        }

    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + attackRate;
        }

    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }

    private void ChassinTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        transform.rotation = sourcePosition.x > targetPosition.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }

}
