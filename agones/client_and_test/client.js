export class GameClient {
    constructor(agonesSDK, clientId, gameName) {
        this.agonesSDK = agonesSDK
        this.clientId = clientId
        this.gameName = gameName
    }

    getClientId() {
        return this.clientId
    }

    async connect() {
        await this.agonesSDK.connect();
        await this.agonesSDK.setLabel('game', this.gameName)
        await this.agonesSDK.alpha.playerConnect(this.clientId);
    }

    async disconnect() {
        await this.agonesSDK.playerDisconnect(this.clientId);
    }

    async sendRandomLocation() {
        await this.agonesSDK.send('LOCATION', { x: Math.random(), y: Math.random() })
    }
}