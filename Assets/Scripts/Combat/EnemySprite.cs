using UnityEngine;
using UnityEngine.UI;

public class EnemySprite : MonoBehaviour {

    public int ID = -1;
    public bool pleaseEnter = false;
    bool hasStarted = false;

    public EnemyChooser enemyChooser;
    public Enemy linkedEnemy = null;
    
	// Use this for initialization
	void Start() {
        linkedEnemy = enemyChooser.EnemyDatabase[ID];
    }
	
	// Update is called once per frame
	void Update () {
        if (pleaseEnter) {
            EnterScreen();
        }
        transform.localScale = new Vector3(Mathf.PingPong(Time.time / 6, 0.2f) + 1, Mathf.PingPong(Time.time / 3, 0.2f) + 1, Mathf.PingPong(Time.time / 3, 0.2f) + 1);
    }

    public void opacity(float alpha) {
        Color tmp = this.GetComponent<Image>().color;
        tmp.a = alpha;
        this.GetComponent<Image>().color = tmp;
        print("Opacity of sprite " + linkedEnemy.Title + ", ID " + ID + ", to " + alpha*100 + "%");
    }

    public void EnterScreen() {
        if (!hasStarted) {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-750f, 0f);
            hasStarted = true;
        }
        else if ((this.transform.position.x / Screen.width * 16) <= 14.257080610021786492374727668845) {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            pleaseEnter = false;
        }
    }
}
