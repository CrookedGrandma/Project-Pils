using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageQueue {

    List<Message> messages = new List<Message>();

    public void Add(Message m)
    {
        messages.Add(m);
    }

    public void Dispatch()
    {
        for(int i = messages.Count - 1; i >= 0; i--)
        {
            Message m = messages[i];

            m.to.onMessage(m);
            messages.RemoveAt(i);
        }
    }
}
