using System;
using UnityEngine;

public interface IGameInput
{
    public event Action OnJumpKeyPressed;
    public event Action OnLeftHandButtonPressed;
    public event Action OnRightHandButtonPressed;

    public Vector2 CameraInput { get;}
    public Vector3 MoveDirection { get; }
}