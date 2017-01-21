using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour {

    public Image cutscenePanel;
    public Text cutsceneText;
        
    public void Start()
    {
        cutscenePanel.CrossFadeAlpha(0.0f, 0.0f, false);
        cutsceneText.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    public void SetText(string s)
    {
        cutsceneText.text = s;
    }

    public void FadeInPanel()
    {
        cutscenePanel.CrossFadeAlpha(1.0f, 0.5f, false);

        Invoke("FadeInText", 0.30f);

        Invoke("FadeOutPanel", 2.5f);
    }

    public void FadeOutPanel()
    {
        cutsceneText.CrossFadeAlpha(0.0f, 0.5f, false);
        cutscenePanel.CrossFadeAlpha(0.0f, 0.5f, false);
    }

    public void FadeInText()
    {
        cutsceneText.CrossFadeAlpha(1.0f, 0.5f, false);
    }

	

}
