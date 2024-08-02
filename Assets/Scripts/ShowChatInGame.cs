using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class ShowChatInGame : MonoBehaviourPunCallbacks
{
    // Reference to the TMP_InputField
    public TMP_InputField inputField;

    // Reference to the TMP_Text that will display the input text
    public TMP_Text displayText;

    public Image img;

    // Reference to the Send Button
    public Button sendButton;

    void Start()
    {
        // Ensure the inputField, displayText, and sendButton are assigned
        if (inputField == null || displayText == null || sendButton == null)
        {
            Debug.LogError("Please assign the inputField, displayText, and sendButton in the inspector.");
            return;
        }

        // Add a listener to the Send Button to call the OnSendButtonPressed method when clicked
        sendButton.onClick.AddListener(OnSendButtonPressed);
    }

    // Method to handle the Send Button press
    void OnSendButtonPressed()
    {
        photonView.RPC("sendMessage", RpcTarget.All);
    }


    [PunRPC]
    void sendMessage()
    {
        img.gameObject.SetActive(true);
        // Update the displayText with the inputField text
        displayText.text = inputField.text;

        // Start the coroutine to hide the text after 2 seconds
        StartCoroutine(HideTextAfterDelay());
    }

    // Coroutine to hide the text after a delay
    IEnumerator HideTextAfterDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Clear the displayText and inputField
        displayText.text = string.Empty;
        inputField.text = string.Empty;

        // Hide the displayText object
        img.gameObject.SetActive(false);
    }
}
