using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager gm;
    public RollingDice dice;

    public int numberOfStepsToMove;
    public bool canPlayerMove = true;
    public bool canDiceRoll = true;
    public bool transferdice = false;
    public bool selfDice = false;

    public int redOutPlayers = 4;
    public int greenOutPlayers = 4;
    public int blueOutPlayers = 4;
    public int yellowOutPlayers = 4;

    public int redCompletePlayers;
    public int greenCompletePlayers;
    public int blueCompletePlayers;
    public int yellowCompletePlayers;

    public GameObject RedRollDiceHome;
    public GameObject BlueRollDiceHome;
    public GameObject YellowRollDiceHome;
    public GameObject GreenRollDiceHome;


    public PlayerPiece[] bluePlayerPiece;
    public PlayerPiece[] redPlayerPiece;
    public PlayerPiece[] greenPlayerPiece;
    public PlayerPiece[] yelloPlayerPiece;

    public int totalPlayerCanPlay;

    public RollingDice[] ManageRollingDice;

    List<PathPoint> playerOnPathPointList = new List<PathPoint>();



    public GameObject Board;

    public GameObject OrangeCanvasTemp;
    public GameObject RedCanvasTemp;
    public GameObject LudoPath;
    public GameObject LudoHome;

    private Coroutine diceTimerCoroutine;
    private void Awake()
    {
        gm = this;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            BlueRollDiceHome.SetActive(false);
            GreenRollDiceHome.SetActive(false);
            HidePlayers(GameManager.gm.bluePlayerPiece);
            HidePlayers(GameManager.gm.greenPlayerPiece);
            GameManager.gm.totalPlayerCanPlay = 2;


            if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
            {

                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
                for (int i = 0; i < 4; i++)
                {
                    Debug.Log("lp0");
                    redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);

                }
            }
            /* if (pieceTypeName == "RedPiece")
             {
                 TransferOwnership(player, 1);
                 TransferOwnership(player, 7);
             }*/

        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {

            GameManager.gm.totalPlayerCanPlay = 4;


            if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
            {

                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
                for (int i = 0; i < 4; i++)
                {

                    redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    bluePlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
                    greenPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);

                }
            }
            else
            if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[2].UserId)
            {

                boardSetUP(0);
            }
            else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[3].UserId)
            {
                boardSetUP(0);
                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
                for (int i = 0; i < 4; i++)
                {
                    redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
                    yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
                    bluePlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
                    greenPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);

                }
            }
            /* if (pieceTypeName == "RedPiece")
             {
                 TransferOwnership(player, 1);
                 TransferOwnership(player, 7);
             }*/

        }
     
        
    }

    private void Start()
    {
        /*  GameManager.gm.dice = GameManager.gm.ManageRollingDice[0];*/
        diceTimerCoroutine=StartCoroutine(DiceTimer());
    }
    void HidePlayers(PlayerPiece[] playerPieces)
    {
        for (int i = 0; i < playerPieces.Length; i++)
        {
            playerPieces[i].gameObject.SetActive(false);
        }
    }
    /*
        void Start()
        {
            // Find all GameObjects in the scene
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            Debug.Log("lplpp");
            // List to store game objects with Rigidbody component
            List<GameObject> objectsWithRigidbodies = new List<GameObject>();

            // Iterate through all game objects and check for Rigidbody component
            foreach (GameObject obj in allObjects)
            {
                if (obj.GetComponent<Rigidbody>() != null)
                {
                    objectsWithRigidbodies.Add(obj);
                    Debug.Log("Found Rigidbody on: " + obj.name);
                }
            }

            // Optionally, do something with the list of objects with Rigidbody components
            // For example, print the names of these objects
            foreach (GameObject obj in objectsWithRigidbodies)
            {
                Debug.Log("GameObject with Rigidbody: " + obj.name);
            }
        }*/




    public void AddPathPoint(PathPoint pathPoint)
    {
        playerOnPathPointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playerOnPathPointList.Contains(pathPoint))
        {
            playerOnPathPointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path Not found to be romved");
        }
    }


    public void RollingDiceManager()
    {
       
        if (GameManager.gm.transferdice)
        {
            if (GameManager.gm.numberOfStepsToMove != 6)
            {
                ShiftDice();
            }

            /* for(int i = 0; i < 4; i++)
             {
                 if (i == 3)
                 {
                     nextDice = 0;
                 }
                 else
                 {
                     nextDice = i + 1;
                 }
                 if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[i]){

                     GameManager.gm.ManageRollingDice[i].gameObject.SetActive(false);
                     GameManager.gm.ManageRollingDice[nextDice].gameObject.SetActive(true);
                 }
             }*/
            GameManager.gm.canDiceRoll = true;
        }
        else
        {
            if (GameManager.gm.selfDice)
            {
                GameManager.gm.selfDice = false;
                GameManager.gm.canDiceRoll = true;
                GameManager.gm.SelfRoll();
            }
        }
    }

    public void SelfRoll()
    {
        if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.dice == GameManager.gm.ManageRollingDice[2])
        {
            Invoke("roled", 0.6f);
        }
    }

    void roled()
    {
        GameManager.gm.ManageRollingDice[2].mouseRoll();
    }

    [PunRPC]
    void getYellow()
    {
        GameManager.gm.ManageRollingDice[0].gameObject.SetActive(false);
        GameManager.gm.ManageRollingDice[2].gameObject.SetActive(true);
        GameManager.gm.dice = GameManager.gm.ManageRollingDice[2];
        diceTimerCoroutine = StartCoroutine(DiceTimer());
    }

    [PunRPC]
    void getRed()
    {

        GameManager.gm.ManageRollingDice[0].gameObject.SetActive(true);
        GameManager.gm.ManageRollingDice[2].gameObject.SetActive(false);
        GameManager.gm.dice = GameManager.gm.ManageRollingDice[0];
        diceTimerCoroutine = StartCoroutine(DiceTimer());
    }

    void ShiftDice()
    {
        int nextDice;
        if (GameManager.gm.totalPlayerCanPlay == 1)
        {
            if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
            {
                GameManager.gm.ManageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.ManageRollingDice[2].gameObject.SetActive(true);
                passout(0);
                GameManager.gm.ManageRollingDice[2].mouseRoll();
            }
            else
            {

                GameManager.gm.ManageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.ManageRollingDice[2].gameObject.SetActive(false);
                passout(2);

            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 2)
        {
            if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[0])
            {/*
                if(photonView.IsMine)*/
                photonView.RPC("getYellow", RpcTarget.AllBuffered);
                passout(0);
            }
            else
            {
                /* if (photonView.IsMine)*/
                photonView.RPC("getRed", RpcTarget.AllBuffered);
                passout(2);

            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }
                i = passout(i);
                if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[i])
                {

                    GameManager.gm.ManageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.ManageRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }
                i = passout(i);
                if (GameManager.gm.dice == GameManager.gm.ManageRollingDice[i])
                {
                    photonView.RPC("changeoDice", RpcTarget.All, i, nextDice);
                }
            }
        }
   
    }

    [PunRPC]
    void changeoDice(int i, int nextDice)
    {

        GameManager.gm.ManageRollingDice[i].gameObject.SetActive(false);
        GameManager.gm.ManageRollingDice[nextDice].gameObject.SetActive(true);
    }

    int passout(int index)
    {
        if (index == 0)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 1)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 2)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        else if (index == 3)
        {
            if (GameManager.gm.redCompletePlayers == 4)
            {
                return index + 1;
            }
        }
        return index;
    }



    public void boardSetUP(int number)
    {
        if (number == 0)
        {
            Board.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoPath.transform.localEulerAngles = new Vector3(0, 0, -90f);
            LudoHome.transform.localEulerAngles = new Vector3(0, 0, -90f);
            OrangeCanvasTemp.transform.localEulerAngles = new Vector3(0, 0, 0);
            RedCanvasTemp.transform.localEulerAngles = new Vector3(0, 180, 0);

            for (int i = 0; i < 4; i++)
            {
                redPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                yelloPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                bluePlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
                greenPlayerPiece[i].gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

            var temp = RedRollDiceHome.transform.localPosition;
            var rot = RedRollDiceHome.transform.localEulerAngles;
            GameObject klp = new GameObject();
            GameObject klp1 = new GameObject();
            GameObject klp2 = new GameObject();
            GameObject klp3 = new GameObject();
            klp1.transform.parent = klp.transform;
            klp2.transform.parent = klp.transform;
            klp3.transform.parent = klp.transform;
            ExchangeProperties(klp.transform, RedRollDiceHome.transform);
            RedRollDiceHome.transform.localPosition = GreenRollDiceHome.transform.localPosition;
            RedRollDiceHome.transform.localEulerAngles = GreenRollDiceHome.transform.localEulerAngles;
            ExchangeProperties(RedRollDiceHome.transform, GreenRollDiceHome.transform);
            GreenRollDiceHome.transform.localPosition = YellowRollDiceHome.transform.localPosition;
            GreenRollDiceHome.transform.localEulerAngles = YellowRollDiceHome.transform.localEulerAngles;
            ExchangeProperties(GreenRollDiceHome.transform, YellowRollDiceHome.transform);
            YellowRollDiceHome.transform.localPosition = BlueRollDiceHome.transform.localPosition;
            YellowRollDiceHome.transform.localEulerAngles = BlueRollDiceHome.transform.localEulerAngles;
            ExchangeProperties(YellowRollDiceHome.transform, BlueRollDiceHome.transform);
            BlueRollDiceHome.transform.localPosition = temp;
            BlueRollDiceHome.transform.localEulerAngles = rot;
            ExchangeProperties(BlueRollDiceHome.transform, klp.transform);
            Destroy(klp.gameObject);
        }
    }

    public void ExchangeProperties(Transform parent1, Transform parent2)
    {


        for (int i = 0; i < parent1.childCount && i < parent2.childCount; i++)
        {
            Debug.Log(parent1.GetChild(i).name);
            parent1.GetChild(i).localPosition = parent2.GetChild(i).localPosition;
            parent1.GetChild(i).localEulerAngles = parent2.GetChild(i).localEulerAngles;

        }
    }


    public void ReturnHomeScreen()
    {
        Destroy(GameObject.FindGameObjectWithTag("Launcher"));

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("HomeScreen");

    }


    private IEnumerator DiceTimer()
    {
        Debug.LogWarning("Pink");
        Debug.LogWarning(GameManager.gm.dice);

        // Get the Timer child with the Image component
        Debug.LogWarning(GameManager.gm.dice.transform.parent.name);
        Transform timerTransform = GameManager.gm.dice.transform.parent.GetChild(0).GetChild(0).Find("Timer");
        if (timerTransform == null)
        {
            Debug.LogError("Timer not found!");
            yield break;
        }

        Image timerImage = timerTransform.GetComponent<Image>();

       
        if (timerImage == null)
        {
            Debug.LogError("Image component not found on Timer!");
            yield break;
        }

        // Start the timer
        timerImage.fillAmount = 0f;
        float duration = 20f;
        float elapsedTime = 0f;
        timerImage.fillAmount = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            timerImage.fillAmount = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        timerImage.fillAmount = 1f;
        Debug.LogWarning(GameManager.gm.dice);
        photonView.RPC("RestartDiceTimer", RpcTarget.All);

        ShiftDice();
   
    }

    [PunRPC]
    public void PlayTimer()
    {
        diceTimerCoroutine = StartCoroutine(DiceTimer());
    }

    [PunRPC]
    public void RestartDiceTimer()
    {
        if (diceTimerCoroutine != null)
        {
            StopCoroutine(diceTimerCoroutine);
            Transform timerTransform = GameManager.gm.dice.transform.parent.GetChild(0).GetChild(0).Find("Timer");
            Image timerImage = timerTransform.GetComponent<Image>();
            timerImage.fillAmount = 0;
        }
       /* diceTimerCoroutine = StartCoroutine(DiceTimer());*/
    }
}
