using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private GameManager _gameManager;
    
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private MeshCollider _spawnArea;

    [SerializeField]
    private float _timer;
    [SerializeField]
    private int _enemySpawned, _waveKillCounter, _waveNumber;
    public int WaveNumber => _waveNumber;

    private bool _waveIsOver;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        _timer = 0f;
        _enemySpawned = 0;
        _waveKillCounter = 0;
        _waveNumber = 1;
        _waveIsOver = false;
    }

    private void Update()
    {
        if (!_waveIsOver)
        {
            _timer += Time.deltaTime;
            
            if (_waveNumber == 1)
                Wave1();
            if (_waveNumber == 2)
                Wave2();
        }
    }

    private void Wave1()
    {
        const int maxEnemies = 5;
        const float spawnTime = 5f;
        
        if (_timer >= spawnTime && _enemySpawned < maxEnemies)
        {
            _timer = 0f;

            Bounds bounds = _spawnArea.bounds;
            float px = Random.Range(bounds.min.x, bounds.max.x);
            float pz = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 spawnPoint = new Vector3(px, 0f, pz);

            GameObject enemy = Instantiate(_enemyPrefab, transform);
            enemy.transform.position = spawnPoint;
            _enemySpawned++;
        }

        if (_waveKillCounter == maxEnemies 
            && _enemySpawned == maxEnemies
            && !_waveIsOver)
        {
            _waveIsOver = true;
            Invoke("StartNextWave", 7f);
        }
    }
    
    private void Wave2()
    {
        const int maxEnemies = 10;
        const float spawnTime = 3f;
        
        if (_timer >= spawnTime && _enemySpawned < maxEnemies)
        {
            _timer = 0f;

            Bounds bounds = _spawnArea.bounds;
            float px = Random.Range(bounds.min.x, bounds.max.x);
            float pz = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 spawnPoint = new Vector3(px, 0f, pz);

            GameObject enemy = Instantiate(_enemyPrefab, transform);
            enemy.transform.position = spawnPoint;
            _enemySpawned++;
        }

        if (_waveKillCounter == maxEnemies 
            && _enemySpawned == maxEnemies
            && !_waveIsOver)
        {
            _waveIsOver = true;
            Invoke("StartNextWave", 7f);
        }
    }

    private void StartNextWave()
    {
        _waveNumber++;
        _waveKillCounter = 0;
        _enemySpawned = 0;
        _timer = 0f;
        _waveIsOver = false;
    }
    
    public void AddKill()
    {
        _waveKillCounter++;
        _gameManager.AddKill();
    }
}
