const express = require("express");
const app = express();

let users =[
    {id: 0 , data: "UserData"}
];

let buildingUnderConstruction = null;

app.use(express.json());

app.get('/jh',(req, res)=>{
    let result = {
        cmd : -1,
        message : 'jh Hello'
    };
    res.send(result);
});

app.listen(5000, () =>{
    console.log('server is running at 5000 port');
})