using System;
using System.Collections;
using UnityEngine;

public class FSMTerrestre : MonoBehaviour, IHit
{
    private BlackBoardTerrestre blackboard;

    public enum StateMachine { IDLE, WALK, HIT, ATTACK }
    public StateMachine state;


    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, blackboard.minDetectDistance);

            float angle = blackboard.angle;
            float rayRange = blackboard.minDetectDistance;
            float halfFOV = angle / 2.0f;
            float coneDirection = 180;

            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.up);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.up);

            Vector3 upRayDirection = upRayRotation * -transform.forward * rayRange;
            Vector3 downRayDirection = downRayRotation * -transform.forward * rayRange;

            if (!blackboard.looking)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
        }
    }

    private void Awake()
    {
        blackboard = GetComponent<BlackBoardTerrestre>();
    }

    private void Start()
    {
       
        state = StateMachine.IDLE;
        ChangeState(StateMachine.IDLE);
    }

    private void Update()
    {
        if (!blackboard.enabledGame)
            return;
        switch (state)
        {
            case StateMachine.IDLE:
                ChangeState(StateMachine.WALK);
                break;
            case StateMachine.WALK:
                if (!blackboard.navMeshAgent.hasPath)
                {
                    blackboard.navMeshAgent.SetDestination(blackboard.GetInterestingPoint());
                }
                else if (blackboard.looking)
                {
                    print("looking");
                    ChangeState(StateMachine.ATTACK);
                }
                break;
            case StateMachine.HIT:
                if (!blackboard.looking)
                {
                    if (blackboard.navMeshAgent.remainingDistance != Mathf.Infinity &&
                        blackboard.navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete &&
                        blackboard.navMeshAgent.remainingDistance == 0)
                    {
                        StartCoroutine(WaitForSomething(blackboard.waitTimer, delegate
                        {
                            ChangeState(StateMachine.IDLE);
                        }));
                    }
                }
                else
                {
                    ChangeState(StateMachine.ATTACK);
                }
                break;
            case StateMachine.ATTACK:
                blackboard.attacking = true;
                if (Vector3.Distance(transform.position, blackboard.player.transform.position) >= blackboard.minFollowingDistance)
                {
                    ChangeState(StateMachine.IDLE);
                }
                if (!blackboard.looking)
                {
                    if (blackboard.navMeshAgent.remainingDistance != Mathf.Infinity &&
                        blackboard.navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete &&
                        blackboard.navMeshAgent.remainingDistance == 0)
                    {
                        StartCoroutine(WaitForSomething(blackboard.waitTimer, delegate
                        {
                            ChangeState(StateMachine.IDLE);
                        }));
                    }
                }
                else
                {
                    Vector3 direction = blackboard.player.transform.position - transform.position;
                    direction.y = 0;
                    direction.Normalize();
                    Vector3 desplacement = blackboard.player.transform.position - direction * blackboard.minApproximation;
                    blackboard.navMeshAgent.SetDestination(desplacement);
                }

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance && !blackboard.attacked)
                {
                    blackboard.playerHeal.TakeDamage();
                    StartCoroutine(AttackRecovery());
                }
                break;
            default:
                break;
        }

        Looking();
    }

    private void Looking()
    {
        if (Vector3.Distance(transform.position, blackboard.player.transform.position) < blackboard.minDetectDistance)
        {
            float angle;
            Ray ray = new Ray(transform.position, blackboard.player.transform.position - transform.position);
            if (Physics.Raycast(ray, out RaycastHit hit, blackboard.minDetectDistance, blackboard.layerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    angle = Vector3.Angle(hit.point - transform.position, transform.forward);
                    if (angle < blackboard.angle / 2)
                    {
                        blackboard.looking = true;
                    }
                }
                else
                    blackboard.looking = false;
            }
        }
    }

    private void ChangeState(StateMachine newState)
    {
        switch (newState)
        {
            case StateMachine.IDLE:

                blackboard.navMeshAgent.SetDestination(blackboard.GetInterestingPoint());
                break;
            case StateMachine.WALK:
                break;
            case StateMachine.HIT:
                Vector3 direction = blackboard.player.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                Vector3 desplacement = blackboard.player.transform.position - direction * blackboard.minApproximation;
                blackboard.navMeshAgent.SetDestination(desplacement);
                break;
            case StateMachine.ATTACK:

                break;
            default:
                break;
        }

        switch (state)
        {
            case StateMachine.IDLE:
                break;
            case StateMachine.WALK:
                break;
            case StateMachine.HIT:
                break;
            case StateMachine.ATTACK:
                blackboard.attacking = false;
                blackboard.looking = false;
                break;
            default:
                break;
        }


        state = newState;
    }

    IEnumerator WaitForSomething(float s, Action action)
    {
        yield return new WaitForSeconds(s);
        action?.Invoke();

    }

    IEnumerator AttackRecovery()
    {
        yield return new WaitForSeconds(1);
        blackboard.attacking = false;
    }

    public void Attacked()
    {
        if (blackboard.attacking)
            return;
        ChangeState(StateMachine.HIT);
    }
}