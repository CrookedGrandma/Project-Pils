using UnityEngine;
using System.Collections;

public enum MsgType
{
    Damage,
    Heal,
    Shield,
    Effect,
    Open,
    Dialogue
}

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
