using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Environment : MonoBehaviour
{
    
    public Image BG;

    public Color Desert = Color.black;
    public Color Sea = Color.black;
    public Color Forest = Color.black;
    public Color Sky = Color.black;
    //Set these colors in Unity Editor (EnemyChooser), replace with backdrop sprites later on

    public void changebg(int bgcolor)
    {
        switch (bgcolor)
        {
            case 0: BG.color = Desert; break;
            case 1: BG.color = Sea; break;
            case 2: BG.color = Forest; break;
            case 3: BG.color = Sky; break;
            default: BG.color = Color.white; break;
        }
    }
}