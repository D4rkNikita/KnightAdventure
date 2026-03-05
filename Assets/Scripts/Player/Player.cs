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
    
    [Header("Base Settings")]
    [SerializeField] private float movingSpeed = 3f;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    
    [Header("Dash settings")]
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashCooldownTime = 0.25f;

    private Vector2 _inputVector;
    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private readonly float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;
    private bool _isDashing;
    private float _initialMovingSpeed;

    private Camera _mainCamera;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _isAlive = true;
        _canTakeDamage = true;
        _currentHealth = maxHealth;
        _initialMovingSpeed = movingSpeed;
        
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
        
    }

    private void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (!_knockBack.IsGettingKnockedBack)
            HadleMovement();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash -= GameInput_OnPlayerDash;
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        return _mainCamera.WorldToScreenPoint(transform.position);
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
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
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();

            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void GameInput_OnPlayerDash(object sender, EventArgs e)
    {
        Dash();
    }

    private void Dash()
    {
        if (!_isDashing)
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        movingSpeed *= dashSpeed;
        trailRenderer.emitting = true;
        
        yield return new WaitForSeconds(dashTime);
        
        trailRenderer.emitting = false;
        movingSpeed = _initialMovingSpeed;
        
        yield return new WaitForSeconds(dashCooldownTime);
        
        _isDashing = false;
    }

    private void HadleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * (movingSpeed * Time.fixedDeltaTime));

        if ((Math.Abs(_inputVector.x) > _minMovingSpeed) || (Math.Abs(_inputVector.y) > _minMovingSpeed))
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }
}