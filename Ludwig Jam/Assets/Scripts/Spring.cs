using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float springForce;
    private float magnitude;

    [SerializeField] private float ballRadius;
    [SerializeField] private LayerMask ballLayer;
    [SerializeField] private float checkOverflow;

    private float yPos;
    private float yScale;

    private bool released;

    [SerializeField] private AudioSource springAudio;
    private bool playAudio;
    [SerializeField] private Animator anim;

    void Awake()
    {
        yPos = transform.position.y;
        yScale = transform.localScale.y;
    }

    void FixedUpdate()
    {   
        if (released)
        {
            if (Physics2D.OverlapBox(transform.position + (0.5f * checkOverflow * transform.parent.up),
                transform.localScale + (transform.parent.up * checkOverflow),
                transform.parent.eulerAngles.z, ballLayer))
            {
                Ball.ball.transform.position = transform.GetChild(0).position + (transform.parent.up * ballRadius);
                //Ball.ball.transform.position = transform.parent.GetChild(2).position;//new Vector3(transform.position.x, yPos) + (((yScale / 2) + ballRadius) * transform.up);

                if (playAudio)
                {
                    springAudio.Play(); //play release audio
                    playAudio = false;
                }

                if (transform.localScale.y == yScale)
                {
                    Fire();
                }
            }

            released = transform.localScale.y != yScale;
        }
    }

    public void StartPull()
    {
        if (anim.GetBool("pulling")) return; //do nothing if spring is already being pulled back
       
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Static")) //if animator is not being pulled or released
        {
            anim.SetBool("pulling", true); //start pulling
        }
    }

    public void Release()
    {
        if (!anim.GetBool("pulling")) return; //do nothing if not pulling

        anim.SetBool("pulling", false); //start releasing

        magnitude = Mathf.Abs(yPos - transform.position.y); //calculate distance pulled back

        released = true;
        playAudio = true;
    }

    void Fire()
    {
        Debug.Log("fire");
        Ball.ball.Rigidbody().AddForce(magnitude * springForce * transform.parent.up, ForceMode2D.Impulse);
        released = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3.up * 0.5f * checkOverflow), transform.localScale + (Vector3.up * checkOverflow));
    }
}
