using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private bool _enableMobileInput;
    [SerializeField] private bool _hideCursor;

    [Header("Input")]
    [SerializeField] private DecktopInput _decktopInput;
    [SerializeField] private MobileInput _mobileInput;

    [Header("CharacterOptions")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    [Header("CameraOptions")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensetivity;
    [SerializeField] private float _yRotationLimit;

    [Header("CheckGround")]
    [SerializeField] private float _sphereRadius;
    [SerializeField] private Transform _castPoint;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _platformMask;

    private bool _isGrounded;
    private IGameInput _input;
    private Vector2 _rotation;
    private Vector3 _playerVelosity;
    private Vector3 _platformVelosity;

    private void Awake()
    {
        Init();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_castPoint.position, _sphereRadius, _groundMask);
        Move();
        PlatformDetection();
        _playerRb.velocity = _playerVelosity + new Vector3(_platformVelosity.x, 0f, _platformVelosity.z);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Init()
    {
        _input = _enableMobileInput ? _mobileInput : _decktopInput;
        _mobileInput.gameObject.SetActive(_enableMobileInput);
        _input.OnJumpKeyPressed += Jump;
        _rotation.x = transform.localRotation.eulerAngles.y;
        HideCursorAtSturtup();
    }

    private void CameraRotation()
    {
        _rotation.x += _input.CameraInput.x * _sensetivity;
        _rotation.y += _input.CameraInput.y * _sensetivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);
        _playerRb.MoveRotation(Quaternion.AngleAxis(_rotation.x, Vector3.up));
        _camera.transform.localRotation = Quaternion.AngleAxis(_rotation.y, Vector3.left);
    }

    private void Move()
    {
        var velosity = (_input.MoveDirection.x * transform.right + _input.MoveDirection.z * transform.forward) * _moveSpeed;
        _playerVelosity = new Vector3(velosity.x, _playerRb.velocity.y, velosity.z);
    }

    private void Jump()
    {
        if (!_isGrounded)
        {
            return;
        }

        var force = Mathf.Clamp(_playerRb.velocity.y + _platformVelosity.y + _jumpForce, 0f, _jumpForce + _platformVelosity.y);
        _playerRb.velocity = new Vector3(_playerRb.velocity.x, force, _playerRb.velocity.z);
    }

    private void PlatformDetection()
    {
        if (!_isGrounded)
        {
            return;
        }

        var isHit = Physics.SphereCast(transform.position, _sphereRadius, transform.TransformDirection(Vector3.down), out var hit, 1f, _platformMask);
        if (!isHit)
        {
            _platformVelosity = Vector3.zero;
            return;
        }

        if (hit.collider.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            _platformVelosity = rigidbody.velocity;
            return;
        }

        _platformVelosity = Vector3.zero;
    }

    private void HideCursorAtSturtup()
    {
        if (!_hideCursor)
        {
            return;
        }

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_castPoint.position, _sphereRadius);
    }
#endif
}
