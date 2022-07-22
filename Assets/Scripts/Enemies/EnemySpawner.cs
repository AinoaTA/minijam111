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
    [Header("Initial Spawn Data")]
    [SerializeField] private List<Transform> spawnPositionsList = new List<Transform>();
    [SerializeField] private float spawnPositionsRadius;
    [SerializeField] private int enemiesPerSpawnPosition;
    
    [Header("Player Spawn Data")]
    [SerializeField] private GameObject player;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private List<GameObject> enemyPrefabList = new List<GameObject>();

    private float _spawnTimer = 0f;


    private void OnDrawGizmosSelected()
    {
        foreach (var spawnPos in spawnPositionsList)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(spawnPos.position, spawnPositionsRadius);
        }
        
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, minDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, maxDistance);
    }

    private void Start()
    {
        SceneStartSpawning();
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= timeBetweenSpawns)
        {
            var randomRadius = Random.Range(minDistance, maxDistance);
            var randomPosition = RandomNavMeshLocation(randomRadius);

            var randomEnemy = Random.Range(0, enemyPrefabList.Count - 1);
            Instantiate(enemyPrefabList[randomEnemy], randomPosition, Quaternion.identity);
            _spawnTimer = 0f;
        }
    }
    
    private void SceneStartSpawning()
    {
        foreach (var spawnPos in spawnPositionsList)
        {
            for(var i = 0; i < enemiesPerSpawnPosition; i++)
            {
                var randomRadius = Random.Range(0.1f, spawnPositionsRadius);
                var randomPos = RandomSceneStartLocation(spawnPos, randomRadius);
                var randomEnemy = Random.Range(0, enemyPrefabList.Count - 1);
                
                Instantiate(enemyPrefabList[randomEnemy], randomPos, Quaternion.identity);
            }
        }

    }
    
    
    private Vector3 RandomSceneStartLocation(Transform spawnPos, float radius)
    {
        var randomDir = Random.insideUnitSphere;
        var randomPos = spawnPos.position + randomDir * radius;
        

        if (NavMesh.SamplePosition(randomPos, out var hit, radius, 1))
        {
            randomPos = hit.position;
        }

        return Vector3.Distance(hit.position, player.transform.position) < minDistance ? RandomSceneStartLocation(spawnPos, radius) : randomPos;
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
