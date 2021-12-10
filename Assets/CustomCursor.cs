using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursor;
    public bool enabled;

    void Start()
    {
        if (!enabled)
        {
            Cursor.visible = false;
        } else
        {
#if UNITY_WEBGL
            float xspot = cursor.width / 2;
            float yspot = cursor.height / 2;
            Vector2 hotSpot = new Vector2(xspot, yspot);
            Cursor.SetCursor(cursor, hotSpot, CursorMode.ForceSoftware);
#else
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
#endif
        }

    }
}
