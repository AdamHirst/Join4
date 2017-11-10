using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Join4
{
    public partial class MainForm : Form
    {
        GameInstance game = new GameInstance();
        // Array to hold buttons within the game
        Button[] buttons = new Button[42];

        // The X and Y offset of the entire board
        int boardOffsetX = 80, 
            boardOffsetY = 80;
        // The size of the buttons
        int buttonSize = 200;
        
        public MainForm()
        {
            Console.WriteLine(JoinFour.applyMove(1UL << 63, 0, 0));

            // Initialise button properties
            for (int i = 0; i < 42; i++)
            {
                // Initiate the button
                buttons[i] = new Button();
                // Set the bounds of the button in the order shown in the bitboard layout
                buttons[i].SetBounds(boardOffsetX + (buttonSize * (int) Math.Floor((double) i/6)), 
                                     boardOffsetY + (buttonSize * (5 - (i%6))), 
                                     buttonSize, buttonSize);
                // Set colour
                buttons[i].BackColor = Control.DefaultBackColor;
                // Add the button listener
                buttons[i].Tag = i;
                buttons[i].Click += new EventHandler(MoveSubmitted);

                Controls.Add(buttons[i]);
            }

            InitializeComponent();
            Size = new Size(600, 600);
            Console.WriteLine(Size);
        }
        
        void MoveSubmitted(object sender, EventArgs args)
        {
            if (game.gameFinished) return;

            int tag = (int)((Button)sender).Tag;
            // Check if the move is valid
            if (!game.submitMove((int) Math.Floor((double) tag / 6)))
            {
                MessageBox.Show("That move is not valid. Please enter a valid move.");
                return;
            }
            else
            {
                redrawBoard();
                if (game.gameFinished)
                {
                    if (JoinFour.hasPlayerWon(game.players[0]))
                        MessageBox.Show("Player 1 has won!");
                    else
                        MessageBox.Show("Player 2 has won!");
                }
            }
        }

        void redrawBoard()
        {
            // Clear existing board
            for (int i = 0; i < 42; i++)
            {
                buttons[i].BackColor = Control.DefaultBackColor;
            }
            for (int i = 0; i < 48; i++)
            {
                // Ignore the bits above the board
                if ((i - 6) % 7 == 0) continue;

                int button = i - (int) Math.Floor((double)i / 7);
                if (((1UL << 63 - i) & game.players[0]) != 0)
                {
                    buttons[button].BackColor = Color.Yellow;
                }
                if (((1UL << 63 - i) & game.players[1]) != 0)
                {
                    buttons[button].BackColor = Color.Red;
                }
            }
        }

        bool isValidMove(ulong state, int x)
        {
            // If x is bigger than the total size of the game board, 
            // it is not a valid move
            if (x < 0 || x >= 42) return false;
            // If the selected move is on the bottom row, check if the
            // return if the space is empty
            if (x % 6 == 0)
                return (state & (1UL << (63 - x))) >> (63 - x) == 0;
            // If the selected move is not on the bottom row, return
            // true if the space under it is filled
            else
                return (state & (3UL << (63 - x))) >> (63 - x) == 2;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGame.save(game);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameInstance loaded = SaveGame.load();
            if (loaded != null)
            {
                game = loaded;
                redrawBoard();
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("During a game on your turn, click on a position on the game board to place a tile.\nTo restart the game, use the 'Reset' menu item under 'Game'.\nYou may save and load the game using the 'Save' and 'Load' options available under the 'Game' menu.");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.restart();
            redrawBoard();
        }
    }
}
