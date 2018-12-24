using Navi.GUI.Faces.Language;
using System;

namespace Navi.GUI.Faces.Debugging
{
    public sealed class Debugger
    {
        private static int importNumber;

        private const int MaxNumberOfSigns = 30;

        public const string End = "end";
        public const string Empty = "   ";

        private bool debugMessages;
        private string name;

        public Debugger(string surfaceName, bool showName, bool debugMessages)
        {
            this.debugMessages = debugMessages;
            if (debugMessages) Console.ResetColor();

            name = surfaceName;
            if (showName) Message(surfaceName);
        }

        #region [loader]
        public void Message(string message)
        {
            SetDepthSymbols(message, false);
            Write(message + "\n", ConsoleColor.White);
        }

        private void Write(string message, ConsoleColor color)
        {
            if (debugMessages)
            {
                Console.ForegroundColor = color;
                Console.Write(message);
                Console.ResetColor();
            }
        }
        #endregion

        public void Comment(string message)
        {
            SetDepthSymbols(message, false);
            Write(message, ConsoleColor.DarkYellow);
        }

        public void Message(string text, bool passed)
        {
            if (debugMessages)
            {
                SetDepthSymbols(text, passed);
                Write(text, MaxNumberOfSigns);

                if (passed)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(passed);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(passed);
                }

                Console.ResetColor();
            }
        }

        private void Write(string message, int maxNumberOfSigns)
        {
            char[] chars = (message + " ").ToCharArray();
            int messageLenght = chars.Length > maxNumberOfSigns ? maxNumberOfSigns : chars.Length;
            Console.Write(chars, 0, messageLenght);
            if (message.Length > maxNumberOfSigns) Console.Write("...");
        }

        private void SetDepthSymbols(string text, bool passed)
        {
            for (int i = 0; i < importNumber; i++)
                Console.Write(Empty);

            if (text == End && importNumber > 0)
                importNumber--;

            if (text == Vocabulary.KeyWords.Import && passed == true)
                importNumber++;
        }
    }
}
