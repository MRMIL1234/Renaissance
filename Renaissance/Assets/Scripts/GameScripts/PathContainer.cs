using UnityEngine;

public class PathContainer : MonoBehaviour
{
    private Transform[] _points;

    public Transform[] Points => _points;

    private void OnEnable()
    {
        _points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _points[i] = transform.GetChild(i);
        }
    }
}