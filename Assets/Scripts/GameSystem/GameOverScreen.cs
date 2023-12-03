using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _gameOverText, _highscoreText;

    [SerializeField]
    private GameObject _restartButton, _quitButton, _eventSystem;

    private float _timer;
    private float _timeAlpha;
    private int _highscore;

    private void OnEnable()
    {
        _gameOverText.gameObject.SetActive(false);
        _highscoreText.gameObject.SetActive(false);
        _restartButton.SetActive(false);
        _quitButton.SetActive(false);
        _eventSystem.SetActive(false);
        _timer = 0f;
        _timeAlpha = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        DisplayText();
        DisplayButtons();
    }

    private void DisplayText()
    {
        if (!_gameOverText.gameObject.activeInHierarchy)
        {
            Color startColor = new Color(_gameOverText.color.r, _gameOverText.color.g, _gameOverText.color.b, 0f);
            _gameOverText.color = startColor;
            _gameOverText.gameObject.SetActive(true);
        }

        if (_gameOverText.color.a < 1f)
        {
            _timeAlpha += Time.deltaTime / 3f;
            Color color = new Color(_gameOverText.color.r, _gameOverText.color.g, _gameOverText.color.b, _timeAlpha);
            _gameOverText.color = color;
        }
        
    }

    private void DisplayButtons()
    {
        if (_timer > 3f)
        {
            _highscoreText.text = "You survived " + _highscore + " waves";
            
            _eventSystem.SetActive(true);
            _highscoreText.gameObject.SetActive(true);
            _restartButton.SetActive(true);
            _quitButton.SetActive(true);
        }
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void SetHighscore(int value)
    {
        _highscore = value;
    }
}
