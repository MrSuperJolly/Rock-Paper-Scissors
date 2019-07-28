using System;
using System.Linq;

namespace Rock_Paper_Scissors
{
    class Program
    {
        static void Main(string[] args)
        {
            bool gameOver = false;
            GameData.Setup();
            Console.Clear();


            while (!gameOver)
            {
                
                
                Turn.GetPlayerChoice();
                Turn.SwitchPlayer();
                Turn.GetPlayerChoice();

                string winner = Results.PickWinner(GameData.allPlayers[0], GameData.allPlayers[1]);

                if (winner != "tie")
                {
                    Results.DisplayResults(winner);
                    gameOver = true;
                    Console.ReadKey();

                }
                else
                {
                    Console.Clear();
                    Results.DisplayResults(winner);
                    Turn.SwitchPlayer();
                   
                }
            }

        }
    }

    class GameData
    {
        public static int tieCount = 0;
        public static Player[] allPlayers = new Player[2];
        
        public static readonly Random rnd = new Random();

        public static Choice rock = new Choice("Rock");
        public static Choice paper = new Choice("Paper");
        public static Choice scissors = new Choice("Scissors");
        public static Choice lizard = new Choice("Lizard");
        public static Choice spock = new Choice("Spock");
        public static Choice[] allChoices = new Choice[] { rock, paper, scissors, lizard, spock };


        //SetupPlayers 
        public static void SetupPlayers()
        { 
            
            string input;

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Player " + (i + 1).ToString() + ". Enter your name or put 'ai' for a computer controlled player.");
                input = Console.ReadLine();

                if (input.ToLower() == "ai")
                {
                    allPlayers[i] = new Player("Computer", true);
                }
                else
                {
                    allPlayers[i] = new Player(input, false);
                }
            }

            
        }

        public static void SetupChoices()
        {
            rock.SetBeats(new Choice[] {scissors, lizard});
            scissors.SetBeats(new Choice[] {paper, lizard});
            paper.SetBeats(new Choice[] { rock, spock});
            spock.SetBeats(new Choice[] { scissors, rock});
            lizard.SetBeats(new Choice[] { paper, spock});
        }

        public static void Setup()
        {
            SetupPlayers();
            SetupChoices();

        }

        public static Choice GetMostUsedChoice()
        {
            Choice mostUsedChoice = rock;

            foreach (Choice choice in allChoices)
            {
                int usedCount = choice.usedCount;
                
                if(choice.usedCount > mostUsedChoice.usedCount)
                {
                    mostUsedChoice = choice;
                }

            }

            return mostUsedChoice;
        }

    }

    public class Turn
    {

        public static Player currentPlayer = GameData.allPlayers[0];

        //Get current players game choice and randomly picks a choice if it's computer controlled
        public static void GetPlayerChoice()
        {
            

            if (currentPlayer.isAI)
            {
                currentPlayer.currentChoice =  currentPlayer.GenerateRandomChoice();
                currentPlayer.currentChoice.usedCount++;
                return;
            }

            string input;
            bool validChoice = false;


            Console.WriteLine(currentPlayer.name + "'s Turn. Please pick Rock, Paper, Scissors, Lizard or Spock");

            while (!validChoice)
            {
                input = Console.ReadLine();

                for (int i = 0; i < GameData.allChoices.Length; i++)
                {
                    if (GameData.allChoices[i].name.ToLower() == input.ToLower())
                    {
                        currentPlayer.currentChoice = GameData.allChoices[i];
                        currentPlayer.currentChoice.usedCount++;
                        validChoice = true;
                        break;

                    }
                }

               if (!validChoice)
               {
                    Console.WriteLine("Please pick Rock, Paper, Scissors, Lizard or Spock");
               }
                    
                

            }
            
        }

        //switches active player
        public static void SwitchPlayer()
        {
            if(currentPlayer == GameData.allPlayers[0])
            {
                currentPlayer = GameData.allPlayers[1];
            }
            else
            {
                currentPlayer = GameData.allPlayers[0];
            }
        
            
        }

    }

    public class Results
    {

        

        public static String PickWinner(Player player1, Player player2)
        {
            Choice player1Choice = player1.currentChoice;
            Choice player2Choice = player2.currentChoice;

            if (player1Choice.name == player2Choice.name)
            {
                GameData.tieCount++;
                return "tie";
            }

            if(player1Choice.beats.Contains(player2Choice))
            {
                return player1.name;
            }

            if (player2Choice.beats.Contains(player1Choice))
            {
                return player2.name;
            }

            return "No winner Found";

        }

        public static void DisplayResults(string winner)
        {
            Console.Clear();

            if (winner != "tie")
            {
                Console.WriteLine(winner + " wins!");

                if (GameData.allPlayers[0].name == winner)
                {
                    Console.WriteLine(GameData.allPlayers[0].currentChoice.name + " beats " + GameData.allPlayers[1].currentChoice.name);
                }
                else if (GameData.allPlayers[1].name == winner)
                {
                    Console.WriteLine(GameData.allPlayers[1].currentChoice.name + " beats " + GameData.allPlayers[0].currentChoice.name);
                }

                Console.WriteLine("This game had " + GameData.tieCount + " ties");
                Choice mostUsedChoice = GameData.GetMostUsedChoice();

                Console.WriteLine("The most used choice was " + mostUsedChoice.name + " " +
                    "it was used " + mostUsedChoice.usedCount.ToString() + " times");

            }

            if(winner == "tie")
            {
                Console.WriteLine("Was a tie, pick again");
            }
            





        }

    }

    public class Player
    {
        public readonly bool isAI;
        public readonly string name;
        public Choice currentChoice;

        public Player(string name, bool isAI)
        {
            this.name = name;
            this.isAI = isAI;
        }

        public Choice GenerateRandomChoice()
        {
            Choice randomChoice;

            int random = GameData.rnd.Next(0, GameData.allChoices.Length);

            randomChoice = GameData.allChoices[random];

            currentChoice = randomChoice;
            return randomChoice;

        }
    }

    public class Choice
    {
        public readonly string name;
        public int usedCount = 0;
        public Choice[] beats; 
        
        public Choice(string name)
        {
            this.name = name;
            
        }
        
        public void SetBeats(Choice[] beats)
        {
            this.beats = beats;
        }
       
    }
}
