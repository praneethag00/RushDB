# RushDB
 Multiplayer 3rd person Game created using Unity and Phonton lib

RUSH DB
- Multiplayer dodge ball game 
- created using Unity

Story 
 Meet Ash Elane, Vaiser Leon, Fleur Zeus, and Calvin Conner the all-star dodgeball players at Rush Sports Academy. 
 RushDB is a very famous sports team in their region. The team went to internationals last year only to lose in the last round.
  Ash, Vasier, Fleur, and Calvin only want one thing this year and that is the first-place trophy in internationals. 

Your Role
  You play as one of these characters to help the team win in the tournament. Have fun with your teammates, battle, and most importantly win.

Music/Art

	Music
	   opengameart.org
	   https://assetstore.unity.com/packages/audio/music/electronic/trip-hop-music-154796
		Trip Hop Music  by Cathran
	   https://assetstore.unity.com/packages/audio/sound-fx/free-crowd-cheering-sounds-225494
		Free crowd Cheering Sounds Gregor Quendel

	Characters/Animation:
	   Miximo

Network/Multiplayer
	Architecture: Server/host architecture or Listen server
	   A player’s device acts as the server and the client. 
	   The host owns the game.
	   Host migration is needed
	   Lagging/Connection issues

Used Photon Plugin which uses Remote Procedure Calls (RPC) - send messages across the network to all clients. The client devices then run this same function.
Photon Plugin - RPC - called from any device within game and specify to where you would like to send it.

Game features 
	- Synchronized movement and gameplay.
	- Menu settings
	- God mode
	- Health
	- Game over Screens(Win/Lose), Menu Screen
	- Rooms and Lobby
	- Health system
	- Target system

How to play
	Movement Controls: 
	  ‘W’ – move forward
	  ‘Cursor X’ – Turn 
	  ‘Right Shift Key’ - Run
	   ‘Space’ – Jump

	Interaction Controls:
 	  Hold ‘E’ – pickup ball
 	  Press ‘Q’ – drop ball
 	  Hold ‘Left Mouse Click’ – Throw BALL

	Setting:
	  Press ‘M’ – Menu Screen
	  Press ‘G’ – God Mode
