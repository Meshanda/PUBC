using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    private List<string> _keyStrokeHistory;

    public TextMeshProUGUI keyStrokeText;
    public TextMeshProUGUI cheatCodeText;

    /*
    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyUp)
        {
            Debug.Log(e.keyCode.ToString());
        }
    }
    */

    private void Awake()
    {
        _keyStrokeHistory = new List<string>();
    }

    private void Update()
    {
        KeyCode keyPressed = DetectKeyPressed();
        AddKeyStrokeToHistory(keyPressed.ToString());
        keyStrokeText.text = "HISTORY: " + GetKeyStrokeHistory();
        if(GetKeyStrokeHistory().Equals("UpArrow UpArrow DownArrow DownArrow LeftArrow RightArrow LeftArrow RightArrow B A"))
        {
            cheatCodeText.text = "KONAMI CHEAT CODE DETECTED!";
        }
        if(keyPressed != KeyCode.None)
            Debug.Log(keyPressed.ToString());
    }

    private KeyCode DetectKeyPressed()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }

    private void AddKeyStrokeToHistory(string keyStroke)
    {
        if(!keyStroke.Equals("None"))
        {
            _keyStrokeHistory.Add(keyStroke);
            if(_keyStrokeHistory.Count > 10)
            {
                _keyStrokeHistory.RemoveAt(0);
            }
        }
    }

    private string GetKeyStrokeHistory()
    {
        return string.Join(" ", _keyStrokeHistory.ToArray());
    }

}
