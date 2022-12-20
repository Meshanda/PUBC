using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : GenericSingleton<UIManager>
{
    [SerializeField] private Texture2D _menuCursor;
    [SerializeField] private Texture2D _crosshairCursor;

    private string _name;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetMenuCursor();
    }

    public void SetMenuCursor()
    {
        Cursor.SetCursor(_menuCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    
    public void SetCrosshairCursor()
    {
        Vector2 test = Vector2.zero;
        test.y = 80f;
        Cursor.SetCursor(_crosshairCursor, test, CursorMode.ForceSoftware);
    }

    public void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
    }
}
