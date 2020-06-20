using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoard : MonoBehaviour
{
    [SerializeField]
    private Transform _center;
    [SerializeField]
    private Transform _end;
    private float _unitLength;
    // Start is called before the first frame update
    private void Awake()
    {
        float radius = Vector3.Distance(_center.localPosition, _end.localPosition);
        _unitLength = radius / 6f;
    }

    public int getScore(Vector3 hitPoint)
    {
        
        Vector3 localHit = transform.InverseTransformPoint(hitPoint);
        float localDistance = Vector3.Distance(localHit, _center.localPosition);
        return calculateScore(localDistance);
    }

    int calculateScore(float distance)
    {
        int unitCount = (int) Mathf.Ceil(distance / _unitLength);
        int score = 11 - unitCount;
        if (score < 5)
        {
            return 0;
        }
        return score;
    }
}
