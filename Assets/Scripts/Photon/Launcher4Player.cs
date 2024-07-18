using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Drawing;

namespace Com.MyCompany.MyGame
{
    public class Launcher4Player : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        string gameVersion = "1";
        bool isConnecting;

        #endregion


        public CoinMovement[] coin;



        public RectTransform player2;
        public RectTransform player3;
        public RectTransform player4;
        private float duration = 1.0f; // Duration for one full move from 0 to 1245



        #region MonoBehaviour CallBacks

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Debug.Log("Launcher: Awake called.");
        }

        public void StartMyGame()
        {
            Debug.Log("Launcher: Start called.");
            StartCoroutine(AnimateRectTransformPosY());
            Connect();
        }

        #endregion

        #region Public Methods

        public void BreackConnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Launcher: Already connected to Photon.");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.Log("Launcher: Connecting to Photon.");
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Launcher: OnJoinRandomFailed called. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4, PublishUserId = true });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom called. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
            {




                photonView.RPC("coinMove", RpcTarget.All);

                photonView.RPC("startDelayAnim", RpcTarget.All);


            }


        }



        IEnumerator AnimateRectTransformPosY()
        {
            while (true && player2 != null && player3!=null && player4!=null)
            {
                // Start from 0 and move to 1245
                yield return MoveRectTransformPosY(0, 1245, duration);

                // Reset position to 0
                player2.anchoredPosition = new Vector2(player2.anchoredPosition.x, 0);
                player3.anchoredPosition = new Vector2(player2.anchoredPosition.x, 0);
                player4.anchoredPosition = new Vector2(player2.anchoredPosition.x, 0);
            }
        }

        IEnumerator MoveRectTransformPosY(float startY, float endY, float duration)
        {
            float elapsedTime = 0;
            Vector2 startPos = new Vector2(player2.anchoredPosition.x, startY);
            Vector2 endPos = new Vector2(player2.anchoredPosition.x, endY);

            while (elapsedTime < duration && player2 != null)
            {
                // Calculate the new position based on the elapsed time
                float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);

                player2.anchoredPosition = new Vector2(player2.anchoredPosition.x, newY);
                player3.anchoredPosition = new Vector2(player3.anchoredPosition.x, newY);
                player4.anchoredPosition = new Vector2(player4.anchoredPosition.x, newY);
                // Increment the elapsed time by the frame's time
                elapsedTime += Time.deltaTime;

                // Yield until the next frame
                yield return null;
            }

            // Ensure the final position is exactly the end position
            player2.anchoredPosition = endPos;
            player3.anchoredPosition = endPos;
            player4.anchoredPosition = endPos;
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





        #endregion



        [PunRPC]
        void coinMove()
        {
            player2.gameObject.GetComponentInParent<AudioSource>().Pause();
            player2.gameObject.GetComponent<AudioSource>().Play();
            player3.gameObject.GetComponentInParent<AudioSource>().Pause();
            player3.gameObject.GetComponent<AudioSource>().Play();
            player4.gameObject.GetComponentInParent<AudioSource>().Pause();
            player4.gameObject.GetComponent<AudioSource>().Play();
            StopCoroutine(AnimateRectTransformPosY());
            StopCoroutine("MoveRectTransformPosY");


            StopAllCoroutines();
            player2.anchoredPosition = new Vector2(player2.anchoredPosition.x, 0);
            player3.anchoredPosition = new Vector2(player3.anchoredPosition.x, 0);
            player4.anchoredPosition = new Vector2(player4.anchoredPosition.x, 0);
            foreach(var i in coin)
            {
                i.gameObject.SetActive(true);

                i.enabled = true;
 
            }
           
        }

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher: OnConnectedToMaster called.");
            if (isConnecting)
            {
                Debug.Log("Launcher: Joining random room.");
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            Debug.LogWarningFormat("Launcher: OnDisconnected called with reason {0}", cause);
        }


        public override void OnPlayerEnteredRoom(Player player)
        {
            Debug.Log("Launcher: OnJoinedRoom called. Now this client is in a room.");
            /*            photonView.RPC("setYellow", RpcTarget.AllBuffered, player);*/
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
                /*            Debug.LogFormat("PhotonView ID {0} is now owned by {1}", viewID, targetPhotonView.Owner.UserId);*/
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
