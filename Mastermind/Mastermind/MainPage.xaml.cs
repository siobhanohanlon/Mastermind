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

        int tapCount = 0;

        //Boxview for Selected Pin
        BoxView currPinSelected;

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
                        guessSelect.StyleId = r+""+c;

                        //Set Value
                        guessSelect.SetValue(Grid.RowProperty, r);
                        guessSelect.SetValue(Grid.ColumnProperty, c);

                        //Add to Children
                        GrdCorrectDisplay.Children.Add(guessSelect);
                    }
                }
            }
            #endregion

            //Just to Clear
            currPinSelected = null;
        }

        //Colour Pins Tapped
        private void ColourPinsTapped(object sender, EventArgs e)
        {
            //Declare Variables
            var imageSender = (Image)sender;
            int selectedColour;

            switch (imageSender.StyleId)
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

                case "RedDot":
                    selectedColour = 1;
                    break;

                case "RedDot":
                    selectedColour = 1;
                    break;

                case "RedDot":
                    selectedColour = 1;
                    break;

                case "RedDot":
                    selectedColour = 1;
                    break;

                case "RedDot":
                    selectedColour = 1;
                    break;
            }
        }

        //Move Piece To
        private void MoveSelectedPieceTo(int destRow, int destColumn)
        {
            //Can only Move diagonally
            String selectedPin;

            //Selected Pin
            selectedPin = currPinSelected.StyleId;

            #region MoveThePin
            //Get and Set Grid properties
            currPinSelected.SetValue(Grid.RowProperty, destRow);
            currPinSelected.SetValue(Grid.ColumnProperty, destColumn);

            //Reset Piece
            ResetCurrentSelectedPiece();
            #endregion

            //Then set current selected to null
            currPinSelected = null;
        }//End MoveSelectedPieceTo


        //Select This Piece
        private void SelectThisPiece(BoxView thisOne)
        {
            //Select Piece
            if (currPinSelected == null)
            {
                //This Piece is Selected
                currPinSelected = thisOne;

                //Change Opacity
                currPinSelected.Opacity = 0.7;
            }

            //Deselect
            else
            {
                //Deselecting Piece
                currPinSelected = null;

                //Reset Piece Selected
                ResetCurrentSelectedPiece();
            }
        }//End SelectThisPiece

        //Reset Current Piece
        private void ResetCurrentSelectedPiece()
        {
            //If no Piece Selected
            if (currPinSelected == null) return;

            //If Piece Selected
            currPinSelected.Opacity = 1;
        }//End ResetCurrrentPiece

    }
}
