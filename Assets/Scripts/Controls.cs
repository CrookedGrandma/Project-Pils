using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {


    public float Vel = 0.2f;
    public float JumpSpeed = 2f;
    private bool IsPaused = false;
    public Canvas canvas;

    // Use this for initialization
    void Start () {
        canvas.enabled = false;
    }
	
	// Update is called once per frame
	public void Update () {

        if (!IsPaused)
        {
            if (Input.GetKey(KeyCode.A))
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
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position += new Vector3(0, JumpSpeed, 0);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vel = 0.3f;
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Vel = 0.2f;
            }
        }

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
