using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon_Enemy : MonoBehaviour
{
    public float velocity = 750f;
    public Animator animator;

    private enum WalkDirection { Up, Down, Left, Right }
    private Rigidbody enemyRB;
    private GameObject player;
    private PlayerFSM playerFSM;
    private DungeonCreator dungeonCreator;
    private float xVelocity, zVelocity;
    private WalkDirection direction;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerFSM = GameObject.FindObjectOfType<PlayerFSM>();
        dungeonCreator = GameObject.FindObjectOfType<DungeonCreator>();

        direction = (WalkDirection)Random.Range(0, 4);
    }

    private void Update()
    {
        if (direction == WalkDirection.Up)
        {
            xVelocity = 0;
            zVelocity = velocity;
        }
        else if (direction == WalkDirection.Down)
        {
            xVelocity = 0;
            zVelocity = -velocity;
        }
        else if (direction == WalkDirection.Left)
        {
            zVelocity = 0;
            xVelocity = -velocity;
        }
        else if (direction == WalkDirection.Right)
        {
            zVelocity = 0;
            xVelocity = velocity;
        }

        enemyRB.velocity = new Vector3(xVelocity * Time.deltaTime, 0, zVelocity * Time.deltaTime);

        // Animations
        if (enemyRB.velocity.z > 0)
        {
            animator.Play("EnemyUp");
        }
        else if (enemyRB.velocity.z < 0)
        {
            animator.Play("EnemyDown");
        }
        else if (enemyRB.velocity.x > 0)
        {
            animator.Play("EnemyRight");
        }
        else if (enemyRB.velocity.x < 0)
        {
            animator.Play("EnemyLeft");
        }
        else
        {
            animator.Play("EnemyIdle");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        var normal = collision.contacts[0].normal;
        if (collision.gameObject.name != "Player" && normal.y <= 0)
        {
            // This only happens when the enemy does not collide with the player and not with the floor.
            direction = (WalkDirection)((int)(direction + Random.Range(1, 4)) % 4); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // Enter combat scene
            Destroy(gameObject);
            dungeonCreator.SaveLayoutOfEnemies();
            PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
            PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
            SceneManager.LoadScene("Combat");
            player.transform.position = PlayerPrefsManager.GetPositionInLevel("Combat", player);

            if (SceneManager.GetActiveScene().name == "Dungeon_FaceBeer")
            {
                playerFSM.Enemy = 4;
                playerFSM.Envi = 0;
            }
            else if (SceneManager.GetActiveScene().name == "Dungeon_PiPi")
            {
                // playerFSM.Enemy = 3;         <-- Dit is degene die het moet worden als ie een sprite heeft
                playerFSM.Enemy = 4;            // <-- Deze is van FaceBeer
                playerFSM.Envi = 2;
            }
        }
    }
}
