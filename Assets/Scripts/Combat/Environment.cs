using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour
{

    public Camera MainCamera;

    public Color Desert = Color.white;
    public Color Sea = Color.white;
    public Color Forest = Color.white;
    public Color Sky = Color.white;
    //Set these colors in Unity Editor, replace with backdrop sprites later on

    public void changebg(int bgcolor)
    {
        switch (bgcolor)
        {
            case 0: MainCamera.backgroundColor = Desert; break;
            case 1: MainCamera.backgroundColor = Sea; break;
            case 2: MainCamera.backgroundColor = Forest; break;
            case 3: MainCamera.backgroundColor = Sky; break;
            default: MainCamera.backgroundColor = Color.black; break;
        }
    }
}