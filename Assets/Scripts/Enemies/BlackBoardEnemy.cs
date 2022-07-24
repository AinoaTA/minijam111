using System.Collections;
using Others;
using Colors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BlackBoardEnemy : MonoBehaviour
{
    public float minDetectDistance;
    public float minAttackDistance;
    public float minFollowingDistance;
    public float minWanderDistance;
    public float minApproximation;
    public float timer;
    public float waitTimer;
    public float speed;
    public LayerMask layerMask;
    public float raycastDistance;
    public float recoveryAttackTime = 1;
    [Range(0, 360)]
    public float angle;
    [SerializeField] private float yPos;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public GameObject player;
    [FormerlySerializedAs("playerHeal")] [HideInInspector] public HealthSystem playerHealth;
    [HideInInspector] public bool enabledGame = true;
    [HideInInspector] public bool looking, attacking, hit, death;
    [HideInInspector] public Animator animator;
    [SerializeField] private bool isBoss;
    [SerializeField] private Material[] allBodiesMaterial;
    [SerializeField] private Renderer bodyMaterial;
    private ColorEntity colorEntity;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        colorEntity = GetComponent<ColorEntity>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
    }

    private void Start()
    {
        bodyMaterial.material = allBodiesMaterial[(int)colorEntity.colorType];
    }


    public void InitNewEnemyColor()
    {
        colorEntity.colorType = (ColorTypes)Random.Range(0, 2);
        ChangeMaterial(colorEntity.colorType);
    }
    public void ChangeMaterial(ColorTypes color)
    {
        bodyMaterial.material = allBodiesMaterial[(int)color];
    }
    public IEnumerator AttackRecovery()
    {
        attacking = true;
        yield return new WaitForSeconds(recoveryAttackTime);
        attacking = false;
    }
    public Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    public IEnumerator FixDeathPos(float estimatedTime = 0.2f, float animdDur = 2f)
    {
        yield return new WaitForSeconds(animdDur);
        animator.enabled = false;
        Vector3 currPos = bodyMaterial.transform.parent.transform.localPosition;
        Vector3 newPos = currPos;
        newPos.y = yPos;
        float i = 0;
        while (i < estimatedTime)
        {
            i += Time.deltaTime;
            bodyMaterial.transform.parent.transform.localPosition = Vector3.Lerp(currPos, newPos, i / estimatedTime);
            yield return null;
        }
        animator.enabled = true;
    }
}
