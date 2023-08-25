# file-sync-win

Windows based data synchronization tool for Winlink-based messages.

## Overview

This is an exploratory project using WPF and .NET Framework 4.x to create a Windows-based data synchronization tool for Winlink-based messages.

The goal is to create a tool that can be used to synchronize messages between a Winlink account and a local file system, although simple file tracking could be configured to monitor any file types at any valid file path (mounted paths are not guaranteed safe).

The prime directive of this tool is to synchronize Winlink message content from multiple LAN-connected computers, and then prep the data to be stored in a database of some type.

Initially the database will be something like SQLite, but that could change based on the needs of the end-users of this software.

The timeline of this project is not set, other than I _hope_ to have something stable and usable before May 2024.

## Requirements

- Windows 7 or later
- Dot NET Framework 4.7 or later
- Build tools (Visual Studio 2022 or later)
- ASP.NET Core 6.0 (for Web API)

## Build

Use Visual Studio 2022 or later to build the solution.

## Dependencies

- Dot NET Framework 4.7.2
- Autofac v7.1.0.0
- Caliburn.Micro v3.2.0

See Project Properties and References in the Solution Explorer tree or in the '.proj' file.

## UI Descriptions

More details to come as the project progresses.

### Desktop

- Logging is automatic and can be turned on or off. For now during early development it will be on. The Log file will be stored in the same directory as the executable (dev: `bin\Debug').
- The Main UI window displays an existing configuration stored in Environment Variables when Load Configuration is clicked.
- Main UI has a Clear Configuration button - for future use.
- Main UI has a Store Configuration button that will save manually-entered configuration items. This is a future feature.
- Main UI Server-side settings are for future use and are not utilized at this time.
- File List UI is a child view that will appear when a configuration has the LOADed or SET.
- File List UI has Start and Stop buttons to control the File Monitoring process.

### Web

- Web UI accepts a POST request with a JSON body containing a collection of records with named elements.
- Response will be either a 200 OK or 400 Bad Request.
- Future: Controller sends received, validated data to the data interface.

Example JSON Body in POST request:

```json
[
  {
    "bibNumber": 138,
    "action": "IN",
    "bibTimeOfDay": 1713,
    "dayOfMonth": 11,
    "location": "WR"
  },
  {
    "bibNumber": 187,
    "action": "IN",
    "bibTimeOfDay": 1713,
    "dayOfMonth": 11,
    "location": "WR"
  }
]
```

### Startup

Since this is a multiple-project solution, in the future the Startup configuration must select both the Desktop and Web projects to run.

For now, select either API or Desktop as the Startup project when running the Solution.

## Usage

1. Build the project.
1. Either Run in Visual Studio Debug mode, or run the EXE file directly ensuring any DLLs are in the same path as the EXE for example all within 'bin/debug' directory.
1. Create new ENVIRONMENT VARIABLES: `FSW_FILEPATH`, `FSW_FILETYPE`, and `FSW_SERVERADDR`
1. LOAD the configuration.
1. START the File Monitoring process.
1. Copy files into the configured PATH.
1. Review the Logfile to see the file name(s) that were detected.

```powershell
> dir env:
...
FSW_FILEPATH                   D:\filemon\messages
FSW_FILETYPE                   *.mime
FSW_SERVERADDR                 "localhost:6001"
...
```

## Implemented Features

- Text-based logging for debugging purposes.
- Import environment variables to set File Monitoring and Database Server settings.
- File Monitoring captures filenames of new files only.
- Implemented UI with menu, basic buttons, and a status bar.
- Model, View, ViewModel architecture to better manage user interface and interactions.
- Inversion of Control (IoC, Dependency Injection) to decrease coupling between components and better manage instance lifecycles.
