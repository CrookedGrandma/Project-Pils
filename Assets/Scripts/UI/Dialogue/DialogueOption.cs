using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueOption : Entity {

    public string identifier;
    public string optionText;
    public string response;
    public string[] resultingIdentifiers;

    public Text buttonText;

    public void Start()
    {
        SetOption();
    }

    public DialogueOption(string id, string txt, string resp, string[] resultingIds)
    {
        identifier = id;
        optionText = txt;
        response = resp;
        resultingIdentifiers = resultingIds;
    }

    public void SetOption()
    {
        buttonText.text = optionText;
    }

    public void AssignButton(Button button)
    {
        buttonText = button.GetComponentInChildren<Text>();
        button.onClick.AddListener(() => HandleClick());
    }

    public void HandleClick()
    {
        Message dia = new Message(this, GameManager.instance.dialogueManager, MsgType.Dialogue, response);
        Message m = new Message(this, GameManager.instance.dialogueManager, MsgType.DialogueResponse, resultingIdentifiers);

        GameManager.instance.messageQueue.Add(dia);
        GameManager.instance.messageQueue.Add(m);
     
    }

    

}
