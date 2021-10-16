namespace AsyncConOut{
    public class Out
    {
        public static ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        public static List<Output> OutputMessages = new List<Output>();
        public static int Index = 0;
        public static bool CancelOutputTask = false;
        public static bool ResetToDefaultColor = true;
        public static FileStream LogFileStream;
        public static uint DelayBetweenOutputChecks = 100;

        public class Output
        {
            public ConsoleColor ForegroundColor;
            public string Text;
            public bool NewLine = true;

            public Output(string text, ConsoleColor color, bool newLine = true)
            {
                ForegroundColor = color;
                Text = text;
                NewLine = newLine;
            }
        }

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
                                if (LogFileStream != null)
                                {
                                    LogFileStream.Write(Encoding.UTF8.GetBytes(OutputMessages[Index].Text + "\n"));
                                }
                            }
                            else
                            {
                                Console.Write(OutputMessages[Index].Text);
                                if (LogFileStream != null)
                                {
                                    LogFileStream.Write(Encoding.UTF8.GetBytes(OutputMessages[Index].Text));
                                }
                            }
                            Index++;
                        }
                        if (ResetToDefaultColor)
                        {
                            Console.ForegroundColor = DefaultForegroundColor;
                        }
                    }
                    Thread.Sleep((int)DelayBetweenOutputChecks);
                }
            });
            return t;
        }

        public static string GetTimeString()
        {
            DateTime now = DateTime.Now;
            return now.Hour.ToString("00") + ":" + now.Minute.ToString("00") + ":" + now.Second.ToString("00") + "." +
                now.Millisecond.ToString();
        }
        public static string GetDateString()
        {
            DateTime now = DateTime.Now;
            return now.Day.ToString("00") + "." + now.Month.ToString("00") + "." + now.Year.ToString();
        }
        public static void ResetColor()
        {
            Console.ForegroundColor = DefaultForegroundColor;
        }
        public static void Info(string text)
        {
            OutputMessages.Add(new Output(text, ConsoleColor.DarkBlue, true));
        }
        public static void Infosl(string text)
        {
            OutputMessages.Add(new Output(text, ConsoleColor.DarkBlue, false));
        }
        public static void Error(string text)
        {
            OutputMessages.Add(new Output(text, ConsoleColor.DarkRed, true));
        }
        public static void Log(string text)
        {
            OutputMessages.Add(new Output(text, DefaultForegroundColor, true));
        }
        public static void LowInfo(string text)
        {
            OutputMessages.Add(new Output(text, ConsoleColor.Blue, true));
        }
        public static void LowInfosl(string text)
        {
            OutputMessages.Add(new Output(text, ConsoleColor.Blue, false));
        }
        public static void Logsl(string text)
        {
            OutputMessages.Add(new Output(text, DefaultForegroundColor, false));
        }
    }
}
