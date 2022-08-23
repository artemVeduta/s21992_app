import {OP_CODE_LOCATION_MESSAGE} from "./messages";

export class Server {
    constructor() {
        this.configuration = {
            pingIntervalTime: 30000,
            maxPlayers: 100
        }
        this.session = null
        this.sessionGameLoopTickTime = 1000
        this.logger = null
        this.startTime = null
        this.activePlayers = 0;
        this.onProcessStartedCalled = false;
    }

    getConfiguration() {
        return this.configuration
    }

    getTimeNowTimestamp() {
        return Math.round(new Date().getTime()/1000);
    }

    // Handle Initialize GameServer
    init(session) {
        this.session = session;
        this.logger = session.getLogger();
    }

    // Handle Server start
    onProcessStarted(args) {
        this.onProcessStartedCalled = true;
        this.logger.info("Server is ready");
        return true;
    }

    // Handle new game session
    onStartGameSession(gameSession) {
        this.startTime = this.getTimeNowTimestamp();
        this.onSessionGameLoop();
    }

    // Handle terminated process by GameLift
    async onProcessTerminate() {
        this.logger.info('Terminate process by GameLift')
        const outcome = await this.session.processEnding();
        this.logger.info("Completed process ending with: " + outcome);
        process.exit(0);
    }

    // Check process is healthy, for test = always healthy
    onHealthCheck() {
        return true;
    }

    // Handle new player connection + validation
    onPlayerConnect(connectMsg) {
        // connect all players without validation
        return true;
    }

    // Handle Player is accepted into the game
    onPlayerAccepted(player) {
        this.activePlayers++;
    }

    // Handle Player Disconnect
    onPlayerDisconnect(peerId) {
        this.activePlayers--;
    }

    // Handle a message (events) to the server
    onMessage(gameMessage) {
        switch (gameMessage.opCode) {
            case OP_CODE_LOCATION_MESSAGE: {
                console.log(`Received new location: x: ${gameMessage.payload.x}, y: ${gameMessage.payload.y}`)
                break;
            }
        }
    }

    // Handle send message from server to player and validate message
    onSendToPlayer(gameMessage) {
        // always true -> server can send all events to client
        return true
    }

    // Handle send message from server to players group and validate message
    onSendToGroup(gameMessage) {
        // always true -> server can send all events to players group (no groups for test cases)
        return true;
    }

    // Handle join player to group
    onPlayerJoinGroup(groupId, peerId) {
        // always true -> (no groups for test cases)
        return true;
    }

    // Handle leave player from group
    onPlayerLeaveGroup(groupId, peerId) {
        // always true -> (no groups for test cases)
        return true;
    }

    onSessionGameLoop() {
        this.logger.info('Session Game Loop Called')
        setTimeout(this.onSessionGameLoop, this.sessionGameLoopTickTime);
    }
}