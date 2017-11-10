using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Join4
{
    class GameInstance
    {

        /* Bitboard for player one and player two which holds
         * the tiles that each player has placed.
         * 
         * The bitboards have the following layout:
         *   5   12  19  26  33  40  46
         *   4   11  18  25  32  39  45
         *   3   10  17  24  31  38  44
         *   2   9   16  23  30  37  43
         *   1   8   15  22  29  36  42
         *   0   7   14  21  28  35  41 */
        public ulong[] players = new ulong[2];

        public bool gameFinished = false;
        public bool playerOnePlaying = true;

        public enum GameType { PlayerVsComputer, PlayerVsPlayer }
        public GameType type = GameType.PlayerVsComputer;

        public GameInstance()
        {
            players[0] = 0;
            players[1] = 0;
        }

        public GameInstance(GameType type)
        {
            this.type = type;
            players[0] = 0;
            players[1] = 0;
        }

        public bool submitMove(int x)
        {
            if (!JoinFour.isMoveValid(players[0] | players[1], x)) return false;

            if (playerOnePlaying)
            {
                players[0] = JoinFour.applyMove(players[0], players[1], x);
                if (JoinFour.hasPlayerWon(players[0])) gameFinished = true;
                else if (type == GameType.PlayerVsComputer)
                {
                    players[1] = JoinFour.applyMove(
                        players[1], 
                        players[0], 
                        AIPlayer.generateNextMove(players[1], players[0]));
                    if (JoinFour.hasPlayerWon(players[1])) gameFinished = true;
                } else
                {
                    playerOnePlaying = false;
                }
                
            } else
            {
                players[1] = JoinFour.applyMove(players[1], players[0], x);
                if (JoinFour.hasPlayerWon(players[1])) gameFinished = true;
                playerOnePlaying = true;
            }
            
            return true;
        }

        public void restart()
        {
            players[0] = 0;
            players[1] = 0;
            gameFinished = false;
        }

    }
}
