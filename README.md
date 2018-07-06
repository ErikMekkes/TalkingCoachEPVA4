# EPVA4

This project is based on [TalkingCoach](https://github.com/ruuddejong/TalkingCoach), a Unity based software framework to create a virtual character which can speak in a natural language. This project focuses on extending that framework by implementing lip-syncing functionalities in it.

### EPVA4 members:
- Emma Sala
- Erik Mekkes
- Joshua Slik
- Lucile Nikkels
- Muhammed Imran Özyar

## Backend server installation
An installation package with all the required components and guidance has been included as talkingcoach_server_setup.zip

The backend server depends on eSpeak NG version 1.49.2 or later.

### Installing eSpeak NG
To properly run this project, you need eSpeak NG installed on your server and the espeak-ng executable on your path, accessible as 'espeak-ng'.

Test if eSpeak NG is correctly installed by opening a terminal and executing: `espeak-ng --version`. It should respond with something like this:

```
$ espeak-ng --version
eSpeak NG text-to-speech: 1.49.2  Data at: C:\Program Files\eSpeak NG\/espeak-ng-data
```

Download eSpeak-NG here: https://github.com/espeak-ng/espeak-ng

### Setting up the backend server
To set up the backend server, please follow these steps:

1. Make sure eSpeak NG is installed according to the instructions above
2. Open a terminal in the folder `website/backend`
3. Execute `npm install`
4. Execute `npm start` if you want to run the server now. You should do this before starting the frontend server.  
The server will run on port 3001 by default.

## Frontend server installation
To install and run the frontend server, please follow these steps:

1. Build the Unity project and place the build files so the folder structure is as such:  
```
    client/
    └── public/
        ├── unity/
        │   ├── Build/
        │   │   ├── UnityLoader.js
        │   │   ├── Build.json
        │   │   └── ...
        │   ├── TemplateData/
        │   │   └── ...
        │   ├── index.html
        │   └── ...
        ├── favicon.ico
        ├── index.html
        └── manifest.json
```
2. Open a terminal in the folder `website/client`
3. Execute `npm install`
4. Execute `npm start` if you want to run the server now. The backend server should already be running on your machine.  
The server will run on port 3000 by default.
