using System.Xml.Serialization;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    public void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

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
            Destroy(gameObject);
        }

    }

}
