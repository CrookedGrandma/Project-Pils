using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Linq;

public class DialogueManager : Entity {

    public TextBox textBox;
    public Dictionary<string, DialogueOption> dialogueOptions = new Dictionary<string, DialogueOption>();
    public List<Button> buttons = new List<Button>();
    private JsonData DialogueData;

    public void Awake()
    {
        DialogueData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Dialogue.json"));

        for(int i = 0; i < DialogueData.Count; i++)
        {
            JsonData EntryData = DialogueData[i];
            string[] responses = EntryData["responses"].ToString().Split(',');
            responses = responses.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            string triggersQuest = (EntryData.Keys.Contains("triggersQuest")) ? EntryData["triggersQuest"].ToString() : "";
            string completesQuest = (EntryData.Keys.Contains("completesQuest")) ? EntryData["completesQuest"].ToString() : "";


            DialogueOption dialogueOption = new DialogueOption(EntryData["identifier"].ToString(), EntryData["text"].ToString(), EntryData["response"].ToString(), responses, triggersQuest, completesQuest);
            dialogueOption.entityName = EntryData["owner"].ToString();

            dialogueOptions.Add(EntryData["identifier"].ToString(), dialogueOption);
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


    public bool DialogueHasEnded()
    {
        bool activeButtonFound = false;

        foreach(Button button in buttons)
        {
            if (button.IsActive()) { activeButtonFound = true; break; }    
        }

        return !activeButtonFound;
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
                        newOption.triggersQuest = data.triggersQuest;
                        newOption.completesQuest = data.completesQuest;

                        newOption.AssignButton(button);
                        newOption.SetOption();

                        i++;

                    }

                }

                break;
        }
    }


}
