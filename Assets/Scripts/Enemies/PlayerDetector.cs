using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool playerDetected { get; private set; }
    public Transform player { get; private set; }
    public string layerToDetect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layerToDetect))
        {
            playerDetected = true;
            player = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(layerToDetect))
        {
            playerDetected = false;
            player = null;
        }
    }
}
