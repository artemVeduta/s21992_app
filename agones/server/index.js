import {Server} from "./server";
import express from "express";

const app = express();
const PORT = 7654;

const expressServer = app.listen(PORT, async () => {
    console.log(`App listening on port ${PORT}`);
    await new Server(true).connect()
});

export default expressServer
