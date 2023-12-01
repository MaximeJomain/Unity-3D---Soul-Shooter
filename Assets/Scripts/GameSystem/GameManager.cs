using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _killCounter;
    private float _elapsedTime;
    private TMP_Text _TMP_timer, _TMP_wave;
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _TMP_timer = GameObject.Find("/Canvas/Timer").GetComponent<TMP_Text>();
        _TMP_wave = GameObject.Find("/Canvas/Wave").GetComponent<TMP_Text>();
        _enemySpawner = GameObject.Find("/Enemy Spawner").GetComponent<EnemySpawner>();
    }

    private void Start()
    {
        _killCounter = 0;
        _elapsedTime = 0f;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        handleGUI();
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

    public void AddKill()
    {
        _killCounter++;
    }
}
