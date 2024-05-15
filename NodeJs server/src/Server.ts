import * as net from "net"
import mongoose from "mongoose";
import { mongo } from "./database";
import Message from "./models/Message";

let databaseConfig = new mongo();

const PORT = 8080;

// List of all players searching for a room
let players: Array<IPlayer> = [];
let searchRoom: Map<String, IPlayer> = new Map<String, IPlayer>();
let playersPaired = 0;
let playersOffline = 0;

export interface IPlayer
{
    id: string;
    conexion: net.Socket;
}

// List of all available instructions
let actions = 
{
    MESSAGE_FROM_SERVER_1: "MESSAGE_FROM_SERVER_1",
    MESSAGE_FROM_SERVER_2: "MESSAGE_FROM_SERVER_2",
    MESSAGE_FROM_SERVER_3: "MESSAGE_FROM_SERVER_3",
    MESSAGE_FROM_SERVER_N: "MESSAGE_FROM_SERVER_N"
};

// Making the web server
let server = net.createServer(socket =>
    {
        const uuid = require('uuid').v4;
        let id = uuid();
        let connected = false;
        
        const message = new Message();
        console.log(`${socket.remoteAddress} is trying to connect...`); // Remote IP
        console.log(`Device connected with id: ${id}`); // Assigned ID
        
        socket.write(Buffer.from(`id: ${id}`, "utf-8"));
        message._id = id;
        //message.save(function (err: any) {
        //    if (err) return handleError(err);
            // saved!
        //    });
        connected = true;

        /**** Process to receive data from clients ****/
        socket.on("data", async data =>
        {
            try
            {
                let jsonData = JSON.parse(data.toString());
                console.log("\n*********************************");
                console.log(jsonData);
                switch (jsonData.command)
                {
                    case actions.MESSAGE_FROM_SERVER_1:
                        console.log(jsonData);
                        break;

                    case actions.MESSAGE_FROM_SERVER_2:
                        try
                        {
                            message._id              = jsonData._id;
                            message.objectAttribute1 = jsonData.objectAttribute1;
                            message.objectAttribute2 = jsonData.objectAttribute2;
                            message.objectAttribute3 = jsonData.objectAttribute3;
                            
                            players.forEach( (playerSocket) => {
                                if(playerSocket.id != jsonData._id)
                                    playerSocket.conexion.write(Buffer.from(data.toString(), "utf-8"));
                            });

                            console.log(jsonData);
                        }

                        catch(dataSaveError)
                        {
                            console.log(dataSaveError);
                        }
                        break;

                    case actions.MESSAGE_FROM_SERVER_3:
                        try
                        {
                            console.log(jsonData);
                            
                            players.forEach( (playerSocket) =>
                            {
                                if(playerSocket.id != jsonData.playerEditor)
                                    playerSocket.conexion.write(Buffer.from(data.toString(), "utf-8"));
                            });
                        }
                        catch(dataSaveError)
                        {
                            console.log(String(dataSaveError));
                        }
                        break;
                        
                    default:
                        console.log("The received command is not recognized");
                        break;
                }

                jsonData = "";
            }
            catch(dataSyncError)
            {
                console.log(data.toString());
                console.log(dataSyncError);
            }
        });

        socket.on('error', (error)=>
        {
            console.log(error.message);
        });

        /**** Process to close socket connection ****/
        socket.on("close", () => 
        {
            socket.end();
            players.forEach( (playerSocket) => {
                playerSocket.conexion.write(Buffer.from(`{"command": "PLAYER_OFFLINE"}`, "utf-8"));
            });
            console.log(`Connection with ${socket.remoteAddress} : ${socket.remotePort} closed.`);
            playersOffline--;
        });
});


/**** Process to start server ****/
server.listen(PORT, () =>
{
    console.log("Conecting to mongo database... ");
    //mongoose.connect(`mongodb:\/\/${databaseConfig.host}/${databaseConfig.db}`);
    console.log("Successful connection.");
    console.log("Server is running on port: " + PORT);
    console.log("Waiting for users...");

    if(searchRoom.size >= 1)
    {
        // List of players
        searchRoom.forEach(j => 
        {
            players.push(j);
        });
        
        players.forEach( (player) => {
            player.conexion.write(Buffer.from(`{"command": "PLAYER_JOINED"}`, "utf-8"));
        });
    }
});

function handleError(err: any) {
    throw new Error("Error saving data.");
}