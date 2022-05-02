using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro; 

public class HUD : MonoBehaviour
{
    [SerializeField] Button playAgainButton;
    [SerializeField] Button quitButton;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText; 
    [SerializeField] GameObject cards;
    private GameManager gameManager;
    private bool songPlayed;
    private float gameEndSoundDuration = 3f;
 
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameManager.GameStarted = false;
        playAgainButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        songPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameEnd();
        DisplayTime(gameManager.Countdown);
        ShowScore(); 
    }

    private void CheckGameEnd()
    {
        if(gameManager.GameOver || gameManager.GameEnd)
        {
            StartCoroutine(GameEndDisplay()); 
        }
    }

    private void DisplayTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        LastSecsSound(minutes, seconds);
        GameOverSound(timer.text);
    }

    private void GameOverSound(string timeText)
    {
        if (!FindObjectOfType<AudioManager>().IsPlaying("game_over") && !songPlayed &&
            timeText == string.Format("{0:00}:{1:00}", 0, 0))
        {
            FindObjectOfType<AudioManager>().Play("game_over");
            songPlayed = true;
        }
    }

    private void LastSecsSound(int min, int sec)
    {
        if (!FindObjectOfType<AudioManager>().IsPlaying("blip") && min == 0
            && sec == 5 && !gameManager.GameEnd && !gameManager.GameOver && gameManager.GameStarted)
        {
            InvokeRepeating("CountDownTick", 0f, 2f);
        }
    }

    private void CountDownTick()
    {
        FindObjectOfType<AudioManager>().Play("blip");

        if (gameManager.GameEnd || gameManager.GameOver)
        {
            FindObjectOfType<AudioManager>().Stop("blip");
            CancelInvoke("CountDownTick");
        }
    }

    private void ShowScore()
    {
        score.text = GameManager.Score.ToString(); 
    }

    public void PlayAgain()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("button_click");

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); 
#endif
    }

    IEnumerator GameEndDisplay()
    {
        yield return new WaitForSeconds(0.5f);

        playAgainButton.gameObject.SetActive(true);

        if (gameManager.GameOver) 
        {
            gameOverText.gameObject.SetActive(true);
        }
        
        if (gameManager.GameEnd && !gameManager.GameOver) 
        {
            if (!FindObjectOfType<AudioManager>().IsPlaying("win_game") && !songPlayed)
            {
                FindObjectOfType<AudioManager>().Play("win_game");
                Invoke("StopWinSound", gameEndSoundDuration); 
            }

            winText.gameObject.SetActive(true); 
        }

        cards.gameObject.SetActive(false);
    }

    private void StopWinSound()
    {
        songPlayed = true;
    }
}
