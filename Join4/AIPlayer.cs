using System;
using System.Collections.Generic;

namespace Join4
{
    static class AIPlayer
    {
        static int searchDepth = 2;

        public static int generateNextMove(ulong player, ulong opponent)
        {
            Node node = new Node {
                opponentPlaying = false,
                player = player,
                opponent = opponent
            };
            return (int)alphabeta(node, searchDepth, Double.NegativeInfinity, Double.PositiveInfinity, true, true);
        }

        /* Node in a search tree */
        public class Node
        {
            public Node[] children; // Child nodes
            public bool opponentPlaying; // True if it is the opponents turn
            public ulong player; // Bitboard of the player
            public ulong opponent; // Bitboard of the opponent 
            public int bestMove; // The best move between 0-6
            public int lastMove;
        }

        static double alphabeta(Node node, int depth, double alpha, double beta, bool max, bool oc)
        {
            node.children = generateMoves(node);
            if (depth == 0 || node.children.Length == 0) return evaluateNode(node);
            if (max)
            {
                double v = Double.NegativeInfinity;
                foreach (Node child in node.children)
                {
                    double ab = alphabeta(child, depth - 1, alpha, beta, false, false);
                    if (ab > v)
                    {
                        v = ab;
                        node.bestMove = child.lastMove;
                    }
                    if (v > alpha)
                    {
                        alpha = v;
                    }
                    if (beta <= alpha) break;
                }
                if (!oc) return v;
                else return node.bestMove;
            } else
            {
                double v = Double.PositiveInfinity;
                foreach (Node child in node.children)
                {
                    double ab = alphabeta(child, depth - 1, alpha, beta, true, false);
                    if (ab < v)
                    {
                        v = ab;
                        node.bestMove = child.lastMove;
                    }
                    if (v < beta)
                    {
                        beta = v;
                    }
                    if (beta <= alpha) break;
                }
                if (!oc) return v;
                else return node.bestMove;
            }
        }

        static Node[] generateMoves(Node node)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < 7; i++)
            {
                if (JoinFour.isMoveValid(node.player | node.opponent, i)
                    && !JoinFour.hasPlayerWon(node.player) 
                    && !JoinFour.hasPlayerWon(node.opponent))
                {
                    nodes.Add(new Node
                    {
                        opponentPlaying = !node.opponentPlaying,
                        player = (node.opponentPlaying) ?
                            node.player :
                            JoinFour.applyMove(node.player, node.opponent, i),
                        opponent = (node.opponentPlaying) ?
                            JoinFour.applyMove(node.opponent, node.player, i) :
                            node.opponent,
                        lastMove = i
                    });
                }
            }

            return nodes.ToArray();
        }

        static double evaluateNode(Node node)
        {
            if (JoinFour.hasPlayerWon(node.player)) return 100;
            if (JoinFour.hasPlayerWon(node.opponent)) return -100;

            double score = 0;
            score += 0.1 * getThreeOpenConnections(node.player, node.opponent);
            score -= 0.1 * getThreeOpenConnections(node.opponent, node.player);
            score += 0.01 * getTwoOpenConnections(node.player, node.opponent);
            score -= 0.01 * getTwoOpenConnections(node.opponent, node.player);

            return score;
        }

        static int getThreeOpenConnections(ulong player, ulong opponent)
        {
            int count = 0;
            // Diagonal `\`
            int s = 6;
            //  (_XXX)
            count += countBits(~opponent & (player >> s) & (player >> s * 2) & (player >> s * 3));
            //  (X_XX)
            count += countBits(player & (~opponent >> s) & (player >> s * 2) & (player >> s * 3));
            //  (XX_X)
            count += countBits(player & (player >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (XXX_)
            count += countBits(player & (player >> s) & (player >> s * 2) & (~opponent >> s * 3));

            // Horizontal `-`
            s = 7;
            //  (_XXX)
            count += countBits(~opponent & (player >> s) & (player >> s * 2) & (player >> s * 3));
            //  (X_XX)
            count += countBits(player & (~opponent >> s) & (player >> s * 2) & (player >> s * 3));
            //  (XX_X)
            count += countBits(player & (player >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (XXX_)
            count += countBits(player & (player >> s) & (player >> s * 2) & (~opponent >> s * 3));

            // Diagonal `/`
            s = 8;
            //  (_XXX)
            count += countBits(~opponent & (player >> s) & (player >> s * 2) & (player >> s * 3));
            //  (X_XX)
            count += countBits(player & (~opponent >> s) & (player >> s * 2) & (player >> s * 3));
            //  (XX_X)
            count += countBits(player & (player >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (XXX_)
            count += countBits(player & (player >> s) & (player >> s * 2) & (~opponent >> s * 3));

            // Vertical `|`
            s = 1;
            //  (XXX_)
            count += countBits(player & (player >> s) & (player >> s * 2) & (~opponent >> s * 3));

            return count;
        }

        static int getTwoOpenConnections(ulong player, ulong opponent)
        {
            int count = 0;
            // Diagonal `\`
            int s = 6;
            //  (__XX)
            count += countBits(~opponent & (~opponent >> s) & (player >> s * 2) & (player >> s * 3));
            //  (X__X)
            count += countBits(player & (~opponent >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (XX__)
            count += countBits(player & (player >> s) & (~opponent >> s * 2) & (~opponent >> s * 3));
            //  (_X_X)
            count += countBits(~opponent & (player >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (X_X_)
            count += countBits(player & (~opponent >> s) & (player >> s * 2) & (~opponent >> s * 3));

            // Horizontal `-`
            s = 7;
            //  (__XX)
            count += countBits(~opponent & (~opponent >> s) & (player >> s * 2) & (player >> s * 3));
            //  (X__X)
            count += countBits(player & (~opponent >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (XX__)
            count += countBits(player & (player >> s) & (~opponent >> s * 2) & (~opponent >> s * 3));
            //  (_X_X)
            count += countBits(~opponent & (player >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (X_X_)
            count += countBits(player & (~opponent >> s) & (player >> s * 2) & (~opponent >> s * 3));

            // Diagonal `/`
            s = 8;
            //  (__XX)
            count += countBits(~opponent & (~opponent >> s) & (player >> s * 2) & (player >> s * 3));
            //  (X__X)
            count += countBits(player & (~opponent >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (XX__)
            count += countBits(player & (player >> s) & (~opponent >> s * 2) & (~opponent >> s * 3));
            //  (_X_X)
            count += countBits(~opponent & (player >> s) & (~opponent >> s * 2) & (player >> s * 3));
            //  (X_X_)
            count += countBits(player & (~opponent >> s) & (player >> s * 2) & (~opponent >> s * 3));

            // Vertical `|`
            s = 1;
            //  (XX__)
            count += countBits(player & (player >> s) & (~opponent >> s * 2) & (~opponent >> s * 3));

            return count;
        }

        /* Counts the number of bits in ulong x using the
         * Brian Kernighan algorithm */
        static int countBits(ulong x)
        {
            int count = 0;
            while (x > 0)
            {
                x &= x - 1;
                count++;
            }
            return count;
        }
    }
}
