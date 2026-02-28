using System;
using System.Xml.Serialization;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]


public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    public void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTrunOn()
    {
        _polygonCollider2D.enabled = true;
    }

    public void PolygonColliderTrunOff()
    {
        _polygonCollider2D.enabled = false;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;

            _enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
        }

    }

}
