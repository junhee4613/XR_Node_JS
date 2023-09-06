var http = require("http"); // HTTP 모듈을 가져온다.

http.createServer(function(request, response)
{
    response.writeHead(200, {"Content-Type": 'text/plain'});

    //웹페이지에 출력
    response.end("Hello world")

}).listen(8000);
//컨트롤 + c는 서버 끊기,  연결 node + 해당 파일명 입력하면 서버 구축!
console.log('server running at http://127.0.0.1:8000/');