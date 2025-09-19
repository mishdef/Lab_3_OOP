namespace MyFunctions
{

    public static class MessageBox
    {
        public enum Buttons { Ok, YesNo, None }
        public enum Button { Ok, Yes, No, None }

        private const int MIN_CONTENT_WIDTH = 20;
        private const int PADDING_HORIZONTAL_TEXT = 2;

        public static Button Show(string message, string header = "Message", Buttons buttons = Buttons.None)
        {
            string actualHeader = " " + header.ToUpper() + " ";
            string actualButtonsString = ButtonsToString(buttons);

            int maxContentLength = message.Length;
            maxContentLength = Math.Max(maxContentLength, actualHeader.Length);
            if (actualButtonsString != null)
            {
                maxContentLength = Math.Max(maxContentLength, actualButtonsString.Length);
            }

            int contentWidth = Math.Max(MIN_CONTENT_WIDTH, maxContentLength + PADDING_HORIZONTAL_TEXT);

            DrawHorizontalLine('┏', '━', '┓', contentWidth);

            Console.Write($"┃{CenterString(actualHeader, contentWidth, '░')}┃\n");

            DrawHorizontalLine('┣', '━', '┫', contentWidth);

            Console.Write($"┃{message.PadRight(contentWidth)}┃\n");

            if (actualButtonsString != null)
            {
                DrawHorizontalLine('┣', '━', '┫', contentWidth);

                Console.Write($"┃{CenterString(actualButtonsString, contentWidth, ' ')}┃\n");
            }

            DrawHorizontalLine('┗', '━', '┛', contentWidth);

            if (buttons == Buttons.None)
            {
                return Button.None;
            }
            else
            {
                return GetChar(buttons);
            }
        }

        private static void DrawHorizontalLine(char leftCorner, char fillChar, char rightCorner, int length)
        {
            Console.Write(leftCorner);
            for (int i = 0; i < length; i++)
            {
                Console.Write(fillChar);
            }
            Console.Write(rightCorner);
            Console.Write("\n");
        }

        private static string CenterString(string s, int width, char fillChar = ' ')
        {
            if (string.IsNullOrEmpty(s))
            {
                return new string(fillChar, width);
            }

            if (s.Length >= width)
            {
                return s.Substring(0, width);
            }

            int totalPadding = width - s.Length;
            int leftPadding = totalPadding / 2;
            int rightPadding = totalPadding - leftPadding;

            return new string(fillChar, leftPadding) + s + new string(fillChar, rightPadding);
        }

        public static void BoxItem(string item)
        {
            Console.Write("┏");
            for (int i = 0; i < item.Length + 2; i++) { Console.Write('━'); }
            Console.Write("┓\n");
            Console.Write($"┃ {item} ┃\n");
            Console.Write("┗");
            for (int i = 0; i < item.Length + 2; i++) { Console.Write('━'); }
            Console.Write("┛\n");
        }

        private static Button GetChar(Buttons buttons)
        {
            Console.CursorVisible = false;
            try
            {
                if (buttons == Buttons.Ok)
                {
                    do
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            Console.WriteLine();
                            return Button.Ok;
                        }
                    } while (true);
                }
                if (buttons == Buttons.YesNo)
                {
                    do
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        char key = char.ToLower(keyInfo.KeyChar);

                        if (key == 'y' || key == 'н')
                        {
                            Console.WriteLine(" -> Yes");
                            return Button.Yes;
                        }
                        if (key == 'n' || key == 'т')
                        {
                            Console.WriteLine(" -> No");
                            return Button.No;
                        }
                    } while (true);
                }
            }
            finally
            {
                Console.CursorVisible = true;
            }
            return Button.None;
        }

        private static string ButtonsToString(Buttons buttons)
        {
            switch (buttons)
            {
                case Buttons.Ok:
                    return "Ok (Enter)";
                case Buttons.YesNo:
                    return "Yes (Y) / No (N)";
                case Buttons.None:
                default:
                    return null;
            }
        }
    }

    public static class Menu
    {
        public static int DisplayMenu(string header, string[] items, bool showHowToUse = true)
        {
            Console.Clear();
            MessageBox.BoxItem($" {header.ToUpper()} ");

            int menuStartLine = Console.CursorTop;

            int selectedIndex = 0;
            int lastIndex = 0;

            Console.CursorVisible = false;

            for (int i = 0; i < items.Length; i++)
            {
                Console.SetCursorPosition(0, menuStartLine + i);
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("-> ");
                }
                else
                {
                    Console.ResetColor();
                    Console.Write("   ");
                }
                Console.WriteLine($"{i+1}. {items[i]}");
            }
            Console.ResetColor();

            if (showHowToUse)
            {
                Console.SetCursorPosition(0, menuStartLine + items.Length);
                Console.WriteLine("\nUse arrow keys to navigate, Enter to select.");
                Console.WriteLine("Or press the number of menu item.");
                Console.WriteLine("You can also press Esc to exit.");
                Console.WriteLine("Press Esc to exit.");
            }

            int oldSelectedIndex;

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                oldSelectedIndex = selectedIndex;

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (IsBordrerCanBeCrossed(keyInfo.Key, selectedIndex, items.Length))
                    {
                        selectedIndex--;
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (IsBordrerCanBeCrossed(keyInfo.Key, selectedIndex, items.Length))
                    {
                        selectedIndex++;
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    Console.Clear();
                    return selectedIndex;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.CursorVisible = true;
                    Console.Clear();
                    return -1;
                }
                else if (char.IsDigit(keyInfo.KeyChar))
                {
                    int number = (int)char.GetNumericValue(keyInfo.KeyChar) - 1;
                    if (number >= 0 && number < items.Length)
                    {
                        selectedIndex = number;
                        if (lastIndex == number)
                        {
                            Console.CursorVisible = true;
                            Console.Clear();
                            return selectedIndex;
                        }
                        lastIndex = number;
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
                else
                {
                    Console.Beep();
                }

                if (selectedIndex != oldSelectedIndex)
                {
                    Console.SetCursorPosition(0, menuStartLine + oldSelectedIndex);
                    Console.ResetColor();
                    Console.Write("   ");
                    Console.Write($"{oldSelectedIndex+1}. {items[oldSelectedIndex]}");

                    Console.SetCursorPosition(0, menuStartLine + selectedIndex);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("-> ");
                    Console.Write($"{selectedIndex+1}. {items[selectedIndex]}");
                    Console.ResetColor();
                }

            } while (true);
        }

        public static bool IsBordrerCanBeCrossed(ConsoleKey key, int corentIndex, int countMenuItems)
        {
            if (key == ConsoleKey.UpArrow)
            {
                if (corentIndex > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (key == ConsoleKey.DownArrow)
            {
                if (corentIndex < countMenuItems - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }

    public static class Tools
    {
        public enum InputType { With, Without }

        public static void DrawLine(int n, char ch = '─')
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write(ch);
            }
            Console.WriteLine();
        }

        public static int InputInt(string promt, InputType inputType = InputType.With, int min = int.MinValue, int max = int.MaxValue)
        {
            int num;
            string maxStr, minStr;

            if (min == int.MinValue) minStr = "minimum int value"; else minStr = min.ToString();
            if (max == int.MaxValue) maxStr = "maximum int value"; else maxStr = max.ToString();

            do
            {
                try
                {
                    Console.Write(promt);
                    num = int.Parse(Console.ReadLine());
                    if (inputType == InputType.With)
                    {
                        if (num < min || num > max) throw new ArgumentException("inclusive");
                    }
                    if (inputType == InputType.Without)
                    {
                        if (num <= min || num >= max) throw new ArgumentException("exclusive");
                    }
                    break;
                }
                catch (ArgumentException ex) { Console.WriteLine(" ERROR! The number must be in the range from " + minStr + " to " + maxStr + $" ({ex.Message}). Please try again!"); }
                catch (FormatException) { Console.WriteLine(" ERROR! Invalid format! Please try again!"); }
                catch (OverflowException) { Console.WriteLine(" ERROR! Number is too large! Please try again!"); }
                catch (Exception ex) { Console.WriteLine($" ERROR! {ex.Message} Please try again!"); }
            }
            while (true);
            return num;
        }

        public static double InputDouble(string promt, InputType inputType = InputType.With, double min = double.MinValue, double max = double.MaxValue)
        {
            double num;
            string maxStr, minStr;

            if (min == double.MinValue) minStr = "minimum double value"; else minStr = min.ToString();
            if (max == double.MaxValue) maxStr = "maximum double value"; else maxStr = max.ToString();

            do
            {
                try
                {
                    Console.Write(promt);
                    num = double.Parse(Console.ReadLine());

                    if (inputType == InputType.With)
                    {
                        if (num < min || num > max) throw new ArgumentException("inclusive");
                    }
                    else
                    {
                        if (num <= min || num >= max) throw new ArgumentException("exclusive");
                    }
                    break;
                }
                catch (ArgumentException ex) { Console.WriteLine(" ERROR! The number must be in the range from " + minStr + " to " + maxStr + $" ({ex.Message}). Please try again!"); }
                catch (FormatException) { Console.WriteLine(" ERROR! Invalid format! Please try again!"); }
                catch (OverflowException) { Console.WriteLine(" ERROR! Number is too large! Please try again!"); }
                catch (Exception ex) { Console.WriteLine($" ERROR! {ex.Message} Please try again!"); }
            }
            while (true);
            return num;
        }

        public static string InputFileName(string promt, string fileExtention)
        {
            string fileName;

            do
            {
                Console.Write(promt);
                fileName = Console.ReadLine();

                if (fileName.EndsWith(fileExtention))
                {
                    fileName = fileName.Substring(0, fileName.Length - fileExtention.Length);
                }

                if (1 > fileName.Length || fileName.Length > 10)
                {
                    Console.WriteLine("File name must be between 1 and 10 characters long.");
                }
            }
            while ((1 > fileName.Length || fileName.Length > 10));
            return fileName + fileExtention;
        }

        public static string InputString(string prompt, int minLength = 0, int maxLength = int.MaxValue, bool allowEmpty = false)
        {
            string maxStr, minStr;

            if (minLength == 0) minStr = "0"; else minStr = minLength.ToString();
            if (maxLength == int.MaxValue) maxStr = "unlimited"; else maxStr = maxLength.ToString();

            try
            {
                do
                {
                    Console.Write(prompt);
                    string str = Console.ReadLine();
                    if (str.Length < minLength || str.Length > maxLength)
                    {
                        if (minLength > 0 && maxLength < int.MaxValue)
                        {
                            Console.WriteLine($"String length must be between {minStr} and {maxStr} chars.");
                        }
                        else if (minLength > 0)
                        {
                            Console.WriteLine($"String length must be at least {minStr} chars.");
                        }
                        else if (maxLength < int.MaxValue)
                        {
                            Console.WriteLine($"String length must not exceed {maxStr} chars.");
                        }
                    }
                    else if (!allowEmpty && str.Length == 0)
                    {
                        Console.WriteLine("String cannot be empty.");
                    }
                    else
                    {
                        return str;
                    }
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            return "";
        }

        public static DateTime InputDateTime(string prompt, DateTime min = default, DateTime max = default)
        {
            DateTime dateTime;
            if (min == default) min = DateTime.MinValue;
            if (max == default) max = DateTime.MaxValue;

            string minStr = (min == DateTime.MinValue) ? "minimum possible date/time" : min.ToString();
            string maxStr = (max == DateTime.MaxValue) ? "maximum possible date/time" : max.ToString();

            do
            {
                try
                {
                    Console.Write(prompt);
                    if (!DateTime.TryParse(Console.ReadLine(), out dateTime)) throw new FormatException(); 

                    if (dateTime < min || dateTime > max)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    break; 
                }
                catch (FormatException)
                {
                    Console.WriteLine(" ERROR! Invalid date/time format. Please try again! (Example: 2023-10-27 14:30 or 10/27/2023)");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"ERROR! The date/time must be between {minStr} and {maxStr}. Please try again!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" ERROR! {ex.Message} Please try again!");
                }
            }
            while (true);
            return dateTime;
        }
    }
}