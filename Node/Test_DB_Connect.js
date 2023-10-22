require('dotenv').config();  

const mysql = require('mysql');

//MySQL 연결 설정

const connection = mysql.createConnection({
    host: process.env.DB_HOST,
    port: process.env.DB_PORT,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME,
});

//MySQL 연결 설정
connection.connect((err) => {
    if(err){
        console.error("MYSQL 연결 오류 : " + err.stack);
        return;
    }

    console.log("연결되었슴다. 연결 ID : " + connection.threadId);

});
const test = 123;
connection.query('INSERT INTO leehan_account (id, password) VALUES (?, ?)', [test, 123456],(err, results) =>{
    if(err) throw err;
    console.log('실행');
});


connection.end((err) => {
    if(err){
        console.error('MYSQL 연결 종료 오류 : ' + err.stack);
        return;
    }
    console.log('MySQL 연결이 성공적으로 종료되었습니다.');
});