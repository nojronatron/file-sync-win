# file-sync-win

Windows based data synchronization tool for Winlink-based messages.

## Overview

This is an exploratory project using WPF and ASP.NET.

Goals:

- Create a tool that can be used to synchronize message data between a Winlink Express instance and another computer.
- Project will be a MVP (Minimum Viable Product) for demonstration purposes.

When this project is ready, a presentation will be made to the interested parties for feedback and to start planning for potential use.

## Primary Components

- A 'Server' service that listens for listing of bib reports, recording them to a log file.
- A 'Client' portion that watches a directory and, whenever a new file is created, parses the content, logs the bib data, and sends it to the Server service.

## Project Status

- Client component is a stand-alone desktop executable.
- Client UI leverages WPF (Windows Presentation Foundation) for a responsive user interface.
- Client can track a specified folder for files of the defined type (the default type is '.mime').
- Server component is a stand-alone web service.
- Build and Distribution: Limited. Utilizes ClickOnce publishing of the client application and the server service as separate executables.

## Usage and Behavior

### Simple Usage Walkthrough

This walkthrough is for a single-computer scenario.

1. Launch FileSyncApi.exe to host the Server component.
1. Look at the FileSyncApi console window to discover the Server Address and Port number.
1. Launch FileSyncDesktop.exe and enter the message path, file extension, server name (or address), and server port, and then click the SET button.
1. Click the OPEN FILE MONITOR button to open the File Monitor Window.
1. Click START MONITORING FILES to start monitoring for new files.
1. When a new file is created, FileSyncDesktop will list the filename.
1. When a new file contains "bib data", FileSyncDesktop will log that data and simultaneously send it to the configured server address and port.
1. When FileSyncApi Server receives data from the client it logs the data to the console window and to a file in the same directory as FileSyncApi.exe.

The FileSyncApi log file can be be accessed with a safe file-handle program like Notepad++ or UltraEdit for analysing the data.

### Other Behavior Expectations

- Only newly created files in the configured path will be detected and processed.
- Newly created files that do not contain bib data will NOT be processed in any way.
- The _Server address and port configuration_ must be entered into the 'Configuration' screen on the client but if the server doesn't respond, the client will continue to monitor files and record data to the log file.
- The Server service can run stand-alone without the Client, waiting to receive data only from client(s) configured to use it.
- Even though it is possible to run multiple instances of the Client only one instance of the Server should be run at a time.

### Security and Data Privacy

- Avoid directly connecting your computer(s) to the Internet.
- Always use a router with a firewall and only allow the inbound and outbound traffic that is absolutely necessary.
- This application does NOT use authentication. In the future this feature may be added.
- This application does NOT use encryption, and only the HTTP endpoint is available on the web service.
- To allow host-to-host communications, you will need to open TCP and UDP ports on your server computer's firewall.

### Firewall TCP and UDP Ports

Open Windows Firewall Advanced Settings and create 2 new rules, one for TCP, and one for UDP, in the Public profile (or All Profiles):

- TCP: Allow inbound traffic on all ports to FileSyncAPI.exe.
- UDP: Allow inbound traffic on all ports to FileSyncAPI.exe.

You will need to enter the full path to `FileSyncApi.exe` in the rule configuration.

Diagnose connection issues:

1. Turn on Advanced Logging for the Windows Firewall to capture both types of actions: `Allow` and `Block`. 
2. Run the Client and Server until data is passed from Client to Server.
3. Review the log to see if any traffic is being blocked.

## Project Structure

### Server Side

