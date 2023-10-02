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

    public string idUrl;                //경로 주소 설정
    public string postUrl;
    public string resDataUrl;
    public string startConstructionUrl;
    public string checkConstructionUrl;

    public Button btnGetExample;
    public Button btnPostExample;
    public Button btnResDataExample;
    public Button btnConstruction_Post;
    public Button btnConstruction_Get;

    public int id;
    public string data;

    private void Start()
    {
        this.btnPostExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, postUrl);      //URL 주소 생성
            Debug.Log(url);

            var req = new Protocols.Packets.req_data();                         //Req 프로토콜 데이터 입력
            req.cmd = 1000;
            req.id = id;
            req.data = data;
            var json = JsonConvert.SerializeObject(req);                        //json형식의 문자열로 직렬화
            Debug.Log(json);
            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject< Protocols.Packets.common>(raw);   //역직렬화 시켜 json데이터를 c#코드로 변환
                Debug.LogFormat("{0}, {1}", res.cmd, res.message);         
            }));

        });

        this.btnGetExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, idUrl);      //URL 주소 생성
            Debug.Log(url);

            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);       //JSON 변환

                Debug.LogFormat("{0}, {1}", res.cmd, res.message);          //디버그로그로 서버에서 보내준것 확인
            }));

        });

        this.btnResDataExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, port, resDataUrl);      //URL 주소 생성
            Debug.Log(url);

            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.res_data>(raw);       //JSON 변환
                foreach(var user in res.result)
                {
                    Debug.LogFormat("{0}, {1}", user.id, user.data);          //디버그로그로 서버에서 보내준것 확인
                }
                
            }));
        });

        this.btnConstruction_Post.onClick.AddListener(() =>             //건설 시작 POST 통신 
        {
            var url = string.Format("{0}:{1}/{2}", host, port, startConstructionUrl);      //URL 주소 생성
            Debug.Log(url);

            var req = new Protocols.Packets.req_data();                                 //프로토콜을 만들어준다. 
            req.cmd = 1000;
            req.id = id;
            req.data = data;
            var json = JsonConvert.SerializeObject(req);
            Debug.Log(json);

            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
                Debug.Log(res);
            }));
        });

        this.btnConstruction_Get.onClick.AddListener(() =>              //건설 확인 GET 통신
        {
            var url = string.Format("{0}:{1}/{2}", host, port, checkConstructionUrl);      //URL 주소 생성
            Debug.Log(url);

            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
                Debug.LogFormat("{0}, {1}", res.cmd, res.message);
            }));
        });
    }

    private IEnumerator GetData(string url, System.Action<string> callback)
    {
        var webRequest = UnityWebRequest.Get(url);                //유니티 함수 UnityWebRequest의 Get
        yield return webRequest.SendWebRequest();                   //통신이 돌아올때 까지 코루틴 대기

        Debug.Log("-->" + webRequest.downloadHandler.text);

        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||   //커넥션 Error 이거나
            webRequest.result == UnityWebRequest.Result.ProtocolError)      //프로토콜 Error 일경우
        {
            Debug.Log("서버 통신 에러");
        }
        else  //에러가 없을 경우
        {
            callback(webRequest.downloadHandler.text);
        }
    }

    private IEnumerator PostData(string url, string json, System.Action<string> callback)
    {
        var webRequest = new UnityWebRequest(url , "POST");/*"POST": "POST"는 HTTP 요청 메서드 중 하나인 POST 메서드를 나타냅니다. 
                                                                POST 메서드는 서버로 데이터를 보낼 때 사용되며, 주로 데이터를 생성 또는 수정하는 데 사용됩니다. 
                                                                이 부분에서는 웹 리소스에 데이터를 POST하는 요청을 생성하고 있습니다.*/

        var bodyRaw = Encoding.UTF8.GetBytes(json);   
        /* 인코딩은 정보를 다른 형식으로 바꾼다는 말이다. Encoding.UTF8.GetBytes(json) 코드는 JSON 문자열을 바이트 배열로 변환하는 작업을 수행하며, 이렇게 변환된 데이터가 웹 요청의 본문으로 서버에 전송됩니다.
         * 데이터의 압축이나 압축 해제는 여기서 직접 수행되지 않습니다.*/

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||   //커넥션 Error 이거나
           webRequest.result == UnityWebRequest.Result.ProtocolError)      //프로토콜 Error 일경우
        {
            Debug.Log("서버 통신 에러");
        }
        else  //에러가 없을 경우
        {
            Debug.LogFormat("{0}\n{1}\n{2}", webRequest.responseCode, webRequest.downloadHandler.data, webRequest.downloadHandler.text);
            callback(webRequest.downloadHandler.text);
        }

        webRequest.Dispose();       //연결 해제 (없으면 메모리 누수)
    }





/*비동기(Asynchronous)와 동기(Synchronous)는 프로그래밍 및 컴퓨터 과학에서 중요한 개념입니다.

동기(Synchronous):

실행 순서: 동기적인 코드 실행은 위에서 아래로 순차적으로 진행됩니다.한 작업이 시작하면 완료될 때까지 다음 작업은 기다려야 합니다.

차단(Blocking): 동기 코드는 한 작업이 진행되는 동안 다른 작업들은 대기 상태에 있어야 합니다.이로 인해 다른 작업들이 차단(blocking) 되는 현상이 발생할 수 있습니다.


간단함: 동기 코드는 일반적으로 순차적이며, 작업의 흐름을 이해하기 쉽고 예측하기 쉽습니다.


비동기 (Asynchronous):


실행 순서: 비동기 코드는 여러 작업이 동시에 진행될 수 있습니다. 한 작업이 시작되면 다른 작업들을 차단하지 않고 계속해서 진행할 수 있습니다.

차단 해제 (Non-blocking): 비동기 코드는 차단되지 않으므로 다른 작업들이 자유롭게 실행될 수 있습니다.이로 인해 시스템 전체적으로 효율적으로 동작할 수 있습니다.

복잡성: 비동기 코드는 여러 작업이 동시에 실행될 수 있기 때문에 코드의 복잡성이 증가할 수 있습니다. 작업이 언제 완료될지 예측하기 어려울 수 있습니다.

예를 들어, 웹 애플리케이션에서 비동기적인 AJAX 요청을 사용하면 웹 페이지의 일부를 다운로드하거나 서버와 통신하는 동안에도 다른 작업을 계속할 수 있습니다. 이것은 웹 페이지가 빠르게 응답하고 사용자 경험을 향상시키는 데 도움이 됩니다.반면 동기적인 요청은 요청이 완료될 때까지 다른 작업을 차단하므로 사용자 경험이 떨어질 수 있습니다.


비동기 및 동기 프로그래밍은 각각의 장단점이 있으며, 상황에 따라 적절한 방법을 선택하는 것이 중요합니다.*/

}
