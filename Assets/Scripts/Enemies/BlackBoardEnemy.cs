using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackBoardEnemy : MonoBehaviour
{
    [SerializeField]bool normalEnemy;
    public NavMeshAgent navMeshAgent;
    public float minDetectDistance;
    public float minAttackDistance;
    public float minFollowingDistance;
    public float minApproximation;
    public float timer;
    public float waitTimer;
    public float speed;
    public LayerMask layerMask;
    public float raycastDistance;
    public float damage;
    [Range(0,360)]
    public float angle;
    public GameObject parentInterestingPoints;
    public GameObject player;
    [HideInInspector] public HealSystem playerHeal;
    [HideInInspector] public bool enabledGame=true;
    [HideInInspector] public bool looking, attacked, attacking;
    [SerializeField] private List<Transform> allInterestingPoints = new List<Transform>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHeal = player.GetComponent<HealSystem>();
        if (normalEnemy)
            return;
        for (int a = 0; a < parentInterestingPoints.transform.childCount; a++)
        {
            allInterestingPoints.Add(parentInterestingPoints.transform.GetChild(a));
        }
    }

    public Vector3 GetInterestingPoint() => allInterestingPoints[Random.Range(0, allInterestingPoints.Count)].position;
    
}
