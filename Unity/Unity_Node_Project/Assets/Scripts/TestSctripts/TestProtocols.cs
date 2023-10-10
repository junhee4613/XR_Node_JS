using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProtocols
{
    public class Packets 
    {
        public class common
        {
            public int cmd;
            public string message;
        }
        public class req_data : common
        {
            public int id;
            public string name;
        }

        public class res_data : common
        {
            public req_data[] result;
        }

        public class ConstructionStatusResponse
        {
            public string message;
        }
    }
    
    
}
