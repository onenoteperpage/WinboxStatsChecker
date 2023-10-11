using System;
using System.Diagnostics;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Linq;

namespace WinboxStatsChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("--version"))
            {
                Console.WriteLine("Winbox Stats Checker v1.0.0");
                return;
            }
            else if (args.Length > 0)
            {
                Console.WriteLine("Winbox Stats Checker\n  Usage:\n  Winbox Stats Checker --version    Shows current version");
                return;
            }


            string currentYearMonth = DateTime.Now.ToString("yyyyMM");
            string dbName = $"{currentYearMonth}.sqlite";

            // Create a database connection
            using (var connection = new SQLiteConnection($"Data Source={dbName};Version=3;"))
            {
                connection.Open();

                // Create tables for RAM and hard drives
                CreateTable(connection, "RAM");
                CreateTable(connection, "CPU");

                // Get RAM usage
                double currentRamUsage = GetRamUsage();

                // Insert records into RAM table
                DateTime currentTime = DateTime.Now;
                InsertRecord(connection, "RAM", currentTime, currentRamUsage);

                // Delay to allow CPU counter to update
                Thread.Sleep(1000);

                // Get and record average CPU usage over a period
                double avgCpuUsage = GetAverageCpuUsage();

                CreateTable(connection, "CPU");
                InsertRecord(connection, "CPU", currentTime, avgCpuUsage);

                // Get information about all drives
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        string driveLetter = drive.Name.Substring(0, 1);
                        double usedSpacePercentage = 1.0 - ((double)drive.TotalFreeSpace / drive.TotalSize);

                        CreateTable(connection, $"{driveLetter}_Drive");
                        InsertRecord(connection, $"{driveLetter}_Drive", currentTime, usedSpacePercentage);
                    }
                }
            }
        }

        private static void CreateTable(SQLiteConnection connection, string tableName)
        {
            using (var command = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS [{tableName}] (Timestamp DATETIME, Value REAL)", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void InsertRecord(SQLiteConnection connection, string tableName, DateTime timestamp, double value)
        {
            using (var insertCommand = new SQLiteCommand($"INSERT INTO [{tableName}] (Timestamp, Value) VALUES (@timestamp, @value)", connection))
            {
                insertCommand.Parameters.AddWithValue("@timestamp", timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                insertCommand.Parameters.AddWithValue("@value", value);
                insertCommand.ExecuteNonQuery();
            }
        }

        private static double GetRamUsage()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue();
        }

        private static double GetAverageCpuUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            float firstValue = cpuCounter.NextValue();
            Thread.Sleep(1000); // Sleep for 1 second
            float secondValue = cpuCounter.NextValue();
            float avgCpuUsage = (secondValue - firstValue) / Environment.ProcessorCount;
            return avgCpuUsage;
        }
    }
}
