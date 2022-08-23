import AgonesSDK from '@google-cloud/agones-sdk';

export class Server {
    constructor() {
        this.agonesSDK = new AgonesSDK();
        this.playerCapacity = 100;
    }

    async connect() {
        try {
            await this.agonesSDK.connect();
            this.agonesSDK.health(this.onHealthCheckError);
            await this.agonesSDK.watchGameServer(this.watchGameServer, this.onError)
            await this.agonesSDK.setLabel('game', 'agones-s21992-game')
            await this.agonesSDK.ready();
            await this.agonesSDK.alpha.setPlayerCapacity(this.playerCapacity);
        } catch (e) {
            await this.shutdown()
        }
    }

    async watchGameServer(result) {
        console.log('Current server capacity:', result.status.players.count)
        console.log('Current server players:', result.status.players.capacity)

        switch (result.event) {
            case 'LOCATION':
                console.log(`Received new location: x: ${result.event.data.x}, y: ${result.event.data.y}`)
                break
        }
    }

    async shutdown() {
        try {
            await this.agonesSDK.shutdown();
            await this.agonesSDK.close();
            process.exit(0);
        } catch (e) {
            process.exit(0);
        }
    }

    async onHealthCheckError() {
        await this.shutdown()
    }

    async onError() {
        await this.shutdown()
    }
}