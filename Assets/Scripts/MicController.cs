using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class MicController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public GameObject RedMicNot;
    public GameObject YellowMicNot;
    void Start()
    {


       
        if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
        {
            Debug.LogWarning("momom0");
            GameManager.gm.ManageRollingDice[2].transform.parent.GetComponentInChildren<Toggle>().interactable = true;
        }
        if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[0].UserId)
        {
            Debug.LogWarning("momom1");
            GameManager.gm.ManageRollingDice[0].transform.parent.GetComponentInChildren<Toggle>().interactable = true;
        }
        // Add similar conditions for other pieces if necessary


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleMicRed()
    {

        photonView.RPC("ToggleRed", RpcTarget.All);
    }


    public void toggleMicYellow()
    {
        photonView.RPC("ToggleYellow", RpcTarget.All);

    }

    [PunRPC]
    void ToggleRed()
    {
        if (RedMicNot.activeSelf)
        {
            RedMicNot.SetActive(false);

        }
        else
        {
            RedMicNot.SetActive(true);
        }

    }

    [PunRPC]
    void ToggleYellow()
    {
        if (YellowMicNot.activeSelf)
        {
            YellowMicNot.SetActive(false);

        }
        else
        {
            YellowMicNot.SetActive(true);
        }

    }

    public void toggleMic(Recorder rc)
    {
        if (rc.RecordingEnabled)
        {
            rc.RecordingEnabled = false;

        }
        else
        {
            rc.RecordingEnabled = true;
        }
    }
}
