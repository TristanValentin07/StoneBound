using System;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Transform cameraTransform;
    public bool showRaycast = false;
    public float raycastHeight = 0.1f;
    public float sprintMultiplier = 2f;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;
    private bool _isGrounded;
    private Animator _animator;
    private bool isSprinting = false;

    private Stamina_Manager staminaManager;
    private Player_Data playerData;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody != null)
        {
            _rigidbody.useGravity = true;
        }

        playerData = new Player_Data();

        staminaManager = FindAnyObjectByType<Stamina_Manager>();
        if (staminaManager != null)
        {
            staminaManager.SetStaminaToPlayer(playerData);
        }
        else
        {
            Debug.LogError(" Stamina_Manager non trouvé dans la scène !");
        }
    }

    void Update()
    {
        HandleSprint();
        HandleInput();
        
        // Mise à jour de l'UI de la stamina
        if (staminaManager != null)
        {
            staminaManager.UpdateStaminaBar();
        }
    }

    void FixedUpdate()
    {
        Move();
        CheckGroundStatus();
        
        // anti blocage du joueur dans le sol
        if (!_isGrounded && _rigidbody.linearVelocity.magnitude < 0.1f)
        {
            Vector3 pushForward = transform.forward * 0.5f;
            _rigidbody.AddForce(pushForward, ForceMode.VelocityChange);
        }

    }

    private void HandleInput()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W) && _isGrounded) vertical = 1f;
        if (Input.GetKey(KeyCode.S) && _isGrounded) vertical = -1f;
        if (Input.GetKey(KeyCode.A) && _isGrounded) horizontal = -1f;
        if (Input.GetKey(KeyCode.D) && _isGrounded) horizontal = 1f;
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) Jump();

        _animator.SetFloat("horizontal", horizontal);
        _animator.SetFloat("vertical", vertical);
        _animator.SetBool("isJumping", !_isGrounded);

        _moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void Move()
    {
        if (_moveDirection != Vector3.zero)
        {
            float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * _moveDirection.z + cameraRight * _moveDirection.x;
            Vector3 velocity = moveDirection * speed;
            velocity.y = _rigidbody.linearVelocity.y;
            _rigidbody.linearVelocity = velocity;
        }
        else
        {
            if (_isGrounded)
            {
                _rigidbody.linearVelocity = new Vector3(0f, _rigidbody.linearVelocity.y, 0f);
            }
        }
    }

    private void HandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && playerData.currentStamina > 0)
        {
            isSprinting = true;
            playerData.currentStamina -= playerData.staminaDrainRate * Time.deltaTime;
        }
        else
        {
            isSprinting = false;
            if (_isGrounded)
            {
                playerData.currentStamina += playerData.staminaRecoveryRate * Time.deltaTime;
            }
        }

        playerData.currentStamina = Mathf.Clamp(playerData.currentStamina, 0, playerData.maxStamina);
        
        //mise à jour de l'UI
        if (staminaManager != null)
        {
            staminaManager.UpdateStaminaBar();
        }
    }

    private void Jump()
    {
        if (playerData.currentStamina > 5)
        {
            playerData.currentStamina -= 5f;
            _isGrounded = false;
            float jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGroundStatus()
    {
        Collider col = GetComponent<Collider>();

        if (col == null)
        {
            _isGrounded = false;
            return;
        }
        float raycastDistance = col.bounds.extents.y + 0.1f * raycastHeight;
        Vector3 raycastOrigin = transform.position + Vector3.up;
        if (showRaycast)
        {
            Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, Color.red);
        }
        _isGrounded = Physics.Raycast(raycastOrigin, Vector3.down, raycastDistance);
    }
}
