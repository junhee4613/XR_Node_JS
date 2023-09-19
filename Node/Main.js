var http = require("http"); // HTTP 모듈을 가져온다.

http.createServer(function(request, response)
{
    response.writeHead(200, {"Content-Type": 'text/plain'});

    //웹페이지에 출력
    response.end("Hello world")

}).listen(8000);
//컨트롤 + c는 서버 끊기,  연결 node + 해당 파일명 입력하면 서버 구축!
console.log('server running at http://127.0.0.1:8000/');

//npm init -y를 터미널에 치면 package.json(파일)이 생긴다. 주의: 옵션을 명확하게 해줘야 된다.
//npm은 모듈(module) 자체가 아니라, Node.js 프로젝트에서 모듈과 패키지를 관리하는 도구.
//js파일을 생성할 땐 파일명.js라고 하면 된다.
//Node.js에서 node는 JavaScript 코드를 실행하는 명령어입니다. node 명령어를 사용하여 Node.js 환경에서 JavaScript 파일을 실행할 수 있습니다.
//npm install express를 터미널에 입력해주면 Express.js 웹 프레임워크(모듈)를 설치하게 된다. 이건 http형태의 통신을 만들어주는 모듈
