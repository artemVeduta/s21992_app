using Quartz.Impl;
using Quartz;
using System;
using gamelift_client_and_test;

public class Program
{
    private static async Task Main(string[] args) {
        var test = new Test();
        var isTestCompleted = false;
        var currentMinute = -1;
        var currentHour = -1;
        while (!isTestCompleted)
        {
            if (currentMinute < DateTime.Now.Minute)
            {
                test.ExecuteMessages();
                currentMinute = DateTime.Now.Minute;
            }
            if (currentHour < DateTime.Now.Hour)
            {
                isTestCompleted = test.Execute();
                currentHour = DateTime.Now.Hour;
            }
        }
    }
}