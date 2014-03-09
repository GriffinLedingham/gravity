player1 = null
player2 = null

clients = []
rooms = []

s4 = () ->
  Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1)


guid = () ->
   s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4()

class Client
  constructor: (@socket) ->
    @guid = guid()
    @socket.on 'message', @onMessage
    @socket.on 'close', @closeSocket
    @room = null

  closeSocket: () =>
    clients.splice clients.indexOf(@),1
    if @room.player1? and @room.player1.guid is @guid
      @room.player1 = null
      @room.full = false
    else if @room.player2? and @room.player2.guid is @guid
      @room.player2 = null
      @room.full = false
    if !@room.player1? and !@room.player2?
      @room.constructor()

    printRooms()

  onMessage: (d) =>
    jsonObj = JSON.parse d
    if jsonObj.type is 'fire'
      if @room.player1? and @room.player1.guid isnt @guid
        @room.player1.socket.send d
      else if @room.player2? and @room.player2.guid isnt @guid
        @room.player2.socket.send d
    else if jsonObj.type is 'turn'
      @room.playerHit jsonObj.hit
      @room.changeTurn()
    else if jsonObj.type is 'end'
      console.log 'end'

class Room
  constructor: () ->
    @player1 = null
    @player2 = null
    @full = false
    @curTurn = 1
    @Health1 = 10
    @Health2 = 10

  addPlayer: (client) =>
    if not @player1?
      @player1 = client
      @startGame()
      #console.log 'starting'
    else if not @player2?
      @player2 = client
      @full = true

  startGame: () =>
    console.log 'starting'
    @player1.socket.send JSON.stringify {type:'auth',mine:'true',number:'one'}
    #@player2.socket.send JSON.stringify {type:'auth',mine:'false',number:'two'}

  changeTurn: () =>
    if @curTurn is 1
      @curTurn = 2
      @player1.socket.send JSON.stringify {type:'turn',mine:'false'}
      #@player2.socket.send JSON.stringify {type:'turn',mine:'true'}
    else if @curTurn is 2
      @curTurn = 1
      @player1.socket.send JSON.stringify {type:'turn',mine:'true'}
      #@player2.socket.send JSON.stringify {type:'turn',mine:'false'}
  playerHit: (hit) =>
    if hit is 'one'
      @Health1 -= 1
    else if hit is 'two'
      @Health2 -= 1
    console.log 'Player 1 Health: ' + @Health1
    console.log 'Player 2 Health: ' + @Health2

ws = require('ws').Server
wss = new ws({port: 8142})

wss.on 'connection', (socket) ->
  client = new Client socket
  clients.push client


  if rooms.length is 0
    room = new Room
    room.addPlayer client
    rooms.push room
    client.room = room
  else
    foundRoom = false
    for i in [0..rooms.length-1]
      if !rooms[i].full
        rooms[i].addPlayer client
        client.room = rooms[i]
        foundRoom = true
        break
    if !foundRoom
      room = new Room
      room.addPlayer client
      rooms.push room
      client.room = rooms[i]
  printRooms()

printRooms = () ->
  console.log '\n\n\n'
  console.log 'Rooms: '
  for i in [0..rooms.length-1]
    console.log '======Room '+i+'========='
    if(rooms[i].player1?)
      console.log '| Player1: ' + rooms[i].player1.guid
    else
      console.log '| Player1: empty'
    if(rooms[i].player2?)
      console.log '| Player2: ' + rooms[i].player2.guid
    else
      console.log '| Player2: empty'
    console.log '==============='
