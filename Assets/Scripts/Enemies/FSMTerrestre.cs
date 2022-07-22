using System;
using System.Collections;
using UnityEngine;

public class FSMTerrestre : MonoBehaviour
{
    private BlackBoardTerrestre blackBoard;

    public enum StateMachine { IDLE, WALK, LOOKING, ATTACK }
    public StateMachine state;
    bool looking, attacked;

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, blackBoard.minDetectDistance);

            float angle = blackBoard.angle;
            float rayRange = blackBoard.minDetectDistance;
            float halfFOV = angle / 2.0f;
            float coneDirection = 180;

            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.up);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.up);

            Vector3 upRayDirection = upRayRotation * -transform.forward * rayRange;
            Vector3 downRayDirection = downRayRotation * -transform.forward * rayRange;

            if (!looking)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
        }
    }

    private void Awake()
    {
        blackBoard = GetComponent<BlackBoardTerrestre>();
    }

    private void Start()
    {
        state = StateMachine.IDLE;
        ChangeState(StateMachine.IDLE);
    }

    private void Update()
    {
        switch (state)
        {
            case StateMachine.IDLE:
                ChangeState(StateMachine.WALK);
                break;
            case StateMachine.WALK:
                if (!blackBoard.navMeshAgent.hasPath)
                {
                    blackBoard.navMeshAgent.SetDestination(blackBoard.GetInterestingPoint());
                }else if (looking)
                {
                    print("looking");
                    ChangeState(StateMachine.ATTACK);
                }
                break;
            case StateMachine.ATTACK:
               
                if (Vector3.Distance(transform.position, blackBoard.player.transform.position) >= blackBoard.minFollowingDistance)
                {
                    ChangeState(StateMachine.IDLE);
                }
                if (!looking)
                {
                    if (blackBoard.navMeshAgent.remainingDistance!=Mathf.Infinity && 
                        blackBoard.navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete &&
                        blackBoard.navMeshAgent.remainingDistance == 0) 
                    {
                        StartCoroutine(WaitForSomething(blackBoard.waitTimer, delegate 
                        {
                            ChangeState(StateMachine.IDLE);
                        }));
                    }
                }
                else
                {
                    Vector3 direction = blackBoard.player.transform.position - transform.position;
                    direction.y = 0;
                    direction.Normalize();
                    Vector3 desplacement = blackBoard.player.transform.position - direction * blackBoard.minApproximation;
                    blackBoard.navMeshAgent.SetDestination(desplacement);
                }

                if (Vector3.Distance(transform.position, blackBoard.player.transform.position) <= blackBoard.minAttackDistance && !attacked)
                {
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
        if (Vector3.Distance(transform.position, blackBoard.player.transform.position) < blackBoard.minDetectDistance)
        {
            float angle;
            Ray ray = new Ray(transform.position, blackBoard.player.transform.position - transform.position);
            if (Physics.Raycast(ray, out RaycastHit hit, blackBoard.minDetectDistance, blackBoard.layerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    angle = Vector3.Angle(hit.point - transform.position, transform.forward);
                    if (angle < blackBoard.angle / 2)
                    {
                        looking = true;
                    }
                }
                else
                    looking = false;
            }
        }
    }
    private void ChangeState(StateMachine newState)
    {
        switch (newState)
        {
            case StateMachine.IDLE:

                blackBoard.navMeshAgent.SetDestination(blackBoard.GetInterestingPoint());
                break;
            case StateMachine.WALK:
                break;
            case StateMachine.LOOKING:
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
            case StateMachine.LOOKING:
                break;
            case StateMachine.ATTACK:
                looking = false;
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
        attacked = true;
        yield return new WaitForSeconds(1);
        attacked = false;
    }
}
