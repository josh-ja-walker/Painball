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
    private Vector2 ballPos;

    public GameObject endScreen;
    public TextMeshProUGUI endTime;

    public AudioSource bounce;

    private void Awake()
    {
        ball = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballPos = transform.position;
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
        endTime.text = "Your time was: " + Math.Round(Time.timeSinceLevelLoad/60f, 2) + " minutes\n Well done!!!";
        endScreen.SetActive(true);
    }

    private void Kill()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = ballPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("end") && !endScreen.activeSelf)
        {
            End();
        }
        else if (collision.gameObject.CompareTag("kill"))
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("confiner"))
        {
            Kill();
        }
    }
}
