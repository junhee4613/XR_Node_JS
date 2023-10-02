using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocols 
{
    public class Packets
    {
        public class common
        {
            public int cmd;             //커맨트 번호
            public string message;      //메세지
        }

        public class req_data : common  //Common 상속
        {
            public int id;
            public string data;
        }
        //서버를 통신할 node에도 변수명을 같게 해야지 데이터 전달이 된다.
        public class res_data : common  //받을 배열 패킷 설정 
        {
            public req_data[] result;
        }

        public class ConstructionStatusResponse     //건설 패킷 설정
        {
            public string message;
        }

    }
}
