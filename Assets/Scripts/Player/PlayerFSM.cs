﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Core.FSM;

public class PlayerFSM : Entity
{

    public static PlayerFSM player;
    public Rigidbody PlayerRB;
    public float velocity = 20f;
    public float sprintVelocity = 30f;
    public float jumpSpeed = 3000f;

    private FSM fsm;
    private FSMState moveState;
    private FSMState idleState;
    private MoveAction moveAction;
    private IdleAction idleAction;
    private GameObject playerObj;
    private int enemyID = -1;
    private int envID = -1;
    private float health = 100f;
    private float maxHealth = 100f;

    public Animator animator;
  

    // Awake
    void Awake()
    {
        if (player != null)
        {
            Destroy(gameObject);
        }
        else
        {
            player = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        fsm = new Core.FSM.FSM("PlayerFSM");
        moveState = fsm.AddState("MoveState");
        idleState = fsm.AddState("IdleState");
        moveAction = new MoveAction(moveState);
        idleAction = new IdleAction(idleState);

        moveState.AddAction(moveAction);
        idleState.AddAction(idleAction);

        PlayerRB = gameObject.GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");

        moveAction.Init(gameObject.transform, PlayerRB, velocity, sprintVelocity, jumpSpeed, "ToIdle");
        idleAction.Init();

        idleState.AddTransition("ToMove", moveState);
        moveState.AddTransition("ToIdle", idleState);

        fsm.Start("IdleState");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance)
        {
            if (!GameManager.instance.IsPaused)
            {
                fsm.Update();
            }
        }
        else
        {
            fsm.Update();
        }
        if (enemyID != -1)
        {
            Debug.Log("Enemy: " + enemyID);
        }
        if (envID != -1)
        {
            Debug.Log("Environment: " + envID);
        }

        // Go to combat scene, purely for developing {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Enemy = 0;
            Envi = 3;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Enemy = 1;
            Envi = 2;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Enemy = 2;
            Envi = 0;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4)) 
        {
            Enemy = 4;
            Envi = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            Enemy = 7;
            Envi = 1;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
            PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, playerObj);
            SceneManager.LoadScene("Combat");
        }

        string scene = SceneManager.GetActiveScene().name;
        // Go to the pause menu, if you are not in one of the levels
        if (Input.GetKeyDown(KeyCode.Escape) && scene != "Pause" && scene != "Inventory" && scene != "Shop" && scene != "MainMenu" && scene != "Options" && scene != "Combat")
        {
            if (scene == "Dungeon_PiPi" || scene == "Dungeon_FaceBeer")
            {
                DungeonCreator dungeonCreator = GameObject.FindObjectOfType<DungeonCreator>();
                dungeonCreator.SaveLayoutOfEnemies();
            }

            PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
            PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, playerObj);
            SceneManager.LoadScene("Pause");
        }

        //Animations
        if (PlayerRB.velocity.z > 0 && Input.GetAxis("Vertical") > 0)
        {
            animator.Play("MeUp");
        }
        else if (PlayerRB.velocity.z < 0 && Input.GetAxis("Vertical") < 0)
        {
            animator.Play("MeDown");
        }
        else if (PlayerRB.velocity.x > 0 && Input.GetAxis("Horizontal") > 0)
        {
            animator.Play("MeRight");
        }
        else if (PlayerRB.velocity.x < 0 && Input.GetAxis("Horizontal") < 0)
        {
            animator.Play("MeLeft");
        }
        else
        {
            animator.Play("MeIdle");
        }

    }

    public override void onMessage(Message m)
    {
        base.onMessage(m);

        switch (m.type)
        {
            case MsgType.Dialogue:
                GameManager.instance.dialogueManager.AddLine(m.from.name, m.data.ToString(), "red");
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Determine if we can jump
        var normal = collision.contacts[0].normal;
        if (normal.y > 0)
        {
            // Hit Bottom
            moveAction.ableToJump = true;
            moveAction.ableToMoveBackward = moveAction.ableToMoveForward = moveAction.ableToMoveLeft = moveAction.ableToMoveRight = true;
        }
        if (normal.x > 0)
        {
            //Hit Left
            moveAction.ableToMoveLeft = false;
        }
        if (normal.x < 0)
        {
            //Hit Right
            moveAction.ableToMoveRight = false;
        }
        if (normal.z < 0)
        {
            //Hit Front
            moveAction.ableToMoveForward = false;
        }
        if (normal.z > 0)
        {
            //Hit Back
            moveAction.ableToMoveBackward = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // When we exit a collision (the collision with the ground), we are not allowed to jump. This is to prevent spamming the jump button to create a rocket.
        moveAction.ableToJump = false;
        moveAction.ableToMoveBackward = moveAction.ableToMoveForward = moveAction.ableToMoveLeft = moveAction.ableToMoveRight = true;
    }

    public int Enemy
    {
        get { return enemyID; }
        set { enemyID = value; }
    }

    public int Envi
    {
        get { return envID; }
        set { envID = value; }
    }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
}
