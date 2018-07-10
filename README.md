# EPVA4

This project is based on [TalkingCoach](https://github.com/ruuddejong/TalkingCoach), a Unity based software framework to create a virtual character which can speak in a natural language. This project focuses on extending that framework by implementing lip-syncing functionalities in it.

### EPVA4 members:
- Emma Sala
- Erik Mekkes
- Joshua Slik
- Lucile Nikkels
- Muhammed Imran Özyar

## Working with the product
### Unity base
The base code of the product can be found as /InteractiveAvatar as a project to import and work on with Unity. We suggest using Rider for working on the included code. Rider can automatically link code changes with Unity. Note that Unity should be set to use WebGL as intended platform for building the project.
### Phoneme Server
The implemented nodejs express phoneme server used for transcribing text to phonemes can be found as /website/backend
### React Web App
Example code for a stylish react web application that uses the product can be found as /website/client
### Simple Web page
Example code for a simple web page that uses the product with demo functions can be found as /InteractiveAvatar/Assets/WebGLTemplates/Interactive. Unity uses this as a template when Building the project, filling in the marked sections to produce a web page such as the one included as /website/client/unity.

## Live demonstration pages
A live version of the simple web page with demo functions can be viewed at : http://ii.tudelft.nl/epva4/


The phoneme server can be accessed at http://talkingcoach.ewi.tudelft.nl/api/.

For example with http://talkingcoach.ewi.tudelft.nl/api/phoneme?text=Hello%20World&lang=en-US

## Server Setup
A zip file with all the required components to set up a server for this product has been included as talkingcoach_server_setup.zip.

It includes a readme that describes the general structure / installation of the server, and a full install guide with included installation script for linux.
