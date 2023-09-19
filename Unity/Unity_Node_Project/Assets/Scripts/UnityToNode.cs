using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class UnityToNode : MonoBehaviour
{
    public string host;                 //127.0.0.1
    public int port;                    //3030

    public string idUri;                //경로 주소 설정
    public string postUrli;

    public Button btnGetExample;
    public Button btnPostExample;

    public int id;
    public string data;

    private void Start()
    {
        this.btnPostExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, idUri);       //URL 주소 생성        //여기 안에 {변수 순번이 들어감} Ex) 0번이 들어가면 host변수에 들어가 있는 데이터가 들어감. 중괄호를 안쓰면 그냥 0:1/2로 찍힘
            Debug.Log(url);

            var req = new Protocols.Packets.req_data();                         //Req 프로토콜 데이터 입력
            req.cmd = 1000;
            req.id = id;
            req.data = data;
            var json = JsonConvert.SerializeObject(req);                    //json형식의 문자열로 직렬화  20230920
            Debug.Log(json);

            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocols.Packets.common res = 
                JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);               //역직렬화 시켜 json데이터를 c#코드로 변환 20230920
                Debug.LogFormat("{0}, {1}", res.cmd, res.message);      //디버그로그로 서버에서 보내준 것 확인
            }));
        });
        this.btnGetExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, idUri);          //URL 주소 생성
            Debug.Log(url);

            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);         //Json 변환

                Debug.LogFormat("{0}, {1}", res.cmd, res.message);      //디버그로그로 서버에서 보내준 것 확인
            }));
        });
    }

    private IEnumerator GetData(string url, System.Action<string> callback)
    {
        var webRequest = UnityWebRequest.Get(url);                  //유니티 함수 UnityWebRequest의 Get
        yield return webRequest.SendWebRequest();                   //통신이 돌아올 때까지 코루틴 대기

        Debug.Log("-->" + webRequest.downloadHandler.text);

        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||   //커넥션 Error 이거나
            webRequest.result == UnityWebRequest.Result.ProtocolError)      //프로토콜 Error 일경우
        {
            Debug.Log("서버 통신 에러");
        }
        else    //에러가 없을 경우
        {
            callback(webRequest.downloadHandler.text);
        }
    }
    private IEnumerator PostData(string url, string json, System.Action<string> callback)
    {
        var webRequest = new UnityWebRequest(url, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(json);     //json인코딩 =>인코딩은 원본 데이터가 가지고 있는 용량을 줄이기 위해 데이터를 코드화하는 과정을 거쳐 최종적으로 용량을 압축하는 과정

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        /*if (webRequest.result == UnityWebRequest.Result.ConnectionError ||   //커넥션 Error 이거나
            webRequest.result == UnityWebRequest.Result.ProtocolError)      //프로토콜 Error 일경우
        {
            Debug.Log("서버 통신 에러");
        }
        else    //에러가 없을 경우
        {
            Debug.LogFormat("{0}\n[1]\n[2]", webRequest.responseCode, webRequest.downloadHandler.data, webRequest.downloadHandler.text);
            callback(webRequest.downloadHandler.text);
        }
        webRequest.Dispose();*/
    }
}
