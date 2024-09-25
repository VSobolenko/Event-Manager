# Unity version
2021.3.11f1

# Settings
To configure the server, in the Url Server field, enter the address of the server, the EventService object<br>
You can use the free server for testing: https://posttestserver.dev/<br>
It is possible to change the cooldownBeforeSend and serverUrl of the EventService object<br>

Server settings, how and where to send, how to save are set in the EventService object. Example below
```
    private void Awake()
    {
        var server = new ServerProvider(urlServer);
        var save = new FileProvider();

        _eventCore = new EventCore(server, save, cooldownBeforeSend);
    }
``` 
