using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Lobby
{
    public class LoginPanelController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private InputField loginInputField;

        [SerializeField]
        private Text statusText;

        private void Awake()
        {
            background = GetComponent<Image>();
            ShowPanel();
        }

        #region UNITY

        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
        }

        #endregion UNITY

        #region UI CALLBACKS

        private void OnLoginButtonClicked()
        {
            string nickname = loginInputField.text;
            if (nickname.Equals(""))
            {
                nickname = "Player 1";
            }

            PhotonNetwork.LocalPlayer.NickName = nickname;

            PhotonNetwork.ConnectUsingSettings();
            SetStatusText("Connecting to server...");
        }

        #endregion UI CALLBACKS

        #region PUN

        public override void OnConnectedToMaster()
        {
            if(!PhotonNetwork.InLobby)
            {
                SetStatusText("Joining server's lobby");
                PhotonNetwork.JoinLobby();
            }
            else
            {
                OnJoinedLobby();
            }
        }

        public override void OnJoinedLobby()
        {
            SetStatusText("");
            HidePanel();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            ShowPanel();
        }
        #endregion PUN

        private void HidePanel()
        {
            /*Text[] texts = GetComponentsInChildren<Text>();
            foreach(Text text in texts)
            {
                text.gameObject.SetActive(false);
            }

            background.gameObject.SetActive(false);
            loginButton.gameObject.SetActive(false);
            loginInputField.gameObject.SetActive(false);*/

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

        private void SetStatusText(string text)
        {
            statusText.text = text;
            statusText.enabled = !text.Equals("");
        }
    }
}