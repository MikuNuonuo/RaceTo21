using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class Game
    {
        public int numberOfPlayers = 1;// number of players in current game
        public List<Player> players = new List<Player>(); // list of objects containing player data
        public string playerName = "";
        public int cardCount; //count how much card play want
        public Deck deck {
            get; private set;
        } // deck of cards
        public int currentPlayer = 0; // current player on list
        public GameTask nextTask;// keeps track of game state
        public bool isEmpty = true;
        public string winner = "";
        public Game()
        {
            deck = new Deck();
            deck.Shuffle();
            deck.ShowAllCards();
            nextTask = GameTask.welcomePage;
        }

        /* Adds a player to the current game
         * Called by DoNextTask() method
         * Return void
         */
        public void AddPlayer(string n)
        {
            players.Add(new Player(n));
        }



        /* Figures out what task to do next in game
         * as represented by field nextTask
         * Calls methods required to complete task
         * then sets nextTask.
         * calls DealTopCard(), AddPlayer(), GetNumberOfPlayers(), GetPlayerName(), OfferACard(), AnnounceWinner(), DoFinalScoring() method
         */
        public void DoNextTask()
        {
            if (nextTask == GameTask.welcomePage)
            {
                nextTask = GameTask.GetNumberOfPlayers;
            }
            else if (nextTask == GameTask.GetNumberOfPlayers)
            {
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    players.Add(new Player(""));
                }
                nextTask = GameTask.GetNames;
            }
            else if (nextTask == GameTask.GetNames)
            {
                //for (var count = 1; count <= numberOfPlayers; count++)
                //{
                // var name = playerName;
                //AddPlayer(name); // NOTE: player list will start from 0 index even though we use 1 for our count here to make the player numbering more human-friendly
                nextTask = GameTask.PlayerTurn;


            }
            else if (nextTask == GameTask.PlayerTurn)
            {
                nextTask = GameTask.GameWinner;
                /*cardTable.ShowHand(player);*/
                //nextTask = GameTask.CheckForEnd;
            }
            else if (nextTask == GameTask.CheckForEnd)
            {

            }
            else if (nextTask == GameTask.GameWinner)
            {
                
                newRound();
            }

            else // we shouldn't get here...
            {
                Console.WriteLine("I'm sorry, I don't know what to do now!");
                nextTask = GameTask.GameOver;
            }

        }

        private void newRound() {
            Player winner = DoFinalScoring();
            bool winner_continue = winner.isContinue;
            List<Player> ContinuePlayers = new List<Player>(); //page2, level2, checke whether players want to continue games
            foreach (Player player in players)
            {
                if (player.isContinue == true) {
                    player.cards.Clear(); //clear all hand cards
                    player.score = 0; //reset score
                    player.status = PlayerStatus.active; // player state become active
                    player.isContinue = false;
                    ContinuePlayers.Add(player);
                }
            }
            numberOfPlayers = ContinuePlayers.Count;
            if (numberOfPlayers >= 2) //if more than 1 player continue playing game, game will begin again
            {
                deck = new Deck();//new deck to start a new game
                deck.Shuffle();
                players = ContinuePlayers;
                currentPlayer = 0; //back to the first player
                PlayerShuffle(); //shuffle players
                if (winner_continue) 
                {
                    players.Remove(winner);//remove winner from players
                    players.Add(winner);//page2, leve3, add winner to the end of players as the dealer
                }
                this.winner = "";
                nextTask = GameTask.PlayerTurn;
            }
            else 
            {
                nextTask = GameTask.GameOver;
            }
        }

        public void checkForEnd()
        {
            if (!CheckActivePlayers())
            {
                Player winner = DoFinalScoring();
                winner.status = PlayerStatus.win;
                AnnounceWinner(winner); //output the winner of this round     
            }
            else
            {
                currentPlayer++;
                if (currentPlayer > players.Count - 1)
                {
                    currentPlayer = 0; // back to the first player...
                }
                nextTask = GameTask.PlayerTurn;
            }

        }

           
        



        public int ScoreHand(Player player)
        {
            int score = 0;           
            
                foreach (Card card in player.cards)
                {
                    string faceValue = card.id.Remove(card.id.Length - 1);
                    switch (faceValue)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            score = score + 10;
                            break;
                        case "A":
                            score = score + 1;
                            break;
                        default:
                            score = score + int.Parse(faceValue);
                            break;
                    }
                }           
            return score;
        }

        public bool CheckActivePlayers()
        {
            int bustNumber = 0; //count number of bust players 
            int stayNumber = 0; // count number of stay players 
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.win)
                {
                    return false; //if anyone is won, stop checking
                }
                if (player.status == PlayerStatus.bust)
                {
                    bustNumber++; //count bust players
                }
                if (player.status == PlayerStatus.stay)
                {
                    stayNumber++; //count stay players
                }

            }
            if (bustNumber == numberOfPlayers - 1)
            {
                return false; //Just one remaining players
            }
            if (stayNumber == numberOfPlayers - bustNumber) {
                return false; //still alive player select stay
            }
            return true; //nobody win or at least 2 remaining players 
        }

        public void AnnounceWinner(Player player)
        {
            if (player != null)
            {
                winner = player.name + " wins!";
            }
            else
            {
                winner = "Everyone busted!";
            }

        }

        public Player DoFinalScoring()
        {
            int highScore = 0;
            foreach (Player player in players)
            {
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    return player;
                }
            }
            foreach (Player player in players)
            {
                if (player.status == PlayerStatus.active)
                {
                    return player; //when just one player is active means he/she is the last remaining player and is the winner.
                }
                if (player.status == PlayerStatus.stay) // still could win...
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                
                // if busted don't bother checking!
            }
            if (highScore > 0) // someone scored, anyway!
            {
                // find the FIRST player in list who meets win condition
                return players.Find(player => player.score == highScore);
            }
            return null; // everyone must have busted because nobody won!
        }

        /* Shuffle players list
         * Is called by DoNextTask() in Game and when CheckActivePlayers is false and start a new game
         * no parameter
         * no return
         */
        public void PlayerShuffle()// page2, level2
        {
            Console.WriteLine("Shuffling players...");

            Random rng = new Random();

            for (int i = 0; i < players.Count; i++)
            {
                Player tmp = players[i];
                int swapindex = rng.Next(players.Count);
                players[i] = players[swapindex];
                players[swapindex] = tmp;
            }
        }
    }
}
