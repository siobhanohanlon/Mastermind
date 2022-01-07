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
        private const int NUM_ROUNDS = 11; //As Starting Round is 1, Actual #Rounds is 10

        //BoxView
        BoxView currPinSelected;

        //Create an instance of Random class
        Random randAns;

        //Global Variables
        private int round;
        private int currRow;
        private Color guessAreaColour = Color.SaddleBrown;

        //Array for Colours
        private static string[] ColourNames = {"Red", "Green", "Blue", "Black",
                                               "White", "Purple", "Yellow", "Orange"};

        Color[] pinColours = new Color[8]
            {Color.Red, Color.Green, Color.Blue, Color.Black,
            Color.White, Color.Purple, Color.Yellow, Color.Orange};

        int[] answerCode = new int[4];
        int[] userGuess = new int[4];

        int[] colPins = new int[4];
        int[] redPins = new int[40];
        int[] whitePins = new int[40];
        #endregion

        //Main
        public MainPage()
        {
            InitializeComponent();
            SetUpBoard();
            NewGame();
        }

        //Set Up Board
        private void SetUpBoard()
        {
            //Declare Variables
            Image guessSelect;
            round = 1;

            #region SetUpGameLayout
            //Set up 4 Columns and 10 Rows to Grid in Xaml
            for (int i = 0; i < 6; i++)
            {
                GrdGameLayout.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < 16; i++)
            {
                GrdGameLayout.RowDefinitions.Add(new RowDefinition());
            }
            #endregion

            #region SetUpAnswerArea
            for (int a = 0; a < 4; a++)
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

            #region SetUpGuessArea
            //Tap Gesture Recongizer
            TapGestureRecognizer emptyTap = new TapGestureRecognizer();
            emptyTap.Tapped += MovePinTo;

            //Add Boxview for each row & column needed
            for (int r = 1; r < NUM_ROW; r++)
            {
                for (int c = 0; c < NUM_COL; c++)
                {
                    //Create new BoxView
                    BoxView emptyGuess;
                    emptyGuess = new BoxView();
                    emptyGuess.Color = guessAreaColour;
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
            pinTap.Tapped += PinTapped;
            int h = 0;

            //Used for Arrays
            if(h < 9)
            {
                //Rows
                for(int ro = 12; ro < 14; ro++)
                {
                    for(int co = 0; co < 4; co++)
                    {
                        BoxView pin;
                        pin = new BoxView();
                        pin.Color = pinColours[h];
                        pin.StyleId = ColourNames[h] + "Pin";
                        pin.HorizontalOptions = LayoutOptions.Center;
                        pin.VerticalOptions = LayoutOptions.Center;
                        pin.HeightRequest = 40;
                        pin.WidthRequest = 40;
                        pin.CornerRadius = 40;

                        pin.SetValue(Grid.RowProperty, ro);
                        pin.SetValue(Grid.ColumnProperty, co);

                        pin.GestureRecognizers.Add(pinTap);

                        GrdGameLayout.Children.Add(pin);

                        h++;
                    }
                }
            }
            #endregion

            #region SetUpCheckAnswerArea

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
            Button btnCheckGuess;
            btnCheckGuess = new Button();
            btnCheckGuess.Text = "Check Guess";
            btnCheckGuess.SetValue(Grid.RowProperty, 13);
            btnCheckGuess.SetValue(Grid.ColumnProperty, 4);
            btnCheckGuess.SetValue(Grid.ColumnSpanProperty, 2);
            btnCheckGuess.HorizontalOptions = LayoutOptions.Center;
            btnCheckGuess.VerticalOptions = LayoutOptions.Center;
            btnCheckGuess.Clicked += BtnCheckGuess_Clicked;
            GrdGameLayout.Children.Add(btnCheckGuess);
            #endregion

            #region CreateNewGameButton

            Button BtnNewGame;
            BtnNewGame = new Button();
            BtnNewGame.Text = "New Game";
            BtnNewGame.SetValue(Grid.RowProperty, 15);
            BtnNewGame.SetValue(Grid.ColumnProperty, 0);
            BtnNewGame.SetValue(Grid.ColumnSpanProperty, 2);
            BtnNewGame.HorizontalOptions = LayoutOptions.Center;
            BtnNewGame.VerticalOptions = LayoutOptions.Center;
            BtnNewGame.Clicked += BtnNewGame_Clicked;
            GrdGameLayout.Children.Add(BtnNewGame);
            #endregion

            #region CreateSaveButton
            Button BtnSaveGame;
            BtnSaveGame = new Button();
            BtnSaveGame.Text = "Load Game";
            BtnSaveGame.SetValue(Grid.RowProperty, 15);
            BtnSaveGame.SetValue(Grid.ColumnProperty, 2);
            BtnSaveGame.SetValue(Grid.ColumnSpanProperty, 2);
            BtnSaveGame.HorizontalOptions = LayoutOptions.Center;
            BtnSaveGame.VerticalOptions = LayoutOptions.Center;
            BtnSaveGame.Clicked += BtnSaveGame_Clicked;
            GrdGameLayout.Children.Add(BtnSaveGame);
            #endregion

            #region CreateLoadButton
            Button BtnLoadGame;
            BtnLoadGame = new Button();
            BtnLoadGame.Text = "Save Game";
            BtnLoadGame.SetValue(Grid.RowProperty, 15);
            BtnLoadGame.SetValue(Grid.ColumnProperty, 4);
            BtnLoadGame.SetValue(Grid.ColumnSpanProperty, 2);
            BtnLoadGame.HorizontalOptions = LayoutOptions.Center;
            BtnLoadGame.VerticalOptions = LayoutOptions.Center;
            BtnLoadGame.Clicked += BtnLoadGame_Clicked;
            GrdGameLayout.Children.Add(BtnLoadGame);
            #endregion

            //Clear Selected
            currPinSelected = null;
        }

        //Colour Pins Tapped
        #region ColourPinTapped
        private void PinTapped(object sender, EventArgs e)
        {
            //Create Boxview
            BoxView currPin = (BoxView)sender;

            foreach(var pin in GrdGameLayout.Children)
            {
                //for all boxview in children
                if (pin.GetType() == typeof(BoxView))
                {
                    //Check if empty Space
                    if (((BoxView)pin).StyleId.Contains("Pin"))
                    {
                        ((BoxView)pin).Opacity = 1;
                    }
                }
            }

            //Select Clicked Colour
            SelectThisColour(currPin);
        }

        //Select the Pin
        private void SelectThisColour(BoxView thisOne)
        {
            //This Piece is Selected
            currPinSelected = thisOne;

            //Change Opacity
            currPinSelected.Opacity = 0.5;
        }

        //Move Selected Pin
        private void MovePinTo(object sender, EventArgs e)
        {
            //Variables
            int destRow, destCol;

            //is there a current piece selected
            if (currPinSelected == null)
                return;

            //Select Place to move to
            BoxView currSq = (BoxView)sender;

            //Where Placing Pin
            destRow = (int)currSq.GetValue(Grid.RowProperty);
            destCol = (int)currSq.GetValue(Grid.ColumnProperty);

            //Tap Gesture Recongizer
            TapGestureRecognizer pinTap = new TapGestureRecognizer();
            pinTap.Tapped += MovePinTo;

            //New BoxView
            BoxView placePin;
            placePin = new BoxView();
            placePin.Color = currPinSelected.Color;
            placePin.StyleId = "SelectedPin";
            placePin.HorizontalOptions = LayoutOptions.Center;
            placePin.VerticalOptions = LayoutOptions.Center;
            placePin.HeightRequest = currPinSelected.HeightRequest;
            placePin.WidthRequest = currPinSelected.WidthRequest;
            placePin.CornerRadius = currPinSelected.CornerRadius;

            placePin.GestureRecognizers.Add(pinTap);

            GrdGameLayout.Children.Add(placePin);

            if (destRow == (NUM_ROUNDS - round))
            {
                placePin.SetValue(Grid.RowProperty, destRow);
                placePin.SetValue(Grid.ColumnProperty, destCol);

                GrdGameLayout.Children.Remove(currSq);

                #region SetPinToUserGuess
                //Red
                if (placePin.Color == Color.Red)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 1;
                            break;
                        case 1:
                            userGuess[1] = 1;
                            break;
                        case 2:
                            userGuess[2] = 1;
                            break;
                        case 3:
                            userGuess[3] = 1;
                            break;

                        default:
                            break;
                    }
                }

                //Green
                if (placePin.Color == Color.Green)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 2;
                            break;
                        case 1:
                            userGuess[1] = 2;
                            break;
                        case 2:
                            userGuess[2] = 2;
                            break;
                        case 3:
                            userGuess[3] = 2;
                            break;

                        default:
                            break;
                    }
                }

                //Blue
                if (placePin.Color == Color.Blue)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 3;
                            break;
                        case 1:
                            userGuess[1] = 3;
                            break;
                        case 2:
                            userGuess[2] = 3;
                            break;
                        case 3:
                            userGuess[3] = 3;
                            break;

                        default:
                            break;
                    }
                }

                //Black
                if (placePin.Color == Color.Black)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 4;
                            break;
                        case 1:
                            userGuess[1] = 4;
                            break;
                        case 2:
                            userGuess[2] = 4;
                            break;
                        case 3:
                            userGuess[3] = 4;
                            break;

                        default:
                            break;
                    }
                }

                //White
                if (placePin.Color == Color.White)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 5;
                            break;
                        case 1:
                            userGuess[1] = 5;
                            break;
                        case 2:
                            userGuess[2] = 5;
                            break;
                        case 3:
                            userGuess[3] = 5;
                            break;

                        default:
                            break;
                    }
                }

                //Purple
                if (placePin.Color == Color.Purple)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 6;
                            break;
                        case 1:
                            userGuess[1] = 6;
                            break;
                        case 2:
                            userGuess[2] = 6;
                            break;
                        case 3:
                            userGuess[3] = 6;
                            break;

                        default:
                            break;
                    }
                }

                //Yellow
                if (placePin.Color == Color.Yellow)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 7;
                            break;
                        case 1:
                            userGuess[1] = 7;
                            break;
                        case 2:
                            userGuess[2] = 7;
                            break;
                        case 3:
                            userGuess[3] = 7;
                            break;

                        default:
                            break;
                    }
                }

                //Orange
                if (placePin.Color == Color.Orange)
                {
                    switch (placePin.GetValue(Grid.ColumnProperty))
                    {
                        case 0:
                            userGuess[0] = 8;
                            break;
                        case 1:
                            userGuess[1] = 8;
                            break;
                        case 2:
                            userGuess[2] = 8;
                            break;
                        case 3:
                            userGuess[3] = 8;
                            break;

                        default:
                            break;
                    }
                }
                #endregion
            }
        }
        #endregion

        #region CheckButton
        //Check if answer is Correct
        private void BtnCheckGuess_Clicked(object sender, EventArgs e)
        {
            currRow = NUM_ROW - round;

            //Check if Guess Row is Full before checking
            if (!CheckCurrRow())
            {
                //Display Alert to user
                DisplayAlert("Warning", "Select 4 Colours!!", "Okay");
                return;
            }

            //Check User Guess Against Answer
            else
            {
                CheckUserGuess(currRow);
            }
        }

        //Check if user entered 4 colours in current row
        private bool CheckCurrRow()
        {
            //Declare Variables
            bool rowFull = true;
            currRow = (NUM_ROW - round);

            foreach (var item in GrdGameLayout.Children)
            {
                //If it is equal to the boxview
                if (item.GetType() == typeof(BoxView))
                {
                    int r = (int)item.GetValue(Grid.RowProperty);
                    if (r == currRow)
                    {
                        if (((BoxView)item).Color == guessAreaColour)
                        {
                            rowFull = false;
                            
                            break;
                        }
                    }
                }
            }

            //Return
            return rowFull;
        }
        #endregion

        #region CheckUserGuessVsAnswer
        private void CheckUserGuess(int currRow)
        {
            //Declare Variables
            //White = 1, Red = 2
            //MinRow - start, MaxRow - Finish
            int minRow = 0, maxRow = 0;
            int red = 0, white = 0;

            switch (currRow)
            {
                case 1:
                    minRow = 1;
                    maxRow = 3;
                    break;
                case 2:
                    minRow = 4;
                    maxRow = 6;
                    break;
                case 3:
                    minRow = 7;
                    maxRow = 9;
                    break;
                case 4:
                    minRow = 10;
                    maxRow = 12;
                    break;
                case 5:
                    minRow = 13;
                    maxRow = 15;
                    break;
                case 6:
                    minRow = 16;
                    maxRow = 18;
                    break;
                case 7:
                    minRow = 19;
                    maxRow = 21;
                    break;
                case 8:
                    minRow = 22;
                    maxRow = 24;
                    break;
                case 9:
                    minRow = 25;
                    maxRow = 27;
                    break;
                case 10:
                    minRow = 28;
                    maxRow = 30;
                    break;

                default:
                    break;
            }

            for (int check = 0; check < 4; check++)
            {
                int index = Array.IndexOf(answerCode, userGuess[check]);

                //If User Colour is Correct
                if (index > -1)
                {
                    //Correct Colour and Place
                    if (answerCode[check].Equals(userGuess[check]))
                    {
                        colPins[check] = 2;//Red
                    }
                    else
                    {
                        //Correct Colour
                        colPins[check] = 1;//White
                    }                    
                }
            }

            //Count Pins Needed to Display
            for(int chq = 0; chq < 4; chq++)
            {
                if (colPins[chq] == 2)
                {
                    ++red;
                }

                else if (colPins[chq] == 1)
                {
                    ++white;
                }
            }

            //Display Dots
            for (int ro = minRow; ro < maxRow; ro++)
            {
                for (int col = 0; col < 2; col++)
                {
                    Image change;
                    change = new Image();
                    GrdCorrectDisplay.Children.Add(change);

                    if(red > 0)
                    {
                        change.Source = "Images/RedDot.png";
                        red--;
                    }
                    else if (white > 0)
                    {
                        change.Source = "Images/WhiteDot.png";
                        white--;
                    }
                    
                    change.SetValue(Grid.RowProperty, ro);
                    change.SetValue(Grid.ColumnProperty, col);
                }
            }
            
            //Update Round Counter
            round++;
        }

        #endregion

        #region NewGame
        private void BtnNewGame_Clicked(object sender, EventArgs e)
        {
            NewGame();  
        }

        private void NewGame()
        {
            //Declare Variables
            randAns = new Random();

            for(int index = 0; index < 4; index++)
            {
                answerCode[index] = randAns.Next(1, 9);
            }
        }
        #endregion

        #region SaveGame
        private void BtnSaveGame_Clicked(object sender, EventArgs e)
        {

        }
        #endregion

        #region LoadGame
        private void BtnLoadGame_Clicked(object sender, EventArgs e)
        {

        }
        #endregion

        #region EndOfGame
        private void WinGame()
        {
            DisplayAlert("Game Over!", "Congratulations you have Won the game!", "Start New Game");
            NewGame();
        }

        private void LoseGame()
        {
            DisplayAlert("Game Over!", "Sorry you lose!!\nBetter Luck Next Time", "New Game");
            NewGame();
        }
        #endregion
    }
}