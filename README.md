# Winbox Stats Checker

## Overview

Winbox Stats Checker is a command-line application designed to collect system statistics on Windows systems. It captures and stores CPU, RAM, and HDD usage data in an SQLite database. This data is organized by date and time, and a new database is created each month. This tool is ideal for scheduled tasks or cron jobs to monitor system performance over time.

## Features

- Collects system statistics: CPU, RAM, and HDD usage.
- Stores data in an SQLite database.
- Organizes data by date and time.
- Automatically creates a new database each month.
- Lightweight and designed for use in scheduled tasks.

## Usage

### Installation

1. Download the Winbox Stats Checker executable.
2. Place it in a directory of your choice.

### Running the Application

To use the application, simply execute it with no parameters:

```bash
WinboxStatsChecker.exe
```
This will run the application and capture system statistics. If you want to check the application's version, you can use the --version parameter:

```bash
WinboxStatsChecker.exe --version
```
The --version parameter will display the application version.

### Data Storage

The application stores system statistics in an SQLite database located in the current directory. A new database is created at the beginning of each month to keep data organized.

### Scheduling

To regularly capture and store system statistics, consider setting up a scheduled task or cron job to run the application at specified intervals.

## Requirements

- Windows operating system.

## License

This application is open-source and distributed under the [MIT License](LICENSE).

## Contributing

Feel free to contribute to this project by creating issues or pull requests on the GitHub repository.

For more detailed information and usage instructions, please refer to the documentation in the repository's [docs](docs) directory.

## Contact

For any questions or issues, please raise an issue on this repo.

Enjoy using Winbox Stats Checker to monitor your system's performance!
