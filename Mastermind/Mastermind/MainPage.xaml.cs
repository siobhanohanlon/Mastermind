using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Mastermind
{
    public partial class MainPage : ContentPage
    {
        #region GlobalVariables
        private SaveGame saveData = new SaveGame();

        //Constants
        private const int NUM_ROW = 11, NUM_COL = 4; //Guess Area
        private const int COR_ROW = 30, COR_COL = 2; //Correct Display
        private const int NUM_ROUNDS = 11; //As Starting Round is 1, Actual #Rounds is 10
        
        #region Instructions Constant
        private const string instructions= 
            "-The Computer picks a random sequence of 4 colours\n" +
            "-The Objective is to guess the sequence correctly in 10 turns\n" +
            "-To select a colour just tap it and then select anywhere on the current row, " +
            "(this row will be a lighter shade)\n" +
            "-You are able to change an already selected colour if it is this that round, " +
            "you can do this the same as picking a colour at first\n" +
            "-After filling a line with your guesses, click 'Check Guess' Button\n" +
            "-Pins will show up at the side for each correct guess\n" +
            "\t-Red: Correct Colour and Correct Position\n" +
            "\t-White: Correct Colour but Wrong Position\n" +
            "\t-None: All Colours are Wrong\n\n" +
            "To Win: Use the pins at the side to help you figure out the sequence " +
            "in 10 goes, if you run out of goes.....You Lose! and Start a New Game\n\n\n" +
            "GoodLuck!\n\n";
        #endregion

        //BoxView
        BoxView currPinSelected;

        //Create an instance of Random class
        Random randAns;

        //Global Variables
        private int round, currRow, pinDisRow, r;
        private readonly Color guessAreaColour = Color.SaddleBrown;

        //Array for Colours
        private static readonly string[] ColourNames = {"Red", "Green", "Blue", "Black",
                                               "White", "Purple", "Yellow", "Orange"};

        private readonly Color[] pinColours = new Color[8]
            {Color.Red, Color.Green, Color.Blue, Color.Black,
            Color.White, Color.Purple, Color.Yellow, Color.Orange};

        private readonly int[] answerCode = new int[4];
        private readonly int[] userGuess = new int[4];
        private readonly int[] colPins = new int[4];

        public static int[,] pastGuess = new int[10,4];
        public static int[,] corPins = new int[10,4];
        #endregion

        //Main
        public MainPage()
        {
            //Start Up Game and Set Up Board
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

            for (int i = 0; i < 16; i++)
            {
                GrdGameLayout.RowDefinitions.Add(new RowDefinition());
            }
            #endregion

            #region AssignAnswerAreaImages
            Answer1.Source = "Images/QuestionMark.png";
            Answer2.Source = "Images/QuestionMark.png";
            Answer3.Source = "Images/QuestionMark.png";
            Answer4.Source = "Images/QuestionMark.png";
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
                    BoxView emptyGuess = new BoxView() 
                    {
                        Color = guessAreaColour, 
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        HeightRequest = 40, WidthRequest = 40,
                        CornerRadius = 40
                    };

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
                        BoxView pin = new BoxView()
                        {
                            Color = pinColours[h],
                            StyleId = ColourNames[h]+"Pin",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            HeightRequest = 40,
                            WidthRequest = 40,
                            CornerRadius = 40
                        };

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
            for (int i = 0; i < 33; i++)
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
                        guessSelect = new Image()
                        {
                            Source = "Images/Empty.png"
                        };

                        //Set Value
                        guessSelect.SetValue(Grid.RowProperty, r);
                        guessSelect.SetValue(Grid.ColumnProperty, c);


                        //Add to Children
                        GrdCorrectDisplay.Children.Add(guessSelect);

                        //Create Boxviews for Pins
                        BoxView change;
                        change = new BoxView()
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            CornerRadius = 100, Scale = .75
                        }; 

                        GrdCorrectDisplay.Children.Add(change);
                        change.SetValue(Grid.RowProperty, r);
                        change.SetValue(Grid.ColumnProperty, c);
                    }
                }
            }
            #endregion

            #region CreateHowToButton
            Button BtnInstructions;
            BtnInstructions = new Button()
            {
                Text = "Click to Read How To Play",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            BtnInstructions.SetValue(Grid.RowProperty, 14);
            BtnInstructions.SetValue(Grid.ColumnProperty, 0);
            BtnInstructions.SetValue(Grid.ColumnSpanProperty, 6);
            BtnInstructions.Clicked += BtnHowTo_Clicked;
            GrdGameLayout.Children.Add(BtnInstructions);

            #endregion

            #region CreateCheckButton
            Button btnCheckGuess;
            btnCheckGuess = new Button()
            {
                Text = "Check Guess",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            btnCheckGuess.SetValue(Grid.RowProperty, 13);
            btnCheckGuess.SetValue(Grid.ColumnProperty, 4);
            btnCheckGuess.SetValue(Grid.ColumnSpanProperty, 2);
            btnCheckGuess.Clicked += BtnCheckGuess_Clicked;
            GrdGameLayout.Children.Add(btnCheckGuess);
            #endregion

            #region CreateNewGameButton
            Button BtnNewGame;
            BtnNewGame = new Button()
            {
                Text = "New Game",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                
            };

            BtnNewGame.SetValue(Grid.RowProperty, 15);
            BtnNewGame.SetValue(Grid.ColumnProperty, 0);
            BtnNewGame.SetValue(Grid.ColumnSpanProperty, 2);
            BtnNewGame.Clicked += BtnNewGame_Clicked;
            GrdGameLayout.Children.Add(BtnNewGame);
            #endregion

            #region CreateSaveButton
            Button BtnSaveGame;
            BtnSaveGame = new Button()
            {
                Text = "Save Game",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            BtnSaveGame.SetValue(Grid.RowProperty, 15);
            BtnSaveGame.SetValue(Grid.ColumnProperty, 2);
            BtnSaveGame.SetValue(Grid.ColumnSpanProperty, 2);
            BtnSaveGame.Clicked += BtnSaveGame_Clicked;
            GrdGameLayout.Children.Add(BtnSaveGame);
            #endregion

            #region CreateLoadButton
            Button BtnLoadGame;
            BtnLoadGame = new Button()
            {
                Text = "Load Game",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            BtnLoadGame.SetValue(Grid.RowProperty, 15);
            BtnLoadGame.SetValue(Grid.ColumnProperty, 4);
            BtnLoadGame.SetValue(Grid.ColumnSpanProperty, 2);
            BtnLoadGame.Clicked += BtnLoadGame_Clicked;
            GrdGameLayout.Children.Add(BtnLoadGame);
            #endregion

            //Clear Selected
            currPinSelected = null;

            DisplayAlert("Welcome to MasterMind", "Start Row will be Highlighted to you.\n\nGoodluck!","Lets Play");

            //Start Game
            NewGame();
        }

        #region HighlightCurrentRow
        private void FindCurrentRow()
        {
            int findCR = NUM_ROW - round;

            if(findCR > 0 && findCR < 11)
            {
                currRow = findCR;
            }
            else
            {
                currRow = -1;
            }
        }

        private void HighlightRow(int currRow)
        {
            foreach(var cur in GrdGameLayout.Children)
            {
                if (cur.GetType() == typeof(BoxView))
                {
                    FindCurrentRow();

                    if ((int)cur.GetValue(Grid.RowProperty) == currRow &&
                        (int)cur.GetValue(Grid.RowProperty) < 11 &&
                        (int)cur.GetValue(Grid.RowProperty) > 1)
                    {
                        if (((BoxView)cur).Color == guessAreaColour)
                        {
                            ((BoxView)cur).Opacity = 0.45;
                        }

                        else
                        {
                            ((BoxView)cur).Opacity = 1;
                        }
                    }

                    else
                    {
                        ((BoxView)cur).Opacity = 1;
                    }
                }
            }
        }
        #endregion

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
                        //Reset Opacity from highlighted rows
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
            FindCurrentRow();

            if(currRow > 0)
            {
                //This Piece is Selected
                currPinSelected = thisOne;

                //Change Opacity
                currPinSelected.Opacity = 0.5;
            }
            else
            {
                currPinSelected = null;
            }
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
            placePin = new BoxView()
            {
                Color = currPinSelected.Color,
                StyleId = "SelectedPin",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = currPinSelected.HeightRequest,
                WidthRequest = currPinSelected.WidthRequest,
                CornerRadius = currPinSelected.CornerRadius
            };

            placePin.GestureRecognizers.Add(pinTap);

            int rnd = (NUM_ROUNDS - round);

            FindCurrentRow();

            if (destRow == currRow && currRow > 0)
            {
                placePin.SetValue(Grid.RowProperty, destRow);
                placePin.SetValue(Grid.ColumnProperty, destCol);

                GrdGameLayout.Children.Add(placePin);

                GrdGameLayout.Children.Remove(currSq);

                PlacePins(placePin);
            }
        }
        #endregion

        #region PlacePins
        private void PlacePins(BoxView placePin) 
        {
            int row = (int)((BoxView)placePin).GetValue(Grid.RowProperty);
            if(row == currRow)
            {
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

                //SaveAllUserGuessToArray
                if (currRow > 0 && currRow < 11)
                {
                    for (int all = 0; all < 4; all++)
                    {
                        saveData.pastGuess[r, all] = userGuess[all];
                    }
                }
            }
        }

        private void PlacePins(int guess, int destRow, int destCol)
        {
            //Declare Variables
            int boxRow, boxCol, colour; ;

            foreach (var sPin in GrdGameLayout.Children)
            {
                //If it is equal to the boxview
                if (sPin.GetType() == typeof(BoxView))
                {
                    boxRow = (int)((BoxView)sPin).GetValue(Grid.RowProperty);
                    boxCol = (int)((BoxView)sPin).GetValue(Grid.ColumnProperty);

                    if(boxRow > 0 && boxRow < 11)
                    {
                        if (boxRow == destRow && boxCol == destCol)
                        {
                            switch (guess)
                            {
                                case 1:
                                    ((BoxView)sPin).Color = Color.Red;
                                    colour = 1;
                                    break;
                                case 2:
                                    ((BoxView)sPin).Color = pinColours[1];
                                    colour = 2;
                                    break;
                                case 3:
                                    ((BoxView)sPin).Color = pinColours[2];
                                    colour = 3;
                                    break;
                                case 4:
                                    ((BoxView)sPin).Color = pinColours[3];
                                    colour = 4;
                                    break;
                                case 5:
                                    ((BoxView)sPin).Color = pinColours[4];
                                    colour = 5;
                                    break;
                                case 6:
                                    ((BoxView)sPin).Color = pinColours[5];
                                    colour = 6;
                                    break;
                                case 7:
                                    ((BoxView)sPin).Color = pinColours[6];
                                    colour = 7;
                                    break;
                                case 8:
                                    ((BoxView)sPin).Color = pinColours[7];
                                    colour = 8;
                                    break;

                                default:
                                    ((BoxView)sPin).Color = guessAreaColour;
                                    ((BoxView)sPin).StyleId = "GuessArea";
                                    colour = 0;
                                    break;
                            }

                            if (((BoxView)sPin).Color == guessAreaColour)
                            {
                                ((BoxView)sPin).Opacity = 0.45;
                            }

                            //Set User Guess to Current Row
                            userGuess[destCol] = colour;
                        }
                    }
                }
            }
        }

        private int PlaceCorrectPins()
        {
            int minRow, maxRow, win;

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
                    minRow = 0;
                    maxRow = 0;
                    break;
            }

            win = PlaceCorrectPins(minRow, maxRow);

            return win;
        }

        private int PlaceCorrectPins(int minRow, int maxRow)
        {
            //Declare Variables
            int red = 0, white = 0, win = 0;


            DisplayAlert("h", colPins[0].ToString() + " " +
                colPins[1].ToString() + " " +
                colPins[2].ToString() + " " +
                colPins[3].ToString(), "k");
            //Count Pins Needed to Display
            for (int chq = 0; chq < 4; chq++)
            {
                if (colPins[chq] == 2)
                {
                    ++red;
                    ++win;
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
                    foreach (var change in GrdCorrectDisplay.Children)
                    {
                        if (change.GetType() == typeof(BoxView))
                        {
                            if ((int)change.GetValue(Grid.RowProperty) == (pinDisRow + col))
                            {
                                if (red > 0)
                                {
                                    ((BoxView)change).Color = Color.Red;
                                    red--;
                                }
                                else if (white > 0)
                                {
                                    ((BoxView)change).Color = Color.White;
                                    white--;
                                }
                            }
                        }
                    }
                }
            }

            //Return
            return win;
        }
        #endregion

        #region HowToButton
        private void BtnHowTo_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("MasterMind How to Play", instructions,"Back to Game");
        }
        #endregion

        #region CheckButton
        //Check if answer is Correct
        private void BtnCheckGuess_Clicked(object sender, EventArgs e)
        {
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
                FindCurrentRow();
                pinDisRow = (currRow * 3) - 2;
                CheckUserGuess();

                DisplayAnswer();
            }
        }

        //Check if user entered 4 colours in current row
        private bool CheckCurrRow()
        {
            //Declare Variables
            bool rowFull = true;
            FindCurrentRow();

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

        #region Play
        private void CheckUserGuess()
        {
            //Declare Variables
            //MinRow - start, MaxRow - Finish
            int win, ansDup = 0, userDup = 0, index;
            bool redFound, found;

            FindCurrentRow();

            //Check for Duplicates in Answer Code
            for (int dup1 = 0; dup1 < 4; dup1++)
            {
                found = false;
                for (int dup2 = 0; dup2 < 4; dup2++)
                {
                    if(dup1 < dup2 && found == false)
                    {
                        if (answerCode[dup1].Equals(answerCode[dup2]))
                        {
                            ++ansDup;

                            found = true;
                        }
                    }
                }
            }

            //Check for Duplicates in User Code
            for (int dup1 = 0; dup1 < 4; dup1++)
            {
                found = false;
                for (int dup2 = 0; dup2 < 4; dup2++)
                {
                    if (dup1 < dup2 && found == false)
                    {
                        if (userGuess[dup1].Equals(userGuess[dup2]))
                        {
                            ++userDup;

                            found = true;
                        }
                    }
                }
            }

            for (int check = 0; check < 4; check++)
            {
                redFound = false;

                //Correct Colour and Place
                if (answerCode[check].Equals(userGuess[check]))
                {
                    colPins[check] = 2;//Red
                            
                    redFound = true;
                }
                    
                //If User Colour is Correct
                if (redFound == false)
                {
                    index = Array.IndexOf(answerCode, userGuess[check]);
                    if (index > -1)
                    {
                        //Correct Colour
                        colPins[check] = 1;//White
                    }
                    else
                    {
                        colPins[check] = 0;//Clear
                    }
                }

                //Duplicates Get rid
                if(userDup > ansDup)
                {
                    colPins[check] = 0;
                    userDup--;
                }
            }

            win = PlaceCorrectPins();

            //Update Round Counter
            UpdateRound();

            //If User Won
            if (win == 4 || round == NUM_ROUNDS)
            {
                EndOfGame(win);
            }
                        
            //Update Row highlighted
            currRow = NUM_ROW - round;

            //Update Saved Round
            saveData.round = round-1;
            
            //Update Highlight Row
            HighlightRow(currRow);
        }

        private void UpdateRound()
        {
            round++;
            r++;

            //Clear Guess Array
            for(int clear = 0; clear < 4; clear++)
            {
                userGuess[clear] = 0;
            }
        }
        #endregion

        #region NewGame
        private async void BtnNewGame_Clicked(object sender, EventArgs e)
        {
            bool askRestart = await DisplayAlert("Start a New Game", "Are you sure you want to start a new game?", "Yes", "No");

            if(askRestart == false)
            {
                return;
            }

            //Start a new game
            NewGame();  
        }

        private void NewGame()
        {
            //Reset Variables
            randAns = new Random();
            currPinSelected = null;
            round = 1;
            r = 0;

            FindCurrentRow();

            for (int index = 0; index < 4; index++)
            {
                answerCode[index] = randAns.Next(1, 9);
                userGuess[index] = 0;
                colPins[index] = 0;

                //Save Answer Code
                saveData.answerCode[index] = answerCode[index];
            }

            //Reset Answer Area
            Answer1.Source = "Images/QuestionMark.png";
            Answer2.Source = "Images/QuestionMark.png";
            Answer3.Source = "Images/QuestionMark.png";
            Answer4.Source = "Images/QuestionMark.png";

            foreach (var item in GrdGameLayout.Children)
            {
                if (item.GetType() == typeof(BoxView))
                {
                    if ((int)((BoxView)item).GetValue(Grid.RowProperty) < 11)
                    {
                        ((BoxView)item).Color = guessAreaColour;
                        ((BoxView)item).StyleId = "GuessArea";
                    }
                }
            }

            foreach (var item in GrdCorrectDisplay.Children)
            {
                if (item.GetType() == typeof(BoxView))
                {
                    ((BoxView)item).Color = Color.Transparent;
                }
            }

            //Highlight Row
            FindCurrentRow();
            HighlightRow(currRow);
        }
        #endregion

        #region SaveGame
        private async void BtnSaveGame_Clicked(object sender, EventArgs e)
        {
            //Create File Variables
            string fullFileName, path, text;

            saveData.round = round;

            //Assign variables
            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            fullFileName = Path.Combine(path, "SaveGameData.txt");

            text = JsonConvert.SerializeObject(saveData);

            File.WriteAllText(fullFileName, text);

            await DisplayAlert("Saved!", "Game Has been Saved", "Okay");
        }
        #endregion

        #region LoadGame
        private void BtnLoadGame_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Loading......", "Game is Loading\nPlease wait.", "Close");

            string fullFileName, path, text;
            int rnd;

            //Assign variables
            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            fullFileName = Path.Combine(path, "SaveGameData.txt");

            if (File.Exists(fullFileName) == true)
            {
                text = File.ReadAllText(fullFileName);
                saveData = JsonConvert.DeserializeObject<SaveGame>(text);

                //Variables
                round = saveData.round;
                rnd = saveData.round;
                int userG, row, rowsUsed = 10;
                row = NUM_ROW - rnd;

                //Load Answer Code
                for(int l = 0; l < 4; l++)
                {
                    answerCode[l] = saveData.answerCode[l];
                }

                for (int load = 0; load < saveData.pastGuess.GetLength(0); load++)
                {
                    FindCurrentRow();
                    if (rnd > 0 && rowsUsed >= row)
                    {
                        for (int j = 0; j < pastGuess.GetLength(1); j++)
                        {
                            userG = saveData.pastGuess[load, j];

                            PlacePins(userG, rowsUsed, j);
                        }

                        CheckUserGuess();

                        --rnd;
                        --rowsUsed;
                    }
                }

                round = saveData.round;
                DisplayAlert("h", round.ToString(), "k");

                //Reset round Counter
                FindCurrentRow();

                HighlightRow(currRow);

                DisplayAlert("Game Loaded Successfully!", "Game Has been Loaded", "Close");
            }

            else
            {
                DisplayAlert("ERROR!", "Game could not be Loaded\n" +
                    "Please Save a Game and try again after", "Close");
            }
        }
        #endregion

        #region EndOfGame
        private void EndOfGame(int win)
        {
            //Display Answer
            DisplayAnswer();

            currRow = 0;
            round = 11;

            //Winner
            if(win == 4)
            {
                DisplayAlert("Game Over!", "Congratulations you have Won the game!", "Look at Results");
            }

            //Loser
            else
            {
                DisplayAlert("Game Over!", "Sorry you lose!!\nBetter Luck Next Time", "Look at Results");
            }
        }

        private void DisplayAnswer()
        {
            switch (answerCode[0])
                {
                    case 1:
                        Answer1.Source = "Images/RedDot.png";
                        break;
                    case 2:
                        Answer1.Source = "Images/GreenDot.png";
                        break;
                    case 3:
                        Answer1.Source = "Images/BlueDot.png";
                        break;
                    case 4:
                        Answer1.Source = "Images/BlackDot.png";
                        break;
                    case 5:
                        Answer1.Source = "Images/WhiteDot.png";
                        break;
                    case 6:
                        Answer1.Source = "Images/PurpleDot.png";
                        break;
                    case 7:
                        Answer1.Source = "Images/YellowDot.png";
                        break;
                    case 8:
                        Answer1.Source = "Images/OrangeDot.png";
                        break;

                    default:
                        break;
                }

            switch (answerCode[1])
            {
                case 1:
                    Answer2.Source = "Images/RedDot.png";
                    break;
                case 2:
                    Answer2.Source = "Images/GreenDot.png";
                    break;
                case 3:
                    Answer2.Source = "Images/BlueDot.png";
                    break;
                case 4:
                    Answer2.Source = "Images/BlackDot.png";
                    break;
                case 5:
                    Answer2.Source = "Images/WhiteDot.png";
                    break;
                case 6:
                    Answer2.Source = "Images/PurpleDot.png";
                    break;
                case 7:
                    Answer2.Source = "Images/YellowDot.png";
                    break;
                case 8:
                    Answer2.Source = "Images/OrangeDot.png";
                    break;

                default:
                    break;
            }

            switch (answerCode[2])
            {
                case 1:
                    Answer3.Source = "Images/RedDot.png";
                    break;
                case 2:
                    Answer3.Source = "Images/GreenDot.png";
                    break;
                case 3:
                    Answer3.Source = "Images/BlueDot.png";
                    break;
                case 4:
                    Answer3.Source = "Images/BlackDot.png";
                    break;
                case 5:
                    Answer3.Source = "Images/WhiteDot.png";
                    break;
                case 6:
                    Answer3.Source = "Images/PurpleDot.png";
                    break;
                case 7:
                    Answer3.Source = "Images/YellowDot.png";
                    break;
                case 8:
                    Answer3.Source = "Images/OrangeDot.png";
                    break;

                default:
                    break;
            }

            switch (answerCode[3])
            {
                case 1:
                    Answer4.Source = "Images/RedDot.png";
                    break;
                case 2:
                    Answer4.Source = "Images/GreenDot.png";
                    break;
                case 3:
                    Answer4.Source = "Images/BlueDot.png";
                    break;
                case 4:
                    Answer4.Source = "Images/BlackDot.png";
                    break;
                case 5:
                    Answer4.Source = "Images/WhiteDot.png";
                    break;
                case 6:
                    Answer4.Source = "Images/PurpleDot.png";
                    break;
                case 7:
                    Answer4.Source = "Images/YellowDot.png";
                    break;
                case 8:
                    Answer4.Source = "Images/OrangeDot.png";
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}