# OpenDataPathsWindow

A Unity editor extension for quickly opening the `Application.persistentDataPath` and `Application.dataPath` directories in your file explorer.

## Features

- **Open Directories**: Open `Application.persistentDataPath` and `Application.dataPath` directly from the Unity Editor.
- **Custom Explorer Path**: Set a custom path to an alternate file explorer application.
- **Dynamic Window Height**: Adjusts the editor window height based on the visibility of the alternate explorer options.
- **Platform Support**: Handles different platforms (Windows, macOS, Linux) for opening file explorers.

## Installation

1. Open your Unity project.
2. Open the Unity Package Manager (`Window` > `Package Manager`).
3. Click the `+` button and select `Add package from git URL...`.
4. Enter the following URL: https://github.com/glurth/OpenDataPathsWindow.git
5. Click `Add`.

## Usage

1. After installation, you can open the window via the Unity Editor menu: `File` > `Open Data Paths`.
2. The window provides two buttons for opening the `Application.persistentDataPath` and `Application.dataPath` directories.
3. Optionally, you can configure an alternate file explorer path using the gear icon. The path can be set and saved, and the window will dynamically resize based on this configuration.

### GUI Overview

- **Path Display**: Shows the current paths for `persistentDataPath` and `dataPath`.
- **Open Buttons**: Opens the respective directories in the file explorer.
- **Gear Icon**: Toggles visibility of the alternate explorer path configuration.
- **Path Configuration**: Allows setting and validating an alternate explorer application path.

## Screenshots

![Editor Window](path/to/screenshot.png)

## Troubleshooting

- **Unsupported Platform**: If your platform is not supported for opening file explorers, a warning message will be displayed.
- **File Does Not Exist Warning**: An exclamation mark icon will appear next to the alternate explorer path if the specified file does not exist.

## Contribution

Feel free to contribute to this project by opening issues or submitting pull requests.
