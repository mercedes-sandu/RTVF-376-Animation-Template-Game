using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform feetTransform;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private KeyCode actionKey = KeyCode.E;

    [SerializeField] private float turnTime;

    [SerializeField] private Vector3 groundCheckSize;

    private Rigidbody _rb;

    private float _horizontal;
    private const float SpeedThreshold = 0.1f;
    private bool _isGrounded = true;
    private bool _facingRight = true;
    private float _startRotationY;

    private int _groundMask;

    private Animator _anim;
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int ActionTrigger = Animator.StringToHash("actionTrigger");

    private bool _turnCoroutineRunning = false;
    private Coroutine _turnCoroutine;

    /// <summary>
    /// Gets Rigidbody and Animator components. Assigns the ground layer mask.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _groundMask = LayerMask.GetMask("Ground");

        _startRotationY = transform.rotation.eulerAngles.y;
    }

    /// <summary>
    /// Checks for input, jumping, action, and flipping.
    /// </summary>
    private void Update()
    {
        GetInput();
        JumpCheck();
        ActionCheck();
        CheckFlip();
    }

    /// <summary>
    /// Moves the player.
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Assigns the horizontal input value to _horizontal and sets the moving bool in the animator.
    /// </summary>
    private void GetInput()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _horizontal = Mathf.Abs(_horizontal) < SpeedThreshold ? 0 : _horizontal;
        _anim.SetBool(Moving, _horizontal != 0);
        if (_horizontal != 0) _anim.ResetTrigger(ActionTrigger);
    }

    /// <summary>
    /// Checks if the player can jump and applies a force if they can.
    /// </summary>
    private void JumpCheck()
    {
        _isGrounded = Physics.CheckBox(feetTransform.position, groundCheckSize, Quaternion.identity,
            _groundMask);
        _anim.SetBool(IsGrounded, _isGrounded);

        if (!_isGrounded) return;
        _anim.ResetTrigger(ActionTrigger);
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Checks if the player has pressed the action key and triggers the action animation if they have.
    /// </summary>
    private void ActionCheck()
    {
        if (!Input.GetKeyDown(actionKey)) return;
        _anim.SetTrigger(ActionTrigger);
    }

    /// <summary>
    /// Sets the player's velocity to move them horizontally.
    /// </summary>
    private void Move()
    {
        _rb.velocity = new Vector3(_horizontal * speed, _rb.velocity.y, _rb.velocity.z);
    }

    /// <summary>
    /// Checks if the player needs to flip directions, and flips them if necessary.
    /// </summary>
    private void CheckFlip()
    {
        if ((!_facingRight || !(_horizontal < 0)) && (_facingRight || !(_horizontal > 0))) return;
        _facingRight = !_facingRight;
        Turn();
    }

    /// <summary>
    /// Stops any existing coroutines and starts a new one to turn the player.
    /// </summary>
    private void Turn()
    {
        if (_turnCoroutineRunning) StopCoroutine(_turnCoroutine);
        _turnCoroutine = StartCoroutine(TurnCoroutine());
    }

    /// <summary>
    /// Turns the player to face the opposite direction.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TurnCoroutine()
    {
        _turnCoroutineRunning = true;
        var startRotation = transform.rotation;
        var endRotation = Quaternion.Euler(0, _facingRight ? _startRotationY : _startRotationY + 180, 0);
        var time = 0f;
        while (time < turnTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / turnTime);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        _turnCoroutineRunning = false;
    }
}