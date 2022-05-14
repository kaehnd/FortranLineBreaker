using System;
using System.Collections.Generic;
using System.IO;

namespace FortranLineBreaker
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.Error.WriteLine("Error: program requires one argument, the file to convert. Usage: \"FortranLineBreaker <path-to-file>\"");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine($"Error: File \"{args[0]}\" does not exist");
                return;
            }

            var lines = new List<string>();

            foreach (var line in File.ReadLines(args[0]))
            {
                if (line.Length > 72)
                {
                    var tmpLine = line;

                    //at the start of a new line, print just the start of the line
                    for (var i = 72; i > 0; --i)
                    {
                        if (tmpLine[i] != ';' && !char.IsWhiteSpace(tmpLine[i])) continue;


                        lines.Add(tmpLine.Substring(0, i));
                        tmpLine = tmpLine[(i + 1)..];
                        break;
                    }

                    //for all breaks, add five spaces, ampersand, and another space
                    while (tmpLine.Length > 65)
                    {
                        for (var i = 65; i > 0; --i)
                        {
                            if (tmpLine[i] != ';' && !char.IsWhiteSpace(tmpLine[i])) continue;


                            lines.Add("     & " + tmpLine.Substring(0, i));
                            tmpLine = tmpLine[(i + 1)..];
                            break;
                        }
                    }
                    //write the last bit of the line
                    lines.Add("     & " + tmpLine);
                }
                else
                {
                    lines.Add(line);
                }
            }
            File.WriteAllLines(args[0]+".out", lines);

            Console.WriteLine($"Done, written to {args[0]}.out");
        }
    }
}
