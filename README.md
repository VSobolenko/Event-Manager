# Unity Version  
**2021.3.11f1**  

# Settings  
To configure the server, enter the server address in the **Url Server** field of the `EventService` object.  

For testing, you can use a free server:  
➡ [Post Test Server](https://posttestserver.dev/)  

You can modify the `cooldownBeforeSend` and `serverUrl` parameters in the `EventService` object.  

Server settings, including request handling, sending data, and saving methods, are defined in the `EventService` object.  

### Example:  
```csharp
private void Awake()
{
    var server = new ServerProvider(urlServer);
    var save = new FileProvider();

    _eventCore = new EventCore(server, save, cooldownBeforeSend);
}
