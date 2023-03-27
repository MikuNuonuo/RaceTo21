using System;
using System.Collections.Generic;

namespace RaceTo21
{
	public class Player
	{
        public string name;
		public List<Card> cards = new List<Card>(); //change String to Card
		public PlayerStatus status = PlayerStatus.active;
		public int score;

		public Player(string n)
		{
			name = n;
        }

		/* Introduces player by name
		 * Called by CardTable object
		 */
		public void Introduce(int playerNum)
		{
			Console.WriteLine("Hello, my name is " + name + " and I am player #" + playerNum);
		}

        /* show all cards on current players' hand
         * Is called by ShowHands() method
         * ShowHands() method provides player
         * Returns void
         */
        public void ShowHand(Player player)
        {
            if (player.cards.Count > 0)
            {
                Console.Write(player.name + " has: ");
                //use  for loop to iterate all cards on each players' hand
                for (int i = 0; i < player.cards.Count; i++)
                {
                    if (i < player.cards.Count - 1) //if this card isn't the last one, add "," 
                    {
                        Console.Write(player.cards[i].displayName + ",");
                    }
                    else
                    { //if it is the last card, we don't need "," in the end
                        Console.Write(player.cards[i].displayName + " ");
                    }

                }

                Console.Write("=" + player.score + "/21 ");
                if (player.status != PlayerStatus.active)
                {
                    Console.Write("(" + player.status.ToString().ToUpper() + ")");
                }
                Console.WriteLine();
            }
        }

        /*show all cards on each players' hand
         * Is called by Game object
         * Game object provides player list
         * Returns void*/
        public void ShowHands(List<Player> players)
        {
            foreach (Player player in players)
            {
                ShowHand(player);
            }
        }


        /*Show the name of winner 
         *Is called by Game object
         *Game object provides player's name.
         *Returns void
         */
        public void AnnounceWinner(Player player)
        {
            if (player != null)
            {
                Console.WriteLine(player.name + " wins!");
            }
            else
            {
                Console.WriteLine("Everyone busted!");
            }

        }

        /*Ending game
         *Is called by Game object when nobody want to play any more
         *no parameter
         *Returns void
         */
        public void RealEnd()
        {// page5, Level2. if nobody wants to continue playing game, it will become real end
            Console.Write("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}
	

