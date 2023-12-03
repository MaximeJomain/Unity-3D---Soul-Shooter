using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _killCounter;
    private float _elapsedTime;
    private TMP_Text _TMP_timer, _TMP_wave;
    private EnemySpawner _enemySpawner;
    private GameObject _handGun;

    private void Awake()
    {
        _TMP_timer = GameObject.Find("/Canvas/Timer").GetComponent<TMP_Text>();
        _TMP_wave = GameObject.Find("/Canvas/Wave").GetComponent<TMP_Text>();
        _enemySpawner = GameObject.Find("/Enemy Spawner").GetComponent<EnemySpawner>();
        _handGun = GameObject.Find("/HandGun");
    }

    private void Start()
    {
        // _handGun.SetActive(false);
        _killCounter = 0;
        _elapsedTime = 0f;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        handleGUI();
        handleWeaponSpawn();
    }

    private void handleGUI()
    {
        // update Timer
        int minutes = Mathf.FloorToInt(_elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(_elapsedTime - minutes * 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        _TMP_timer.text = formattedTime;
        
        //update Wave Counter
        string formattedText = $"Wave {_enemySpawner.WaveNumber}";
        _TMP_wave.text = formattedText;
    }
    
    private void handleWeaponSpawn()
    {
        if (_enemySpawner.WaveNumber == 2)
        {
            _handGun.SetActive(true);
        }
    }

    public void AddKill()
    {
        _killCounter++;
    }
}
