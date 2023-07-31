using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool playerDetected { get; private set; }
    public Transform player { get; private set; }
    public LayerMask layers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(layers))
        {
            playerDetected = true;
            player = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerDetected = false;
            player = null;
        }
    }

}
