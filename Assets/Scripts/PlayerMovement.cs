using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5f;
    [Header("Input Settings")]
    [SerializeField] private float inputSensitivity = 1f;
    [SerializeField] private float inputGravity = 1f;
    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] Enemy enemy;
    
    private Vector3 moveDirection;
    Vector3 velocity;
    float currentSpeed;
    float horizontal;
    float vertical;
    bool _canMove = true;
    void Update()
    { 
       Movement();
       Run();
       MoveAnimations();
    }

    void Movement()
    {
        if (GetCanMove())
        {
            horizontal = GetSmoothRawAxis("Horizontal", ref horizontal, inputSensitivity, inputGravity);
            vertical = GetSmoothRawAxis("Vertical", ref vertical, inputSensitivity, inputGravity);
        }else
        {
            horizontal = 0;
            vertical = 0;
        }
        
        
        moveDirection = new Vector3(horizontal, 0, vertical);
        // change move direction to player forward direction
       // moveDirection = transform.TransformDirection(new Vector3(horizontal, 0, vertical));
        
        
        
        velocity = moveDirection * currentSpeed;
        controller.Move(velocity * Time.deltaTime);
        
        // if (enemy)
        // {
        //     Vector3 direction = enemy.transform.position - transform.position;
        //     direction.y = 0;
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        // }
    }

    void Run()
    {
        if (!GetCanMove()) return;
        if (Input.GetKey(KeyCode.LeftShift) && moveDirection.magnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime);
        }
        else if (moveDirection.magnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime);
        }else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime);
        }
    }

    void MoveAnimations()
    {
        animator.SetFloat("X", -velocity.x);
        animator.SetFloat("Y", velocity.z);
    }

    public void SetCanMove(bool status)
    {
        _canMove = status;
    }
    
    public bool GetCanMove()
    {
        return _canMove;
    }
    
    
    private float GetSmoothRawAxis(string name, ref float axis, float sensitivity, float gravity)
    {
        var r = Input.GetAxisRaw(name);
        var s = sensitivity;
        var g = gravity;
        var t = Time.unscaledDeltaTime;

        if (r != 0)
        {
            return axis = Mathf.Clamp(axis + r * s * t, -1f, 1f);
        }
        else
        {
            return axis = Mathf.Clamp01(Mathf.Abs(axis) - g * t) * Mathf.Sign(axis);
        }
    }
}
