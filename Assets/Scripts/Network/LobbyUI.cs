using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Network;
using ScriptableObjects.Variables;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;

    [Space(10)] 
    [Header("Ready Button")] 
    [SerializeField] private Button _startButton;
    
    [Space(10)]
    [Header("Ready Button")]
    [SerializeField] private Button _readyButton;
    [SerializeField] private TextMeshProUGUI _readyText;

    [Space(10)] 
    [Header("Game Settings")] 
    [SerializeField] private GameObject _paramParent;
    [SerializeField] private TMP_InputField _killText;
    [SerializeField] private IntVariable _killSo;
    [SerializeField] private TMP_InputField _timeText;
    [SerializeField] private FloatVariable _timeSo;

    [Space(10)]
    [Header("Map Selection")]
    [SerializeField] private Button _mapLeftButton;
    [SerializeField] private Button _mapRightButton;
    [SerializeField] private TextMeshProUGUI _mapName;
    [SerializeField] private MapSelectionData _mapSelectionData;
    [SerializeField] private List<GameObject> _mapsGo;
    private int _currentMapIndex = 0;

    [Space(10)] 
    [Header("Skin Selection")] 
    [SerializeField] private Button _skinLeftButton;
    [SerializeField] private Button _skinRightButton;
    [SerializeField] private SkinSelectionData _skinDatas;
    [SerializeField] private TextMeshProUGUI _skinName;

    // ToDo : Replace with SO
    private const int NUMBER_OF_PLAYER_SKINS = 3;
    
    private void OnEnable()
    {
        _readyButton.onClick.AddListener(OnReadyPressed);
        _skinLeftButton.onClick.AddListener(OnSkinLeftButtonClicked);
        _skinRightButton.onClick.AddListener(OnSkinRightButtonClicked);

        if (GameLobbyManager.Instance.IsHost)
        {
            _mapLeftButton.onClick.AddListener(OnMapLeftButtonClicked);
            _mapRightButton.onClick.AddListener(OnMapRightButtonClicked);
            _startButton.onClick.AddListener(OnStartButtonClicked);
            
            GameLobbyEvents.OnLobbyReady += OnLobbyReady;
            GameLobbyEvents.OnLobbyUnReady += OnLobbyUnReady;
        }

        GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }
    

    private void OnDisable()
    {
        _readyButton.onClick.RemoveAllListeners();
        _startButton.onClick.RemoveAllListeners();
        
        _mapLeftButton.onClick.RemoveAllListeners();
        _mapRightButton.onClick.RemoveAllListeners();
        
        _skinLeftButton.onClick.RemoveAllListeners();
        _skinRightButton.onClick.RemoveAllListeners();
        
        GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        GameLobbyEvents.OnLobbyReady -= OnLobbyReady;
        GameLobbyEvents.OnLobbyUnReady -= OnLobbyUnReady;

    }

    private void Start()
    {
        _lobbyCodeText.text = GameLobbyManager.Instance.GetLobbyCode();

        if (!GameLobbyManager.Instance.IsHost)
        {
            _mapLeftButton.gameObject.SetActive(false);
            _mapRightButton.gameObject.SetActive(false);
            
        }
        else
        {
            _paramParent.SetActive(true);
            _killText.text = _killSo.value.ToString();
            _timeText.text = _timeSo.value + "s";
        }
    }

    private void Update()
    {
        _killSo.value = Int32.Parse(_killText.text);
        _timeSo.value = Int32.Parse(_timeText.text);
    }

    private async void OnSkinRightButtonClicked()
    {
        var skinIndex = GameLobbyManager.Instance.GetLocalSkinIndex();
        
        if (skinIndex + 1 <= NUMBER_OF_PLAYER_SKINS - 1)
        {
            skinIndex++;
        }
        else
        {
            skinIndex = 0;
        }
        
        UpdateSkin(skinIndex);
        await GameLobbyManager.Instance.SetLocalSkinIndex(skinIndex);
    }

    private async void OnSkinLeftButtonClicked()
    {
        var skinIndex = GameLobbyManager.Instance.GetLocalSkinIndex();
        
        if (skinIndex - 1 >= 0)
        {
            skinIndex--;
        }
        else
        {
            skinIndex = NUMBER_OF_PLAYER_SKINS - 1;
        }

        UpdateSkin(skinIndex);
        await GameLobbyManager.Instance.SetLocalSkinIndex(skinIndex);
    }

    private void UpdateSkin(int index)
    {
        _skinName.text = _skinDatas.Skins[index].SkinName;
    }

    private async void OnMapLeftButtonClicked()
    {
        if (_currentMapIndex - 1 >= 0)
        {
            _currentMapIndex--;
        }
        else
        {
            _currentMapIndex = _mapSelectionData.Maps.Count - 1;
        }

        UpdateMap();
        await GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapSelectionData.Maps[_currentMapIndex].SceneName);
    }

    private async void OnMapRightButtonClicked()
    {
        if (_currentMapIndex + 1 <= _mapSelectionData.Maps.Count - 1)
        {
            _currentMapIndex++;
        }
        else
        {
            _currentMapIndex = 0;
        }

        UpdateMap();
        await GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapSelectionData.Maps[_currentMapIndex].SceneName);
    }
    private async void OnReadyPressed()
    {
        var isReady = GameLobbyManager.Instance.IsPlayerReady();
        _readyText.text = !isReady ? "Not Ready" : "Ready";
        
        await GameLobbyManager.Instance.SetPlayerReady();
    }

    private void UpdateMap()
    {
        _mapName.text = _mapSelectionData.Maps[_currentMapIndex].MapName;

        for (var i = 0; i < _mapsGo.Count; i++)
        {
            _mapsGo[i].SetActive(i == _currentMapIndex);
        }
    }
    
    private void OnLobbyUpdated()
    {
        _currentMapIndex = GameLobbyManager.Instance.GetMapIndex();
        UpdateMap();
    }
    
    private void OnLobbyReady()
    {
        _startButton.gameObject.SetActive(true);
    }

    private void OnLobbyUnReady()
    {
        _startButton.gameObject.SetActive(false);
    }

    private async void OnStartButtonClicked()
    {
        _startButton.interactable = false;
        
        await GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapSelectionData.Maps[_currentMapIndex].SceneName);
        await GameLobbyManager.Instance.StartGame();
    }
}
