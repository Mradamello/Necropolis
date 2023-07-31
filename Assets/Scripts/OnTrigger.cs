using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private Text pointsText;
    private Animator anim;
    private int points = 0;
    void Start()
    {
        // shows UI text field as game start
        pointsText.text = "Points: " + points;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // upon entering trigger of cherry collect it, and add 1 point to the counter and update the counter
        if (collision.gameObject.CompareTag("Collectible")) {
            anim = collision.gameObject.GetComponent<Animator>();
            anim.SetBool("isTriggered", true);
            points++;
            pointsText.text = "Points: " + points;
        }
    }
}