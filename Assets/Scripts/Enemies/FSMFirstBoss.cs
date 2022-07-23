using System;
using System.Collections;
using Colors;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FSMFirstBoss : MonoBehaviour, IHit
{
    private BlackBoardEnemy blackboard;

    public enum StateMachine { IDLE, WALK, HIT, ATTACK }
    public StateMachine state;

    [SerializeField] private float changeColorTime = 3.0f;
    [SerializeField] private float invulnerabilityTime = 5.0f;
    [SerializeField] private int maxHealth = 3;

    private ColorEntity _colorEntity;
    private float _currentHealth;
    private float _invulnerabilityTimer = 0.0f;
    private float _changeColorTimer = 0.0f;
    private bool _invulnerable;

    [SerializeField] private Animator animator;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Punch = Animator.StringToHash("OnPunchTrigger");
    private static readonly int Kick = Animator.StringToHash("OnKickTrigger");
    private static readonly int Death = Animator.StringToHash("OnDeathTrigger");
    private static readonly int Invulnerable = Animator.StringToHash("Invulnerable");
    
    public GameObject prefabProyectile;
    

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
            Vector3 downRayDirection = downRayRotation * - transform.forward * rayRange;

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
        blackboard = GetComponent<BlackBoardEnemy>();
        _colorEntity = GetComponent<ColorEntity>();
    }

    private void Start()
    {
        state = StateMachine.IDLE;
        ChangeState(StateMachine.IDLE);
        _currentHealth = maxHealth;
        _invulnerable = false;
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
                    blackboard.navMeshAgent.SetDestination(blackboard.RandomNavSphere(transform.position,blackboard.minDetectDistance,1));
                }
                else if (blackboard.looking)
                {
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
                blackboard.hit = true;
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
                    Vector3 displacement = blackboard.player.transform.position - direction * blackboard.minApproximation;
                    blackboard.navMeshAgent.SetDestination(displacement);
                }

                if (Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance && !blackboard.attacking)
                {
                    animator.SetBool(Idle, true);
                    Attack();
                }
                else if (blackboard.attacking && Vector3.Distance(transform.position, blackboard.player.transform.position) <= blackboard.minAttackDistance)
                {
                    animator.SetBool(Idle, true);
                }
                else
                {
                    animator.SetBool(Idle, false);
                }
                break;
            default:
                break;
        }

        _changeColorTimer += Time.deltaTime;
        if (_changeColorTimer >= changeColorTime)
        {
            _colorEntity.colorType = ColorEntity.GetNextColor(_colorEntity.colorType);
        }
        
        if (_invulnerable)
            _invulnerabilityTimer += Time.deltaTime;
        else
            animator.SetBool(Invulnerable, false);
        
        if (_invulnerabilityTimer >= invulnerabilityTime)
        {
            _invulnerable = false;
            _invulnerabilityTimer = 0f;
        }
        
        Looking();
    }

    private void Attack()
    {
        var random = Random.Range(0, 2);
        Debug.Log(random);
        switch (random)
        {
            case 0:
                Debug.Log("Punch");
                animator.SetTrigger(Punch);
                break;
            case 1:
                Debug.Log("Kick");
                animator.SetTrigger(Kick);
                break;
        }

        blackboard.attacking = true;
        //blackboard.playerHealth.TakeDamage();
        StartCoroutine(blackboard.AttackRecovery());
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
                animator.SetBool(Idle, true);
                if (blackboard.navMeshAgent.remainingDistance != Mathf.Infinity &&
                       blackboard.navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete &&
                       blackboard.navMeshAgent.remainingDistance == 0)
                {
                    blackboard.navMeshAgent.SetDestination(blackboard.RandomNavSphere(transform.position, blackboard.minWanderDistance, 1));
                }
                break;
            case StateMachine.WALK:
                break;
            case StateMachine.HIT:
                Vector3 direction = blackboard.player.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                Vector3 displacement = blackboard.player.transform.position - direction * blackboard.minApproximation;
                blackboard.navMeshAgent.SetDestination(displacement);

                Damage();
                break;
            case StateMachine.ATTACK:

                break;
            default:
                break;
        }

        switch (state)
        {
            case StateMachine.IDLE:
                animator.SetBool(Idle, false);
                break;
            case StateMachine.WALK:
                break;
            case StateMachine.HIT:
                break;
            case StateMachine.ATTACK:
                blackboard.hit = false;
                blackboard.looking = false;
                break;
            default:
                break;
        }
        state = newState;
    }

    private void Damage()
    {
        if (_invulnerable) return;
        
        _currentHealth--;
        if(_currentHealth <= 0)
            Die();

        _invulnerable = true;
        animator.SetBool(Invulnerable, true);
    }

    IEnumerator WaitForSomething(float s, Action action)
    {
        yield return new WaitForSeconds(s);
        action?.Invoke();
    }
    public void Attacked()
    {
        Damage();
        
        if (blackboard.hit)
            return;
        ChangeState(StateMachine.HIT);
    }

    private void Die()
    {
        animator.SetTrigger(Death);
        Destroy(gameObject);
    }

    public void BeingHit()
    {
        if (blackboard.hit)
            return;
        ChangeState(StateMachine.HIT);
    }
}
