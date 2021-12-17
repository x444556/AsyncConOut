namespace AsyncConOut
    {
        public class Out
        {
            public static ConsoleColor DefaultForegroundColor = ConsoleColor.White;
            public static List<Output> OutputMessages = new List<Output>();
            public static int Index = 0;
            public static bool CancelOutputTask = false;
            public static bool ResetToDefaultColor = true;
            public static FileStream LogFileStream;
            public static int DelayBetweenOutputChecks = 100;

            public class Output
            {
                public ConsoleColor ForegroundColor;
                public string Text;
                public bool NewLine = true;
                public bool ConsoleOnly = false;

                public Output(string text, ConsoleColor color, bool newLine = true, bool consoleOnly=false)
                {
                    ForegroundColor = color;
                    Text = text;
                    NewLine = newLine;
                    ConsoleOnly = consoleOnly;
                }
            }

            /// <summary>
            /// Calling this function creates a task that displays messages asynchronously in the console.
            /// </summary>
            /// <param name="clearQueue">If true, then the message queue is deleted before the task starts.</param>
            /// <returns>Returns the Task, that is used for the output.</returns>
            public static Task RunOutputTaskAsync(bool clearQueue = false)
            {
                if (clearQueue)
                {
                    OutputMessages.Clear();
                }
                Task t = Task.Run(() =>
                {
                    while (!CancelOutputTask)
                    {
                        if (OutputMessages.Count > Index)
                        {
                            while (Index < OutputMessages.Count)
                            {
                                Console.ForegroundColor = OutputMessages[Index].ForegroundColor;
                                if (OutputMessages[Index].NewLine)
                                {
                                    Console.WriteLine(OutputMessages[Index].Text);
                                    if (LogFileStream != null && !OutputMessages[Index].ConsoleOnly)
                                    {
                                        byte[] msg = Encoding.UTF8.GetBytes(OutputMessages[Index].Text + "\n");
                                        LogFileStream.Write(msg, 0, msg.Length);
                                    }
                                }
                                else
                                {
                                    Console.Write(OutputMessages[Index].Text);
                                    if (LogFileStream != null && !OutputMessages[Index].ConsoleOnly)
                                    {
                                        byte[] msg = Encoding.UTF8.GetBytes(OutputMessages[Index].Text);
                                        LogFileStream.Write(msg, 0, msg.Length);
                                    }
                                }
                                Index++;
                            }
                            if (ResetToDefaultColor)
                            {
                                Console.ForegroundColor = DefaultForegroundColor;
                            }
                        }
                        Thread.Sleep(DelayBetweenOutputChecks);
                    }
                });
                return t;
            }

            /// <summary>
            /// This function returns the representation of the time as a string.
            /// </summary>
            /// <returns>String</returns>
            public static string GetTimeString()
            {
                DateTime now = DateTime.Now;
                return now.Hour.ToString("00") + ":" + now.Minute.ToString("00") + ":" + now.Second.ToString("00") + "." +
                    now.Millisecond.ToString();
            }
            /// <summary>
            /// This function returns the representation of the date as a string.
            /// </summary>
            /// <returns>String</returns>
            public static string GetDateString()
            {
                DateTime now = DateTime.Now;
                return now.Day.ToString("00") + "." + now.Month.ToString("00") + "." + now.Year.ToString();
            }
            /// <summary>
            /// This function sets the foreground color of the console to the color specified in DefaultForegroundColor.
            /// </summary>
            /// <returns></returns>
            public static void ResetColor()
            {
                Console.ForegroundColor = DefaultForegroundColor;
            }
            /// <summary>
            /// This function inserts a line into the queue and uses the color blue in the output.
            /// </summary>
            /// <param name="text">The message to output</param>
            /// <param name="conOnly">Only display in console</param>
            public static void Info(string text, bool conOnly=false)
            {
                OutputMessages.Add(new Output(text, ConsoleColor.DarkBlue, true, conOnly));
            }
            /// <summary>
            /// This function inserts a message into the queue and uses the color blue in the output.
            /// There is no line break.
            /// </summary>
            /// <param name="text">The message to output</param>
            /// <param name="conOnly">Only display in console</param>
            public static void Infosl(string text, bool conOnly = false)
            {
                OutputMessages.Add(new Output(text, ConsoleColor.DarkBlue, false, conOnly));
            }
            /// <summary>
            /// This function inserts a line into the queue and uses the color red in the output.
            /// </summary>
            /// <param name="text">The message to output</param>
            /// <param name="conOnly">Only display in console</param>
            public static void Error(string text, bool conOnly = false)
            {
                OutputMessages.Add(new Output(text, ConsoleColor.DarkRed, true, conOnly));
            }
            /// <summary>
            /// This function inserts a line into the queue and uses the default foreground color.
            /// </summary>
            /// <param name="text">The message to output</param>
            /// <param name="conOnly">Only display in console</param>
            public static void Log(string text, bool conOnly = false)
            {
                OutputMessages.Add(new Output(text, DefaultForegroundColor, true, conOnly));
            }
            /// <summary>
            /// This function inserts a line into the queue and uses the default foreground color.
            /// There is no line break.
            /// </summary>
            /// <param name="text">The message to output</param>
            /// <param name="conOnly">Only display in console</param>
            public static void Logsl(string text, bool conOnly = false)
            {
                OutputMessages.Add(new Output(text, DefaultForegroundColor, false, conOnly));
            }
        }
    }
