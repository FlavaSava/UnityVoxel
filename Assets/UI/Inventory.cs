using System;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private bool showInventar;

    void Start () {
        showInventar = false;
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.I)) {
            this.showInventar = !this.showInventar;
        }
    }

    void OnGUI () {
        if (showInventar) {
            GUI.Window(1, new Rect(10,10, 500, Screen.height - 20), DrawInventory, "Inventar");
        }
    }

    void DrawInventory (int windiwID) {
        GUI.Button (new Rect (20, 20, 100, 100), "Test 1");
        GUI.Button (new Rect (140, 20, 100, 100), "Test 2");
        GUI.Button (new Rect (260, 20, 100, 100), "Test 3");
        GUI.Button (new Rect (380, 20, 100, 100), "Test 4");
    }

    public bool IsVisible () {
        return this.showInventar;
    }


}