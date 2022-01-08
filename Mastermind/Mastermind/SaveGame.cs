using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastermind
{
    public class SaveGame
    {
        //Declare Variables
        public static int round;
        public static int[,] correctPins = new int[10, 2];
        public static int[,] pastGuess = new int[10,4];

        public static void WriteToFile(string fullFileName, string path)
        {
            using (var writer = File.Create(path))
            {
                for(int i = 0; i < pastGuess.GetLength(0); i++)
                {
                    for(int j = 0; j < pastGuess.GetLength(1); j++)
                    {
                        //Write to File
                        writer.Write(pastGuess[i,j]);
                    }
                }
            }
        }
    
        public static string ReadFromFile(string fullFileName, string path)
        {
            string readText = "";

            try
            {
                using (var reader = new StreamReader(fullFileName))
                {
                    for (int i = 0; i < pastGuess.GetLength(0); i++)
                    {
                        for (int j = 0; j < pastGuess.GetLength(1); j++)
                        {
                            //Write to File
                            pastGuess[i, j] = reader.ReadLine;
                        }
                    }
                    readText = reader.ReadToEnd();
                }
            }

            catch
            {
                readText = "Couldn't read the file!! Check if file exosts";
            }

            return readText;
        }
    }
}
