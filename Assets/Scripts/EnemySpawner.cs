using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    private void Start()
    {
        Instantiate(_enemyPrefab, transform.position, transform.rotation);
    }
}
