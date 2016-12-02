using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelVisible : MonoBehaviour {

    public Image panel;
    public bool IsVisible = false;

    // Use this for initialization
    void Start () {
        panel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)&& IsVisible)
        {
            panel.gameObject.SetActive(false);
            IsVisible = false;
        }
    }

    public void SetVisible()
    {
        panel.gameObject.SetActive(true);
        IsVisible = true;
    }
    public void SetInvisible()
    {
        print("WE SWITCHEN HOOR");
        panel.gameObject.SetActive(false);
        IsVisible = false;
    }
}
