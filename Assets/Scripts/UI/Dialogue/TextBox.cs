using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TextBox : MonoBehaviour {

    public Text textBox;

    public void AddLine(string name, string text, string color)
    {
        textBox.text = "<color=" + color + ">[" + name + "] " + text + "</color>\r\n" + textBox.text;
    }

    public void Clear()
    {
        textBox.text = "";
    }

}
