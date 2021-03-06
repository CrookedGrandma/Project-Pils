﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Cutscene : MonoBehaviour {

    public GameObject main;
    public Image cutscenePanel;
    public Text cutsceneText;

        
    public void Start()
    {
        main = cutscenePanel.gameObject;

        cutscenePanel.CrossFadeAlpha(0.0f, 0.0f, false);
        cutsceneText.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    public void SetText(string s)
    {
        cutsceneText.text = s;
    }

    public void FadeInPanel(bool manualFade = false)
    {
        GameManager.instance.IsPaused = true;
        main.SetActive(true);

        cutscenePanel.CrossFadeAlpha(1.0f, 0.5f, false);

        Invoke("FadeInText", 0.30f);



        if (!manualFade)
        {

            int count = cutsceneText.text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
            float secondsToDisplay = count / 5.5f;
            Invoke("FadeOutPanel", 1 + secondsToDisplay);
        }
    }

    public void FadeOutPanel()
    {
        GameManager.instance.IsPaused = false;

        cutsceneText.CrossFadeAlpha(0.0f, 0.5f, false);
        cutscenePanel.CrossFadeAlpha(0.0f, 0.5f, false);

        Invoke("DisablePanel", 0.5f);
    }

    public void DisablePanel()
    {
        main.SetActive(false);
    }

    public void FadeInText()
    {
        cutsceneText.CrossFadeAlpha(1.0f, 0.5f, false);
    }

	

}
