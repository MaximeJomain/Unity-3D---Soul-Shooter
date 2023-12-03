using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _killCounter;
    private float _elapsedTime;
    private TMP_Text _TMP_timer, _TMP_wave;
    private EnemySpawner _enemySpawner;
    private HandgunWeapon _handGun;
    private PlayerCharacter _playerCharacter;
    private int _highScore;
    
    [SerializeField]
    private GameOverScreen _gameOverScreen;
    private bool _isGameOver;
    private RifleWeapon _rifle;

    private void Awake()
    {
        _TMP_timer = GameObject.Find("/Canvas/Timer").GetComponent<TMP_Text>();
        _TMP_wave = GameObject.Find("/Canvas/Wave").GetComponent<TMP_Text>();
        _enemySpawner = GameObject.Find("/Enemy Spawner").GetComponent<EnemySpawner>();
        _handGun = GameObject.Find("/HandGun").GetComponent<HandgunWeapon>();
        _rifle = GameObject.Find("/M4A1").GetComponent<RifleWeapon>();
        _playerCharacter = GameObject.Find("/Paladin Player").GetComponent<PlayerCharacter>();
        // _gameOverScreen = GameObject.Find("/Canvas/GameOverScreen").GetComponent<GameOverScreen>();
    }

    private void Start()
    {
        _highScore = PlayerPrefs.GetInt("High-score", 0);
        
        _handGun.gameObject.SetActive(false);
        _rifle.gameObject.SetActive(false);
        if (_gameOverScreen.gameObject.activeInHierarchy)
        {
            _gameOverScreen.gameObject.SetActive(false);
        }
        _killCounter = 0;
        _elapsedTime = 0f;
        _isGameOver = false;
    }

    private void Update()
    {
        if (!_isGameOver)
            _elapsedTime += Time.deltaTime;

        handleGUI();
        handleWeaponSpawn();

        if (!_playerCharacter.IsAlive && !_isGameOver)
        {
            GameOver();
        }
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
        if (_enemySpawner.WaveNumber == 2 && !_handGun.IsEquipped)
        {
            _handGun.gameObject.SetActive(true);
        }
        
        if (_enemySpawner.WaveNumber == 4 && !_rifle.IsEquipped)
        {
            _rifle.gameObject.SetActive(true);
        }
    }

    private void GameOver()
    {
        _isGameOver = true;
        int finalScore = _enemySpawner.WaveNumber;
        
        _gameOverScreen.SetHighscore(finalScore);

        if (finalScore > _highScore)
        {
            PlayerPrefs.SetInt("High-score", finalScore);
            PlayerPrefs.Save();
        }
        
        if (!_gameOverScreen.gameObject.activeInHierarchy)
        {
            _gameOverScreen.gameObject.SetActive(true);
        }
    }

    public void AddKill()
    {
        _killCounter++;
    }

    public void AddWave()
    {
        _playerCharacter.Health = 200f;
    }
}
