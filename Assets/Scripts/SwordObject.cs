using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordObject : MonoBehaviour
{
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    [SerializeField] float radius = 1f;
    [SerializeField] LayerMask layerMask;
    public bool check = false;
    bool _onHitted;
    List<Vector3> hitPoints = new List<Vector3>();
    public Action<Enemy, Vector3, Vector3> OnHitEnemy;
    Enemy enemy;
    void Update()
    {
        if (!check)
        {
            hitPoints.Clear();
            _onHitted = false;
            return;
        }

        if (_onHitted) return;
        CheckPoints();
    }

    void CheckPoints()
    {
        Collider[] colliders = Physics.OverlapCapsule(point1.position, point2.position, radius, layerMask);
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                hitPoints.Add(collider.ClosestPoint(point2.position));
                
                if (!enemy)
                    enemy = collider.GetComponentInParent<Enemy>();
            }
        }
        
        // get direction of the sword object and find the middle point of the hit points
         
        if (hitPoints.Count > 0)
        {
            Vector3 direction = hitPoints[^1] - hitPoints[0];
            Vector3 middlePoint = hitPoints[0] + (hitPoints[^1] - hitPoints[0]) / 2;
            OnHitEnemy?.Invoke(enemy, middlePoint, direction);
            _onHitted = true;
        }
         
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point1.position, radius);
        Gizmos.DrawWireSphere(point2.position, radius);
        Gizmos.DrawLine(point1.position, point2.position);

        if (hitPoints.Count > 0)
        {
            Gizmos.color = Color.green;
            foreach (var point in hitPoints)
            {
                Gizmos.DrawSphere(point, radius);
            }
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hitPoints[0], radius);
            Gizmos.DrawSphere(hitPoints[^1], radius);
        }

    }
}
