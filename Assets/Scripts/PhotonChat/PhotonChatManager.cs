using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    #region Setup
    [SerializeField] GameObject joinChat;
    bool isConnected;
    ChatClient chatClient;
    [SerializeField] string userID;

    public void UserNameOnValueChange(string valueIn)
    {
        userID = valueIn;
    }

    public void ChatConnectOnClick()
    {
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
        Debug.Log("CONNECTING...");
    }
    #endregion Setup



    [SerializeField] GameObject chatPanel;
    [SerializeField] InputField chatField;
    [SerializeField] Text chatDisplay;
    string privatereceiver = "";
    string currentChat;


    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }

    public void SubmitPublicChatOnClick()
    {
        if (privatereceiver == "") {
            chatClient.PublishMessage("RegionalChannel", currentChat);
            chatField.text = "";
            currentChat = "";
        }
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        Debug.Log("CONNECTED");
        joinChat.SetActive(false);
        chatClient.Subscribe(new string[] { "RegionalChannel" });
    }
    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs = string.Format("{0} : {1}", senders[i], messages[i]);
            chatDisplay.text += "\n" + msgs;
            Debug.Log(msgs);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        chatPanel.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
        }

        if (chatField.text != "" && Input.GetKey(KeyCode.Return)) {
            SubmitPublicChatOnClick();
        }
    }
}
