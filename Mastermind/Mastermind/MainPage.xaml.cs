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
        private const int NUM_ROW = 11, NUM_COL = 4; //Guess Area
        private const int COR_ROW = 30, COR_COL = 2; //Correct Display
        private const int NUM_ROUNDS = 10;

        //BoxView
        BoxView currPinSelected;
        BoxView originalLocation;

        //Global Variables
        int pinAnswerCounter = 1;
        int round = 1;

        //Array for Colours
        private static string[] ColourNames = {"Red", "Green", "Blue", "Black",
                                               "White", "Purple", "Yellow", "Orange"};

        Color[] pinColours = new Color[]
            {Color.Red, Color.Green, Color.Blue, Color.Black,
            Color.White, Color.Purple, Color.Yellow, Color.Orange};

        Color[] userGuess = new Color[4];
        Color[] answerCode = new Color[4];
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

            #region SetUpGameLayout
            //Set up 4 Columns and 10 Rows to Grid in Xaml
            for (int i = 0; i < 6; i++)
            {
                GrdGameLayout.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < 14; i++)
            {
                GrdGameLayout.RowDefinitions.Add(new RowDefinition());
            }
            #endregion

            #region AnswerArea
            for(int a = 0; a < 4; a++)
            {
                //Create Image
                Image answer;
                answer = new Image();
                answer.Source = "Images/QuestionMark.png";
                answer.StyleId = "A" + a;
                answer.SetValue(Grid.RowProperty, 0);
                answer.SetValue(Grid.ColumnProperty, a);
                answer.HeightRequest = 40;
                answer.WidthRequest = 40;

                //Add to Children
                GrdGameLayout.Children.Add(answer);
            }
            #endregion

            #region SetUpGuessArea-Working
            //Tap Gesture Recongizer
            TapGestureRecognizer emptyTap = new TapGestureRecognizer();
            emptyTap.NumberOfTapsRequired = 1;
            emptyTap.Tapped += Empty_Tapped;

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

            #region CreateColourPins
            //Tap Gesture Recongizer
            TapGestureRecognizer pinTap = new TapGestureRecognizer();
            pinTap.NumberOfTapsRequired = 1;
            pinTap.Tapped += PinTapped;
            
            #region Red
            //Create new BoxView
            for(int j = 0; j < 4; j++)
            {
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
            }
            
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
                //New Row Definitions
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

            #region CreateCheckButton
            Button checkGuess;
            checkGuess = new Button();
            checkGuess.Text = "Check Guess";
            checkGuess.SetValue(Grid.RowProperty, 14);
            checkGuess.SetValue(Grid.ColumnProperty, 0);
            checkGuess.SetValue(Grid.ColumnSpanProperty, 3);
            checkGuess.HorizontalOptions = LayoutOptions.Center;
            checkGuess.VerticalOptions = LayoutOptions.Center;
            checkGuess.Clicked += BtnCheckGuess_Clicked;
            GrdGameLayout.Children.Add(checkGuess);
            #endregion

            //Clear Selected
            currPinSelected = null;
        }

        //Colour Pins Tapped
        #region ColourPinTapped
        private void PinTapped(object sender, EventArgs e)
        {
            //Create Boxview
            BoxView currPiece = (BoxView)sender;

            //Select Clicked Colour
            SelectThisColour(currPiece);

            //Update Pin Count
            pinAnswerCounter++;
        }

        //Select the Pin
        private void SelectThisColour(BoxView thisOne)
        {
            //Select Piece
            if (currPinSelected == null)
            {
                //This Piece is Selected
                currPinSelected = thisOne;
                originalLocation = thisOne;

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

        //Empty Square Tapped
        private void Empty_Tapped(object sender, EventArgs e)
        {
            //is there a current piece selected
            if (currPinSelected == null)
                return;

            //Select Place to move to
            BoxView currSq = (BoxView)sender;

            //Where Placing Pin
            MoveColourPinto((int)currSq.GetValue(Grid.RowProperty),
                          (int)currSq.GetValue(Grid.ColumnProperty));
        }

        #region Working
        //Move Colour Pin to Guess
        private void MoveColourPinto(int destRow, int destCol)
        {
            //Declare Variables
            int curRow, curCol;

            if (destRow == round)
            {
                //Get Pins Original Location
                curRow = (int)currPinSelected.GetValue(Grid.RowProperty);
                curCol = (int)currPinSelected.GetValue(Grid.ColumnProperty);

                currPinSelected.SetValue(Grid.RowProperty, destRow);
                currPinSelected.SetValue(Grid.ColumnProperty, destCol);

                originalLocation.SetValue(Grid.RowProperty, curRow);
                originalLocation.SetValue(Grid.ColumnProperty, curCol);
            }

            ResetCurrentSelectedPin();
        }
        #endregion

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

        #region CheckAnswer
        //Check if answer is Correct
        private void BtnCheckGuess_Clicked(object sender, EventArgs e)
        {
            //Check if Guess Row is Full before checking
            if (!CheckIfRowIsFull())
            {
                //Display Alert to user
                DisplayAlert("Warning", "Select 4 Colours!!", "Okay");

                return;
            }
        }
        
        //Check if user entered 4 colours in current row
        private bool CheckIfRowIsFull()
        {
            //Declare Variables
            bool answer = true;

            //Checking boxview for current row
            foreach(var item in GrdGameLayout.Children)
            {
                //for all boxview in children
                if(item.GetType() == typeof(BoxView))
                {
                    //Only Check Current Row
                    int r = (int)item.GetValue(Grid.RowProperty);
                    
                    //If it is on current row
                    if (r == round)
                    {
                        //Check if empty Space
                        if (((BoxView)item).StyleId == "GuessArea")
                        {
                            //If so return
                            answer = false;
                            break;
                        }
                    }
                }
            }

            //Return
            return answer;
        }
        #endregion
    }
}
