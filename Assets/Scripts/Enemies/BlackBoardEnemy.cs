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
    public float recoveryAttackTime=1;
    [Range(0,360)]
    public float angle;
   /* [HideInInspector]*/ public NavMeshAgent navMeshAgent;
    [HideInInspector]public GameObject player;
    [FormerlySerializedAs("playerHeal")] [HideInInspector] public HealthSystem playerHealth;
    [HideInInspector] public bool enabledGame=true;
    [HideInInspector] public bool looking, attacking, hit;

     [SerializeField] private bool isBoss;
    [SerializeField] private Material[] allEyesMaterial;
    [SerializeField] private Renderer eyeMaterial;
    private ColorEntity colorEntity;
    private void Awake()
    {
        colorEntity = GetComponent<ColorEntity>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
    }

    private void Start()
    {
        eyeMaterial.material = allEyesMaterial[(int)colorEntity.colorType];
    }

    public void ChangeMaterial(ColorTypes color)
    {
        eyeMaterial.material = allEyesMaterial[(int)color];
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
}
