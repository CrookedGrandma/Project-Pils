using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour
{
    public float velocity = 5f;
    public float sprintVelocity = 7f;
    public float jumpSpeed = 50f;
    private float usedVelocity;
    private bool ableToJump, ableToMoveLeft, ableToMoveRight, ableToMoveForward, ableToMoveBackward;
    public Rigidbody PlayerRB;

    // Use this for initialization
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
            usedVelocity = velocity;
            
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
        if (collision.rigidbody)
        {
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
