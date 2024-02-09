using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _distanceFromCameraToSpawnEnemies = 10f;
    [SerializeField] private int _totalEnemiesToSpawn = 5;

    private int _currentNumberOfEnemiesSpawned = 0;
    private List<Transform> _selectedSpawnPoints_List = new List<Transform>();
    private List<int> _randomIndexOfSP_List = new List<int>();

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        SpawnEnemies();
    }
    public void DecrementEnemy()
    {
        _currentNumberOfEnemiesSpawned--;
        Invoke("SpawnEnemies", 1);
    }
    void SpawnEnemies()
    {
        _selectedSpawnPoints_List.Clear();
        _randomIndexOfSP_List.Clear();

        //save spawn points which are far from camera
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (Vector3.Distance(_spawnPoints[i].position, GameManager.instance._camera.position) >
                _distanceFromCameraToSpawnEnemies)
            {
                _selectedSpawnPoints_List.Add(_spawnPoints[i]);
            }
        }
        //add indexes from _selectedSpawnPoints_List into _randomIndexOfSP_List for randomly spawn enemies
        while (_currentNumberOfEnemiesSpawned < _totalEnemiesToSpawn)
        {
            int randomNumbers = Random.Range(0, _selectedSpawnPoints_List.Count);
            if (!_randomIndexOfSP_List.Contains(randomNumbers))
            {
                _randomIndexOfSP_List.Add(randomNumbers);
                _currentNumberOfEnemiesSpawned++;
            }
        }
        //spawn enemies at indexes stored in _randomIndexOfSP_List
        for (int i = 0; i < _randomIndexOfSP_List.Count; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, _selectedSpawnPoints_List[_randomIndexOfSP_List[i]].position, Quaternion.identity);
            enemy.GetComponent<AIDestinationSetter>().target = GameManager.instance._player.transform;
        }
    }
}
