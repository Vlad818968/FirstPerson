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

    void Update()
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

        var isHit = Physics.SphereCast(transform.position, _character.Controller.radius, transform.TransformDirection(Vector3.down), out var hit, _rayDistance, _platformMask);
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_character.Controller.radius * 1.5f, _character.Controller.height, _character.Controller.radius * 1.5f));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - _rayDistance, transform.position.z), _character.Controller.radius);
    }
#endif
}
