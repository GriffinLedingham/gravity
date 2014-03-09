var WebSocketServer = require('ws').Server
  , wss = new WebSocketServer({port: 8142});

var player1 = '';
var player2 = '';

wss.on('connection', function(ws) {
  socket.guid = guid();
  ws.send(JSON.stringify({type:'message',data:'hi'}));
  ws.on('message',function(data){
    if(data.type === 'auth')
    {
      if(player1 === '')
      {
        player1 = socket.guid;
      }
      else if(player2 === '')
      {
        player2 = socket.guid;
      }
    }
    else if(data.type === 'move')
    {

    }
  });
});

function s4() {
  return Math.floor((1 + Math.random()) * 0x10000)
             .toString(16)
             .substring(1);
};

function guid() {
  return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
         s4() + '-' + s4() + s4() + s4();
}
