# file-sync-win

Windows based data synchronization tool for Winlink-based messages.

## Overview

This is an exploratory project using WPF and .NET.

The goal is to create a tool that can be used to synchronize message data between a Winlink Express instance and another computer.

The 'Server' portion of program listens for notifications and receives a listing of bib reports, recording them to a log file.

The 'Client' portion of the program watches a directory and, whenever a new file is created, parses the content and stores only bib data fields `bibNumber`, `action`, `timeOfDay`, `dayOfMonth`, and `location`.

The Client software will also finds a configured server instance and ship the parsed data to it.

The prime directive of this tool is to synchronize Winlink message content between LAN-connected computers in a hub-and-spoke type format, and then prep the data to be stored in a logfile and/or database.

## Project Status

- Core functionality is working.
- Additional UI tweaks are planned for the short term.
- In the short term, publish a proof of concept and basic functionality (MVP) for demonstration purposes to potential end-users.
- Distribution: Limited. Use ClickOnce to publish and distribute the client application and the server service.
- Mid term: Develop a solution based on this MVP concept that incorporates new and/or updated features based on feedback, and a better designed repository and project for the longer-term.
- Before end of May 2024: Publish a well tested client app and server service with a minimum set of features ready for BigFoot 2024.

## Usage and Behavior

### Simple Usage Walkthrough

1. Download the Published App (ClickOnce Published folder -- TBD) to a directory on the computer that has Winlink Express installed (Client).
1. Run the installer (setup.exe) from the ClickOne Publish folder (TBD).
1. Find the directory where Winlink Express stores its message files.
1. Run the Executable and enter the directory path into the UI (Future).
1. A separate folder will have 'FileSyncAPI.exe' along with some DLL files. Run 'FileSyncAPI.exe' to host the Server component (additional configuration might be necessary in a future revision).
1. Optional: Just run 'FileSyncAPI.exe' on any computer where you want to collect "client" data 
1. Client (required), Server (optional): Enter the Server's IP address and port number into the UI. *Note: This step may change in a future version*
1. Client: Click the 'Start' button to begin the File Monitoring process. Click 'Stop' to stop it.
1. Client: Whenever a new file is created in the configured directory, the Client UI will display the discovered filename, log the parsed entry, and send the parsed data to the configured Server. If there is no server configuration, no data will be sent.
1. Server: When the bib data is received, it will be logged to a file in the same directory as 'FileSyncAPI.exe'.

### Other Behavior Expectations

- Only newly created files in the configured path will be detected and processed.
- Newly created files that do not contain bib data will NOT be processed in any way.
- The _Server address and port configuration_ is completely optional. The Client will still function normally without them but will only record data to the local log file.

### Security and Data Privacy

- Avoid directly connecting your computer(s) to the Internet.
- Always use a router with a firewall and only allow the inbound and outbound traffic that is absolutely necessary.
- This application does NOT use authentication. In the future this feature may be added.
- This application does NOT use encryption. By default, Microsoft Hosting provides a web server with both HTTP and HTTPS endpoints.

## Project Structure

### Server Side

- Web server for receiving and processing data from one or more clients.
- Data processing to validate the data and format it for storage.
- Data storage component for saving to a log file, and potentially a database.
- Currently, no user interface is planned.
- SwaggerUI is included for testing the API. See [JSON Body Format](#json-body-format) below.

### Client Side

- User interface to display the current configuration and status of the file monitoring process.
- File monitoring component to watch the configured directory for new files.
- File parsing component to extract bib data from each detected new file.
- Data processing to validate the data and format it for storage and transmission.
- Data storage component for saving data to a log file.
- Data transmission component to send data to the server.

### Both

Log files are stored in the same directory as the executable:

- Development: `bin\Debug'
- Deployment/Installation: Whatever directory the EXE and associated DLL files are in

## Requirements

### Run

- Windows 7 or later
- Dot NET Framework 4.x Runtimes
- Dot NET 6 Runtimes (for Web API)

## UI Descriptions

More details to come as the project progresses.

### Desktop

- Main UI: A window displays an existing configuration stored in Environment Variables when Load Configuration is clicked. This may change in a future version.
- Main UI: The Clear Configuration button removes the configuration for File Monitoring and the configured Server Address.
- Main UI: The Store Configuration button saves manually-entered configuration items. This is a future feature.
- Main UI: Server address should be like `http://localhost:5432`. During early development this is acquired through Environment Variables. This could change in a future version.
- File List UI: Appears when a configuration has be loaded or set using the LOAD or SET buttons.
- File List UI: Start and Stop buttons control the File Monitoring process. Currently, if files are created while the process is stopped, those files will _not be processed_ even when the process is restarted.

### Web

- SwaggerUI is available at `server-address:port/swagger/index.html`.

### Web Non UI Components

- Web UI accepts a POST request with a JSON body containing a collection of records with named elements.
- Response will be either a `200 OK` or `4xx` (usually 400 Bad Request. Do not expect additional information in the response body or headers.

### JSON Body Format

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
- Refresh NuGet packages is _required_ see [Dependencies](#dependencies) below)

### Startup

Since this is a multiple-project solution the Startup configuration must be done in a particular order:

1. FileSyncAPI (ASP.NET)
2. FileSyncDesktop.Library (Dot NET Library)
3. FileSyncDesktop (WPF)

## Usage

1. Build the project.
1. Either Run in Visual Studio Debug mode, or run the EXE file directly ensuring any DLLs are in the same path as the EXE for example all within 'bin/debug' directory.
1. Create new ENVIRONMENT VARIABLES: `FSW_FILEPATH`, `FSW_FILETYPE`, and `FSW_SERVERADDR`
1. Desktop UI: Click LOAD to load the configuration.
1. Desktop UI: Click START Monitoring Files button to trigger the File Monitoring process.
1. Copy files with (and without) valid and invalid bib number entries into the configured PATH.
1. Review the Files Detected window to see the detected file names.
1. Review the Log file to see the results of the File Monitoring process including errors, notification, and bib numbers.

Example ENVIRONMENT VARIABLES:

```powershell
> dir env:
...
FSW_FILEPATH                   D:\filemon\messages
FSW_FILETYPE                   *.mime
FSW_SERVERADDR                 "localhost:6001"
...
```

_Note_: FSW_SERVERADDR must include the port number. In the UI, the Server Address/Name and Port are individual fields.

## Implemented Features

- Responsive Desktop User Interface.
- Text-based logging for debugging purposes.
- Text-based logging of received Bib Records on Server-side.
- Semi-automated import of environment variables to set File Monitoring and Server configurations.
- File Monitoring captures filenames of *created* files, as well as processed bib data on Client-side.
- 
### Nerdy Features

- MVVM (Model, View, ViewModel) architecture to better manage user interface and interactions.
- Inversion of Control (IoC aka Dependency Injection) to decrease coupling between components, to better manage instance lifecycles, and simplify testing.
- Dot NET Framework 4.7 for Desktop UI support back to Windows 7.
- ASP.NET Core 6 Web API for receiving and properly handling REST POST request payloads.
- Collections used for in-memory storage while app is running.
