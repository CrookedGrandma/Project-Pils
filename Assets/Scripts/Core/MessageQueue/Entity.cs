using UnityEngine;
using System.Collections;

//Extension of the basic MonoBehaviour to give everything a customizable name and implement a onMessage function at base    
public class Entity : MonoBehaviour {

    public string Name;

    public string entityName
    {
        get
        {
            return (this.Name.Length == 0) ? gameObject.name: this.Name;
        }
        set { this.Name = value; }
    }



    public virtual void onMessage(Message m)
    {
        Debug.Log("[" + m.from.name + "] → [" + m.to.name + "][" + m.type + "] " + m.data);
    }
}
