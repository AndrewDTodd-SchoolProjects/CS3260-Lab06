// Project Prolog
// Name: Andrew Todd
// CS3260 Section 001
// Project: Lab_03
// Date: 09/11/22
// Purpose: This Project is the first stages of a banking program. The program impements
// the basic functionality and error checking of a bank account structure, as well as Console
// based input for testing and usabuility.
//
// I declare that the following code was written by me or provided
// by the instructor for this project. I understand that copying source
// code from any other source constitutes plagiarism, and that I will receive
// a zero on this project if I am found in violation of this policy.
// ---------------------------------------------------------------------------



using System.Diagnostics;
using System.Text;

namespace StockTracker
{
    internal class Program
    {
        private static List<ReversalInfo>? reversalInfo = new();

        readonly static string supportedFileTypes = "csv";

        ///<summary>
        /// This class is the program entrypoint and is responsible for handling input and output for the program.
        ///</summary>
        static void Main(string[] args)
        {
            string? filePath = null;

            if(args.Length != 0)
            {
                filePath = args[0];
            }

        INPUTLOOP: do
            {
                if (filePath == null)
                {
                    Console.WriteLine($"Enter a file path of a supported file format to parse stock reversal info from, or hit esc to exit program.\nSupported file types are {supportedFileTypes}");

                    if (!CancelableReadLine(out filePath))
                        return;
                }

                string extension = filePath.Split('.')[^1].ToLower();

                try
                {
                    switch (extension)
                    {
                        case "csv":
                            Console.WriteLine("Reading data from csv file.\n");
                            CSVParser parser = new();
                            reversalInfo = parser.ParseData(filePath);
                            break;

                        default:
                            Console.WriteLine($"Unsupported file extension ({extension}). Enter a supported file type.\nSupported file types are {supportedFileTypes}\n");
                            filePath = null;
                            goto INPUTLOOP;
                    }

                    PrintReversalInfo();
                    filePath = null;

                    //break;
                }
                catch (IOException ioe)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An IO Exeption occured!!!");
                    Console.WriteLine(ioe.Message);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    filePath = null;
                    goto INPUTLOOP;
                }
                catch (ArgumentException arge)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An Argument Exeption occured!!!");
                    Console.WriteLine(arge.Message);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    filePath = null;
                    goto INPUTLOOP;
                }
                catch (FormatException fe)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A Format Exeption occured!!!");
                    Console.WriteLine(fe.Message);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    filePath = null;
                    goto INPUTLOOP;
                }
            } while (true);
        }

        /// <summary>
        /// Purpose: Allows the user to enter input or cancel input by cheaking for escape key press
        /// </summary>
        /// <out param name="value">reference to string to write input to</param>
        /// <returns>true if underlying input was taken and escape was not pressed, false is escape key was pressed.</returns>
        public static bool CancelableReadLine(out string value)
        {
            value = string.Empty;
            var buffer = new StringBuilder();
            var key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
            {
                if (key.Key == ConsoleKey.Backspace && Console.CursorLeft > 0)
                {
                    var cli = --Console.CursorLeft;
                    buffer.Remove(cli, 1);
                    Console.CursorLeft = 0;
                    Console.Write(new String(Enumerable.Range(0, buffer.Length + 1).Select(o => ' ').ToArray()));
                    Console.CursorLeft = 0;
                    Console.Write(buffer.ToString());
                    Console.CursorLeft = cli;
                    key = Console.ReadKey(true);
                }
                else if (Char.IsLetterOrDigit(key.KeyChar) || Char.IsWhiteSpace(key.KeyChar) || Char.IsPunctuation(key.KeyChar))
                {
                    var cli = Console.CursorLeft;
                    buffer.Insert(cli, key.KeyChar);
                    Console.CursorLeft = 0;
                    Console.Write(buffer.ToString());
                    Console.CursorLeft = cli + 1;
                    key = Console.ReadKey(true);
                }
                else if (key.Key == ConsoleKey.LeftArrow && Console.CursorLeft > 0)
                {
                    Console.CursorLeft--;
                    key = Console.ReadKey(true);
                }
                else if (key.Key == ConsoleKey.RightArrow && Console.CursorLeft < buffer.Length)
                {
                    Console.CursorLeft++;
                    key = Console.ReadKey(true);
                }
                else
                {
                    key = Console.ReadKey(true);
                }
            }

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                value = buffer.ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Purpose: Prints out the reversal info stored in Program.reversalInfo
        /// </summary>
        public static void PrintReversalInfo()
        {
            if (reversalInfo == null || reversalInfo.Count == 0)
            {
                Console.WriteLine("No stock reversal info obtained.");
                return;
            }

            foreach(ReversalInfo info in reversalInfo)
            {
                if(info.reversalUp)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Reversed up on {info.reversalDate}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Reversed down on {info.reversalDate}");
                    Console.ForegroundColor = ConsoleColor.White;
                }    
            }
        }
    }
}