using System;
using System.Linq;

namespace csharp_learning
{
    class TitleCapitalizationTool
    {
        private const string DoubleSpace = "  ";
        private const string Hyphen = "-";
        private static string[] PunctuationMarks = { ".", ",", ":", ";", "!", "?" };
        private static string[] SpecialWords = { "a", "an", "the", "and", "but", "for", "nor", "so", "yet",
                                                    "at", "by", "in", "of", "on", "or", "out", "to", "up" };

        public static void Main(string[] arguments)
        {
            if (arguments.Length > 0)
            {
                foreach (string argument in arguments)
                {
                    Console.Write("Original title: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(argument);
                    Console.ResetColor();
                    Console.Write("Capitalized title: ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(ApplyTitleCapitalizationRules(argument));
                    Console.ResetColor();
                }
            }
            else
            {
                while (true)
                {
                    string userInput = string.Empty;
                    Console.Write("Enter title to capitalize: ");
                    int cursorTop = Console.CursorTop;
                    int cursorLeft = Console.CursorLeft;

                    Console.ForegroundColor = ConsoleColor.Red;
                    while (true)
                    {
                        userInput = Console.ReadLine();
                        if (userInput.Trim().Length > 0)
                        {
                            break;
                        }
                        Console.CursorLeft = cursorLeft;
                        Console.CursorTop = cursorTop;
                    }
                    Console.ResetColor();

                    Console.Write("Capitalized title: ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(ApplyTitleCapitalizationRules(userInput));
                    Console.ResetColor();

                    Console.WriteLine();
                }
            }
        }

        private static string ApplyTitleCapitalizationRules(string text)
        {
            text = SeparatePunctuationMarks(text.ToLower());
            string[] words = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            text = GetStringWithAppliedUppercaseRules(words);
            return TrimExtraSpacesAroundPunctuationMarks(text);
        }

        private static string SeparatePunctuationMarks(string text)
        {
            text = text.Replace(Hyphen, $" {Hyphen} ");
            foreach (string punctuationMark in PunctuationMarks)
            {
                text = text.Replace(punctuationMark, $" {punctuationMark} ");
            }
            return text;
        }

        private static string GetStringWithAppliedUppercaseRules(string[] words)
        {
            int lastWordIndex = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (PunctuationMarks.Contains(words[i]) || words[i].Equals(Hyphen))
                {
                    continue;
                }
                lastWordIndex = i;
                if (SpecialWords.Contains(words[i]))
                {
                    continue;
                }
                words[i] = FirstLetterUppercase(words[i]);
            }
            words[0] = FirstLetterUppercase(words[0]);
            words[lastWordIndex] = FirstLetterUppercase(words[lastWordIndex]);
            return string.Join(" ", words);
        }

        private static string FirstLetterUppercase(string word)
        {
            char[] wordChars = word.ToCharArray();
            wordChars[0] = char.ToUpper(wordChars[0]);
            return new string(wordChars);
        }

        private static string TrimExtraSpacesAroundPunctuationMarks(string text)
        {
            int oldLength = 0;
            foreach (string punctuationMark in PunctuationMarks)
            {
                while (true)
                {
                    oldLength = text.Length;
                    text = text.Replace($" {punctuationMark}", punctuationMark);
                    text = text.Replace($"{punctuationMark}{DoubleSpace}", $"{punctuationMark} ");
                    if (oldLength == text.Length)
                    {
                        break;
                    }
                }
            }
            while (true)
            {
                oldLength = text.Length;
                text = text.Replace($"{DoubleSpace}{Hyphen}", $" {Hyphen}");
                text = text.Replace($"{Hyphen}{DoubleSpace}", $"{Hyphen} ");
                if (oldLength == text.Length)
                {
                    break;
                }
            }
            return text;
        }
    }
}