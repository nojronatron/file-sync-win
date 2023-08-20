# file-sync-win

Windows based data synchronization tool for Winlink-based messages.

## Overview

This is an exploratory project using WPF and .NET Framework 4.x to create a Windows-based data synchronization tool for Winlink-based messages.  The goal is to create a tool that can be used to synchronize messages between a Winlink account and a local file system.

The prime directive of this tool is to synchronize Winlink message content from multiple LAN-connected computers, and then prep the data to be stored in a database of some type.

Initially the database will be something like SQLite, but that could change based on the needs of the end-users of this software.

The timeline of this project is not set, other than I _hope_ to have something stable and usable before May 2024.

## Requirements

- Windows 7 or later
- Dot NET Framework 4.7.2 or later
- Build tools (Visual Studio 2022 or later)

## Build

Use Visual Studio 2022 or later to build the solution.

## Dependencies

See Project Properties and References in the Solution Explorer tree or in the '.proj' file.

## Usage

- Logging is automatic and can be turned on or off. For now during early development it will be on. The Log file will be stored in the same directory as the executable (dev: `bin\Debug').
- The Main UI window will request the location of Winlink Message files.
- The Main UI window will request the address of a 'Database Server' for file storage.

More details to come as the project progresses.
