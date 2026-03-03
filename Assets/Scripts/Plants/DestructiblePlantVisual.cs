using System.Xml.Serialization;
using UnityEngine;

public class DestructablePlantVisual : MonoBehaviour
{
    [SerializeField] private DestructablePlant _destructablePlant;
    [SerializeField] private GameObject _bushDeathVPXPrefab;

    private void Start()
    {
        _destructablePlant.OnDestructibleTakeDamage += DestructablePlant_OnDestructibleTakeDamage;
    }

    private void OnDestroy()
    {
        _destructablePlant.OnDestructibleTakeDamage -= DestructablePlant_OnDestructibleTakeDamage;
    }

    private void DestructablePlant_OnDestructibleTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(_bushDeathVPXPrefab, _destructablePlant.transform.position, Quaternion.identity);
    }
}
