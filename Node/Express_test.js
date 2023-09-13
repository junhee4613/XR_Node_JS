const express = require('express');     //express 모듈을 가져온다.
const app = express();                  //app 키워드로 express 사용

app.use(express.json());                //json 사용

app.get('/',(req , res)=>{              //경로가 이상해서 오류가 떴음 '/,' => '/'로 수정 후 잘됐음
    res.send('hello world!');
});

app.get('/about',(req , res)=>{
    res.send('about !!!');
});


app.listen(3030,()=>{
    console.log('server is runnung at 3030 port');
});