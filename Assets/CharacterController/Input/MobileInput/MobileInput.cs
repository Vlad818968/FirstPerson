using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour, IGameInput, IPointerClickHandler
{
    public event Action OnJumpKeyPressed;
    public event Action OnLeftHandButtonPressed;
    public event Action OnRightHandButtonPressed;

    public Vector2 CameraInput => _mobileCameraInput.TouchDist;
    public Vector3 MoveDirection => new Vector3(_joystick.Direction.x, 0f, _joystick.Direction.y);

    [SerializeField] private MobileCameraInput _mobileCameraInput;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Button _jumpButton;

    private void Awake()
    {
        _jumpButton.onClick.AddListener(() => OnJumpKeyPressed?.Invoke());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.IsDoubleClick())
        {
            OnRightHandButtonPressed?.Invoke();
            return;
        }

        OnLeftHandButtonPressed?.Invoke();
    }
}
