using System.Collections;
using System.Collections.Generic;
using Others;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

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
    [FormerlySerializedAs("playerHeal")] [HideInInspector] public HealthSystem playerHealth;
    [HideInInspector] public bool enabledGame=true;
    [HideInInspector] public bool looking, attacked, attacking;
    [SerializeField] private List<Transform> allInterestingPoints = new List<Transform>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
        if (normalEnemy)
            return;
        for (int a = 0; a < parentInterestingPoints.transform.childCount; a++)
        {
            allInterestingPoints.Add(parentInterestingPoints.transform.GetChild(a));
        }
    }

    public Vector3 GetInterestingPoint() => allInterestingPoints[Random.Range(0, allInterestingPoints.Count)].position;

    public IEnumerator AttackRecovery()
    {
        attacked = true;
        yield return new WaitForSeconds(1);
        attacked = false;
    }
}
