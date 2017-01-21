using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveTypes
{
    Bool,
    Percentage,
    Number
}

public class QuestObjective : Entity {

    string identifier;
    string objectiveDesc;
    ObjectiveTypes objectiveType;
    private bool objectiveReached;
    public bool completed { get { return objectiveReached;  } }
    float progress = 0.0f;
    float goal;

    public QuestObjective(string id, string desc, ObjectiveTypes type, float g)
    {
        identifier = id;
        objectiveDesc = desc;
        objectiveType = type;
        goal = g;
    }

    public void ProgressObjective(bool progress)
    {
        if(!objectiveReached)
            if (progress)
                objectiveReached = true;
    }

    public void ProgressObjective(float f)
    {
        if (!objectiveReached)
        {
            goal += f;

            switch (objectiveType)
            {
                case ObjectiveTypes.Number:
                    if (progress >= goal)
                        objectiveReached = true;
                    break;
                case ObjectiveTypes.Percentage:
                    if (progress >= 100.0f)
                        objectiveReached = true;
                    break;
            }
        }
    }

    public string CreateDescription()
    {
        string v = "";
        switch (objectiveType)
        {
            case ObjectiveTypes.Bool:
                v = objectiveDesc;
                break;
            default:
                v = string.Format(objectiveDesc, progress, goal);
                break;
        }

        return v;
    }

}
