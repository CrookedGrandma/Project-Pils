using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Core.FSM;

public class PlayerFSM : Entity {

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
    private int enemyID = -1;
    private int envID = -1;
    private bool pleaseDie = false;

    // Awake
    void Awake() {
        if (player != null) {
            Destroy(gameObject);
        }
        else {
            player = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        fsm = new Core.FSM.FSM("PlayerFSM");
        moveState = fsm.AddState("MoveState");
        idleState = fsm.AddState("IdleState");
        moveAction = new MoveAction(moveState);
        idleAction = new IdleAction(idleState);

        moveState.AddAction(moveAction);
        idleState.AddAction(idleAction);

        PlayerRB = gameObject.GetComponent<Rigidbody>();

        moveAction.Init(gameObject.transform, PlayerRB, velocity, sprintVelocity, jumpSpeed, "ToIdle");
        idleAction.Init();

        idleState.AddTransition("ToMove", moveState);
        moveState.AddTransition("ToIdle", idleState);

        fsm.Start("IdleState");
    }

    // Update is called once per frame
    void Update() {
        if (!GameManager.instance.IsPaused) {
            fsm.Update();
        }
        if (enemyID != -1) {
            Debug.Log("Enemy Plate: " + enemyID);
        }
        if (envID != -1) {
            Debug.Log("Environment Plate: " + envID);
        }
        if (pleaseDie) {
            Destroy(gameObject);
        }

        // Go to combat scene, purely for developing {
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            PlayerFSM.player.Enemy = 0;
            PlayerFSM.player.Envi = 3;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            PlayerFSM.player.Enemy = 1;
            PlayerFSM.player.Envi = 2;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            PlayerFSM.player.Enemy = 2;
            PlayerFSM.player.Envi = 0;
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            SceneManager.LoadScene("Combat");
        }
    }

    public override void onMessage(Message m) {
        base.onMessage(m);

        switch (m.type) {
            case MsgType.Dialogue:
            GameManager.instance.dialogueManager.AddLine(m.from.name, m.data.ToString(), "red");
            break;
        }
    }

    private void OnCollisionStay(Collision collision) {
        // Determine if we can jump
        var normal = collision.contacts[0].normal;
        if (normal.y > 0) {
            // Hit Bottom
            moveAction.ableToJump = true;
        }
    }

    private void OnCollisionExit() {
        // When we exit a collision (the collision with the ground), we are not allowed to jump. This is to prevent spamming the jump button to create a rocket.
        moveAction.ableToJump = false;
    }

    public int Enemy {
        get { return enemyID; }
        set { enemyID = value; }
    }

    public int Envi {
        get { return envID; }
        set { envID = value; }
    }

    public bool PleaseDie {
        get { return pleaseDie; }
        set { pleaseDie = value; }
    }

}
