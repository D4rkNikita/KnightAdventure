using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private float _movingSpeed = 3f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageReacoveryTime = 0.5f;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        _isAlive = true;
        _canTakeDamage = true;
        _currentHealth = _maxHealth;
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
    }

     private void FixedUpdate()
    {
        if(!_knockBack.IsGettingKnockedBack)
            HadleMovement();   
    }

    private void OnDestroy()
    {
         GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

     public Vector3 GetPlayerScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if(_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Math.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockBack(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);
            
            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    public bool IsAlive() => _isAlive;

    private void DetectDeath()
    {
        if(_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();

            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageReacoveryTime);
        _canTakeDamage = true;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
       ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void HadleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        
        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if ((Math.Abs(inputVector.x) > _minMovingSpeed) || (Math.Abs(inputVector.y) > _minMovingSpeed))
        {
            _isRunning = true;
        } else
        {
            _isRunning = false;
        }
    }
}
