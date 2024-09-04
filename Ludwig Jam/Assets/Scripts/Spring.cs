using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {

    [Header("Values")]
    [SerializeField] private float springForce = 30.0f;
    [SerializeField] private float ballRadius = 0.2f;
    [SerializeField] private float checkOverflow = 0.1f;
    [SerializeField] private float checkShrink = 0.8f;
    private Vector3 checkSize;

    private float magnitude;
    private float maxScale;
    private const float minScale = 0.5f;
    
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform ballPos;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private LayerMask ballLayer;
    [SerializeField] private AudioSource springAudio;

    private Transform ballParent;

    private bool released;
    private bool held;
    private bool ballContact;


    private void Start() {
        checkSize = new Vector2(collider.transform.localScale.x * checkShrink, GetScale() + checkOverflow);
        maxScale = GetScale();

        ballParent = Ball.ball.transform.parent;
    }

    void Update() {
        ballContact = CheckBall();
    }

    void FixedUpdate() {
        if (ballContact) {
            if (held || released) {
                Ball.ball.transform.SetParent(ballPos);
            }

            if (released && GetScale() == maxScale) {
                Fire();
            }
        } else if (released) {
            released = false;
        }
    }

    public void StartPull() {
        if (anim.GetBool("pulling")) return; //do nothing if spring is already being pulled back
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Static")) { //if animator is not being pulled or released
            anim.SetBool("pulling", true); //start pulling
        }

        held = true;
    }

    public void Release() {
        if (!anim.GetBool("pulling")) return; //do nothing if not pulling
        anim.SetBool("pulling", false); //start releasing
         
        magnitude = Mathf.Abs(maxScale - GetScale()) / (maxScale - minScale); //calculate distance pulled back

        if (ballContact) {
            springAudio.Play(); //play release audio
        }

        held = false;
        released = true;
    }

    void Fire() {
        Ball.ball.transform.SetParent(ballParent);

        Ball.ball.Rigidbody()
            .AddForce(magnitude * springForce * transform.up, ForceMode2D.Impulse);

        released = false;
    }

    float GetScale() {
        return collider.transform.localScale.y;
    }

    bool CheckBall() {
        return Physics2D.OverlapBox(
            transform.position + ((checkOverflow / 2.0f) * transform.up),
            checkSize,
            transform.eulerAngles.z,
            ballLayer);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ballPos.position, ballRadius);
        Gizmos.DrawCube(
            transform.position + ((checkOverflow / 2.0f) * transform.up),
            checkSize
        );
    }
}
