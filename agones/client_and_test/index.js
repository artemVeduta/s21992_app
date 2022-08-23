import express from "express";
import {Test} from "./test";

const app = express();
const PORT = 8080;

const expressServer = app.listen(8080, async () => {
    console.log(`App listening on port ${PORT}`);
    new Test()
});

export default expressServer