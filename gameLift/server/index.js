const {Server} = require("./server");

const gameServer = new Server()

exports.ssExports = {
    configuration: gameServer.getConfiguration(),
    init: gameServer.init,
    onProcessStarted: gameServer.onProcessStarted,
    onMessage: gameServer.onMessage,
    onPlayerConnect: gameServer.onPlayerConnect,
    onPlayerAccepted: gameServer.onPlayerAccepted,
    onPlayerDisconnect: gameServer.onPlayerDisconnect,
    onSendToPlayer: gameServer.onSendToPlayer,
    onSendToGroup: gameServer.onSendToGroup,
    onPlayerJoinGroup: gameServer.onPlayerJoinGroup,
    onPlayerLeaveGroup: gameServer.onPlayerLeaveGroup,
    onStartGameSession: gameServer.onStartGameSession,
    onProcessTerminate: gameServer.onProcessTerminate,
    onHealthCheck: gameServer.onHealthCheck
};