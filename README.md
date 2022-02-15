# Unity version
2020.3.15f2

# Settings
For example, there is a Main scene where you can send data<br>
To configure the server, in the Url Server field, enter the address of the server, the EventService object<br>
You can use the free server for testing: http://ptsv2.com<br>
It is possible to change the cooldown BeforeSend of the EventService object<br>

If there are problems with the server, you can use the console as a temporary test, for this you need to change to console when creating an EventCore. Example below
```
    private void Awake()
    {
        IServerProvider<string> server = new ConsoleProvider<string>(); // Change provider
        ISaveProvider<EventData> save = new FileProvider();
        
        _eventCore = new EventCore(server, save);
        _eventCore.EnableEventTimer(cooldownBeforeSend);
    }
``` 
