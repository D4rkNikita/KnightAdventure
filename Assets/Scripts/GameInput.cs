using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerDash;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Player.Dash.started += PlayerDash_performed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Combat.Attack.started -= PlayerAttack_started;
        _playerInputActions.Player.Dash.started -= PlayerDash_performed;
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

    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }

    private void PlayerDash_performed(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }
}