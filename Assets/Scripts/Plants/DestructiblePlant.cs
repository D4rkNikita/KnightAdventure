using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestructablePlant : MonoBehaviour
{
    public event EventHandler OnDestructibleTakeDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Sword>(out _))
        {
            OnDestructibleTakeDamage?.Invoke(this, EventArgs.Empty);

            Destroy(gameObject);
            
            NavMeshSurfaceManagement.Instance.RebakeNavmeshSurface();
        }
    }
}
