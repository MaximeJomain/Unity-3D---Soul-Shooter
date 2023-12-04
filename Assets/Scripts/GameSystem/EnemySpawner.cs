using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private List<EnemyCharacter> _enemyList;

    private int _waveValue;

    [SerializeField]
    private MeshCollider _spawnArea;

    private float _timer;
    private int _enemySpawned, _waveKillCounter, _waveNumber;
    public int WaveNumber => _waveNumber;

    private bool _waveIsOver;
    
    private List<EnemyCharacter> enemiesToSpawn = new List<EnemyCharacter>();

    private int _waveSize;

    private float _healthMultiplier;

    private float _spawnTime;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        _timer = 0f;
        _spawnTime = 5f;
        _enemySpawned = 0;
        _waveKillCounter = 0;
        _waveNumber = 1;
        _healthMultiplier = 1;
        _waveValue = _waveNumber * 50;
        GenerateWave();
        _waveIsOver = false;
    }
    
    private void Update()
    {
        if (!_waveIsOver)
        {
            _timer += Time.deltaTime;
            
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        if (enemiesToSpawn.Count > 0)
        {
            if (_timer >= _spawnTime)
            {
                _timer = 0f;

                Bounds bounds = _spawnArea.bounds;
                float px = Random.Range(bounds.min.x, bounds.max.x);
                float pz = Random.Range(bounds.min.z, bounds.max.z);
                Vector3 spawnPoint = new Vector3(px, 0f, pz);

                GameObject enemy = Instantiate(enemiesToSpawn[0].gameObject, transform);
                enemy.GetComponent<Character>().Health *= _healthMultiplier;
                
                enemiesToSpawn.RemoveAt(0);
                
                enemy.transform.position = spawnPoint;
                _enemySpawned++;
            }
        }

        if (_waveKillCounter == _waveSize)
        {
            _waveIsOver = true;
            Invoke("StartNextWave", 7f);
        }
    }
    
    // private void Wave2()
    // {
    //     const int maxEnemies = 1;
    //     const float spawnTime = 3f;
    //     
    //     if (_timer >= spawnTime && _enemySpawned < maxEnemies)
    //     {
    //         _timer = 0f;
    //
    //         Bounds bounds = _spawnArea.bounds;
    //         float px = Random.Range(bounds.min.x, bounds.max.x);
    //         float pz = Random.Range(bounds.min.z, bounds.max.z);
    //         Vector3 spawnPoint = new Vector3(px, 0f, pz);
    //
    //         GameObject enemy = Instantiate(_enemyPrefab, transform);
    //         enemy.transform.position = spawnPoint;
    //         _enemySpawned++;
    //     }
    //
    //     if (_waveKillCounter == maxEnemies 
    //         && _enemySpawned == maxEnemies
    //         && !_waveIsOver)
    //     {
    //         _waveIsOver = true;
    //         Invoke("StartNextWave", 7f);
    //     }
    // }

    private void StartNextWave()
    {
        _waveNumber++;
        _waveKillCounter = 0;
        _enemySpawned = 0;
        _timer = 0f;
        _healthMultiplier *= 1.2f;
        if (_spawnTime > 1.5f)
        {
            _spawnTime -= 0.3f;
        }
        _waveValue = _waveNumber * 50;
        GenerateWave();
        _waveIsOver = false;
        _gameManager.AddWave();
    }
    
    private void GenerateWave()
    {
        List<EnemyCharacter> generatedEnemies = new List<EnemyCharacter>();
        while(_waveValue>0)
        {
            int randEnemyId = Random.Range(0, _enemyList.Count);
            int randEnemyCost = _enemyList[randEnemyId].SpawnCost;
 
            if(_waveValue-randEnemyCost>=0)
            {
                generatedEnemies.Add(_enemyList[randEnemyId]);
                _waveValue -= randEnemyCost;
            }
            else if(_waveValue<=0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
        _waveSize = enemiesToSpawn.Count;
    }
    
    public void AddKill()
    {
        _waveKillCounter++;
        _gameManager.AddKill();
    }
}
