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
    public static EnemySpawner Instance;

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
    
    [Header("Enemy Wave Data")]
    [SerializeField] private float waveMinDistance;
    [SerializeField] private float waveMaxDistance;
    [SerializeField] private int enemiesPerWave;
    public bool burstWave = true;
    [SerializeField] private float timeBetweenWaveSpawn;

    private bool _spawnWave;
    private float _waveSpawnTimer;
    private float _spawnTimer;
    private int _waveSpawnCounter;


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
        
        
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.transform.position, waveMinDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.transform.position, waveMaxDistance);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SceneStartSpawning();

        if (waveMinDistance == 0) waveMinDistance = minDistance;
        if (waveMaxDistance == 0) waveMaxDistance= maxDistance;

        _spawnTimer = 0f;
        _waveSpawnTimer = 0f;
        _waveSpawnCounter = 0;
        _spawnWave = false;
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= timeBetweenSpawns)
        {
            var location = SpawnNear(player.transform, minDistance, maxDistance);
            var randomEnemy = ChooseRandomEnemy();
            
            Instantiate(randomEnemy, location, Quaternion.identity);
            _spawnTimer = 0f;
        }

        if (_spawnWave)
        {
            if (burstWave)
            {
                GenerateEnemyWave();
            }
            else
            {
                if (_waveSpawnCounter >= enemiesPerWave)
                {
                    _waveSpawnCounter = 0;
                    _waveSpawnTimer = 0;
                    _spawnWave = false;
                }
                else
                {
                    _waveSpawnTimer += Time.deltaTime;

                    if (_waveSpawnTimer >= timeBetweenWaveSpawn)
                    {
                        var location = SpawnNear(player.transform, waveMinDistance, waveMaxDistance);
                        var randomEnemy = ChooseRandomEnemy();
                        
                        Instantiate(randomEnemy, location, Quaternion.identity); 
                    }
                    
                }
            }
        }
    }
    
    private void SceneStartSpawning()
    {
        foreach (var spawnPos in spawnPositionsList)
        {
            for(var i = 0; i < enemiesPerSpawnPosition; i++)
            {
                var location = SpawnNear(spawnPos, 0.1f, spawnPositionsRadius);
                var randomEnemy = ChooseRandomEnemy();
                
                Instantiate(randomEnemy, location, Quaternion.identity);
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

    private GameObject ChooseRandomEnemy()
    {
        var randomEnemy = Random.Range(0, enemyPrefabList.Count);

        return enemyPrefabList[randomEnemy];
    }

    private void GenerateEnemyWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            var location = SpawnNear(player.transform ,waveMinDistance, waveMaxDistance);
            var randomEnemy = Random.Range(0, enemyPrefabList.Count - 1);
                
            Instantiate(enemyPrefabList[randomEnemy], location, Quaternion.identity);
        }
        
        _waveSpawnTimer = 0f;
        _spawnWave = false;
    }

    private Vector3 SpawnNear(Transform nearPosition, float minRadius, float maxRadius)
    {
        /*
        var randomRadius = Random.Range(minRadius, maxRadius);
        var randomPosition = RandomNavMeshLocation(randomRadius);

        var randomEnemy = Random.Range(0, enemyPrefabList.Count - 1);
        Instantiate(enemyPrefabList[randomEnemy], randomPosition, Quaternion.identity);
        */
        
        var randomRadius = Random.Range(minRadius, maxRadius);
        var randomDirection = Random.insideUnitSphere;
        var randomPoint = nearPosition.position + randomDirection * randomRadius;

        if (NavMesh.SamplePosition(randomPoint, out var hit, randomRadius, 1))
        {
            randomPoint = hit.position;
        }

        return Vector3.Distance(hit.position, nearPosition.position) < minRadius ? SpawnNear(nearPosition, minRadius, maxRadius) : randomPoint;
    }

    public void SpawnWave()
    {
        _spawnWave = true;
    }
}
