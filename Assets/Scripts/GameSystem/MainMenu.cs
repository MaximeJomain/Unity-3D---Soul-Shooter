using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   [SerializeField]
   private TMP_Text _highscoreText;
   
   private void Start()
   {
      int highScore = PlayerPrefs.GetInt("High-score", 0);
      _highscoreText.text = "Highest wave survived : " + highScore;
   }

   public void PlayGame()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
   }

   public void QuitGame()
   {
      Application.Quit();
   }
}
