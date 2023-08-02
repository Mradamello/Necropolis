using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private PlayerDetector playerDetector;
    private GameObject player;
    [SerializeField] private float speed;
    private EnemyWaypointFollower enemyWaypointFollower;
    public bool usingPlayerFollower = false;
    public bool change = false;

    void Start()
    {
        playerDetector = GetComponent<PlayerDetector>();
        enemyWaypointFollower = GetComponent<EnemyWaypointFollower>();
        player = GameObject.Find("Wizard");
    }

    
    void Update()
    {
        if (enemyWaypointFollower == null) return;
        if (playerDetector.playerDetected && !enemyWaypointFollower.usingEnemyWaypointFollower)
        {
            usingPlayerFollower = true;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        } else if (change)
        {
            change = false;
            usingPlayerFollower = false;
            enemyWaypointFollower.i = enemyWaypointFollower.i == 0 ? 1 : 0;
        }
    }
}
