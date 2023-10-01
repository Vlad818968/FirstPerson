using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecktopInput : MonoBehaviour, IGameInput
{
    public event Action OnJumpKeyPressed;
    public event Action OnLeftHandButtonPressed;
    public event Action OnRightHandButtonPressed;

    public Vector3 MoveDirection => _moveDirection;
    public Vector2 CameraInput => GetCameraInput();

    [Space]
    [SerializeField] private List<KeyCode> _forwardButtons;
    [SerializeField] private List<KeyCode> _backwardButtons;
    [SerializeField] private List<KeyCode> _leftButtons;
    [SerializeField] private List<KeyCode> _rightButtons;

    [Space]
    [SerializeField] private List<KeyCode> _jumpButtons;

    [Space]
    [SerializeField] private List<KeyCode> _leftHandAction;
    [SerializeField] private List<KeyCode> _rightHandAction;

    private Vector2 _cameraRotation;
    private Vector3 _moveDirection;

    private const float ChangeMoveDirectionSpeed = 6f;

    private void Update()
    {
        if (IsAnyButtonPressed(_jumpButtons))
        {
            OnJumpKeyPressed?.Invoke();
        }

        if (IsAnyButtonPressed(_leftHandAction))
        {
            OnLeftHandButtonPressed?.Invoke();
        }

        if (IsAnyButtonPressed(_rightHandAction))
        {
            OnRightHandButtonPressed?.Invoke();
        }

        _moveDirection = Vector3.Lerp(_moveDirection, GetMoveInput(), Time.deltaTime * ChangeMoveDirectionSpeed);
    }

    private Vector3 GetMoveInput()
    {
        var z = ReadButtonPress(_forwardButtons) - ReadButtonPress(_backwardButtons);
        var x = ReadButtonPress(_rightButtons) - ReadButtonPress(_leftButtons);
        return Vector3.ClampMagnitude(new Vector3(x, 0, z), 1f);
    }

    private Vector2 GetCameraInput()
    {
        _cameraRotation.x = Input.GetAxis("Mouse X");
        _cameraRotation.y = Input.GetAxis("Mouse Y");
        return _cameraRotation;
    }

    private int ReadButtonPress(List<KeyCode> keys)
    {
        return IsAnyButtonPressed(keys) ? 1 : 0;
    }

    private bool IsAnyButtonPressed(List<KeyCode> keys)
    {
        return keys.Any(k => Input.GetKey(k));
    }
}
