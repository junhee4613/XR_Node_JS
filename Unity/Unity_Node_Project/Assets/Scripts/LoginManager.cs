using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;        //유저 필드
    public InputField passwordInput;        //패스워드 필드
    public Text loginStatuText;

    public string token;

    private const string apiUrl = "http://localhost:3000";  //Node.js 주소

    public void Login()
    {
        StartCoroutine(AttemptLogin(usernameInput.text, passwordInput.text));
    }

    IEnumerator AttemptLogin(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using(UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl + "/login", form))
        {
            yield return webRequest.SendWebRequest();
            //서버로 부터 데이터를 성공적으로 받아오지 못했을 때(아래 구문)
            if(webRequest.result != UnityWebRequest.Result.Success)
            {
                loginStatuText.text = "Login failed : " + webRequest.error;
            }
            else
            {
                loginStatuText.text = "Login successful!";
                string responseText = webRequest.downloadHandler.text;
                //JSON 응답에서 토큰 값을 추출
                var responseData = JsonConvert.DeserializeObject<ResponseData>(responseText);
                token = responseData.token;

                Debug.Log("Login successful! Token : " + token);

            }
        }
    }

    public void SendAytienticatedRequest(string endpoint)
    {
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Token is missing");
            return;
        }

        UnityWebRequest www = UnityWebRequest.Get(apiUrl + endpoint);
        www.SetRequestHeader("Authorization", token);

        StartCoroutine(SendRequest(www));
    }
    IEnumerator SendRequest(UnityWebRequest webRequest)
    {
        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("서버 통신 에러 " + webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Request successful" + webRequest.downloadHandler.text);
        }
    }

    public void SendAuthenticateReauestToProtectedEndpoin()
    {
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Token is missing");
            return;
        }

        SendAytienticatedRequest("/protected");
    }
    [System.Serializable]
    private class ResponseData 
    {
        public string token;
    }

}
