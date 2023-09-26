const express = require("express");
const app = express();

app.use(express.json());

app.get('/', (req, res) => {        //이 매개변수는 HTTP 요청(request)과 HTTP 응답(response)
    res.send('hello world')

});
        //위 아래는 동작은 같으나 문법이 다른것
app.get('/about', function (req, res){
    res.send('about !!!')
});

app.listen(3030, () =>{
    console.log('코드 따라적기 시작!')
});
//node:internal/modules/cjs/loader:1080
//  throw err;
//  ^
//
//Error: Cannot find module 'C:\Users\82106\Main.js'
//    at Module._resolveFilename (node:internal/modules/cjs/loader:1077:15)
//    at Module._load (node:internal/modules/cjs/loader:922:27)
//   at Function.executeUserEntryPoint [as runMain] (node:internal/modules/run_main:86:12)
//    at node:internal/main/run_main_module:23:47 {
//  code: 'MODULE_NOT_FOUND',
//  requireStack: []
//}
//
//Node.js v18.18.0 문제 파악X 모듈은 불러온것같은데 맞나
//해결: 해당 파일 경로를 열고 실행했더니 됐음
//다른 주의점으로 npm install express를 해줘서 패키지를 다운 받아야됌