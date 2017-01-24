using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : Entity {


    string identifier;
    string title;
    public string dialogueText;
    public Dictionary<string, QuestObjective> objectives = new Dictionary<string, QuestObjective>();
    string[] items;
    int xp;
    bool hasBeenRewarded = false;

    public Quest(string i, string t, string dialogue, int xpReward = 0, string itemRewards = "")
    {
        identifier = i;
        title = t;
        dialogueText = dialogue;

        xp = xpReward;
        items = itemRewards.Split(',');
    }

    public void AddObjective(string identifier, QuestObjective objective)
    {
        objectives.Add(identifier, objective);
    }

    public bool QuestCompleted()
    {
        bool completed = true;

        foreach(KeyValuePair<string, QuestObjective> kvp in objectives)
        {
            if (!kvp.Value.completed)
            {
                completed = false;
                break;
            }
        }

        if (completed)
        {
            if (!hasBeenRewarded)
            {
                GiveRewards();
            }
        }

        return completed;
    }

    public void GiveRewards()
    {
        hasBeenRewarded = true;



        if (!hasBeenRewarded)
        {
            XPManager.xpmanager.addxp(xp);
            foreach (string s in items)
            {
                int itemID;

                if (int.TryParse(s, out itemID))
                {
                    PersistentInventoryScript.instance.addItemToEnd(itemID);
                }
            }
        }
    }

    public string CreateDescription()
    {
        string questDesc = "";

        questDesc += "<b><color=#DAB12BD2>" + title + "</color></b>\n";

        foreach(KeyValuePair<string, QuestObjective> kvp in objectives)
        {
            if(!kvp.Value.completed)
                questDesc += kvp.Value.CreateDescription() + "\n";
        }

        questDesc += "\n";

        return questDesc;
    }

}
