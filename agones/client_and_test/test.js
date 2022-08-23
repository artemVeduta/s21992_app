import {GameClient} from "./client";
import moment from "moment";
import cron from 'node-cron';
import AgonesSDK from "@google-cloud/agones-sdk";

const amountOfPlayersByHour = [
    4800, //00:00
    3300, //01:00
    2300, //02:00
    1800, //03:00
    1300, //04:00
    1000, //05:00
    900,  //06:00
    800,  //07:00
    900,  //08:00
    1100, //09:00
    1500, //10:00
    2400, //11:00
    3400, //12:00
    4300, //13:00
    4200, //14:00
    3700, //15:00
    3600, //16:00
    3700, //17:00
    4250, //18:00
    4700, //19:00
    5400, //20:00
    5900, //21:00
    6500, //22:00
    6100, //23:00
]

export class Test {
    constructor() {
        this.gameName = 'agones-s21992-game'
        this.hourlyCronExpression = '0 * * * *' // Every 1h for update amount of connected clients
        this.minuteCronExpression = '* * * * *' // Every 1m for random send event Random Location
        cron.schedule(this.hourlyCronExpression, this.hourlyCronHandler)
        cron.schedule(this.minuteCronExpression, this.minuteCronHandler)
        this.clients = []
    }

    async hourlyCronHandler() {
        // Change amount of players every 1h
        // Get current amount of players for feature hour
        const featureAmountOfPlayersForNextHour = amountOfPlayersByHour[parseInt(this.getCurrentHour().toString())]
        // Calculate how many disconnect and how many connect
        if (this.clients.length >= featureAmountOfPlayersForNextHour) {
            const amountOfPlayersToDisconnect = this.clients.length - featureAmountOfPlayersForNextHour
            await this.removeClients(amountOfPlayersToDisconnect)
        } else {
            const amountOfPlayersToConnect = featureAmountOfPlayersForNextHour - this.clients.length
            await this.addClients(amountOfPlayersToConnect)
        }
    }

    async minuteCronHandler() {
        if (this.clients.length < 0) return
        // Random Select clients and send event Location
        for(const index of this.clients) {
            await this.clients[index].sendRandomLocation()
        }
    }


    async removeClients(amount) {
        for (let i = 0; i < amount; i++) {
            await this.clients[this.clients.length - 1].disconnect()
            this.clients.pop()
        }
    }

    async addClients(amount) {
        for (let i = 0; i < amount; i++) {
            const newGameClient = new GameClient(new AgonesSDK(), uuidv4(), this.gameName)
            await newGameClient.connect()
            this.clients.push(newGameClient)
        }
    }

    getCurrentHour() {
        return moment().hour()
    }
}