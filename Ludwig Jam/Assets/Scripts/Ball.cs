using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Ball : MonoBehaviour
{
    public static Ball ball;

    [Header("Stats")]
    [SerializeField] private float bounceForce;
    [SerializeField] private Vector2 respawnPos = new Vector2(8.375f, 0f);
    public Vector2 RespawnPos() { return respawnPos; }

    private bool bounced;

    [Header("References")]
    [SerializeField] private AudioSource bounce;
    
    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody() {
        return rb;
    }

    [Header("Camera")]
    private CinemachineBrain cBrain;
    private CinemachineBlendDefinition defaultBlend;

    private void Awake() {
        ball = this;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        cBrain = Camera.main.GetComponent<CinemachineBrain>();
        defaultBlend = cBrain.m_DefaultBlend;
    }

    private void Kill() {
        cBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0);

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = respawnPos;

        Invoke(nameof(ResetDefaultBlend), 1f);
    }

    private void ResetDefaultBlend() {
        cBrain.m_DefaultBlend = defaultBlend;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("kill")) {
            Kill();
        } else if (collision.gameObject.CompareTag("bouncy") && !bounced) {
            bounce.Play();

            ContactPoint2D contact = collision.GetContact(0);
            rb.AddForceAtPosition(contact.normal * bounceForce, contact.point, ForceMode2D.Impulse);
            
            Debug.Log("bounced");
            bounced = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("bouncy")) {
            bounced = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("end")) {
            GameManager.GM.End();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(respawnPos, 0.5f);
    }
}
