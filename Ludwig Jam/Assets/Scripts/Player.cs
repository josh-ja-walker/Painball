using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float flipForce;
    [SerializeField] private float flipGravity;

    [SerializeField] private float checkOverflow;
    [SerializeField] private LayerMask lFlipLayer;
    [SerializeField] private LayerMask rFlipLayer;
    [SerializeField] private LayerMask springLayer;
    
    enum Direction { L, R }

    private bool isLFlip;
    private bool isRFlip;

    private void Update() {
        isLFlip = Input.GetButton("FlipLeft");
        isRFlip = Input.GetButton("FlipRight");
    }

    private void FixedUpdate() {
        if (isLFlip) Flip(Direction.L);
        if (isRFlip) Flip(Direction.R);

        if (isLFlip && isRFlip) {
            PullSpring();
        } else {
            ReleaseSpring();
        }
    }

    private Collider2D[] Check(LayerMask layer) {
        Vector3 vcamPos = Camera.main.transform.position;
        Vector2 corner = new Vector2(
            (Camera.main.orthographicSize * (32f / 9f)) + checkOverflow,
            (Camera.main.orthographicSize * 2) + checkOverflow);

        return Physics2D.OverlapAreaAll(
            (Vector2)vcamPos + corner,
            (Vector2)vcamPos - corner, 
            layer);
    }

    private Collider2D[] Check(LayerMask layer, string tag) {
        return Check(layer)
            .AsEnumerable()
            .Where(col => col.CompareTag(tag))
            .ToArray();
    }

    void PullSpring() {
        foreach (Collider2D col in Check(springLayer)) {
            col.transform.parent.GetComponent<Spring>().StartPull();
        }
    }

    void ReleaseSpring() {
        foreach (Collider2D col in Check(springLayer)) {
            col.transform.parent.GetComponent<Spring>().Release();
        }
    }

    void Flip(Direction dir) {
        foreach (Collider2D col in Check(dir.Equals(Direction.L) ? lFlipLayer : rFlipLayer)) {
            col.attachedRigidbody.AddTorque((dir.Equals(Direction.L) ? 1 : -1) * flipForce);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            Camera.main.transform.position, 
            new Vector2((Camera.main.orthographicSize * (32f / 9f)) + checkOverflow, 
            (Camera.main.orthographicSize * 2) + checkOverflow));
    }
}
