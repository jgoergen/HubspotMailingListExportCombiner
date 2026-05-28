# Hubspot Mailinglist Export Combiner Utility

## Use
Export your mailing list results as advanced csv with all fields included, drag any number of related exports onto the executable to combine them into a single timestamped CSV file, such as `output-2026-05-28_14-30-00.csv`.

## What it does
For every export dragged onto it, it takes all rows, omits any with events that are not UNSUBSCRIBE, BOUNCE, CLICK, OPEN, DELIVERED.

Once done, it combines the list in order builds a new export with the following logic.
- If the event is bounce; always keep it.
- If the event is unsubscribe always keep it.
- If the event is click; always keep it, but remove all open and delivered events from that recipient.
- If the event is open and there isn't already a click for that recipient; keep it, but remove all other delivered events for that recipient.
- If the event is delivered and there isn't already an open for that recipient, then keep it.

## Console output while processing
- "X" = skipping, don't want this event type.
- "x" = skipping, something 'better' already exists (see above.)
- "+" = upgrading event (replacing lesser events.)
- "." = adding event.

## Requirements
[.NET 10](https://dotnet.microsoft.com/download/dotnet/10.0) is required to build from source or run the framework-dependent build output.

Use `dotnetcore31app\Build Single EXE Release.bat` to create a self-contained Windows x64 single-file executable.


This project is licensed under the terms of the MIT license.
