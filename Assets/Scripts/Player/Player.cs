using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float  movingSpeed = 3f;

    private Rigidbody2D rb;

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void HadleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if ((Math.Abs(inputVector.x) > minMovingSpeed) || (Math.Abs(inputVector.y) > minMovingSpeed))
        {
            isRunning = true;
        } else
        {
            isRunning = false;
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void FixedUpdate()
    {
        HadleMovement();   
    }

}
