var http = require("http"); // HTTP 모듈을 가져온다.

http.createServer(function(request, response)
{
    response.writeHead(200, {"Content-Type": 'text/plain'});

    //웹페이지에 출력
    response.end("Hello world")

}).listen(8000);

console.log('server running at http://127.0.0.1:8000/');