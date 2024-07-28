using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class MicController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        Debug.Log(GameManager.gm.ManageRollingDice[2].GetComponent<PhotonView>().IsMine + "momo");
        Debug.Log(GameManager.gm.ManageRollingDice[0].GetComponent<PhotonView>().IsMine + "momo99");
        if (GameManager.gm.ManageRollingDice[2].GetComponent<PhotonView>().IsMine)
        {
            Debug.LogWarning("momom0");
            GameManager.gm.ManageRollingDice[2].transform.parent.GetComponentInParent<Toggle>().interactable = true;
        }
        if (GameManager.gm.ManageRollingDice[0].GetComponent<PhotonView>().IsMine)
        {
            Debug.LogWarning("momom1");
            GameManager.gm.ManageRollingDice[0].transform.parent.GetComponentInParent<Toggle>().interactable = true;
        }
        // Add similar conditions for other pieces if necessary


    }

    // Update is called once per frame
    void Update()
    {
        
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
