# Demo 2 - Worker Service

1. Open Visual Studio 2019.
2. Choose **Create a new project**.
3. Choose **ASP.NET Core Web Application**.
4. Name it **WorkerSample** and save the project in a folder of your preference.
5. Look for the **Worker Service** template and select it.
6. Double click on the **Worker.cs** file and showcase to the audience the **ExecuteAsync()** method, which is included in a loop.
7. Press F5 to launch the project. It will be launched like a console app. Highlight that the project has automatically been injected with the correct logger implementation, which is sending the output to the console. If, for example, the worker was running as a Windows service, it would have logged the information in the Event Viewer. We're going to see more in the next demo.
8. Remind the audience about the cross-platform nature of this worker: it could be deployed as a Windows service, as a web job on Azure, inside a container, etc. In the next demo, we're going to see a real example.