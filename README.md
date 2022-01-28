# Unity version
2020.3.15f2

# Settings
Для примера имеется сцена Main где можно отправить данные<br>
Для настройки сервера необходимо в поле Url Server прописать адрес сервера, объекта EventService<br>
Можно использовать бесплатный сервер для теста: http://ptsv2.com<br>
Емеется возможность изменять cooldownBeforeSend, объекта EventService<br>

Если имеются проблемы с сервером можно использовать консоль в качетве временного теста, для этого необходимо при создании EventCore (это происходит в EventService классе), где мы передаём провайдер сервера изменить на консоль. Пример ниже
```
    private void Awake()
    {
        IServerProvider<string> server = new ConsoleProvider<string>(); //  Изменить провайдер 
        ISaveProvider<EventData> save = new FileProvider();
        
        _eventCore = new EventCore(server, save);
        _eventCore.EnableEventTimer(cooldownBeforeSend);
    }
``` 

# Implementation
Вся логика вынесена отдельно от MonoBehaviour, тем самым мы не зависим от него<br>
Класс EventCore не зависит как и куда отправлять данные. Его задача в нужное время в нужном месте сказать что нужно сделать (сохранить/отправить)<br>
Класс EventService является "точкой входа" для EventCore<br>
