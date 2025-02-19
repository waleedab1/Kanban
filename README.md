# Kanban Project

## Overview
This is a Kanban board application built using C# with a GUI designed in XAML and SQLite as the database. The application allows users to create, manage, and track tasks across different stages of a project.

## Features
- Create, update, and delete tasks
- Drag-and-drop functionality to move tasks between columns
- Persistent task storage using SQLite
- User-friendly GUI built with XAML
- Supports multiple Kanban boards

## Technologies Used
- **Programming Language:** C#
- **GUI Framework:** XAML (WPF or UWP)
- **Database:** SQLite

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/waleedab1/Kanban.git
   ```
2. Navigate to the project directory:
   ```sh
   cd Kanban
   ```
3. Open the solution file (`.sln`) in Visual Studio.
4. Restore dependencies:
   ```sh
   dotnet restore
   ```
5. Build and run the application:
   ```sh
   dotnet run
   ```

## Database Setup
SQLite is used as the database for storing tasks. The database file (`kanban.db`) is automatically generated in the application's directory.

## Usage
1. Launch the application.
2. Create a new Kanban board.
3. Add tasks to the board and assign them to different stages (To Do, In Progress, Done).
4. Drag and drop tasks between columns.
5. Save and reload the board data, which persists in the SQLite database.
