using UnityEngine.AI;
using UnityEngine;
using Projectiles;

public class FlyEnemy : MonoBehaviour, IHit
{
    BlackBoardEnemy blackboard;
    Vector3 dir;
    public GameObject prefabProyectile;

    public void Attacked()
    {
        Destroy(gameObject);
    }
    public void BeingHit()
    {
        blackboard.attacking = true;
        GetDir();
    }
    void Awake()
    {
        blackboard = GetComponent<BlackBoardEnemy>();
    }

    void Update()
    {
        if (!blackboard.enabledGame || blackboard.hit)
            return;

        if (Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.minDetectDistance && !blackboard.attacking)
        {
            GetDir();

            if (Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance)
            {
                ThrowProjectile();
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
                        blackboard.attacking = false;
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
    public void ThrowProjectile()
    {
        EnemyProjectile projectile = Instantiate(prefabProyectile, transform.GetChild(0).transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        Vector3 dir = (blackboard.player.transform.position - transform.GetChild(0).transform.position);
        Vector3 correctedDir = dir.normalized + new Vector3(0, 0.1f, 0);
        projectile.InitializedProjectile(correctedDir);
    }


}
