using UnityEngine;
using System.Collections;

//MsgType's determine behavior of the OnMessage method
public enum MsgType
{
    Dialogue,
    DialogueResponse,
    QuestCompletion,
    QuestTrigger
}

//Simple structure that holds information regarding who sent what to whom
public class Message {

    public Entity from;
    public Entity to;
    public MsgType type;
    public object data;

    public Message(Entity f, Entity t, MsgType ty, object d)
    {
        from = f;
        to = t;
        type = ty;
        data = d;
    }

}
