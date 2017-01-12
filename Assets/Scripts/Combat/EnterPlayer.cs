using UnityEngine;
using System.Collections;

public class EnterPlayer : MonoBehaviour {

    bool hasMoved = false;
    bool enter = false;

    public bool Enter {
        set { enter = value; }
    }

    // Update is called once per frame
    void Update() {
        if (enter) {
            if (!hasMoved) {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(750f, 0f);
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
}
