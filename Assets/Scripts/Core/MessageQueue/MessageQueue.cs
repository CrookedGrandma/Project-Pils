using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageQueue {

    //Contains a list of messages that have yet to be handled
    List<Message> messages = new List<Message>();

    //Adds a message to the list for the next round of dispatching
    public void Add(Message m)
    {
        messages.Add(m);
    }

    //Dispatch all the messages in the list
    public void Dispatch()
    {
        //Iterate backwards to allow for modifying the list during the loop
        for(int i = messages.Count - 1; i >= 0; i--)
        {
            //Get the message
            Message m = messages[i];

            //Call the handler function on the receiver
            m.to.onMessage(m);
            
            //Remove it from the list
            messages.RemoveAt(i);
        }
    }
}
