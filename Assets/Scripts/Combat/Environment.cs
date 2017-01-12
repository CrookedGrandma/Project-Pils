using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour
{

    public Camera MainCamera;

    public Color Desert = Color.black;
    public Color Sea = Color.black;
    public Color Forest = Color.black;
    public Color Sky = Color.black;
    //Set these colors in Unity Editor, replace with backdrop sprites later on

    public void changebg(int bgcolor)
    {
        switch (bgcolor)
        {
            case 0: MainCamera.backgroundColor = Desert; break;
            case 1: MainCamera.backgroundColor = Sea; break;
            case 2: MainCamera.backgroundColor = Forest; break;
            case 3: MainCamera.backgroundColor = Sky; break;
            default: MainCamera.backgroundColor = Color.white; break;
        }
    }
}