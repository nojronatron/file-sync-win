# file-sync-win

Windows based data synchronization tool for Winlink-based messages.

## Overview

This is an exploratory project using WPF and ASP.NET.

Goals:

- Create a tool that can be used to synchronize message data between a Winlink Express instance and another computer.
- A 'Server' service that listens for listing of bib reports, recording them to a log file.
- A 'Client' portion that watches a directory and, whenever a new file is created, parses the content, logs the bib data, and sends it to the Server service.

When this project is ready, a presentation will be made to the interested parties for feedback and to start planning for potential use.

## Project Status

- Core functionality is working.
- Additional UI tweaks are in the works for MVP release.
- In the short term, publish a proof of concept and basic functionality (MVP) for demonstration purposes to potential end-users.
- Distribution: Limited. ClickOnce publishing of the client application and the server service.
- Mid term: Develop a solution based on this MVP concept that incorporates new and/or updated features based on feedback, and a better designed repository and project for the longer-term.
- Before end of May 2024: Publish a well tested client app and server service with a minimum set of features ready for BigFoot 2024.

## Usage and Behavior

### Simple Usage Walkthrough

1. Download the Published App (ClickOnce Published folder -- TBD) to a directory on the computer that has Winlink Express installed (Client).
1. Run the installer (setup.exe) from the ClickOnce Publish folder (TBD).
1. Find the directory where Winlink Express stores its message files and copy the path.
1. Run the Executable and enter that message path into the Sync Tool UI.
1. A separate folder will have the service executable 'FileSyncAPI.exe' along with some DLL files. Run to host the Server component.
1. Back in the Sync Tool UI, enter the Server's IP address. The Default port number should be fine.
1. Click 'Set' to open the File Monitor Window. Click 'Start' to start monitoring for files; 'Stop' to stop it.
1. Server: When the bib data is received, it will be logged to a file in the same directory as 'FileSyncAPI.exe', as well as to a console window.

### Other Behavior Expectations

- Only newly created files in the configured path will be detected and processed.
- Newly created files that do not contain bib data will NOT be processed in any way.
- The _Server address and port configuration_ is completely optional.
- The Client will still function without them but will only record data to the local log file.
- The Server service can run stand-alone without the Client, but it will only receive data from a Client.

### Security and Data Privacy

- Avoid directly connecting your computer(s) to the Internet.
- Always use a router with a firewall and only allow the inbound and outbound traffic that is absolutely necessary.
- This application does NOT use authentication. In the future this feature may be added.
- This application does NOT use encryption, and only the HTTP endpoint is available on the web service.
- To allow host-to-host communications, you will need to open TCP and UDP ports on your server computer's firewall.

#### Firewall TCP and UDP Ports

Open Windows Firewall Advanced Settings and create 2 new rules, one for TCP, and one for UDP, in the Public profile (or All Profiles):

- TCP: Allow inbound traffic on port 5000.
- UDP: Allow inbound traffic on port 5001.
- Program: `FileSyncAPI.exe`

To help you diagnose connection issues, turn on Advanced Logging for the Windows Firewall to capture both types of actions: `Allow` and `Block`.

## Project Structure

### Server Side

- Web server for receiving and processing data from one or more clients.
- Data processing to validate the data and format it for storage.
- Data storage component for saving to a log file, and potentially a database.
- SwaggerUI is included for testing the API in Debug mode. available at `server-address:port/swagger/index.html`
- See [JSON Body Format](#json-body-format) below.
- Currently, no user interface is planned.

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
- Deployment/Installation: Whatever directory each EXE is installed to.

## Requirements

### Run

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

- Autofac v7.1.0.0
- Caliburn.Micro v3.2.0
- Swashbuckle.AspNetCore (Swagger) v6.5.0
- Newtonsoft.Json v13.0.0.0

NuGet Package Mappings might be required to successfully restore packages.

See Project Properties and References in the Solution Explorer tree or in the Project file.

### Build

- Build tools (Visual Studio 2022 or later)
- Dot NET Framework 4.7 (for Desktop and user interface)
- ASP.NET Core 6.0 (for Web API)
- Set Startup Configuration following the instructions in the [Startup](#startup) section below.
- Refresh NuGet packages is _required_ to build and debug the solution.

### Startup

Since this is a multi-project solution, the Startup configuration must be done in a particular order:

1. FileSyncAPI (ASP.NET)
2. FileSyncDesktop.Library (Dot NET Library)
3. FileSyncDesktop (WPF)

### Usage

1. Build the project.
1. Either Run in Visual Studio Debug mode, or run the EXE file directly ensuring any DLLs are in the same path as the EXE for example all within 'bin/debug' directory.
1. Desktop UI: Enter the filepath where messages are stored, and the server IP address (or 'localhost') and click SET.
1. Desktop UI: Click START Monitoring Files button to trigger the File Monitoring process.
1. Copy files with (and without) valid and invalid bib number entries into the configured PATH.
1. Review the Files Detected window to see the detected file names.
1. Review the Log file to see the results of the File Monitoring process including errors, notification, and bib numbers.

### Environment Variables

This feature will no longer be supported in the MVP.

```powershell
> dir env:
...
FSW_FILEPATH                   D:\filemon\messages
FSW_FILETYPE                   *.mime
FSW_SERVERADDR                 "localhost:6001"
...
```

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
