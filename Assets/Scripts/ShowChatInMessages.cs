using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;

public class ShowChatInMessages : MonoBehaviourPunCallbacks
{
    public TMP_InputField messageInputField;
    public TMP_Text playerOneText;
    public TMP_Text playerTwoText;
    public TMP_Text playerThreeText;
    public TMP_Text playerFourText;
    public Image playerOneImage;
    public Image playerTwoImage;
    public Image playerThreeImage;
    public Image playerFourImage;
    public Button sendChatButton;

    private List<string> chatHistory = new List<string>();
    public ScrollRect chatScrollView;
    public GameObject chatContentContainer;
    public GameObject chatMessageTemplate;

    void Start()
    {
        if (messageInputField == null || sendChatButton == null || playerOneText == null || playerTwoText == null || playerThreeText == null || playerFourText == null || chatScrollView == null || chatContentContainer == null || chatMessageTemplate == null)
        {
            Debug.LogError("Please assign all required references in the inspector.");
            return;
        }
        sendChatButton.onClick.AddListener(OnSendChatButtonClicked);
    }

    void OnSendChatButtonClicked()
    {
        string chatMessage = messageInputField.text;
        if (!string.IsNullOrEmpty(chatMessage))
        {
            Debug.Log(chatMessage);
            photonView.RPC("BroadcastChatMessage", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId, chatMessage);
            messageInputField.text = string.Empty;
        }
    }


    [PunRPC]
    void BroadcastChatMessage(string senderId, string chatMessage)
    {
        chatHistory.Add(chatMessage);

        if (senderId == PhotonNetwork.PlayerList[0].UserId)
        {
            UpdatePlayerChatDisplay(playerOneText, playerOneImage, chatMessage);
        }
        else if (senderId == PhotonNetwork.PlayerList[1].UserId)
        {
            UpdatePlayerChatDisplay(playerTwoText, playerTwoImage, chatMessage);
        }
        else if (senderId == PhotonNetwork.PlayerList[2].UserId)
        {
            UpdatePlayerChatDisplay(playerThreeText, playerThreeImage, chatMessage);
        }
        else if (senderId == PhotonNetwork.PlayerList[3].UserId)
        {
            UpdatePlayerChatDisplay(playerFourText, playerFourImage, chatMessage);
        }

        AddChatMessageToScrollView(chatMessage);

        StartCoroutine(HidePlayerChatAfterDelay(playerOneText, playerOneImage));
        StartCoroutine(HidePlayerChatAfterDelay(playerTwoText, playerTwoImage));
        StartCoroutine(HidePlayerChatAfterDelay(playerThreeText, playerThreeImage));
        StartCoroutine(HidePlayerChatAfterDelay(playerFourText, playerFourImage));
    }

    void UpdatePlayerChatDisplay(TMP_Text playerChatText, Image playerChatImage, string chatMessage)
    {
        playerChatImage.gameObject.SetActive(true);
        playerChatText.text = chatMessage;
    }

    IEnumerator HidePlayerChatAfterDelay(TMP_Text playerChatText, Image playerChatImage)
    {
        yield return new WaitForSeconds(2f);
        playerChatText.text = string.Empty;
        playerChatImage.gameObject.SetActive(false);
    }

    void AddChatMessageToScrollView(string chatMessage)
    {
        GameObject newChatMessage = Instantiate(chatMessageTemplate, chatContentContainer.transform);
        TMP_Text chatMessageText = newChatMessage.GetComponent<TMP_Text>();
        chatMessageText.text = chatMessage;

        Canvas.ForceUpdateCanvases();
        chatScrollView.verticalNormalizedPosition = 0;
    }
}
