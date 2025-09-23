using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFunctions
{

    public static class MessageBox
    {
        public enum Buttons { Ok, YesNo, None }
        public enum Button { Ok, Yes, No, None }

        public static ConsoleColor SelectedButtonBgColor = ConsoleColor.DarkBlue;
        public static ConsoleColor SelectedButtonFgColor = ConsoleColor.White;
        public static ConsoleColor UnselectedButtonBgColor = ConsoleColor.Black;
        public static ConsoleColor UnselectedButtonFgColor = ConsoleColor.Gray;
        public static ConsoleColor ButtonDividerColor = ConsoleColor.DarkGray;

        public static ConsoleColor HeaderFgColor = ConsoleColor.White;
        public static ConsoleColor HeaderBgColor = ConsoleColor.Black;
        public static char HeaderFillChar = '░';
        public static string OkButtonText = "Ok (Enter)";
        public static string YesButtonText = " Yes (y)";
        public static string NoButtonText = " No (n)";
        public static string YesNoDividerString = " | ";
        public static string OkResultText = " -> Ok";
        public static string YesResultText = " -> Yes";
        public static string NoResultText = " -> No";

        public static int MinContentWidth = 20;
        public static int PaddingHorizontalText = 2;

        public static Button Show(string message, string header = "Message", Buttons buttons = Buttons.None)
        {
            string actualHeader = " " + header.ToUpper() + " ";

            int maxContentLength = message.Length;
            maxContentLength = Math.Max(maxContentLength, actualHeader.Length);

            if (buttons == Buttons.Ok)
            {
                maxContentLength = Math.Max(maxContentLength, OkButtonText.Length);
            }
            else if (buttons == Buttons.YesNo)
            {
                maxContentLength = Math.Max(maxContentLength, YesButtonText.Length + NoButtonText.Length + YesNoDividerString.Length);
            }

            int contentWidth = Math.Max(MinContentWidth, maxContentLength + PaddingHorizontalText);

            DrawHorizontalLine('┏', '━', '┓', contentWidth);

            Console.Write("┃");
            Console.BackgroundColor = HeaderBgColor;
            Console.ForegroundColor = HeaderFgColor;
            Console.Write(CenterString(actualHeader, contentWidth, HeaderFillChar));
            Console.ResetColor();
            Console.Write("┃\n");

            DrawHorizontalLine('┣', '━', '┫', contentWidth);

            Console.Write($"┃{message.PadRight(contentWidth)}┃\n");

            if (buttons != Buttons.None)
            {
                DrawHorizontalLine('┣', '━', '┫', contentWidth);
                int buttonRowStartCursorTop = Console.CursorTop;

                return GetChar(buttons, contentWidth, buttonRowStartCursorTop);
            }

            DrawHorizontalLine('┗', '━', '┛', contentWidth);

            return Button.None;
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

        private static Button GetChar(Buttons buttons, int contentWidth, int buttonRowStartCursorTop)
        {
            Console.CursorVisible = false;
            Button resultButton = Button.None;

            try
            {
                if (buttons == Buttons.Ok)
                {
                    Console.SetCursorPosition(0, buttonRowStartCursorTop);
                    Console.Write($"┃{CenterString(OkButtonText, contentWidth, ' ')}┃\n");
                    DrawHorizontalLine('┗', '━', '┛', contentWidth);

                    do
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            resultButton = Button.Ok;
                            break;
                        }
                        else
                        {
                            Menu.PerformBeep();
                        }
                    } while (true);

                    Console.WriteLine(OkResultText);
                    return resultButton;
                }
                else if (buttons == Buttons.YesNo)
                {
                    int selectedButtonIndex = 0;
                    string[] buttonTexts = { YesButtonText, NoButtonText };

                    DrawInteractiveButtons(buttonTexts, selectedButtonIndex, contentWidth, buttonRowStartCursorTop);
                    Console.Write("\n");
                    DrawHorizontalLine('┗', '━', '┛', contentWidth);

                    do
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                        int oldSelectedButtonIndex = selectedButtonIndex;

                        if (keyInfo.Key == ConsoleKey.LeftArrow)
                        {
                            if (selectedButtonIndex > 0)
                            {
                                selectedButtonIndex--;
                            }
                            else
                            {
                                Menu.PerformBeep();
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.RightArrow)
                        {
                            if (selectedButtonIndex < buttonTexts.Length - 1)
                            {
                                selectedButtonIndex++;
                            }
                            else
                            {
                                Menu.PerformBeep();
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            resultButton = selectedButtonIndex == 0 ? Button.Yes : Button.No;
                            break;
                        }
                        else if (char.ToLower(keyInfo.KeyChar) == 'y' || char.ToLower(keyInfo.KeyChar) == 'н')
                        {
                            if (selectedButtonIndex != 0)
                            {
                                selectedButtonIndex = 0;
                                DrawInteractiveButtons(buttonTexts, selectedButtonIndex, contentWidth, buttonRowStartCursorTop);
                                Console.SetCursorPosition(0, buttonRowStartCursorTop + 1);
                                DrawHorizontalLine('┗', '━', '┛', contentWidth);
                            }
                            resultButton = Button.Yes;
                            break;
                        }
                        else if (char.ToLower(keyInfo.KeyChar) == 'n' || char.ToLower(keyInfo.KeyChar) == 'т')
                        {
                            if (selectedButtonIndex != 1)
                            {
                                selectedButtonIndex = 1;
                                DrawInteractiveButtons(buttonTexts, selectedButtonIndex, contentWidth, buttonRowStartCursorTop);
                                Console.SetCursorPosition(0, buttonRowStartCursorTop + 1);
                                DrawHorizontalLine('┗', '━', '┛', contentWidth);
                            }
                            resultButton = Button.No;
                            break;
                        }
                        else
                        {
                            Menu.PerformBeep();
                        }

                        if (selectedButtonIndex != oldSelectedButtonIndex)
                        {
                            DrawInteractiveButtons(buttonTexts, selectedButtonIndex, contentWidth, buttonRowStartCursorTop);
                            Console.SetCursorPosition(0, buttonRowStartCursorTop + 1);
                            DrawHorizontalLine('┗', '━', '┛', contentWidth);
                        }

                    } while (true);

                    DrawInteractiveButtons(buttonTexts, -1, contentWidth, buttonRowStartCursorTop);
                    Console.SetCursorPosition(0, buttonRowStartCursorTop + 1);
                    DrawHorizontalLine('┗', '━', '┛', contentWidth);

                    if (resultButton == Button.Yes)
                    {
                        Console.WriteLine(YesResultText);
                    }
                    else if (resultButton == Button.No)
                    {
                        Console.WriteLine(NoResultText);
                    }
                    return resultButton;
                }
            }
            finally
            {
                Console.CursorVisible = true;
                Console.ResetColor();
            }
            return Button.None;
        }

        private static void DrawInteractiveButtons(string[] buttonTexts, int selectedIndex, int contentWidth, int cursorTop)
        {
            Console.SetCursorPosition(0, cursorTop);
            Console.Write("┃");

            int totalButtonsTextLength = buttonTexts.Sum(s => s.Length) + (buttonTexts.Length > 1 ? YesNoDividerString.Length : 0);
            int paddingLeft = (contentWidth - totalButtonsTextLength) / 2;
            int paddingRight = contentWidth - totalButtonsTextLength - paddingLeft;

            Console.Write(new string(' ', paddingLeft));

            for (int i = 0; i < buttonTexts.Length; i++)
            {
                if (i == selectedIndex && selectedIndex != -1)
                {
                    Console.BackgroundColor = SelectedButtonBgColor;
                    Console.ForegroundColor = SelectedButtonFgColor;
                }
                else
                {
                    Console.BackgroundColor = UnselectedButtonBgColor;
                    Console.ForegroundColor = UnselectedButtonFgColor;
                }
                Console.Write(buttonTexts[i]);

                if (i < buttonTexts.Length - 1)
                {
                    Console.BackgroundColor = UnselectedButtonBgColor;
                    Console.ForegroundColor = ButtonDividerColor;
                    Console.Write(YesNoDividerString);
                }
            }

            Console.ResetColor();
            Console.Write(new string(' ', paddingRight));
            Console.Write("┃");
        }
    }

    public static class Menu
    {
        public static ConsoleColor SelectedItemColor = ConsoleColor.Yellow;
        public static string SelectionArrow = "-> ";
        public static bool EnableBeepOnError = true;

        public static void PerformBeep()
        {
            if (EnableBeepOnError)
            {
                Console.Beep();
            }
        }

        public static int DisplayMenu(string header, string[] items, bool showHowToUse = true, bool allowLooping = false)
        {
            MessageBox.BoxItem($" {header.ToUpper()} ");

            int menuStartLine = Console.CursorTop;

            int selectedIndex = 0;
            int lastDigitPressedIndex = -1;

            Console.CursorVisible = false;

            for (int i = 0; i < items.Length; i++)
            {
                Console.SetCursorPosition(0, menuStartLine + i);
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = SelectedItemColor;
                    Console.Write(SelectionArrow);
                }
                else
                {
                    Console.ResetColor();
                    Console.Write("   ");
                }
                Console.WriteLine($"{i + 1}. {items[i]}");
            }
            Console.ResetColor();
            if (showHowToUse)
            {
                Console.SetCursorPosition(0, menuStartLine + items.Length);
                Console.WriteLine("\nUse arrow keys to navigate, Enter to select.");
                Console.WriteLine("Or press the number of menu item.");
                if (items.Length > 9)
                {
                    Console.WriteLine("Selection by number is disabled for more than 9 items.");
                }
                else
                {
                    Console.WriteLine("Or press the number of menu item.");
                }
                Console.WriteLine("You can also press Esc to exit.");
                Console.WriteLine("Press Esc to exit.");
            }
            int oldSelectedIndex;

            int finalCursorPositionAfterMenu = Console.CursorTop;

            do
            {
                oldSelectedIndex = selectedIndex;

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (selectedIndex == 0)
                    {
                        if (allowLooping)
                        {
                            selectedIndex = items.Length - 1;
                        }
                        else
                        {
                            PerformBeep();
                        }
                    }
                    else
                    {
                        selectedIndex--;
                    }
                    lastDigitPressedIndex = -1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (selectedIndex == items.Length - 1)
                    {
                        if (allowLooping)
                        {
                            selectedIndex = 0;
                        }
                        else
                        {
                            PerformBeep();
                        }
                    }
                    else
                    {
                        selectedIndex++;
                    }
                    lastDigitPressedIndex = -1;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    Console.SetCursorPosition(0, finalCursorPositionAfterMenu);
                    Console.WriteLine();
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
                    if (items.Length > 9)
                    {
                        PerformBeep();
                        lastDigitPressedIndex = -1;
                    }
                    else 
                    {
                        int number = (int)char.GetNumericValue(keyInfo.KeyChar) - 1;

                        if (number >= 0 && number < items.Length)
                        {
                            if (selectedIndex == number)
                            {
                                Console.CursorVisible = true;
                                Console.SetCursorPosition(0, finalCursorPositionAfterMenu);
                                Console.WriteLine();
                                return selectedIndex;
                            }
                            else
                            {
                                selectedIndex = number;
                                lastDigitPressedIndex = number;
                            }
                        }
                        else
                        {
                            PerformBeep();
                            lastDigitPressedIndex = -1;
                        }
                    }
                }
                else
                {
                    PerformBeep();
                    lastDigitPressedIndex = -1;
                }

                if (selectedIndex != oldSelectedIndex)
                {
                    Console.SetCursorPosition(0, menuStartLine + oldSelectedIndex);
                    Console.ResetColor();
                    Console.Write("   ");
                    Console.Write($"{oldSelectedIndex + 1}. {items[oldSelectedIndex]}");

                    Console.SetCursorPosition(0, menuStartLine + selectedIndex);
                    Console.ForegroundColor = SelectedItemColor;
                    Console.Write(SelectionArrow);
                    Console.Write($"{selectedIndex + 1}. {items[selectedIndex]}");
                    Console.ResetColor();
                }

            } while (true);
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