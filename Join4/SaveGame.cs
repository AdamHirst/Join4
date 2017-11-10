using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Join4
{
    static class SaveGame
    {
        public static bool save(GameInstance game)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Connect 4 Savegame|*.c4";
            sfd.Title = "Save the game";
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                try
                {
                    System.IO.File.WriteAllText(sfd.FileName, 
                        (int)game.type + ";" + game.players[0] + ";" + game.players[1]);
                    return true;
                } catch
                {
                    MessageBox.Show("Could not save the file.");
                    return false;
                }
            } else
            {
                // No file was selected
                return false;
            }
        }

        public static GameInstance load()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Connect 4 Savegame|*.c4";
            ofd.Title = "Open a game";
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                try
                {
                    string[] v = System.IO.File.ReadAllText(ofd.FileName).Split(';');
                    GameInstance game = new GameInstance();
                    game.type = (GameInstance.GameType)Int32.Parse(v[0]);
                    game.players[0] = (ulong)UInt64.Parse(v[1]);
                    game.players[1] = (ulong)UInt64.Parse(v[2]);
                    return game;
                } catch
                {
                    return null;
                }
            } else
            {
                // No file was selected
                return null;
            }
        }
    }
}
