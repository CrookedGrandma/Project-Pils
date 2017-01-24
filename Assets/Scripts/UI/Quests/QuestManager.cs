using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Linq;

public class QuestManager : Entity {
    public Text questTextBox;

    private JsonData QuestData;
    private JsonData QuestObjectiveData;

    public Dictionary<string, QuestObjective> questObjectives = new Dictionary<string, QuestObjective>();
    public Dictionary<string, Quest> quests = new Dictionary<string, Quest>();

    public Dictionary<string, Quest> questLog = new Dictionary<string, Quest>();

    public void Awake()
    {
        DontDestroyOnLoad(this);

        QuestData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Quests.json"));
        QuestObjectiveData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/QuestObjectives.json"));


        for (int i = 0; i < QuestObjectiveData.Count; i++)
        {
            JsonData EntryData = QuestObjectiveData[i];
            string id = EntryData["identifier"].ToString();
            ObjectiveTypes objType;

            switch (EntryData["type"].ToString())
            {
                case "bool":
                    objType = ObjectiveTypes.Bool;
                    break;
                case "number":
                    objType = ObjectiveTypes.Number;
                    break;
                case "percentage":
                    objType = ObjectiveTypes.Percentage;
                    break;
                default:
                    objType = ObjectiveTypes.Number;
                    break;
            }

            QuestObjective objective = new QuestObjective(id, EntryData["description"].ToString(), objType, float.Parse(EntryData["goal"].ToString()));
            questObjectives.Add(id, objective);
        }

        for (int i = 0; i < QuestData.Count; i++)
        {
            JsonData EntryData = QuestData[i];
            string id = EntryData["identifier"].ToString();
            string dialogue = EntryData["dialogue"].ToString();
            int xpReward = (EntryData.Keys.Contains("xp")) ? int.Parse(EntryData["xp"].ToString()) : 0;
            string itemRewards = (EntryData.Keys.Contains("items")) ? EntryData["items"].ToString() : "";

            Quest quest = new Quest(id, EntryData["title"].ToString(), dialogue, xpReward, itemRewards);

            string[] objectives = EntryData["objectives"].ToString().Split(',');

            foreach(string s in objectives)
            {
                quest.AddObjective(s, questObjectives[s]);
            }

            quests.Add(id, quest);
        }


    }
    
    public void AddQuestToLog(string id)
    {
        questLog.Add(id, quests[id]);

        GameManager.instance.cutscene.SetText(quests[id].dialogueText);
        GameManager.instance.cutscene.FadeInPanel();

        RefreshText();
    }

    public void RefreshText() {
        questTextBox.text = "";

        foreach(KeyValuePair<string, Quest> kvp in questLog)
        {
            if(!kvp.Value.QuestCompleted())
                questTextBox.text += kvp.Value.CreateDescription() + "\n";
        }
    }

    public void CompleteObjective(string objectiveIdentifier)
    {
        foreach(KeyValuePair<string, Quest> kvp in questLog)
        {
            foreach(KeyValuePair<string, QuestObjective> kvp2 in kvp.Value.objectives)
            {
                if(kvp2.Key == objectiveIdentifier)
                {
                    kvp2.Value.ProgressObjective(true);
                }
            }
        }

        RefreshText();
    }

    public void Start()
    {
        AddQuestToLog("Quest000");
        RefreshText();
    }

    public override void onMessage(Message m)
    {
        base.onMessage(m);

        switch (m.type)
        {
            case MsgType.QuestCompletion:
                CompleteObjective(m.data.ToString());
                break;
            case MsgType.QuestTrigger:
                AddQuestToLog(m.data.ToString());
                break;
            default:
                break;
        }
    }

}
