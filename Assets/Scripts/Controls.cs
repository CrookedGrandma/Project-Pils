using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour
{

    public float velocity = 20f;
    public float sprintVelocity = 30f;
    public float jumpSpeed = 3000f;
    private float usedVelocity;
    private bool isPaused = false;
    private bool ableToJump, ableToMoveLeft, ableToMoveRight, ableToMoveForward, ableToMoveBackward;
    public Canvas canvas;
    public Rigidbody PlayerRB;

    // Use this for initialization
    void Start()
    {
        canvas.enabled = false;
        PlayerRB = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    public void Update()
    {
        if (!isPaused)
        {
            // Determine wether we are walking or sprinting
            if (Input.GetAxis("Sprint") <= 0)
            {
                // Walking
                usedVelocity = velocity;
            }
            else
            {
                // Sprinting
                usedVelocity = sprintVelocity;
            }

            // Walking or Sprinting
            float moveLeftRight = Input.GetAxis("Horizontal") * usedVelocity * Time.deltaTime;
            float moveForwardBackward = Input.GetAxis("Vertical") * usedVelocity * Time.deltaTime;
            if (!ableToMoveLeft && moveLeftRight < 0)
            {
                moveLeftRight = 0;
            }
            if (!ableToMoveRight && moveLeftRight > 0)
            {
                moveLeftRight = 0;
            }
            if (!ableToMoveForward && moveForwardBackward > 0)
            {
                moveForwardBackward = 0;
            }
            if (!ableToMoveBackward && moveForwardBackward < 0)
            {
                moveForwardBackward = 0;
            }
            transform.position += new Vector3(moveLeftRight, 0, moveForwardBackward);

            // Jump
            if (Input.GetButtonDown("Jump") && ableToJump)
            {
                PlayerRB.AddForce(Vector3.up * jumpSpeed);
                ableToJump = false;
            }
        }

        // Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                isPaused = false;
                canvas.enabled = false;
            }
            else
            {
                Time.timeScale = 0;
                isPaused = true;
                canvas.enabled = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Determine if we can jump
        var normal = collision.contacts[0].normal;
        if (normal.y > 0)
        {
            //Hit Bottom
            ableToJump = true;
            ableToMoveBackward = ableToMoveForward = ableToMoveLeft = ableToMoveRight = true;
        }
        if (collision.collider.tag != "Terrain") 
        {
            /* 
             * Terrain does not have mass, so there was a NullReferenceException. 
             * The collision with the terrain is already handled just before this.
             */
            if (PlayerRB.mass <= collision.rigidbody.mass)
            {
                if (normal.y < 0)
                {
                    //Hit Roof
                    // Maybe needed in houses or dungeons
                }
                else if (normal.x > 0)
                {
                    //Hit Left
                    ableToMoveLeft = false;
                }
                else if (normal.x < 0)
                {
                    //Hit Right
                    ableToMoveRight = false;
                }
                else if (normal.z < 0)
                {
                    //Hit Front
                    ableToMoveForward = false;
                }
                else if (normal.z > 0)
                {
                    //Hit Back
                    ableToMoveBackward = false;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ableToMoveBackward = ableToMoveForward = ableToMoveLeft = ableToMoveRight = true;
        ableToJump = false;
    }
}
