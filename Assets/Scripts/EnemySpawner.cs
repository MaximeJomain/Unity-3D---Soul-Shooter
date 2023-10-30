using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private MeshCollider _spawnArea;

    private float _timer;

    private void Start()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 5f)
        {
            _timer = 0f;
            
            Bounds bounds = _spawnArea.bounds;
            float px = Random.Range(bounds.min.x, bounds.max.x);
            float pz = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 spawnPoint = new Vector3(px, 0f, pz);
            
            GameObject enemy = Instantiate(_enemyPrefab, transform);
            enemy.transform.position = spawnPoint;
        }
    }
    
}
