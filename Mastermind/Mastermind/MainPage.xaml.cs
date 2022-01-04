using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mastermind
{
    public partial class MainPage : ContentPage
    {
        //Constants
        const int GUESSROW = 11;
        const int GUESSCOL = 4;
        const int NUM_COLS = 7;
        const int NUM_ROWS = 14;

        public MainPage()
        {
            InitializeComponent();
            SetUpBoard();
        }

        //Set Up Board
        private void SetUpBoard()
        {
            //Declare Variables
            Image guess;
            Image answer;
            Image guessSelect;

            //Set up Board in xaml
            #region SetUpMainBoard

            //Set up Board in xaml
            for (int i = 0; i < NUM_COLS; i++)
            {
                GrdGameLayout.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < NUM_ROWS; i++)
            {
                GrdGameLayout.RowDefinitions.Add(new RowDefinition());
            }

            //Answer Area
            for (int r = 0; r < 1; r++)
            {
                for (int c = 0; c < GUESSCOL; c++)
                {
                    //Create new Image
                    answer = new Image();
                    answer.Source = "Images/QuestionMark.png";

                    //Set Value
                    answer.SetValue(Grid.RowProperty, r);
                    answer.SetValue(Grid.ColumnProperty, c);

                    //Add to Children
                    GrdGameLayout.Children.Add(answer);
                }
            }

            //Empty Guess Area
            for (int r = 1; r < GUESSROW; r++)
            {
                for (int c = 0; c < GUESSCOL; c++)
                {
                    //Create new Image
                    guess = new Image();
                    guess.Source = "Images/Empty.png";

                    //Set Value
                    guess.SetValue(Grid.RowProperty, r);
                    guess.SetValue(Grid.ColumnProperty, c);

                    //Add to Children
                    GrdGameLayout.Children.Add(guess);
                }
            }
            #endregion

            #region CheckIfCorrectGuess

            //Set up Board in xaml
            for (int i = 0; i < 2; i++)
            {
                GrdCorrectDisplay.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 32; i++)
            {
                GrdCorrectDisplay.RowDefinitions.Add(new RowDefinition());
            }

            //Show how Correct Guesses are
            for (int r = 1; r < (GUESSROW * 2); r++)
            {
                for(int c = 0; c < 2; c++)
                {
                    guessSelect = new Image();
                    guessSelect.Source = "Images/Empty.png";

                    //Set Value
                    guessSelect.SetValue(Grid.RowProperty, r);
                    guessSelect.SetValue(Grid.ColumnProperty, c);

                    GrdCorrectDisplay.Children.Add(guessSelect);
                }
            }
            #endregion

            CreateColourPins();
        }

        //Set Up Colour Pins
        private void CreateColourPins()
        {
            //Tapped Gesture
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            //t.Tapped += Pin_Tapped; //Creating Event Handler

            //Put a single boxview on the board - One piece in the Game
            BoxView pin;

            //Loop for pins
            //for (r = startRow; r < startRow + 3; r++)
            //{
                //c is the index in array
               // for (startArrayCol = 0; startArrayCol < 4; startArrayCol++)
               // {
               // }
           // }
                }
    }
}
