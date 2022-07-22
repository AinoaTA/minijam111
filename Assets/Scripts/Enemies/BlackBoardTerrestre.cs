using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackBoardTerrestre : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float minDetectDistance;
    public float minAttackDistance;
    public float minFollowingDistance;
    public float minApproximation;
    public float timer;
    public float waitTimer;
    public LayerMask layerMask;
    public float raycastDistance;
    [Range(0,360)]
    public float angle;
    public GameObject parentInterestingPoints;
    public GameObject player;
   [SerializeField] private List<Transform> allInterestingPoints = new List<Transform>();

    public void GetDamage()
    { 
    
    
    }

    private void Start()
    {
        for (int a = 0; a < parentInterestingPoints.transform.childCount; a++)
        {
            allInterestingPoints.Add(parentInterestingPoints.transform.GetChild(a));
        }
    }

    public Vector3 GetInterestingPoint() => allInterestingPoints[Random.Range(0, allInterestingPoints.Count)].position;
    
}
