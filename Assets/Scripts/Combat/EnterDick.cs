using UnityEngine;
using System.Collections;

public class EnterDick : MonoBehaviour {

    bool hasMoved = false;

	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(750f, 0f);
	}

    // Update is called once per frame
    void Update() {
        if (!hasMoved) {
            if (this.transform.position.x / Screen.width * 16 >= 1.7429193899782135076252723311547f) {
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                hasMoved = true;
            }
        }
        else {
            this.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * -1000));
        }
    }
}
