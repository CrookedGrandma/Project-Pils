using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueOption : Entity {

    public string identifier;
    public string optionText;
    public string response;
    public string[] resultingIdentifiers;

    public string triggersQuest;
    public string completesQuest;

    public Text buttonText;

    public void Start()
    {
        SetOption();
    }

    public DialogueOption(string id, string txt, string resp, string[] resultingIds, string triggersQuestP = "", string completesQuestP = "")
    {
        identifier = id;
        optionText = txt;
        response = resp;
        resultingIdentifiers = resultingIds;

        triggersQuest = triggersQuestP;
        completesQuest = completesQuestP;
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
        Message dia = new Message(GameManager.instance.questManager, GameManager.instance.dialogueManager, MsgType.Dialogue, response);
        Message m = new Message(GameManager.instance.questManager, GameManager.instance.dialogueManager, MsgType.DialogueResponse, resultingIdentifiers);

        if(triggersQuest != "")
        {
            Message triggerQuest = new Message(GameManager.instance.questManager, GameManager.instance.questManager, MsgType.QuestTrigger, triggersQuest);
            GameManager.instance.messageQueue.Add(triggerQuest);
        }

        if(completesQuest != "")
        {
            Message completeQuest = new Message(GameManager.instance.questManager, GameManager.instance.questManager, MsgType.QuestCompletion, completesQuest);
            GameManager.instance.messageQueue.Add(completeQuest);
        }

        GameManager.instance.messageQueue.Add(dia);
        GameManager.instance.messageQueue.Add(m);
     
    }

    

}
