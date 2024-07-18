using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text msg;
    public Button submitBtn;
  
   




    public void callRegister()
    {
        StartCoroutine(LoginStart());
    }



    IEnumerator LoginStart()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username.text);
        form.AddField("password", password.text);
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1/sqlconnect/login.php", form);
        BasicUI.instance.showLoader();
        yield return www.SendWebRequest();
        BasicUI.instance.hideLoader();
        if (www.downloadHandler.text[0] == '0')
        {
            DBManager.username = username.text;
            DBManager.TotalBalance = int.Parse(www.downloadHandler.text.Split('\t')[1]);
            DBManager.MobileNumber = www.downloadHandler.text.Split('\t')[2];
            username.text = "";
            string dop = password.text;
            password.text = "";
            msg.text = "Enter Username and Password";
            this.transform.parent.gameObject.SetActive(false);
            BasicUI.instance.setUsername();
            Debug.Log(www.downloadHandler.text);
           /* uiLogin.showLogout();*/

        }
        else
        {
            msg.text = "Wrong Credentials";
            Debug.Log("UserLoginn Falied" + www.downloadHandler.text);
        }


    }


}
