using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float springForce;

    public float ballRadius;
    public LayerMask ballLayer;

    public float minScale;
    public float ballOnScale;
    public float decreaseSpeed;

    private Vector3 scale;
    private Vector3 pos;
    private Vector3 minPos;
    private float minBallDist;

    private bool shrink;
    public bool release;

    public BoxCollider2D col;
    public float waitTime;

    public AudioSource springAudio;

    void Start()
    {
        scale = transform.localScale;
        pos = transform.position;
        minPos = pos - (transform.up * ((1 - minScale) * 0.5f * scale.y));
        minBallDist = (minScale * scale.y * 0.5f) + ballRadius;
    }

    void Update()
    {
        if (Input.GetButton("FlipLeft") && Input.GetButton("FlipRight"))
        {
            shrink = true;
        }
            
        if (shrink)
        {
            if (!(Input.GetButton("FlipLeft") && Input.GetButton("FlipRight")))
            {
                release = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (shrink)
        {
            transform.localScale = new Vector3(scale.x, Mathf.MoveTowards(transform.localScale.y, minScale * scale.y, decreaseSpeed * Time.deltaTime), scale.z);
            transform.position = pos - transform.up * (scale.y - transform.localScale.y) / 2;

            if (release)
            {
                shrink = false;
                Fire();
            }
        }
    }

    void Fire()
    {
        if (Physics2D.OverlapBox(pos, scale * 0.9f, transform.eulerAngles.z, ballLayer))
        {
            float magnitude = minBallDist / (Ball.ball.transform.position - minPos).magnitude;
            Debug.Log("magnitude =" +magnitude);

            Ball.ball.transform.position = pos + transform.up * scale.y;
            Ball.ball.rb.velocity = Vector2.zero;
            Ball.ball.rb.AddForce(magnitude * springForce * transform.up, ForceMode2D.Impulse);
            springAudio.Play();
        }

        col.enabled = false;
        StartCoroutine(ActivateCol());

        transform.localScale = scale;
        transform.position = pos;

        release = false;
    }

    IEnumerator ActivateCol()
    {
        yield return new WaitForSeconds(waitTime);
        col.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, scale);
        Gizmos.DrawSphere(minPos, 0.1f);
    }
}
