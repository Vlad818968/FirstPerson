using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private LayerMask _platformMask;
    [SerializeField] private float _rayDistance;

    private float _maxPlatformNormalAngle;
    private Transform _parent;

    private void Awake()
    {
        _maxPlatformNormalAngle = _character.Controller.slopeLimit;
    }

    void FixedUpdate()
    {
        CheckPlatform();
    }

    private void CheckPlatform()
    {
        if (!_character.IsGrounded)
        {
            SetNullParrent();
            return;
        }

        var isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out var hit, _rayDistance, _platformMask);
        if (!isHit)
        {
            SetNullParrent();
            return;
        }

        if (hit.collider.gameObject == _parent)
        {
            return;
        }

        if (Vector3.Angle(transform.up, hit.normal) > _maxPlatformNormalAngle)
        {
            return;
        }

        _parent = hit.collider.transform;
        transform.parent = _parent;
    }

    private void SetNullParrent()
    {
        _parent = null;
        transform.parent = _parent;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var downVector = new Vector3(transform.position.x, transform.position.y - _rayDistance, transform.position.z);
        Gizmos.DrawLine(transform.position, downVector);
    }
#endif
}
