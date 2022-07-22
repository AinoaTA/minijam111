using System.Collections;
using UnityEngine;

public class FlyEnemy : MonoBehaviour, IHit
{
    BlackBoardEnemy blackboard;

    public enum StateMachine { FLY, ATTACK }
    public StateMachine state;
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
    private void Start()
    {
        ChangeState(StateMachine.FLY);
    }

    void Update()
    {
        if (!blackboard.enabledGame)
            return;
        switch (state)
        {
            case StateMachine.FLY:

                dir = (blackboard.player.transform.position - transform.position).normalized;
               
                Vector3 destination = blackboard.player.transform.position - dir * blackboard.minApproximation;
                blackboard.navMeshAgent.speed = blackboard.speed;
                blackboard.navMeshAgent.SetDestination(destination);

                transform.LookAt(blackboard.player.transform.position);

                //dir.y = 0;
                //Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
                //transform.rotation = rot;

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance && !blackboard.attacked)
                {
                    ChangeState(StateMachine.ATTACK);
                }

                break;
            case StateMachine.ATTACK:

                if (!blackboard.attacked)
                    return;

                //blackboard.playerHeal.TakeDamage();
                //StartCoroutine(AttackRecovery());

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) >= blackboard.minDetectDistance) 
                {
                    ChangeState(StateMachine.FLY);
                }
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
