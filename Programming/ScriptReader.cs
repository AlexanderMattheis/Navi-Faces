using Navi.Helper.System;
using System.Collections.Generic;
using System.IO;

namespace Navi.Programming
{
    /// <summary>
    /// Allows to read-in programming language commands.
    /// </summary>
    public sealed class ScriptReader
    {
        #region [loader]
        public List<string> Commands(string path, string commentPattern, char variableAssignment)
        {
            List<string> commands = null;

            FileLoader fileLoader = new FileLoader();
            bool success = fileLoader.Load(path);

            if (success)
                commands = RetrieveCommands(fileLoader.DataReader, commentPattern, variableAssignment);

            fileLoader.Dispose();

            return commands;
        }

        private List<string> RetrieveCommands(StreamReader reader, string commentPattern, char variableAssignment)
        {
            List<string> commands = new List<string>();

            string variableName = string.Empty;
            string values = string.Empty;
            string finalCommand = string.Empty;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Trim() != string.Empty)
                {
                    if (line.Contains(commentPattern))  // remove comments
                    {
                        int commentStart = line.IndexOf(commentPattern);
                        line = line.Substring(0, commentStart);
                    }

                    if (line.Contains(variableAssignment.ToString()))
                    {
                        string[] split = line.Split(variableAssignment);
                        variableName = split[0];
                        values = split[1];
                        finalCommand = variableName + variableAssignment + " " + values;
                        commands.Add(finalCommand);
                    }
                    else
                    {
                        finalCommand += values;
                        commands.RemoveAt(commands.Count - 1);
                        commands.Add(finalCommand);
                    }
                }
            }

            return commands;
        }

        public List<string> Commands(string path, string commentPattern, char escape, char statementDelimeter, char lineTerminal)
        {
            List<string> commands = null;

            FileLoader fileLoader = new FileLoader();
            bool success = fileLoader.Load(path);

            if (success)
                commands = RetrieveCommands(fileLoader.DataReader, commentPattern, escape, statementDelimeter, lineTerminal);

            fileLoader.Dispose();

            return commands;
        }

        private List<string> RetrieveCommands(StreamReader reader, string commentPattern, char escape, char delimeter, char lineTerminal)
        {
            List<string> commands = new List<string>();
            string commandCharacters = string.Empty;
            bool escapedCodeLineEndSymbol = false;

            char lastCharacter = delimeter;  // to get a codeline ended for the first time
            string line;
            
            Leave:
            while ((line = reader.ReadLine()) != null)
            {
                bool codelineEnded = (lastCharacter == delimeter || lastCharacter == lineTerminal || lastCharacter == commentPattern[1]) && !escapedCodeLineEndSymbol;
                commandCharacters = !codelineEnded ? commandCharacters : string.Empty;  // if a codeline is continued in the next line we do not a reset

                foreach (char character in line)
                {
                    if (character == escape)
                    {
                        if (lastCharacter == escape) // to escape an escape symbol (ESC ESC), the second escape symbol should not work as an ESC symbol
                            lastCharacter = '\0';
                        else  // to escape a symbol after an escape symbol (ESC [A-Za-z])
                        {
                            lastCharacter = character;
                            continue;
                        }
                    }
                    else if (character == delimeter)
                    {
                        if (lastCharacter != escape)
                        {
                            lastCharacter = character;
                            escapedCodeLineEndSymbol = false;
                            break;
                        }
                        else
                            escapedCodeLineEndSymbol = true;
                    }
                    else if (character == lineTerminal)
                    {
                        if (lastCharacter != escape)
                        {
                            lastCharacter = character;
                            commandCharacters += character;
                            escapedCodeLineEndSymbol = false;
                            break;
                        }
                        else
                            escapedCodeLineEndSymbol = true;
                    }
                    else if (character == commentPattern[0])
                    {
                        if (lastCharacter == commentPattern[1])
                            goto Leave;
                        lastCharacter = character;
                        //continue;
                    }
                    
                    lastCharacter = character;
                    commandCharacters += character;
                }

                AddCommand(commandCharacters, codelineEnded, commands);
            }

            return commands;
        }

        private void AddCommand(string commandCharacters, bool codelineEnded, List<string> commands)
        {
            bool emptyLine = commandCharacters.Trim() == string.Empty;

            if (!emptyLine)
            {
                if (codelineEnded)
                    commands.Add(commandCharacters);
                else
                {
                    commands.RemoveAt(commands.Count - 1);
                    commands.Add(commandCharacters);
                }
            }
        }
        #endregion
    }
}
