using UnityEngine;

public class Move_Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Transform cameraTransform;
    public bool showRaycast = false;
    public float raycastHeight = 0.1f;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;
    private bool _isGrounded;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody != null)
        {
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void Update()
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

        RotatePlayerToCamera();
    }

    void FixedUpdate()
    {
        if (_moveDirection != Vector3.zero)
        {
            Move();
        }
        CheckGroundStatus();
    }

    private void RotatePlayerToCamera()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }

    private void Move()
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * _moveDirection.z + cameraRight * _moveDirection.x;

        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }

    private void Jump()
    {
        _isGrounded = false;
        float jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravity);
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
