using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class SkeletonVisual : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash(IsRunning);
    private static readonly int SpeedMultiplier = Animator.StringToHash(ChasingSpeedMultiplier);
    private static readonly int Hit = Animator.StringToHash(TakeHit);
    private static readonly int Attack1 = Animator.StringToHash(Attack);
    private static readonly int IsDie = Animator.StringToHash(IsDei);
    
    private const string IsRunning = "IsRunning";
    private const string TakeHit = "TakeHit";
    private const string ChasingSpeedMultiplier = "ChasingSpeedMultiplier";
    private const string Attack = "Attack";
    private const string IsDei = "IsDie";

    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }
    
    private void OnDestroy()
    {
       enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
       enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
       enemyEntity.OnDeath -= _enemyEntity_OnDeath;
    }

    private void Update()
    {
        _animator.SetBool(Running, enemyAI.IsRunning);
        _animator.SetFloat(SpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }

    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }

    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(Hit);
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(Attack1);
    }

    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IsDie, true);
        _spriteRenderer.sortingOrder -= 1;
        enemyShadow.SetActive(false);
    }


}
