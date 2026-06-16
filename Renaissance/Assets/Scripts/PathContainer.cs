using UnityEngine;

public class PathContainer : MonoBehaviour
{
    private Transform[] points;

    private void OnEnable()
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}