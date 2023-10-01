using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }

    [Space]
    [SerializeField] private bool _hideCursor;
    [SerializeField] private bool _useMobileInput;

    [Header("Inputs")]
    [SerializeField] private DecktopInput _decktopInput;
    [SerializeField] private MobileInput _mobileInput;

    [Header("Camera")]
    [SerializeField] private Camera _camera;
    [SerializeField, Range(0.1f, 9f)] private float _sensitivity = 2f;
    [SerializeField, Range(0f, 90f)] private float _yRotationLimit = 88f;

    [Header("CharacterOptions")]
    [SerializeField, Range(0.3f, 50f)] private float _speed = 17f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 _velosity;
    private float _groundDistance = 0.4f;

    private Vector2 _rotation;

    private IGameInput _gameInput;

    private void Awake()
    {
        Init();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(_groundTransform.position, _groundDistance, _groundMask);
        Move();
        Gravity();
    }

    private void Init()
    {
        _gameInput = _useMobileInput ? _mobileInput : _decktopInput;
        _mobileInput.gameObject.SetActive(_useMobileInput);
        _rotation.x = transform.localRotation.eulerAngles.y;
        _gameInput.OnJumpKeyPressed += Jump;
        HideCursorAtSturtup();
    }

    public void AddForce(Vector3 force)
    {
        _velosity = force;
    }

    private void Move()
    {
        var moveDirection = _gameInput.MoveDirection.x * transform.right + _gameInput.MoveDirection.z * transform.forward;
        Controller.Move(moveDirection * _speed * Time.deltaTime);
    }

    private void Gravity()
    {
        if (IsGrounded && _velosity.y <= 0f)
        {
            _velosity = Vector3.zero;
            _velosity.y = -2f;
            return;
        }

        _velosity.y += _gravity * Time.deltaTime;
        Controller.Move(_velosity * Time.deltaTime);
    }

    private void Jump()
    {
        if (!IsGrounded)
        {
            return;
        }

        _velosity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    private void CameraRotation()
    {
        _rotation.x += _gameInput.CameraInput.x * _sensitivity;
        _rotation.y += _gameInput.CameraInput.y * _sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);
        transform.rotation = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        _camera.transform.localRotation = Quaternion.AngleAxis(_rotation.y, Vector3.left);
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
}

