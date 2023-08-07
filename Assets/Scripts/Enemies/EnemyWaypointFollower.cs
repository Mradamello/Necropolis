using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    public int i = 0;
    [SerializeField] private float speed = 2f;
    private PlayerDetector playerDetector;
    private PlayerFollower playerFollower;
    public bool usingEnemyWaypointFollower = true;
    private void Start()
    {
        playerDetector = GetComponent<PlayerDetector>();
        playerFollower = GetComponent<PlayerFollower>();
    }
    private void Update()
    {
        if (playerDetector == null) return;
        if (playerFollower == null) return;
        if (!playerDetector.playerDetected && !playerFollower.usingPlayerFollower)
        {
            usingEnemyWaypointFollower = true;
            if (Vector2.Distance(waypoints[i].transform.position, transform.position) < .1f)
            {
                i++;
                if (i >= waypoints.Length) i = 0;
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[i].transform.position, Time.deltaTime * speed);
        }
        else if (!playerFollower.change && !playerFollower.usingPlayerFollower)
        {
            playerFollower.change = true;
            usingEnemyWaypointFollower = false;
        }
    }
}