using Assets.Lib;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ExitGamesHashtable = ExitGames.Client.Photon.Hashtable;
using PhotonPlayer = Photon.Realtime.Player;

namespace Assets.Scripts.UI.Lobby
{
    public class PlayerListEntry : MonoBehaviourPunCallbacks
    {
        public Button readyButton;
        public Text readyText;
        public Text playerNickName;
        public PhotonPlayer player;

        [SerializeField]
        private bool readyValue = false;

        private IEnumerator Start()
        {
            readyButton.gameObject.SetActive(false);
            readyText.gameObject.SetActive(false);
            yield return new WaitUntil(() => player != null);

            playerNickName.text = player.NickName;
            
            if(player.Equals(PhotonNetwork.LocalPlayer))
            {
                readyButton.gameObject.SetActive(true);
                readyButton.onClick.AddListener(this.OnReadyButtonClicked);
            }
            else
            {
                readyText.gameObject.SetActive(true);
            }
            UpdateReadyStatus();
        }

        private void OnReadyButtonClicked()
        {
            ExitGamesHashtable exitGamesHashtable = player.CustomProperties;
            
            exitGamesHashtable[NetworkHelper.Constants.PLAYER_READY] = !readyValue;

            player.SetCustomProperties(exitGamesHashtable);
        }

        public override void OnPlayerPropertiesUpdate(PhotonPlayer target, ExitGamesHashtable changedProps)
        {
            if(target.Equals(player))
            {
                if(changedProps.TryGetValue(NetworkHelper.Constants.PLAYER_READY, out object ready))
                {
                    this.readyValue = (bool) ready;

                    UpdateReadyStatus();
                }
            }
        }

        private void UpdateReadyStatus()
        {
            readyButton.gameObject.GetComponent<Image>().color = !readyValue ? Color.green : Color.red;
            readyButton.GetComponentInChildren<Text>().text = !readyValue ? "Ready" : "Cancel";
            
            readyText.text = readyValue ? "Ready" : "Not ready";
            readyText.color = readyValue ? Color.green : Color.red;
        }
    }
}

