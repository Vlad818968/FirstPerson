#if UNITY_EDITOR
using UnityEngine;

public class BalisticDraw : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Vector3 _gravity;
    [SerializeField] private Vector3 _force;

    public void DrawTrajectory(Vector3 force)
    {
        var points = new Vector3[100];
        _lineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = transform.position + force * time + _gravity * time * time / 2f;
        }

        _lineRenderer.SetPositions(points);
    }

    public void SetVisible(bool isVisible)
    {
        _lineRenderer.gameObject.SetActive(isVisible);
    }

    private void OnValidate()
    {
        DrawTrajectory(_force);
    }
}
#endif