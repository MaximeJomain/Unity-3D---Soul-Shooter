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
            float py = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 spawnPoint = new Vector3(px, py);
            
            GameObject enemy = Instantiate(_enemyPrefab, transform);
            enemy.transform.position = spawnPoint;
        }
    }
    
}
