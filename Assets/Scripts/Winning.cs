using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Winning : MonoBehaviour
{
    public GameObject WinningScreen;

    public GameObject RedWinner;
    public GameObject BlueWinner;
    public GameObject GreenWinner;
    public GameObject YellowWinner;


    public GameObject WinnerList;

    byte position = 1;

    byte RedPosition = 0;
    byte GreenPosition = 0;
    byte YellowPosition = 0;
    byte BluePosition = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.gm.ManageRollingDice[1].isAllowed && GameManager.gm.blueCompletePlayers == 4)
        {
            GameObject WinningTag = GameManager.gm.ManageRollingDice[1].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            BluePosition = position;

            GameObject op = Instantiate(BlueWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManager.gm.BluePlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            position++;
            GameManager.gm.ManageRollingDice[1].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;

        }
        else if (GameManager.gm.ManageRollingDice[0].isAllowed && GameManager.gm.redCompletePlayers == 4)
        {
            GameObject WinningTag = GameManager.gm.ManageRollingDice[0].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(RedWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManager.gm.RedPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            RedPosition = position;
            position++;
            GameManager.gm.ManageRollingDice[0].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;
        }
        else if (GameManager.gm.ManageRollingDice[2].isAllowed && GameManager.gm.yellowCompletePlayers == 4)
        {
            GameObject WinningTag = GameManager.gm.ManageRollingDice[2].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(YellowWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManager.gm.YellowPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            YellowPosition = position;
            position++;
            GameManager.gm.ManageRollingDice[2].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;
        }
        else if (GameManager.gm.ManageRollingDice[3].isAllowed && GameManager.gm.greenCompletePlayers == 4)
        {
            GameObject WinningTag = GameManager.gm.ManageRollingDice[3].transform.parent.GetChild(3).gameObject;
            WinningTag.SetActive(true);
            GameObject op = Instantiate(GreenWinner, WinnerList.transform);
            op.GetComponentInChildren<TMP_Text>().text = position.ToString();
            op.transform.GetChild(2).GetComponent<TMP_Text>().text = GameManager.gm.GreenPlayerName.text;
            WinningTag.GetComponentInChildren<TMP_Text>().text = position.ToString();
            GreenPosition = position;
            position++;
            GameManager.gm.ManageRollingDice[3].isAllowed = false;
            GameManager.gm.PlayerRemainingToPlay--;
        }
        if (GameManager.gm.PlayerRemainingToPlay == 1)
        {
            foreach (var k in GameManager.gm.ManageRollingDice)
            {
                if (k.isAllowed)
                {
                    if (k.name.Contains("Red"))
                    {

                        GameManager.gm.redCompletePlayers = 4;
                    }
                    if (k.name.Contains("Blue"))
                    {

                            GameManager.gm.blueCompletePlayers = 4;
                    }
                    if (k.name.Contains("Green"))
                    {
 
                            GameManager.gm.greenCompletePlayers = 4;
                    }
                    if (k.name.Contains("Yellow"))
                    {

                            GameManager.gm.yellowCompletePlayers = 4;
                    }

                }
            }

            WinningScreen.gameObject.SetActive(true);
        }
    }


    public void ReturnToHomeScreen()
    {
        PhotonNetwork.Disconnect();
        foreach (var op in GameObject.FindGameObjectsWithTag("Launcher"))
        {
            Destroy(op.gameObject);
        }

        // Load the home screen scene
        SceneManager.LoadScene("HomeScreen");
    }
}
