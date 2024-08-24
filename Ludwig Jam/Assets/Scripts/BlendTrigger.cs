using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class BlendTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    private CinemachineBrain cBrain;

    private void Start()
    {
        cBrain = Camera.main.GetComponent<CinemachineBrain>();
        if (vcam == null && transform.parent.CompareTag("cam parent"))
        {
            vcam = transform.parent.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //collides with blend trigger
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            vcam.Priority++; //increase new priority

            if (cBrain.ActiveVirtualCamera != null)
            {
                cBrain.ActiveVirtualCamera.Priority--; //decrease old priority
            }
        }
    }
}
