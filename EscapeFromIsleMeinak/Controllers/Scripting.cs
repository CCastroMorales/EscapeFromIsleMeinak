using MeinakEsc.Components;
using System;
using System.Collections.Generic;

namespace MeinakEsc
{
    public class Scripting
    {
        public Scene Scene { get; set; }

        public List<string> lineBuffer { get; set; }
        public bool parseRawLines = false;
        public int linenum = 0;

        public void Parse(string script)
        {
            Parse(script.Split('\n'));
        }

        public void Parse(string[] script)
        {
            DoParse(script);
        }

        public void DoParse(string[] script)
        {
            string symbol = "";
            linenum = 0;

            foreach (string rawline in script)
            {
                symbol = "";
                string line = rawline.Trim();
                bool validSymbol = false;

                foreach (char c in line)
                {
                    symbol += c;

                    if (symbol == "scene.")
                    {
                        int length = line.Length - symbol.Length;
                        if (ParseSceneSymbol(line.Substring(symbol.Length, length)))
                        {
                            validSymbol = true;
                            break;
                        }
                    }
                }

                if (!validSymbol && parseRawLines)
                    lineBuffer.Add(line);
                
                linenum++;
            }
        }

        private bool ParseSceneSymbol(string input)
        {
            string symbol = "";

            foreach (char c in input)
            {
                if (symbol == "title" && c == ' ' && !parseRawLines)
                {
                    int length = input.Length - symbol.Length;
                    AssignSceneTitleValue(input.Substring(symbol.Length, length).Trim());
                    return true;
                } 
                else if (symbol == "autoEnd" && c == ' ' && !parseRawLines)
                {
                    char charSymbol = input.Substring(symbol.Length + 1, 1)[0];
                    if (charSymbol != '1' || charSymbol != '0')
                    {
                        bool autoEnd = charSymbol == '1' ? true : false;
                        AssignSceneAutoEnd(autoEnd);
                        return true;
                    }
                }
                else
                    symbol += c;

                if (symbol == "initialScriptBegin" || symbol == "scriptBegin")
                {
                    parseRawLines = true;
                    lineBuffer = new List<string>();
                    return true;
                }
                else if (symbol == "initialScriptEnd")
                {
                    parseRawLines = false;
                    AssignSceneInitialScriptValue(lineBuffer.ToArray());
                    return true;
                }
                else if (symbol == "scriptEnd")
                {
                    parseRawLines = false;
                    AssignSceneScriptValue(lineBuffer.ToArray());
                    return true;
                }
            }

            return false;
        }

        private void AssignSceneTitleValue(string input)
        {
            Scene.Title = input;
        }

        private void AssignSceneAutoEnd(bool autoEnd)
        {
            Scene.AutoEnd = autoEnd;
        }

        private void AssignSceneInitialScriptValue(string[] input)
        {
            Scene.InitialScript.AddRange(TrimScript(input));
        }

        private void AssignSceneScriptValue(string[] input)
        {
            Scene.Script.AddRange(TrimScript(input));
        }

        private string[] TrimScript(string[] input)
        {
            List<string> cleansed = new List<string>();
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0 && input[i].Trim() == "")
                    continue;
                if (i == input.Length - 1 && input[i].Trim() == "")
                    continue;
                else
                    cleansed.Add(input[i]);
            }

            return cleansed.ToArray();
        }
    }
}
