using System;
using UnityEngine;

public class Fadenkreuz : MonoBehaviour {

    public Texture2D fadenkreuz;

    private Rect position;

    void Start () {
        position = new Rect((Screen.width-fadenkreuz.width)/2, (Screen.height-fadenkreuz.height)/2, fadenkreuz.width, fadenkreuz.height);
    }

    void OnGUI () {
        GUI.DrawTexture(position, fadenkreuz);
    }
}