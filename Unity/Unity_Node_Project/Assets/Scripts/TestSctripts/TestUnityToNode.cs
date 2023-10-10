using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class TestUnityToNode : MonoBehaviour
{
    public string host;
    public int port;

    public string idUrl;
    public string postUrl;
    public string startConstructionUrl;
    public string checkConstructructionUrl;

    public Button btnGetExample;
    public Button btnPostExample;
    public Button btnResDataExample;
    public Button btnConstruction_Post;
    public Button btnConstruction_Get;

    public int id;
    public string test_name;


    // Start is called before the first frame update
    void Start()
    {
        this.btnPostExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, postUrl);

            var req = new TestProtocols.Packets.req_data();
            req.cmd = 1000;
            req.id = id;
            req.name = test_name;
            var json = JsonConvert.SerializeObject(req);

            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
            }));
        });
    }

    IEnumerator GetData(string url, System.Action<string> callback)
    {
        var webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("서버통신 에러");
        }
        else
        {
            callback(webRequest.downloadHandler.text);
        }
    }
    IEnumerator PostData(string url, string json, System.Action<string> callback)
    {
        using (var webRequest = new UnityWebRequest(url, "POST"))
        {
            var bodyRaw = Encoding.UTF8.GetBytes(json);

            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("서버통신 에러");
            }
            else
            {
                callback(webRequest.downloadHandler.text);
            }
        }

            

    }
    
}
