using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: Program to Control the Multiple Functions of the Finch Robot
    // Application Type: Console
    // Author: Hockin, Noah
    // Dated Created: 2/20/2020
    // Last Modified: 4/29/2020
    //
    // **************************************************

    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DONE
    }


    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tt) Theme Settings");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "t":
                        DisplaySetTheme();
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        static void DisplaySetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;

            //
            // set current theme data
            //
            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to change the current theme [ yes / no ]?");
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    //
                    // set new theme
                    //
                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Theme");
                    Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\tIs this the theme you would like?");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                    }
                } while (!themeChosen);
            }
            
            DisplayContinuePrompt();
        }

        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tEnter a value for the {property}: ");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\tIt appears you have entered an invalid console color. Please try again.\n");
                }
                else
                {
                    validConsoleColor = true;
                }

            } while (!validConsoleColor);

            return consoleColor;
        }

        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData()
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);
        }


        static void WriteThemeData(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"Data/Theme.txt";

            File.WriteAllText(dataPath, foreground.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString());
        }


        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mixing it up!");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(myFinch);
                        break;

                    case "b":
                        DisplayDance(myFinch);
                        break;

                    case "c":
                        DisplayMix(myFinch);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
            DisplayContinuePrompt();

            finchRobot.noteOn(523);
            finchRobot.setLED(0, 0, 250);
            finchRobot.wait(250);
            finchRobot.setLED(0, 125, 125);
            finchRobot.wait(250);
            finchRobot.setLED(0, 250, 0);
            finchRobot.wait(250);
            finchRobot.setLED(125, 125, 0);
            finchRobot.wait(250);
            finchRobot.setLED(250, 0, 0);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        #endregion

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Dance                  *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDance(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Dance");

            Console.WriteLine("\tThe Finch robot will now Dance!");
            DisplayContinuePrompt();

            finchRobot.setMotors(-75, 75);
            finchRobot.wait(1000);
            finchRobot.setMotors(75, -75);
            finchRobot.wait(1000);
            finchRobot.setMotors(-75, 75);
            finchRobot.wait(1000);
            finchRobot.setMotors(75, -75);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 200);
            finchRobot.wait(5000);
            finchRobot.setMotors(200, 0);
            finchRobot.wait(5000);
            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Mixing it up                 *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayMix(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Mixing it up");

            Console.WriteLine("\tThe Finch robot will now Dance to Saria's Song from the Legend of Zelda series!");
            DisplayContinuePrompt();

            finchRobot.setMotors(-66, 66);
            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(698);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(880);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.setMotors(66, -66);
            finchRobot.setLED(0, 100, 255);

            finchRobot.noteOn(988);
            finchRobot.wait(500);
            finchRobot.noteOff();

            finchRobot.setMotors(-66, 66);
            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(698);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(880);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.setMotors(66, -66);
            finchRobot.setLED(0, 100, 255);

            finchRobot.noteOn(988);
            finchRobot.wait(500);
            finchRobot.noteOff();

            finchRobot.setMotors(-66, 66);
            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(698);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(880);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.setMotors(66, -66);
            finchRobot.setLED(0, 100, 255);

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(1319);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.setMotors(-66, 66);
            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(1175);
            finchRobot.wait(500);
            finchRobot.noteOff();

            finchRobot.setMotors(66, -66);
            finchRobot.setLED(0, 100, 255);

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(1047);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.setMotors(-66, 66);
            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(784);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(659);

            finchRobot.setMotors(66, -66);
            finchRobot.setLED(0, 100, 255);

            finchRobot.wait(1000);
            finchRobot.noteOff();
            finchRobot.noteOn(587);

            finchRobot.setMotors(-66, 66);
            finchRobot.setLED(0, 255, 0);

            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(659);

            finchRobot.setMotors(66, -66);
            finchRobot.setLED(0, 100, 255);

            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(784);

            finchRobot.setMotors(-66, 66);
            finchRobot.setLED(0, 255, 0);

            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.noteOn(659);

            finchRobot.setMotors(66, -66);
            finchRobot.setLED(0, 100, 255);

            finchRobot.wait(1000);
            finchRobot.noteOff();
            finchRobot.setMotors(0, 0);
            finchRobot.setLED(0, 0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnected.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region DATA RECORDER

        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;
          
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayGetData(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void DataRecorderDisplayGetData(double[] temperatures)
        {
            DisplayScreenHeader("Show Data");

            DataRecorderDisplayTable(temperatures);

            DisplayContinuePrompt();
        }

        static void DataRecorderDisplayTable(double[] temperatures)
        {
            //
            //display table headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(14) +
                "Temp in °C".PadLeft(15)
                );
            Console.WriteLine(
                "___________".PadLeft(15) +
                "___________".PadLeft(15)
                );

            //
            //display table data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    temperatures[index].ToString("n2").PadLeft(15)
                    );
            }
        }


        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfDataPoints];
            
            DisplayScreenHeader("Get Data");

            Console.WriteLine($"\tNumber of data points: {numberOfDataPoints}");
            Console.WriteLine($"\tData point frequency: {dataPointFrequency}");
            Console.WriteLine();
            Console.WriteLine("The finch robot is ready to begin recording the temerature data.");
            DisplayContinuePrompt();
            Console.WriteLine();
            Console.WriteLine();
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperatures[index] = finchRobot.getTemperature();
                Console.WriteLine($"\tReading {index + 1}: {temperatures[index].ToString("n2")}");
                int waitInSeconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(waitInSeconds);
            }

            Console.WriteLine("Data Recording Complete!");
            DisplayContinuePrompt();
            DisplayScreenHeader("Get Data");

            Console.WriteLine();
            Console.WriteLine("\tTable of Temperatures");
            Console.WriteLine();
            DataRecorderDisplayTable(temperatures);



            return temperatures;
        }

        /// <summary>
        /// get the frequency of the data points from the user
        /// </summary>
        /// <returns>frequency of data points</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            string userResponse;
            bool validResponse;

            do
            {
                
                DisplayScreenHeader("Data Point Frequency");
                Console.Write("Please enter the time interval in seconds at which the temperature measurements will be recorded. ");

                userResponse = Console.ReadLine();
                
                //validate user input
                double.TryParse(Console.ReadLine(), out dataPointFrequency);

                validResponse = double.TryParse(userResponse, out dataPointFrequency);

                if (!validResponse)
                {

                    Console.WriteLine();
                    Console.WriteLine("please enter a value such as 0.5, 1, 1.3...");
                    Console.WriteLine();
                    DisplayContinuePrompt();

                }
                

            } while (!validResponse);

            Console.WriteLine("Measurements will be taken at " + dataPointFrequency + " second intervals.");
            Console.WriteLine();
            DisplayContinuePrompt();

            return dataPointFrequency;
        }


        /// <summary>
        /// get the number of data points from user
        /// </summary>
        /// <returns>number of data points</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            string userResponse;
            bool validResponse;
            
            do
            {
                DisplayScreenHeader("Number of Data Point");

                Console.Write("Please enter the amount of temperature readings that will be taken. ");
                userResponse = Console.ReadLine();

                //validate user input
                int.TryParse(userResponse, out numberOfDataPoints);

                validResponse = int.TryParse(userResponse, out numberOfDataPoints);

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Pease enter a number, example: 1, 2, 3, 4...");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

            } while (!validResponse);

            Console.WriteLine("Measurements will be taken " + numberOfDataPoints + " times.");
            Console.WriteLine();
            DisplayContinuePrompt();

            return numberOfDataPoints;
        }

        #endregion

        #region ALARM SYSTEM
        
        /// <summary>
        /// **********************************************
        /// *             Light Alarm Menu               *
        /// **********************************************
        /// </summary>
        /// <param name="finchRobot"></param>
        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {

            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Light Alarm Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Values");
                Console.WriteLine("\td) Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmSetMinMaxThresholdValue(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmSetTimeToMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void LightAlarmSetAlarm(
            Finch finchRobot, 
            string sensorsToMonitor, 
            string rangeType, 
            int minMaxThresholdValue, 
            int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            int currentLightSensorValue = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\tSensors to Monitor: {sensorsToMonitor}");
            Console.WriteLine("\tRange Type: {0}", rangeType);
            Console.WriteLine("\tMin/Max threshold value: " + minMaxThresholdValue);
            Console.WriteLine($"\tTime to Monitor: {timeToMonitor}");
            Console.WriteLine();
            
            Console.WriteLine("\tPress any key to begin monitoring.");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && !thresholdExceeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = finchRobot.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
                        break;
                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;
                }

                finchRobot.wait(1000);
                secondsElapsed++;
            }

            if (thresholdExceeded)
            {
                Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was exceeded by the current light sensor value of {currentLightSensorValue}.");
                finchRobot.noteOn(1500);
                finchRobot.wait(3000);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was not exceeded.");
            }


            DisplayMenuPrompt("Light Alarm");
        }

        static int LightAlarmSetTimeToMonitor()
        {
            int timeToMonitor;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Time To Monitor");


                //validate value
                Console.Write($"\tTime to Monitor: ");
                userResponse = Console.ReadLine();

                int.TryParse(userResponse, out timeToMonitor);

                validResponse = int.TryParse(userResponse, out timeToMonitor);

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Pease enter a number, example: 3, 5, 9, 15...");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);


            Console.WriteLine($"The ambient light will be monitored every second for {timeToMonitor} seconds.");

            DisplayMenuPrompt("Light Alarm");

            return timeToMonitor;
        }


        static int LightAlarmSetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            string userResponse;
            int minMaxThresholdValue;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Minimum/Maximum Threshold Value");

                Console.WriteLine($"\tLeft light sensor ambient value: {finchRobot.getLeftLightSensor()}");
                Console.WriteLine($"\tLeft light sensor ambient value: {finchRobot.getRightLightSensor()}");
                Console.WriteLine();

                //validate value
                Console.Write($"\tEnter the {rangeType} light sensor value: ");
                userResponse = Console.ReadLine();
                int.TryParse(userResponse, out minMaxThresholdValue);

                validResponse = int.TryParse(userResponse, out minMaxThresholdValue);

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Pease enter a number, example: 10, 15, 79, 154...");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            //echo value

            Console.WriteLine($"The {rangeType} threshold will be {minMaxThresholdValue}.");

            DisplayMenuPrompt("Light Alarm");

            return minMaxThresholdValue;
        }

        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor = "";
            string userResponse;
            bool validResponse;

            do
            {
                validResponse = true;
                DisplayScreenHeader("Senosrs to Monitor");

                Console.Write("\tSensors to Monitor [left, right, both]: ");
                userResponse = Console.ReadLine().ToLower();

                if (userResponse == "left")
                {
                    sensorsToMonitor = "left";
                }
                else if (userResponse == "right")
                {
                    sensorsToMonitor = "right";
                }
                else if (userResponse == "both")
                {
                    sensorsToMonitor = "both";
                }
                else
                {
                    Console.WriteLine("Please enter either left, right, or both for the sensors to monitor.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    validResponse = false;
                }

            } while (!validResponse);

            Console.WriteLine($"sensors to monitor will be {sensorsToMonitor}.");

            DisplayMenuPrompt("Light Alarm");

            return sensorsToMonitor;
        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;
            string userResponse;
            bool validResponse;
            bool isMinimum = false;

            do
            {
                validResponse = true;
                
                DisplayScreenHeader("Range Type");

                Console.Write("\tRange Type [minimum, maximum]: ");
                userResponse = Console.ReadLine().ToLower();

                if (userResponse == "minimum")
                {
                    isMinimum = true;
                }
                else if (userResponse == "maximum")
                {
                    isMinimum = false;
                }
                else
                {
                    Console.WriteLine("Please enter either minimum or maximum for the range type.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    validResponse = false;
                }

            } while (!validResponse);

            if (isMinimum == true)
            {
                rangeType = "minimum";
            }
            else
            {
                rangeType = "maximum";
            }

            Console.WriteLine($"Range type will be {rangeType}.");

            DisplayMenuPrompt("Light Alarm");

            return rangeType;
        }

        #endregion

        #region USER PROGRAMMING
        
        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            bool quitMenu = false;
            string menuChoice;

            //
            // tuple to store all three command parameters
            //
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Quit");
                Console.WriteLine("\t\tEnter Choice: ");
                menuChoice = Console.ReadLine().ToLower();

                //
                //Process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommands(finchRobot, commands, commandParameters);
                        break;

                    case "q":
                        quitMenu = true;
                        break;
                }
            } while (!quitMenu);
        }

        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            DisplayScreenHeader("Command Parameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;
            string userResponse;
            bool validResponse;

            do
            {
                Console.Write("\tEnter Motor Speed [1 - 255]: ");
                userResponse = Console.ReadLine();

                int.TryParse(userResponse, out commandParameters.motorSpeed);
                validResponse = int.TryParse(userResponse, out commandParameters.motorSpeed);

                if (!validResponse)
                {
                    Console.WriteLine("\tPlease enter a valid speed [1 - 255]");
                    Console.WriteLine();
                }
            } while (!validResponse);

            do
            {
                Console.Write("\tEnter LED Brightness [1 - 255]: ");
                userResponse = Console.ReadLine();

                int.TryParse(userResponse, out commandParameters.ledBrightness);
                validResponse = int.TryParse(userResponse, out commandParameters.ledBrightness);

                if (!validResponse)
                {
                    Console.WriteLine("\tPlease enter a valid brightness [1 - 255]");
                    Console.WriteLine();
                }
            } while (!validResponse);

            do
            {
                Console.Write("\tEnter Wait in Seconds [1 - 255]: ");
                userResponse = Console.ReadLine();

                double.TryParse(userResponse, out commandParameters.waitSeconds);
                validResponse = double.TryParse(userResponse, out commandParameters.waitSeconds);

                if (!validResponse)
                {
                    Console.WriteLine("\tPlease enter a wait in seconds [1 - 255]");
                    Console.WriteLine();
                }
            } while (!validResponse);
            
            Console.WriteLine();
            Console.WriteLine($"\tMotor speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\tLED Brightness: {commandParameters.ledBrightness}");
            Console.WriteLine($"\tWait command duration: {commandParameters.waitSeconds}");

            DisplayMenuPrompt("User Programming");

            return commandParameters;
        }

        static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            //
            // list commands
            //
            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands");
            Console.WriteLine();
            Console.WriteLine("\t-");
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.Write($"- {commandName.ToLower()} -");
                if (commandCount % 5 == 0) Console.Write("-\n\t-");
                commandCount++;
            }
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write("\tEnter Command: ");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("\t\t*******************************************");
                    Console.WriteLine("\t\tPlease enter a command from the list above.");
                    Console.WriteLine("\t\t*******************************************");
                }
            }

            // echo commands

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayExecuteFinchCommands(
            Finch finchRobot, 
            List<Command> commands, 
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitMilliSeconds = (int)(commandParameters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("\tThe Finch robot is ready to execute the list of commands.");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Command.STOPMOTORS.ToString();
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNLEFT.ToString();
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;

                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature: {finchRobot.getTemperature().ToString("n2")}\n";
                        break;

                    case Command.DONE:
                        commandFeedback = Command.DONE.ToString();
                        break;
                    default:

                        break;
                }

                Console.WriteLine($"\t{commandFeedback}");
            }

            DisplayMenuPrompt("UserProgramming");
    }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
