using System;
using System.Linq;

namespace csharp_learning
{
    class TitleCapitalizationTool
    {
        private static readonly string DoubleSpace = "  ";
        private static readonly string Hyphen = "-";
        private static readonly string[] PunctuationMarks = { ".", ",", ":", ";", "!", "?" };
        private static readonly string[] SpecialWords = { "a", "an", "the", "and", "but", "for", "nor", "so", "yet",
                                                    "at", "by", "in", "of", "on", "or", "out", "to", "up" };

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (string argument in args)
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
            } else {
                while (true)
                {
                    string userInput = string.Empty;
                    Console.Write("Enter title to capitalize: ");

                    Console.ForegroundColor = ConsoleColor.Red;
                    do
                    {
                        userInput = Console.ReadLine() ?? string.Empty;
                    } while (userInput.Length == 0);
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
            text = text.Replace(Hyphen, " " + Hyphen + " ");
            foreach (string punctuationMark in PunctuationMarks)
            {
                text = text.Replace(punctuationMark, " " + punctuationMark + " ");
            }
            return text;
        }

        private static string GetStringWithAppliedUppercaseRules(string[] words)
        {
            int lastWordIndex = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (PunctuationMarks.Contains(words[i]) || words[i].Equals(Hyphen)) continue;
                lastWordIndex = i;
                if (SpecialWords.Contains(words[i])) continue;
                words[i] = FirstLetterUppercase(words[i]);
            }
            words[0] = FirstLetterUppercase(words[0]);
            words[lastWordIndex] = FirstLetterUppercase(words[lastWordIndex]);
            return string.Join(" ", words);
        }

        private static string FirstLetterUppercase(string word)
        {
            return char.ToString(word[0]).ToUpper() + word.Substring(1);
        }

        private static string TrimExtraSpacesAroundPunctuationMarks(string text)
        {
            int oldLength = 0;
            foreach (string punctuationMark in PunctuationMarks)
            {
                while (true)
                {
                    oldLength = text.Length;
                    text = text.Replace(" " + punctuationMark, punctuationMark);
                    text = text.Replace(punctuationMark + DoubleSpace, punctuationMark + " ");
                    if (oldLength == text.Length)
                        break;
                }
            }
            while (true)
            {
                oldLength = text.Length;
                text = text.Replace(DoubleSpace + Hyphen, " " + Hyphen);
                text = text.Replace(Hyphen + DoubleSpace, Hyphen + " ");
                if (oldLength == text.Length)
                    break;
            }
            return text;
        }
    }
}