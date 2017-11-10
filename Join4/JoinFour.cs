using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Join4
{
    static class JoinFour
    {
        public static bool hasPlayerWon(ulong tiles)
        {
            /* The following conditions check if a winning connection has been
             * made in any of the four directions. By applying a bitshift to
             * the tiles and anding them, if a line exists for four consecutive
             * shifts, a connection has been made. The distance of each shift
             * depends on the direction. For checking vertical, a shift of 1
             * is applied four times (since the bitboard is numbered from the
             * bottom to the top). For checking the horizontal direction, a
             * shift of 7 is applied four times (such that position 0 on the
             * bitboard (bottom left corner) will be shifted to position 6,
             * 12 and 18. If after anding the shifts active bits remain, there
             * exists a connection of 4 tiles in that direction. */
            if ((tiles & tiles >> 6 & tiles >> 12 & tiles >> 18) != 0 // `\`
             || (tiles & tiles >> 7 & tiles >> 14 & tiles >> 21) != 0 // `-`
             || (tiles & tiles >> 8 & tiles >> 16 & tiles >> 24) != 0 // `/`
             || (tiles & tiles >> 1 & tiles >> 2 & tiles >> 3) != 0)  // `|`
                return true;
            /* Default return path */
            return false;
        }

        public static bool isMoveValid(ulong tiles, int x)
        {
            // If x is bigger less than 0 or more than 6, it is not a 
            // valid move
            if (x < 0 || x >= 7) return false;
            return getCol(tiles, x) != 63;
        }

        public static ulong applyMove(ulong tiles, ulong opponent, int x)
        {
            if (x < 0 || x > 7) return tiles;
            int col = (int) getCol(tiles | opponent, x);
            if (col == 63) return tiles;
            int y = (int) Math.Floor(col*0.5 + 32) - col;
            tiles |= ((ulong) y << (58 - (x * 7)));
            return tiles;
        }

        private static ulong getCol(ulong tiles, int x)
        {
            if (x < 0 || x > 7) return 0;
            return (tiles & (63UL << (58 - (x * 7)))) >> (58 - (x * 7));
        }
    }
}
