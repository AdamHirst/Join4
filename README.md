<div align="center"><h1>Join4</h1></div>
<div align="center">An alpha-beta pruning minimaxing AI for Four in a Row</div>
<br><br>
<div align="center"><img src="https://raw.githubusercontent.com/AdamHirst/Join4/master/Screenshots/Display.png" /></div>
<br><br>
This project was created as part of a second year university assignment. This implementation utilises a heuristic minimax algorithm to generate the AI's move.
<br><br>
To achieve this, the game board is stored as two bit array; one for each player. Storing the board as a bit array allows faster computation of static evaluations allowing for faster generation of AI moves using bit manipulations. The bit array is mapped to the game board as shown below:
<br><br>
<div align="center"><img src="https://raw.githubusercontent.com/AdamHirst/Join4/master/Screenshots/DisplayGrid.png" width="300" height="300" /></div>
<div align="center">where the number represents the index of the tile within the bit array</div>
<br>
Once a player makes a move, the AI generates a tree of all possible moves from the current state. An static evaluation function is applied to generate a score for any given state and is used as the value of each node in the tree. The depth of this tree is set to 2 (meaning the first layer represents a maximum played by the AI and the second is a minimum played by the player).
<br><br>
By performing a minmax on the tree (with AB pruning to speed up computation), a move for the AI can be generated. 
