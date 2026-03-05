using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SwordVisual : MonoBehaviour
{
    
    [SerializeField] private Sword sword;
    
    private static readonly int Attack = Animator.StringToHash(IsAttack);
    private const string IsAttack = "IsAttack";
    
    private Animator  _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    private void OnDestroy()
    {
        sword.OnSwordSwing -= Sword_OnSwordSwing;
    }

    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(Attack);
    }

    public void TriggerEndAttackAnimation()
    {
        sword.AttackColliderTurnOff();
    }
}
