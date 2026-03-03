using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set;}

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

    }

    private void Start()
    {
        _playerInputActions.Combat.Attack.started +=  PlayerAttack_started;
    }

    private void OnDestroy()
    {
        _playerInputActions.Combat.Attack.started -=  PlayerAttack_started;
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }
}
