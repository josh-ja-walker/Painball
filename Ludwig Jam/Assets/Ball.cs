using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    public static Ball ball;

    public float bounceForce;
    public Rigidbody2D rb;
    private bool bounced;
    public Vector2 ballPos;

    public GameObject endScreen;
    public TextMeshProUGUI endTime;
    public TextMeshProUGUI endTimeBest;

    public AudioSource bounce;

    private void Awake()
    {
        ball = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Kill();
        }
    }

    void End()
    {
        rb.velocity = Vector2.zero;

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

    private void Kill()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = ballPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("kill"))
        {
            Kill();
        }
        else if (collision.gameObject.CompareTag("bouncy") && !bounced)
        {
            bounce.Play();
            ContactPoint2D contact = collision.GetContact(0);
            rb.AddForceAtPosition(contact.normal * bounceForce, contact.point, ForceMode2D.Impulse);
            Debug.Log("bounced");
            bounced = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bouncy"))
        {
            bounced = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("end") && !endScreen.activeSelf)
        {
            End();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("confiner"))
        {
            Kill();
        }
    }
}
