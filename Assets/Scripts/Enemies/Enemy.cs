using UnityEngine;

public class Enemy : MonoBehaviour, IHit
{
    private BlackBoardTerrestre blackBoard;
    private float distance;
    Vector3 dir;

    public void Attacked()
    {
        print("nada aun");
    }

    private void Awake()
    {
        blackBoard = GetComponent<BlackBoardTerrestre>();
    }

    private void Update()
    {
        dir = blackBoard.player.transform.position - transform.position;

        Vector3 destination = blackBoard.player.transform.position - dir * blackBoard.minApproximation;
        blackBoard.navMeshAgent.speed = blackBoard.speed;
        blackBoard.navMeshAgent.SetDestination(destination);

        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rot;
    }
}
