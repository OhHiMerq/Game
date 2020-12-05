using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField] private GameObject Platform;
    private Transform _platform;

    [SerializeField] private Transform pointA, pointB;
    private Transform GoToPoint;
    [SerializeField] private float MoveSpeed;

    private void Start()
    {
        GoToPoint = pointA;

        if(Platform != null)
        {
            _platform = newPlatform();
        }
    }

    private void Update()
    {
        float pointDistance = Mathf.Abs(Vector2.Distance(_platform.transform.position, GoToPoint.position));

        if(pointDistance < 0.01f && GoToPoint == pointA)
        {
            GoToPoint = pointB;
        } else if (pointDistance < 0.01f && GoToPoint == pointB)
        {
            GoToPoint = pointA;
        }

        _platform.position = Vector2.MoveTowards(_platform.position, GoToPoint.position, MoveSpeed * Time.deltaTime);

    }

    private Transform newPlatform()
    {
        GameObject _platform = Instantiate(Platform, transform.position, Quaternion.identity);
        _platform.transform.SetParent(transform);

        return _platform.transform;

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
