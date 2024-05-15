# Local Server for Unity
This repository describes how to configure and use a local server for Unity using Typescript and NodeJs.

The project is divided in two sections: the local server configuration (_NodeJs server folder_) and the app design (_App folder_). In each folder you will find complete instructions on how to configure all the necessary modules, but it's important to know that you have to satisfies the following:

## Requirements
### [Server](./"NodeJs server"/README.md)
- **Ubuntu 20.04 or later**. The operating system is irrelevant for running the server. You can perform the tests on Linux, Windows, or Mac; however, the setup instructions will be developed for this Ubuntu version.
- **NodeJs**. It is used to facilitate server functions such as reading and sending messages.
- **Typescript**. Programming language selected to program the functionalities and operations of the server.
- **Mongoose**. Library for *Node.js* that facilitates the execution of queries to a MongoDB database.
- **Typegoose**. Library based on mongoose that facilitates the integration of its syntax using Typescript.
- **MongoDB**. NoSQL database system used to store the objects created in the mobile application.
- **JSON**. Object notation used for sending messages.
### [App](./App/README.md)
- **Unity**. Cross-platform game engine used to create the mobile app.
- **.NET**. Platform used for message control between the server and the mobile application.

**Note:** In each section you will be guided to install each requirement listed above.

<!-- ## Table of contents

* [Preview](#preview)
  * [Android devices](#android-devices)
  * [iOS devices](#ios-devices)
* [Features](#features)
* [Download project](#download-project)
  * [App](#app)
  * [Server](#server)

## Preview

### Android devices

### iOS devices

## Features

## Download project

## Requirements
NodeJS:
- Version minima: 18.0.3

```sudo apt install nodejs```

NPM 
- Version minima: 18.0.3

```sudo apt install npm```


npm i typescript ts-node-dev -D

-->



