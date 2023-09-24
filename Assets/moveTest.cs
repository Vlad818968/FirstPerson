using UnityEngine;

public class moveTest : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private Transform moveTransform;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, moveTransform.position, Speed * Time.fixedDeltaTime);
    }
}
