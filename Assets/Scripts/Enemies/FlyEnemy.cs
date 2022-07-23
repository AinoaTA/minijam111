using UnityEngine.AI;
using UnityEngine;

public class FlyEnemy : MonoBehaviour, IHit
{
    BlackBoardEnemy blackboard;

    Vector3 dir;
    public GameObject prefabProyectile;

    public void Attacked()
    {
        print("nada de momento");
    }
    void Awake()
    {
        blackboard = GetComponent<BlackBoardEnemy>();
    }

    void Update()
    {
        if (!blackboard.enabledGame || blackboard.attacking)
            return;

        if (Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.minDetectDistance)
        {
            dir = (blackboard.player.transform.position - transform.position).normalized;

            Vector3 destination = blackboard.player.transform.position - dir * blackboard.minApproximation;
            blackboard.navMeshAgent.speed = blackboard.speed;
            blackboard.navMeshAgent.SetDestination(destination);

            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = rot;

            if (Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance && !blackboard.attacked)
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
                Vector3 dest = blackboard.RandomNavSphere(transform.position, blackboard.minWanderDistance, 1);
                blackboard.navMeshAgent.SetDestination(dest);
            }
        }
    }
}
