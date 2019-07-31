using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using ExitGamesHashtable = ExitGames.Client.Photon.Hashtable;
using Assets.Lib;
using Assets.Scripts.UI.Lobby;
using ExitGames.Client.Photon;
using System;

public class MainPanelController : MonoBehaviourPunCallbacks
{

    private Image background;

    [Header("Panels")]
    public GameObject mainSelectionPanel;
    public GameObject createGamePanel;
    public GameObject roomPanel;
    public GameObject roomListPanel;

    [Header("Main Selection")]
    public Button createGameButton;
    public Button joinRandomButton;
    public Button gameListButton;
    public Button exitToLoginButton;
    public Button exitToDesktopButton;

    [Header("Room Creation")]
    public InputField gameNameInputField;
    public InputField maxPlayersInputField;
    public Button createRoomButton;
    public Text createRoomStatusText;
    
    [Header("Room")]
    public GameObject playerEntryPrefab;
    public Text roomNameText;
    public Button startGameButton;
    public GameObject playerEntries;
    public Button leaveRoomButton;

    #region UNITY
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        background = GetComponent<Image>();

        createGameButton.onClick.AddListener(this.OnCreateGameButtonClicked);
        joinRandomButton.onClick.AddListener(this.OnJoinRandomButtonClicked);
        gameListButton.onClick.AddListener(this.OnGameListButtonClicked);
        exitToLoginButton.onClick.AddListener(this.OnExitToLoginButtonClicked);
        exitToDesktopButton.onClick.AddListener(this.OnExitToDesktopButtonClicked);


        createRoomButton.onClick.AddListener(this.OnCreateRoomButtonClicked);
        startGameButton.onClick.AddListener(this.OnStartGameButtonClicked);
        leaveRoomButton.onClick.AddListener(this.OnLeaveRoomButtonClicked);
        ShowSubPanel(null);
        HidePanel();
    }

    



    #endregion

    #region UI CALLBACKS

    #region MAIN PANEL
    private void OnCreateGameButtonClicked()
    {
        ShowSubPanel(createGamePanel);
        SetStatusText(createRoomStatusText, "");
    }   

    private void OnJoinRandomButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnGameListButtonClicked()
    {

    }

    private void OnExitToLoginButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }

    private void OnExitToDesktopButtonClicked()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
    #endregion

    #region ROOM

    private void OnCreateRoomButtonClicked()
    {
        SetStatusText(createRoomStatusText, "");

        string roomName = gameNameInputField.text;

        if(roomName.Equals(""))
        {
            SetStatusText(createRoomStatusText, "The game's name must be not empty");
            return;
        }

        string value = maxPlayersInputField.text.Trim();
        bool valid = int.TryParse(value, out int maxPlayers);
        if(valid)
        {
            if(maxPlayers >= 2 && maxPlayers <= 8)
            {
                RoomOptions roomOptions = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = byte.Parse(value) };
                PhotonNetwork.CreateRoom(roomName, roomOptions);
                SetStatusText(createRoomStatusText, "Creating game...");
            }
            else
            {
                SetStatusText(createRoomStatusText, "The maximum number of players must be between 2 and 8");
            }
        }
        else
        {
            SetStatusText(createRoomStatusText, "Invalid number");
        }
    }

    private void OnStartGameButtonClicked()
    {
       if(PhotonNetwork.IsMasterClient && AreAllPlayersReady)
       {
            SceneManager.Instance.StartMultiplayerGame();
       }
    }

    private void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #endregion


    #region PUN

    public override void OnConnectedToMaster()
    {
        ShowPanel();
        ShowSubPanel(mainSelectionPanel);
    }

    public override void OnCreatedRoom()
    {
        SetStatusText(createRoomStatusText, "");
        //ShowSubPanel(roomPanel);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetStatusText(createRoomStatusText, message);
    }

    public override void OnJoinedRoom()
    {
        
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        
        ShowSubPanel(roomPanel);

        ExitGamesHashtable exitGamesHashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        exitGamesHashtable[NetworkHelper.Constants.PLAYER_READY] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(exitGamesHashtable);
        CreatePlayerList();
        startGameButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient && AreAllPlayersReady);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CreatePlayerList();
    }

    public override void OnLeftRoom()
    {
        roomNameText.text = "";
        roomNameText.gameObject.SetActive(false);
        ShowSubPanel(null);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        HidePanel();
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGamesHashtable changedProps)
    {
        startGameButton.gameObject.SetActive(roomPanel.activeSelf == true && PhotonNetwork.LocalPlayer.IsMasterClient && AreAllPlayersReady);
    }


    #endregion


    public void CreatePlayerList()
    {
        for(int y = 0; y < playerEntries.transform.childCount; y++)
        {
            GameObject gameObject = playerEntries.transform.GetChild(y).gameObject;
            if (!gameObject.Equals(playerEntryPrefab))
            {
                Destroy(gameObject);
            }
        }
 
        const float offsetY = 6f;
        int i = 0;
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playerEntryPrefab, playerEntries.transform);
            RectTransform rectTransform = entry.GetComponent<RectTransform>();

            float naturalOffset = i == 0 ? 0 : rectTransform.sizeDelta.y;

            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y - (offsetY * i++) - naturalOffset, rectTransform.localPosition.z);
            
            entry.GetComponent<PlayerListEntry>().player = p;
            entry.SetActive(true);
        }
        
    }

    private void HidePanel()
    { 
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(false);
        }
        background.enabled = false;
    }

    private void ShowPanel()
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(true);
        }
        background.enabled = true;
    }


    private void ShowSubPanel(GameObject subPanel)
    {
        mainSelectionPanel.SetActive(mainSelectionPanel.Equals(subPanel));
        createGamePanel.SetActive(createGamePanel.Equals(subPanel));
        roomPanel.SetActive(roomPanel.Equals(subPanel));
        //roomListPanel.SetActive(roomListPanel.Equals(subPanel));
    }

    private void SetStatusText(Text statusText, string value)
    {
        statusText.text = value;
        statusText.enabled = !value.Equals("");
    }

    private bool AreAllPlayersReady
    {
        get
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(NetworkHelper.Constants.PLAYER_READY, out object ready))
                {
                    if (!(bool)ready)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
