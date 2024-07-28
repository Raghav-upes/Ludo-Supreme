using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;

public class MicController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
