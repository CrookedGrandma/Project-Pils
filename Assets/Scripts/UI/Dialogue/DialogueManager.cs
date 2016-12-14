﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManager : Entity {

    public TextBox textBox;
    public Dictionary<string, DialogueOption> dialogueOptions = new Dictionary<string, DialogueOption>();
    public List<Button> buttons = new List<Button>();
        
    public void Start()
    {
        //Read and create dialogue options
        for (int i = 0; i < 16; i++)
        {
            string leadsTo = (i + 1).ToString();
            string leadsToSecond = (i == 14) ? "0" : (i + 2).ToString();

            int random = Random.Range(0, 100);

            string[] responses;
            if(i < 15)
            {
                if (random > 50)
                {
                    responses = new string[] { leadsTo, leadsToSecond };
                }
                else
                {
                    responses = new string[] { leadsTo };
                }
            } else
            {
                responses = new string[] { };
            }

            DialogueOption dialogueOption = new DialogueOption(i.ToString(), "This leads to option " + leadsTo, "You chose option" + i.ToString(), responses);
            dialogueOption.entityName = "DialogueManager";
            dialogueOptions.Add(i.ToString(), dialogueOption);
        }

    }

    public void ClearDialogue()
    {
        textBox.Clear();
    }

    public void AddLine(string name, string text, string color)
    {
        textBox.AddLine(name, text, color);
    }

    public void AddDialogueOption(string identifier, DialogueOption dialogueOption)
    {
        dialogueOptions.Add(identifier, dialogueOption);
    }

    public override void onMessage(Message m)
    {
        base.onMessage(m);

        switch (m.type)
        {
            case MsgType.Dialogue:
                AddLine(m.from.entityName, m.data.ToString(), "white");
                break;
            case MsgType.DialogueResponse:

                int i = 0;
                string[] responses = (string[])m.data;

                foreach (Button button in buttons)
                {

                    button.gameObject.GetComponentInChildren<Text>().text = "";
                    Destroy(button.gameObject.GetComponent<DialogueOption>());
                    button.gameObject.SetActive(false);

                    if(i < responses.Length)
                    {
                        button.gameObject.SetActive(true);

                        DialogueOption newOption = button.gameObject.AddComponent<DialogueOption>();
                        DialogueOption data = dialogueOptions[responses[i]];
                        newOption.resultingIdentifiers = data.resultingIdentifiers;
                        newOption.identifier = data.identifier;
                        newOption.optionText = data.optionText;
                        newOption.response = data.response;
                        newOption.entityName = data.entityName;
                        newOption.name = data.entityName;

                        newOption.AssignButton(button);
                        newOption.SetOption();

                        i++;

                    }

                }

                break;
        }
    }


}