- Web server for receiving and processing data from one or more clients.
- Data processing to validate the data and format it for storage.
- Data storage component for saving to a log file, and potentially a database.
- SwaggerUI is included for testing the API in Debug mode, and can be accessed at `server-address:port/swagger/index.html`
- See [JSON Body Format](#json-body-format) below.
- Currently, no user interface is planned for MVP.

### Client Side

- User interface to allow configuring the Client and monitoring file detections.
- File monitoring component watches the configured directory for new files.
- File parsing component extracts bib data from each detected new file.
- Data processing validates message data and formats it for storage and transmission.
- Data storage component saves data to a log file.
- Data transmission component to sends data to the server.

### Both

Log files are stored in the same directory as the executable:

- Development: `bin\Debug'
- Deployment or desktop installation: Whatever directory each EXE is installed to.

## Requirements to Run

- Windows 7 or later
- Dot NET Framework 4.x Runtimes
- Dot NET 6 Runtimes (for Web API)

## Web Server Behavior

- Web UI accepts a POST request with a JSON body containing a collection of records with named elements.
- Response will be either a `200 OK` or `4xx` (usually 400 Bad Request).

## JSON POST Body Format

Example JSON Body in POST request:

```json
{
    "bibRecords": [
		{
			"bibNumber": 123,
			"action": "OUT",
			"bibTimeOfDay": 1713,
			"dayOfMonth": 11,
			"location": "WR"
		},
		{
			"bibNumber": 234,
			"action": "IN",
			"bibTimeOfDay": 0913,
			"dayOfMonth": 12,
			"location": "CH"
		},
		{
			"bibNumber": 123,
			"action": "DNF",
			"bibTimeOfDay": 0003,
			"dayOfMonth": 13,
			"location": "TS"
		}
	]
]
```

See SwaggerUI in Debug mode for an interactive Schema and example.

## Development

### Dependencies

- Caliburn.Micro v3.2.0
- Swashbuckle.AspNetCore (Swagger) v6.5.0
- Newtonsoft.Json v13.0.0.0
- Build tools (Visual Studio 2022 or later)
- Dot NET Framework 4.7 (for Desktop and user interface)
- ASP.NET Core 6.0 (for Web API)

NuGet Package Mappings might be required to successfully restore packages.

See Project Properties and References in the Solution Explorer tree or in the Project file.

### Build

1. Clone the repository.
1. Open the solution in Visual Studio 2022 or later.
1. Configure Solution Startup to Multiple. See the [Startup](#startup) section below.
1. Build the solution for "Any CPU".

### Publish

1. Right-click and Publish project FileSyncAPI to a Folder. Don't forget to click the PUBLISH button at the end of the configuration wizard.
1. Right-click and Publish project FileSyncDesktop to a Folder. De-select the option to automatically check for updates.
1. Execute Setup.exe to install FileSyncDesktop. The App automatically launches but will be available on the Start Menu.
1. Execute FileSyncAPI.exe in the publish folder to launch the Server.

Note: You can also choose to publish the API to a web server but that is not required for this project.

### Startup

Since this is a multi-project solution, the Startup configuration must be done in a particular order:

1. FileSyncAPI (ASP.NET)
2. FileSyncDesktop.Library (Dot NET Library)
3. FileSyncDesktop (WPF)

### Environment Variables

This feature will be supported in the MVP and, depending on feedback, may be used in v.1 of the App.

Create new System-level Environment Variables using PowerShell or the Windows Control Panel.

When done, the variables should look something like this:

```powershell
> dir env:
...
FSW_FILEPATH                   C:\Winlink Express\N0CAL\Messages
FSW_FILETYPE                   *.mime
FSW_SERVERADDR                 localhost
FSW_SERVERPORT                 5000
...
```

When you launch the App, click the LOAD button to import the variables into the App, then click SET to apply them. Once set, click OPEN FILE MONITOR to initialize the file monitoring system.

## Implemented Features

- Responsive Desktop User Interface.
- Text-based logging for debugging purposes.
- Text-based logging of received Bib Records on Server-side.
- Semi-automated import of environment variables to set File Monitoring and Server configurations.
- File Monitoring captures filenames of *created* files, as well as processed bib data on Client-side.

## Nerdy Features

- MVVM (Model, View, ViewModel) architecture to better manage user interface and interactions.
- Inversion of Control (IoC aka Dependency Injection) to decrease coupling between components, to better manage instance lifecycles, and simplify testing.
- Dot NET Framework 4.7 for Desktop UI support back to Windows 7.
- ASP.NET Core 6 Web API for receiving and properly handling REST POST request payloads.
- Collections used for in-memory storage while app is running.

## Known Issues

Although the Client UI will allow you to change the Server Address and Port number, it will not be able to connect to the Server if the File Monitoring process is already running, or has run and then was stopped. The workaround is to restart the Client application.
