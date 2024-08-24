using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.ComponentModel;
using System.Linq;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    [Header("UI")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private float pauseAlpha;
    private bool paused;

    [SerializeField] private GameObject optionScreen;
    [SerializeField] private Image optionImage;
    
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject contButton;

    [Header("End")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI endTime;
    [SerializeField] private TextMeshProUGUI endTimeBest;


    private void Awake()
    {
        if (PlayerPrefs.GetFloat("timeSoFar") <= 0)
        {
            contButton.SetActive(false);
        }

        GM = this;
    }

    private void Start()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Quit();
            }
            else
            {
                if (paused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }


    public void StartGame()
    {
        Time.timeScale = 1;
        
        Ball.ball.transform.position = Ball.ball.RespawnPos();

        //vcam.PreviousStateIsValid = false;
    }

    public void Continue()
    {
        Time.timeScale = 1;

        Ball.ball.transform.position = new Vector3(
            PlayerPrefs.GetFloat("ballPosX"),
            PlayerPrefs.GetFloat("ballPosY"),
            0);

        //vcam.PreviousStateIsValid = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        
        pauseScreen.SetActive(true);
        
        paused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;

        pauseScreen.SetActive(false);
        optionScreen.SetActive(false);

        paused = false;
    }

    public void OptionColour()
    {
        if (paused)
        {
            optionImage.color = new Color(0, 0, 0, pauseAlpha/255f);
        }
        else
        {
            optionImage.color = new Color(0, 0, 0, 1);
        }
    }

    public void BackFromOption()
    {
        if (paused)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            startScreen.SetActive(true);
        }
    }

    public void Restart()
    {
        if (endScreen.activeSelf)
        {
            PlayerPrefs.SetFloat("ballPosX", 0f);
            PlayerPrefs.SetFloat("ballPosY", 0f);

            PlayerPrefs.SetFloat("timeSoFar", 0f);
        }
        else
        {
            PlayerPrefs.SetFloat("ballPosX", Ball.ball.transform.position.x);
            PlayerPrefs.SetFloat("ballPosY", Ball.ball.transform.position.y);

            PlayerPrefs.SetFloat("timeSoFar", PlayerPrefs.GetFloat("timeSoFar") + Time.timeSinceLevelLoad);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void End()
    {
        if (!endScreen.activeSelf) return;

        Ball.ball.Rigidbody().velocity = Vector2.zero;

        float time = Time.timeSinceLevelLoad + PlayerPrefs.GetFloat("timeSoFar");
        endTime.text = "Your time was: " + FormatTime(time);

        float bestTime = PlayerPrefs.GetFloat("bestTime");

        if (bestTime == 0)
        {
            endTimeBest.text = "Good job!";
            PlayerPrefs.SetFloat("bestTime", time);
        }
        else if (bestTime < time)
        {
            endTimeBest.text = "Your best time is: " + FormatTime(bestTime) + "\nBetter luck next time!";
        }
        else
        {
            endTimeBest.text = "You beat your best time of " + FormatTime(bestTime) + "\nGood job!";
            PlayerPrefs.SetFloat("bestTime", time);
        }

        endScreen.SetActive(true);
    }

    private string FormatTime(float time)
    {
        Debug.Log(time);
        int minutes = (int) Math.Floor(time / 60);
        Debug.Log(minutes);
        int seconds = (int) Math.Floor(time - (60 * minutes));

        if (seconds < 10)
        {
            string secStr = "0" + seconds;
            return $"{minutes}:{secStr}";
        }

        Debug.Log(seconds);

        return $"{minutes}:{seconds}";
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
