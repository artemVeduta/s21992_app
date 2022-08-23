const uuid = require('uuid');
const AWS = require('aws-sdk');
const GameLift = new AWS.GameLift({region: 'eu-central-1'}); // Frankfurt

const fleetID = "fleet-00aaaa00-a000-00a0-00a0-aa00a000aa0a";

const searchGameSessions = () => {
    return GameLift.searchGameSessions({
        FleetId: fleetID,
        FilterExpression: "hasAvailablePlayerSessions=true"
    }).promise()
}

const createGameSessions = () => {
    return GameLift.createGameSession({
        MaximumPlayerSessionCount: 100,   // 100 players per session
        FleetId: fleetID
    }).promise()
}

const createPlayerSession = (selectedGameSession) => {
    return GameLift.createPlayerSession({
        GameSessionId : selectedGameSession.GameSessionId ,
        PlayerId: uuid.v4()
    }).promise()
}

exports.handler = async (event) => {
    const availableGameSessionsResult = await searchGameSessions();
    // error
    if (availableGameSessionsResult.$response !== null) {
        return availableGameSessionsResult.$response
    }

    let currentGameSession;
    // Check how many game session exists
    if (availableGameSessionsResult.GameSessions.length === 0) {
        // create new session
        const createGameSessionsResult = await createGameSessions();
        if (createGameSessionsResult.$response !== null) {
            return createGameSessionsResult.$response
        }
        currentGameSession = createGameSessionsResult.GameSession
    } else {
        // get session with index 0
        currentGameSession = availableGameSessionsResult.GameSessions[0]
    }

    // check if currentGameSession IsNotNull
    if (currentGameSession === null) {
        return {
            statusCode: 500,
            body: JSON.stringify({
                message: "Unable to find game session"
            })
        }
    } else {
        // currentGameSession exists -> create player session
        const createPlayerSessionResult = await createPlayerSession(currentGameSession)
        if (createPlayerSessionResult.$response !== null) {
            return createPlayerSessionResult.$response
        }
        return {
            statusCode: 200,
            body: JSON.stringify({
                playerSessionId: createPlayerSessionResult.PlayerSession.PlayerSessionId
            })
        }
    }
}