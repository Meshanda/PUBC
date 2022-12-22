using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KonamiCodeScript : MonoBehaviour
{
    [SerializeField] private float _konamiDuration = 20f;
    [SerializeField] private TMP_FontAsset _fontIcon;
    
    [Header("Title")]
    [SerializeField] private TitleScript _titleScript;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private float _titleSpeed;
    [SerializeField] private float _titleMinSize = 110f;
    [SerializeField] private float _titleMaxSize = 125f;
    [SerializeField] private int _titleTextLength = 16;
    
    [Header("Buttons")]
    [SerializeField] private int _buttonTextLength = 4;
    [SerializeField] private List<TextMeshProUGUI> _buttonTexts;

    private List<string> _keyStrokeHistory;
    
    private void Awake()
    {
        _keyStrokeHistory = new List<string>();
    }
    
    private void Update()
    {
        KeyCode keyPressed = DetectKeyPressed();
        AddKeyStrokeToHistory(keyPressed.ToString());
        if(GetKeyStrokeHistory().Equals("UpArrow UpArrow DownArrow DownArrow LeftArrow RightArrow LeftArrow RightArrow B A"))
        {
            _keyStrokeHistory.Clear();
            KonamiCodeRealized();
        }
    }

    private void KonamiCodeRealized()
    {
        ChangeButtons();
        ChangeTitle();
    }

    private void ChangeTitle()
    {
        StartCoroutine(TitleRoutine());
    }

    private IEnumerator TitleRoutine()
    {
        var oldTitle = _titleText.text;
        var oldColor = _titleText.colorGradient;
        var oldFont = _titleText.font;
        var oldSpeed = _titleScript.Speed;
        var oldMinSize = _titleScript.TextMinSize;
        var oldMaxSize = _titleScript.TextMaxSize;
        
        _titleScript.Speed = _titleSpeed;
        _titleScript.TextMinSize = _titleMinSize;
        _titleScript.TextMaxSize = _titleMaxSize;
        _titleText.font = _fontIcon;
        
        var routine = StartCoroutine(TextFlash(_titleText, _titleTextLength));

        yield return new WaitForSeconds(_konamiDuration);
        StopCoroutine(routine);
        
        _titleScript.TextMinSize = oldMinSize;
        _titleScript.TextMaxSize = oldMaxSize;
        _titleScript.Speed = oldSpeed;
        _titleText.text = oldTitle;
        _titleText.colorGradient = oldColor;
        _titleText.font = oldFont;
    }

    private void ChangeButtons()
    {
        foreach (var buttonText in _buttonTexts)
        {
            StartCoroutine(ButtonsRoutine(buttonText));
        }
    }

    private IEnumerator ButtonsRoutine(TextMeshProUGUI text)
    {
        var oldText = text.text;
        var oldColor = text.colorGradient;
        var oldFont = text.font;
        text.font = _fontIcon;
        
        var routine = StartCoroutine(TextFlash(text, _buttonTextLength));

        yield return new WaitForSeconds(_konamiDuration);
        
        StopCoroutine(routine);
        text.font = oldFont;
        text.text = oldText;
        text.colorGradient = oldColor;
    }

    private IEnumerator TextFlash(TextMeshProUGUI text, int textSize)
    {
        while (true)
        {
            text.text = RandomText(textSize);

            text.colorGradient = new VertexGradient(GetRandomColor(), GetRandomColor(), GetRandomColor() ,GetRandomColor());
            text.SetAllDirty();
            
            yield return new WaitForSeconds(.03f);
        }
    }

    private Color GetRandomColor()
    {
        return new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));
    }
    private string RandomText(int nb)
    {
        var letters = "abcdefghijklmnopqrstuvwxyz0123456789";
        var str = "";

        for (int j = 0; j < nb; j++)
        {
            str += letters[Random.Range(0, letters.Length)];
        }

        return str;
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
