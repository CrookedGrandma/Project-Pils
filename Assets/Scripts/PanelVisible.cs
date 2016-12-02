using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelVisible : MonoBehaviour {

    public Image panel;
    bool IsVisible = false;

    // Use this for initialization
    void Start () {
        panel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    public void VisibleSwitch()
    {
        if (IsVisible)
        {
            panel.gameObject.SetActive(false);
            IsVisible = false;
        }
        if (!IsVisible)
        {
            panel.gameObject.SetActive(true);
            IsVisible = true;

        }
    }
}
