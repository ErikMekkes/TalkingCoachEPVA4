# EPVA4

This project is based on [TalkingCoach](https://github.com/ruuddejong/TalkingCoach), a Unity based software framework to create a virtual character which can speak in a natural language. This project focuses on extending that framework by implementing lip-syncing functionalities in it.

## Dependencies

This project depends on eSpeak NG version 1.49.2 or later. To properly run this project, you need eSpeak NG installed on your system and the espeak-ng executable on your path as 'espeak-ng'.

Test if eSpeak NG is correctly installed by opening a terminal and executing: `espeak-ng --version`. It should respond with something like this:

```
$ espeak-ng --version
eSpeak NG text-to-speech: 1.49.2  Data at: C:\Program Files\eSpeak NG\/espeak-ng-data
```

Download eSpeak-NG here: https://github.com/espeak-ng/espeak-ng