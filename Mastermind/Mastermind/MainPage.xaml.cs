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
        #region GlobalVariables
        //Constants
        const int NUM_ROW = 11, NUM_COL = 4; //Guess Area
        const int COR_ROW = 30, COR_COL = 2; //Correct Display

        //BoxView
        BoxView currPinSelected;

        //Global Vraible for Round
        int round = 10;
        #endregion

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
            Image guessSelect;

            #region AnswerArea
            //<!-- Answer Area -->
            // < Image Source = "Images/QuestionMark.png" Grid.Column = "0" Grid.Row = "0" />
            //< Image Source = "Images/QuestionMark.png" Grid.Column = "1" Grid.Row = "0" />
            //< Image Source = "Images/QuestionMark.png" Grid.Column = "2" Grid.Row = "0" />
            //< Image Source = "Images/QuestionMark.png" Grid.Column = "3" Grid.Row = "0" />

            #endregion

            #region SetUpGuessArea-Working
            //Tap Gesture Recongizer
            TapGestureRecognizer emptyTap = new TapGestureRecognizer();
            emptyTap.NumberOfTapsRequired = 1;
            emptyTap.Tapped += Empty_Tapped;//squaretapped

            //Add Boxview for each row & column needed
            for (int r = 1; r < NUM_ROW; r++)
            {
                for (int c = 0; c < NUM_COL; c++)
                {
                    //Create new BoxView
                    BoxView emptyGuess;
                    emptyGuess = new BoxView();
                    emptyGuess.BackgroundColor = Color.SaddleBrown;
                    emptyGuess.StyleId = "GuessArea";
                    emptyGuess.HorizontalOptions = LayoutOptions.Center;
                    emptyGuess.VerticalOptions = LayoutOptions.Center;
                    emptyGuess.HeightRequest = 40;
                    emptyGuess.WidthRequest = 40;
                    emptyGuess.CornerRadius = 40;

                    //Set Value
                    emptyGuess.SetValue(Grid.RowProperty, r);
                    emptyGuess.SetValue(Grid.ColumnProperty, c);

                    //Add Tap Gesture
                    emptyGuess.GestureRecognizers.Add(emptyTap);

                    //Add to Children
                    GrdGameLayout.Children.Add(emptyGuess);
                }
            }
            #endregion

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

            #region CreateColourPins
            //Tap Gesture Recongizer
            TapGestureRecognizer pinTap = new TapGestureRecognizer();
            pinTap.NumberOfTapsRequired = 1;
            pinTap.Tapped += PinTapped;
            
            #region Red
            //Create new BoxView
            BoxView redPin;
            redPin = new BoxView();
            redPin.BackgroundColor = Color.Red;
            redPin.StyleId = "RedPin";
            redPin.HorizontalOptions = LayoutOptions.Center;
            redPin.VerticalOptions = LayoutOptions.Center;
            redPin.HeightRequest = 40;
            redPin.WidthRequest = 40;
            redPin.CornerRadius = 40;

            //Set Value
            redPin.SetValue(Grid.RowProperty, 12);
            redPin.SetValue(Grid.ColumnProperty, 0);

            //Add Gesture Recognizer
            redPin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(redPin);
            #endregion

            #region Green
            //Create new BoxView
            BoxView greenPin;
            greenPin = new BoxView();
            greenPin.BackgroundColor = Color.Green;
            greenPin.StyleId = "GreenPin";
            greenPin.HorizontalOptions = LayoutOptions.Center;
            greenPin.VerticalOptions = LayoutOptions.Center;
            greenPin.HeightRequest = 40;
            greenPin.WidthRequest = 40;
            greenPin.CornerRadius = 40;

            //Set Value
            greenPin.SetValue(Grid.RowProperty, 12);
            greenPin.SetValue(Grid.ColumnProperty, 1);

            //Add Gesture Recognizer
            greenPin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(greenPin);
            #endregion
            
            #region Blue
            //Create new BoxView
            BoxView bluePin;
            bluePin = new BoxView();
            bluePin.BackgroundColor = Color.Blue;
            bluePin.StyleId = "BluePin";
            bluePin.HorizontalOptions = LayoutOptions.Center;
            bluePin.VerticalOptions = LayoutOptions.Center;
            bluePin.HeightRequest = 40;
            bluePin.WidthRequest = 40;
            bluePin.CornerRadius = 40;

            //Set Value
            bluePin.SetValue(Grid.RowProperty, 12);
            bluePin.SetValue(Grid.ColumnProperty, 2);

            //Add Gesture Recognizer
            bluePin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(bluePin);
            #endregion
            
            #region Black
            //Create new BoxView
            BoxView blackPin;
            blackPin = new BoxView();
            blackPin.BackgroundColor = Color.Black;
            blackPin.StyleId = "BlackPin";
            blackPin.HorizontalOptions = LayoutOptions.Center;
            blackPin.VerticalOptions = LayoutOptions.Center;
            blackPin.HeightRequest = 40;
            blackPin.WidthRequest = 40;
            blackPin.CornerRadius = 40;

            //Set Value
            blackPin.SetValue(Grid.RowProperty, 12);
            blackPin.SetValue(Grid.ColumnProperty, 3);

            //Add Gesture Recognizer
            blackPin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(blackPin);
            #endregion
            
            #region White
            //Create new BoxView
            BoxView whitePin;
            whitePin = new BoxView();
            whitePin.BackgroundColor = Color.White;
            whitePin.StyleId = "WhitePin";
            whitePin.HorizontalOptions = LayoutOptions.Center;
            whitePin.VerticalOptions = LayoutOptions.Center;
            whitePin.HeightRequest = 40;
            whitePin.WidthRequest = 40;
            whitePin.CornerRadius = 40;

            //Set Value
            whitePin.SetValue(Grid.RowProperty, 13);
            whitePin.SetValue(Grid.ColumnProperty, 0);

            //Add Gesture Recognizer
            whitePin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(whitePin);
            #endregion
            
            #region Purple
            //Create new BoxView
            BoxView purplePin;
            purplePin = new BoxView();
            purplePin.BackgroundColor = Color.Purple;
            purplePin.StyleId = "PurplePin";
            purplePin.HorizontalOptions = LayoutOptions.Center;
            purplePin.VerticalOptions = LayoutOptions.Center;
            purplePin.HeightRequest = 40;
            purplePin.WidthRequest = 40;
            purplePin.CornerRadius = 40;

            //Set Value
            purplePin.SetValue(Grid.RowProperty, 13);
            purplePin.SetValue(Grid.ColumnProperty, 1);

            //Add Gesture Recognizer
            purplePin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(purplePin);
            #endregion
            
            #region Yellow
            //Create new BoxView
            BoxView yellowPin;
            yellowPin = new BoxView();
            yellowPin.BackgroundColor = Color.Yellow;
            yellowPin.StyleId = "YellowPin";
            yellowPin.HorizontalOptions = LayoutOptions.Center;
            yellowPin.VerticalOptions = LayoutOptions.Center;
            yellowPin.HeightRequest = 40;
            yellowPin.WidthRequest = 40;
            yellowPin.CornerRadius = 40;

            //Set Value
            yellowPin.SetValue(Grid.RowProperty, 13);
            yellowPin.SetValue(Grid.ColumnProperty, 2);

            //Add Gesture Recognizer
            yellowPin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(yellowPin);
            #endregion
            
            #region Orange
            //Create new BoxView
            BoxView orangePin;
            orangePin = new BoxView();
            orangePin.BackgroundColor = Color.Orange;
            orangePin.StyleId = "OrangePin";
            orangePin.HorizontalOptions = LayoutOptions.Center;
            orangePin.VerticalOptions = LayoutOptions.Center;
            orangePin.HeightRequest = 40;
            orangePin.WidthRequest = 40;
            orangePin.CornerRadius = 40;

            //Set Value
            orangePin.SetValue(Grid.RowProperty, 13);
            orangePin.SetValue(Grid.ColumnProperty, 3);

            //Add Gesture Recognizer
            orangePin.GestureRecognizers.Add(pinTap);

            //Add to Children
            GrdGameLayout.Children.Add(orangePin);
            #endregion
            #endregion

            //Clear Selected
            currPinSelected = null;
        }

        //Colour Pins Tapped
        #region ColourPinTapped
        private void PinTapped(object sender, EventArgs e)
        {
            BoxView currPiece = (BoxView)sender;

            SelectThisColour(currPiece);
        }

        //
        private void SelectThisColour(BoxView thisOne)
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
                ResetCurrentSelectedPin();
            }
        }

        #region Working
        //Move Colour Pin to Guess
        private void MoveColourPinto(int destRow, int destCol)
        {
            if (destRow == round)
            {
                currPinSelected.SetValue(Grid.RowProperty, destRow);
                currPinSelected.SetValue(Grid.ColumnProperty, destCol);
            }

            ResetCurrentSelectedPin();
        }
        #endregion

        //Square Tapped
        private void Empty_Tapped(object sender, EventArgs e)
        {
            //Select Place to move to
            BoxView currSq = (BoxView)sender;

            //Where Placing Pin
            MoveColourPinto((int)currSq.GetValue(Grid.RowProperty),
                          (int)currSq.GetValue(Grid.ColumnProperty));
        }
        
        private void ColourPinsTapped(object sender, EventArgs e)
        {
        }
        #endregion

        //Reset Current Colour
        private void ResetCurrentSelectedPin()
        {
            //If no Piece Selected
            if (currPinSelected == null) return;

            //If Piece Selected
            currPinSelected.Opacity = 1;
        }//End ResetCurrrentPiece


        //
        private void Answer(int selectedColour)
        {
            var answer = new Random();
        }
    }
}
