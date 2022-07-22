using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IHit
{
    private BlackBoardTerrestre blackboard;
    private float distance;
    Vector3 dir;
    

    public void Attacked()
    {
        print("nada aun");
    }

    private void Awake()
    {
        blackboard = GetComponent<BlackBoardTerrestre>();
    }

    private void Update()
    {
        if (!blackboard.enabledGame || blackboard.attacking)
            return;
        dir = blackboard.player.transform.position - transform.position;

        Vector3 destination = blackboard.player.transform.position - dir * blackboard.minApproximation;
        blackboard.navMeshAgent.speed = blackboard.speed;
        blackboard.navMeshAgent.SetDestination(destination);

        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rot;


        if (Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance && !blackboard.attacked)
        {
            blackboard.playerHeal.TakeDamage();
            StartCoroutine(AttackRecovery());
        }
    }

    IEnumerator AttackRecovery()
    {
        blackboard.attacked = true;
        yield return new WaitForSeconds(1);
        blackboard. attacked = false;
    }
}
