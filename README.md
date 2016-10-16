# TradeDataMonitor
Compact showcase of using Plugin Architecture, MVVM in WPF, IoC, Unit Tests coverage and Mocks

The application does monitoring of specified folder in filesystem in order to track appearance of new files with data in that folder, more specifically trade data. Once the file is detected the trade data in it meant to be processed, for simpicity we are just going to dynamically update GUI view with all trades from file.
The main trick is we have an open requirement on file types being used for trade data storage, so we have to come with an extendable solution - plugin architecture. In the result we have three plugins that could process xml, csv, txt files and is open to extension with new plugins. 

So as part of the case described above you could examine a simple application that provides extendable plugin architecture, such aspects as test code coverage, dependency injection and separation of concerns (UI vs BusinessLogic) are not forgotten here too. You will find well covered codebase (MSTest and FakeItEasy), example on how to setup a IoC container (NInject), as well as elementary scaffold of MVVM pattern being implemented for GUI part (WPF).

\* Sidenote: if you are really want to incorporate plugins in a big enterprise application you should consider Managed Extensibility Framework (MEF) and Managed AddIn Framework (MAF, aka System.AddIn). These are different in extent of decoupling\isolation you'll get, for details please check http://stackoverflow.com/a/840441
