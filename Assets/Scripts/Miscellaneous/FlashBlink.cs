using Unity.VisualScripting;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.2f;

    private float _blinkTimer;
    private Material _deafaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _deafaultMaterial = _spriteRenderer.material;

        _isBlinking = true;
       
    }

    private void Start()
    {
         if (_damagableObject is Player)
            (_damagableObject as Player).OnFlashBlink += DamagableObject_OnFlashBlink;
    }

    private void Update()
    {
        if (_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;

            if (_blinkTimer < 0)
                SetDefaultMaterial();
        }
    }

    private void OnDestroy()
    {
        if (_damagableObject is Player)
            (_damagableObject as Player).OnFlashBlink -= DamagableObject_OnFlashBlink;
    }

    public void StopBlinking()
    {
        SetDefaultMaterial();
        _isBlinking = false;
    }

    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _deafaultMaterial;
    }

    private void SetBlinkingMaterial()
    {
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }

    private void DamagableObject_OnFlashBlink(object sender, System.EventArgs e)
    {
        SetBlinkingMaterial();
    }
}
