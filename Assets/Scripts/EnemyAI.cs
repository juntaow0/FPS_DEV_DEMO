using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float _lookRadius = 10f;
    [SerializeField]
    private float _lookAngle = 90f;
    [SerializeField]
    private Transform _player;
    private NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(_player.position, transform.position);
        Vector3 dir = (_player.position - transform.position).normalized;
        float polarAngle = Vector3.Angle(dir, transform.forward);
        if (distance <= _lookRadius && polarAngle < _lookAngle * 0.5f)
        {
            _agent.SetDestination(_player.position);
            if (distance <= _agent.stoppingDistance)
            {
                LookAtTarget();
            }
        }
    }

    void LookAtTarget()
    {
        Vector3 dir = (_player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x,0,dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation, Time.deltaTime*3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _lookRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(_lookAngle*0.5f, transform.up) * transform.forward * _lookRadius);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-_lookAngle * 0.5f, transform.up) * transform.forward * _lookRadius);
    }
}
