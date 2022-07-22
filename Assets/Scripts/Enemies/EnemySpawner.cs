using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Data")]
    [SerializeField] private GameObject player;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private List<GameObject> enemyPrefabList = new List<GameObject>();

    private float _spawnTimer = 0f;


    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, minDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, maxDistance);
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= timeBetweenSpawns)
        {
            var randomRadius = Random.Range(minDistance, maxDistance);
            var randomPosition = RandomNavMeshLocation(randomRadius);

            var randomEnemy = Random.Range(0, enemyPrefabList.Count - 1);
            Instantiate(enemyPrefabList[randomEnemy], randomPosition, quaternion.identity);
            _spawnTimer = 0f;
        }
    }

    private Vector3 RandomNavMeshLocation(float radius)
    {
        var randomDirection = Random.insideUnitSphere;
        var randomPoint = player.transform.position + randomDirection * radius;

        if (NavMesh.SamplePosition(randomPoint, out var hit, radius, 1))
        {
            randomPoint = hit.position;
        }

        return Vector3.Distance(hit.position, player.transform.position) < minDistance ? RandomNavMeshLocation(Random.Range(minDistance, maxDistance)) : randomPoint;
    }
}
