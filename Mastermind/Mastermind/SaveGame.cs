using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Mastermind
{
    public class SaveGame
    {
        //Declare Variables
        public int round, corPinRow;
        public int[,] pastGuess;
        public int[] answerCode;

        //If no data to save
        public SaveGame()
        {
            round = 0; 
            corPinRow = 0;
            pastGuess = new int[10, 4];
            answerCode = new int[4] { 0,0,0,0};

            for (int i = 0; i < pastGuess.GetLength(0); i++)
            {
                for (int j = 0; j < pastGuess.GetLength(1); j++)
                {
                    pastGuess[i, j] = 0;
                }
            }
        }
    }
}
