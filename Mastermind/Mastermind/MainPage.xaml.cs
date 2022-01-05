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
        const int NUM_ROW = 11, NUM_COL = 4; //Guess Area
        const int COR_ROW = 30, COR_COL = 2; //Correct Display

        Image imagePinSender;
        int tapCount = 1;

        //Main
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
            Image guessSelect;

            //Empty Guess Area
            //Tap Gesture Recongizer
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.NumberOfTapsRequired = 1;
            tap.Tapped += Square_Tapped;

            //Add Image for each row & column needed
            for (int r = 1; r < NUM_ROW; r++)
            {
                for (int c = 0; c < NUM_COL; c++)
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
            #region CheckIfCorrectGuess

            //Set up Board in xaml
            //Column
            for (int i = 0; i < 2; i++)
            {
                //New Column Definition
                GrdCorrectDisplay.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //Row
            for (int i = 0; i < 30; i++)
            {
                //New Row Definition
                GrdCorrectDisplay.RowDefinitions.Add(new RowDefinition());
            }

            //Area for Correct Guess
            //Row
            for (int r = 1; r < COR_ROW; r++)
            {
                //Column
                for (int c = 0; c < COR_COL; c++)
                {
                    //Skip every 3rd Row for an Empty Space
                    if ((r % 3) > 0)
                    {
                        //Create Image
                        guessSelect = new Image();
                        guessSelect.Source = "Images/Empty.png";

                        //Will be used to change later
                        guessSelect.StyleId = r + "" + c;

                        //Set Value
                        guessSelect.SetValue(Grid.RowProperty, r);
                        guessSelect.SetValue(Grid.ColumnProperty, c);

                        //Add to Children
                        GrdCorrectDisplay.Children.Add(guessSelect);
                    }
                }
            }
            #endregion
        }

        //Colour Pins Tapped
        private void ColourPinsTapped(object sender, EventArgs e)
        {
            //Update Counter
            tapCount++;

            //Declare Variables
            imagePinSender = (Image)sender;
            int selectedColour;

            switch (imagePinSender.StyleId)
            {
                case "RedDot":
                    selectedColour = 1;
                    break;

                case "GreenDot":
                    selectedColour = 2;
                    break;

                case "BlueDot":
                    selectedColour = 3;
                    break;

                case "BlackDot":
                    selectedColour = 4;
                    break;

                case "WhiteDot":
                    selectedColour = 5;
                    break;

                case "TanDot":
                    selectedColour = 6;
                    break;

                case "YellowDot":
                    selectedColour = 7;
                    break;

                case "OrangeDot":
                    selectedColour = 8;
                    break;

                default:
                    selectedColour = 0;
                    break;
            }

            if(tapCount%2 > 0)
            {
                imagePinSender.Opacity = 0.5;
            }
            else
            {
                foreach(var piece in GrdGameLayout.Children)
                {
                    if(piece.StyleId.Contains("Dot"))
                    {
                        piece.Opacity = 1;
                    }
                }
            }
            
        }

        //
        private void Answer(int selectedColour)
        {
            var answer = new Random();
        }

        //Square Tapped
        private void Square_Tapped(object sender, EventArgs e)
        {
            //Declare Variables
            int selectedColour;
            var imageSender = (Image)sender;

            switch (imagePinSender.StyleId)
            {
                case "RedDot":
                    selectedColour = 1;
                    break;

                case "GreenDot":
                    selectedColour = 2;
                    break;

                case "BlueDot":
                    selectedColour = 3;
                    break;

                case "BlackDot":
                    selectedColour = 4;
                    break;

                case "WhiteDot":
                    selectedColour = 5;
                    break;

                case "TanDot":
                    selectedColour = 6;
                    break;

                case "YellowDot":
                    selectedColour = 7;
                    break;

                case "OrangeDot":
                    selectedColour = 8;
                    break;

                default:
                    selectedColour = 0;
                    break;
            }
        }
    }
}
