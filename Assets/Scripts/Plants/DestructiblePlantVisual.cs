using System.Xml.Serialization;
using UnityEngine;

public class DestructablePlantVisual : MonoBehaviour
{
    [SerializeField] private DestructablePlant destructiblePlant;
    [SerializeField] private GameObject bushDeathVFXPrefab;

    private void Start()
    {
        destructiblePlant.OnDestructibleTakeDamage += DestructiblePlantOnDestructibleTakeDamage;
    }

    private void OnDestroy()
    {
        destructiblePlant.OnDestructibleTakeDamage -= DestructiblePlantOnDestructibleTakeDamage;
    }

    private void DestructiblePlantOnDestructibleTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(bushDeathVFXPrefab, destructiblePlant.transform.position, Quaternion.identity);
    }
}
