using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour
{

    public float Vel = 0.2f;
    public float VelMultiplier = 100f;
    public float JumpSpeed = 2f;
    private bool IsPaused = false;
    public Canvas canvas;

    // Use this for initialization
    void Start ()
    {
        canvas.enabled = false;
    }
	
	
	// Update is called once per frame
	public void Update ()
    {
        if (!IsPaused)
        {
            // Walking
            float moveLeftRight = Input.GetAxis("Horizontal") * Vel * VelMultiplier * Time.deltaTime;
            float moveForwardBackward = Input.GetAxis("Vertical") * Vel * VelMultiplier * Time.deltaTime;
            transform.position += new Vector3(moveLeftRight, 0, moveForwardBackward);

            // Jumping
            /*
             * This doesn't work as it is supposed to work. You can keep jumping endlessly by holding space.
             * I've searched for a solution, but haven't found anything that works. 
             * Possible solutions are using the "AddForce" method on the rigidbody, but then there is no movement, so it didn't work.
             * It is still possible to just use the GetKey or GetKeyDown methods, but then you don't use the InputManager at all.
            */
            float jump = Input.GetAxis("Jump") * JumpSpeed * Time.deltaTime;
            transform.position += new Vector3(0, jump, 0);

            // Normal walking
            /*if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-Vel, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(Vel, 0, 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 0, Vel);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, 0, -Vel);
            }*/

            // Jumping
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position += new Vector3(0, JumpSpeed * Time.deltaTime, 0);
            }*/

            // Sprinting
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vel = 0.3f;
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Vel = 0.2f;
            }
        }

        // Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {    
                Time.timeScale = 1;
                IsPaused = false;
                canvas.enabled = false;  
            }
            else
            {
                Time.timeScale = 0;
                IsPaused = true;
                canvas.enabled = true;
            }
        }
    }
}
