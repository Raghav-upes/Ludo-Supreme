using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

namespace Com.MyCompany.MyGame
{
    public class LauncherFriends : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        string gameVersion = "1";
        bool isConnecting;
        string roomCode;

        #endregion

        public GameObject ConnectFriends;
        public GameObject Friends;
        public TMP_Text code;
        public CoinMovement[] coin;
        public TMP_InputField joinCode;
        public GameObject PlayButton;
        private float duration = 1.0f; // Duration for one full move from 0 to 1245

        #region MonoBehaviour CallBacks


        public void openWhatsapp()
        {
            string url = "https://api.whatsapp.com/send?text=Hey! 🎲 I've created a room in Ludo Supreme. Join me using the room code: "+roomCode+". Let's play!";
  

            Application.OpenURL(url);
        }

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Debug.Log("Launcher: Awake called.");
        }

        void Start()
        {
            Debug.Log("Launcher: Start called.");
            Debug.Log("Launcher: Not connected to Photon. Connecting...");
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }

        #endregion

        #region Public Methods

        public void CreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                roomCode = GenerateRoomCode();
                code.text = roomCode;
                Debug.Log("Launcher: Creating room with code " + roomCode);
                RoomOptions options = new RoomOptions { MaxPlayers = 4, PublishUserId = true };
                PhotonNetwork.CreateRoom(roomCode, options);
            }
            else
            {
                Debug.LogWarning("Launcher: Not connected to Photon. Cannot create room.");
            }
        }

        public void JoinRoom()
        {
            if (joinCode.text.Length == 6)
            {
                Debug.Log("Launcher: Joining room with code " + joinCode.text);
                PhotonNetwork.JoinRoom(joinCode.text);
            }
        }

        string GenerateRoomCode()
        {
            return Random.Range(100000, 999999).ToString();
        }

        public void PlayMyGame()
        {
            Debug.Log("Launcher: PlayMyGame called.");
            
           /*     photonView.RPC("coinMove", RpcTarget.All);*/
                photonView.RPC("startDelayAnim", RpcTarget.All);
           
        }

        [PunRPC]
        void startDelayAnim()
        {
            StartCoroutine(delayAnima());
        }

        IEnumerator delayAnima()
        {
            DontDestroyOnLoad(this.gameObject);
            yield return new WaitForSeconds(1f);
            PhotonNetwork.LoadLevel("SampleScene");
            photonView.RPC("AssignPieceScript", RpcTarget.All);
            photonView.RPC("AssignOwnershipToAllPlayers", RpcTarget.All);
        }

        [PunRPC]
        void coinMove()
        {
         
            foreach (var i in coin)
            {
                i.gameObject.SetActive(true);
                i.enabled = true;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher: OnConnectedToMaster called.");
            if (isConnecting)
            {
                isConnecting = false;
                if (!string.IsNullOrEmpty(roomCode))
                {
                    Debug.Log("Launcher: Creating room with code " + roomCode);
                    RoomOptions options = new RoomOptions { MaxPlayers = 4, PublishUserId = true };
                    PhotonNetwork.CreateRoom(roomCode, options);
                }
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            Debug.LogWarningFormat("Launcher: OnDisconnected called with reason {0}", cause);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom called. Player has joined the room.");
            ConnectFriends.SetActive(true);
            Friends.SetActive(false);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("lplplp");
                code.text = roomCode;
            }
            else
            {
                Debug.Log("bnbnb");
                code.text = joinCode.text;
            }

          
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            Debug.Log("Launcher: OnPlayerEnteredRoom called. Player: " + player.NickName);
            Debug.Log(PhotonNetwork.PlayerList.Length);
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PlayButton.SetActive(true);

                }
            }
        }

        [PunRPC]
        void AssignPieceScript()
        {
            Debug.Log("Launcher: AssignPieceScript called.");

            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                Player player = players[i];
                string piece = "";

                switch (i)
                {
                    case 0:
                        piece = "RedPiece";
                        break;
                    case 1:
                        piece = "YellowPiece";
                        break;
                    case 2:
                        piece = "BluePiece";
                        break;
                    case 3:
                        piece = "GreenPiece";
                        break;
                }

                if (player.IsLocal)
                {
                    Debug.LogFormat("Launcher: Assigning {0} to local player {1}", piece, player.UserId);
                }
                else
                {
                    Debug.LogFormat("Launcher: Assigning {0} to player {1}", piece, player.UserId);
                }

                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Piece", piece } });
            }
        }

        public void AssignOwnershipBasedOnPiece(Player player)
        {
            if (player.CustomProperties.TryGetValue("Piece", out object pieceType))
            {
                string pieceTypeName = pieceType as string;

                if (pieceTypeName == "YellowPiece")
                {
                    TransferOwnership(player, 3);
                    GameManager.gm.ManageRollingDice[2].gameObject.SetActive(true);
                    TransferOwnership(player, 5);
                    GameManager.gm.ManageRollingDice[2].gameObject.SetActive(false);
                }
                if (pieceTypeName == "RedPiece")
                {
                    TransferOwnership(player, 1);
                    TransferOwnership(player, 7);
                }
                if (pieceTypeName == "BluePiece")
                {
                    TransferOwnership(player, 2);
                    GameManager.gm.ManageRollingDice[1].gameObject.SetActive(true);
                    TransferOwnership(player, 8);
                    GameManager.gm.ManageRollingDice[1].gameObject.SetActive(false);
                }
                if (pieceTypeName == "GreenPiece")
                {
                    TransferOwnership(player, 4);
                    GameManager.gm.ManageRollingDice[3].gameObject.SetActive(true);
                    TransferOwnership(player, 100);
                    GameManager.gm.ManageRollingDice[3].gameObject.SetActive(false);
                }
            }
        }

        void TransferOwnership(Player player, int viewID)
        {
            PhotonView targetPhotonView = PhotonView.Find(viewID);
            if (targetPhotonView != null)
            {
                targetPhotonView.TransferOwnership(player);
                Debug.LogFormat("Transferred ownership of PhotonView ID {0} to player {1}", viewID, player.UserId);
            }
            else
            {
                Debug.LogError("PhotonView with ID " + viewID + " not found.");
            }
        }

        [PunRPC]
        void AssignOwnershipToAllPlayers()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                AssignOwnershipBasedOnPiece(player);
            }
        }

        #endregion
    }
}
