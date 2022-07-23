using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour, IHit
{
    private BlackBoardEnemy blackboard;
    Vector3 dir;

    public void Attacked()
    {
        Destroy(gameObject);
    }

    public void BeingHit()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        blackboard = GetComponent<BlackBoardEnemy>();
    }

    private void Update()
    {
        if (!blackboard.enabledGame || blackboard.hit)
            return;

        if (Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.minDetectDistance)
        {
            GetDir();

            if (Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance && !blackboard.attacking)
            {
                blackboard.playerHealth.TakeDamage();
                StartCoroutine(blackboard.AttackRecovery());
            }
        }
        else
        {
            if (blackboard.navMeshAgent.remainingDistance != Mathf.Infinity &&
                           blackboard.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                           blackboard.navMeshAgent.remainingDistance == 0)
            {
                {
                    if (blackboard.attacking)
                    {
                        GetDir();
                    }
                    else
                    {
                        Vector3 dest = blackboard.RandomNavSphere(transform.position, blackboard.minWanderDistance, 1);
                        blackboard.navMeshAgent.SetDestination(dest);
                    }
                }
            }
        }

    }
    private void GetDir()
    {
        dir = (blackboard.player.transform.position - transform.position).normalized;

        Vector3 destination = blackboard.player.transform.position - dir * blackboard.minApproximation;
        blackboard.navMeshAgent.speed = blackboard.speed;
        blackboard.navMeshAgent.SetDestination(destination);
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rot;
    }
}
