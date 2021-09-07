using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot = new Vector2(20,25);

    void Start(){
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
