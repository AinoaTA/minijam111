using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour, IHit
{
    public BlackBoardEnemy blackboard;

    public enum StateMachine { FLY, ATTACK }
    public StateMachine state;
    Vector3 dir;

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
        if (!blackboard.enabledGame)
            return;
        switch (state)
        {
            case StateMachine.FLY:

                //if(blackboard.attacking)
                //    return;

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

                break;
            case StateMachine.ATTACK:
                break;
            default:
                break;
        }
    }

    public void ChangeState(StateMachine newstate)
    {

        switch (newstate)
        {
            case StateMachine.FLY:
                break;
            case StateMachine.ATTACK:
                break;
            default:
                break;
        }

        switch (state)
        {
            case StateMachine.FLY:
                break;
            case StateMachine.ATTACK:
                break;
            default:
                break;
        }
        state = newstate;
    }

    IEnumerator AttackRecovery()
    {
        blackboard.attacked = true;
        yield return new WaitForSeconds(1);
        blackboard.attacked = false;
    }
}
