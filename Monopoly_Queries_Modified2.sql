-- Lists the game records in chronological order.
SELECT * 
FROM Game
ORDER BY time;

-- The WHERE clause is optional.
SELECT * 
FROM Player;

-- Using name=NULL doesn't work like you think it does.
SELECT *
FROM Player
WHERE name IS NULL;

SELECT *
FROM Player
WHERE name = NULL;

-- Get all the users with Calvin email addresses.
SELECT *
FROM Player
WHERE emailAddress LIKE '%calvin%';

-- Get the highest score ever recorded.
SELECT score
FROM PlayerGame
ORDER BY score DESC
LIMIT 1;

-- Get the cross-product of all the tables.
SELECT *
FROM Player, PlayerGame, Game;

-- Get the name of the player with the highest recorded score.
SELECT Player.name, score
FROM Player, PlayerGame
WHERE Player.ID = PlayerGame.playerID
ORDER BY score DESC
LIMIT 1;

-- Get the names of all the players that played in game #2.
SELECT Player.name, score
FROM Player, PlayerGame, Game
WHERE Player.ID = PlayerGame.playerID
  AND PlayerGame.gameID = Game.ID
  AND Game.ID = 2;

-- Get the names of the players who share the same name.
SELECT P1.name
FROM Player AS P1, Player AS P2
WHERE P1.name = P2.name
  AND P1.ID < P2.ID;
  
-- Added GET commands for custom tables
SELECT * 
FROM PlayerAttributes;

SELECT * 
FROM Estates;

SELECT * 
FROM Residences;

SELECT * 
FROM Businesses;

SELECT * 
FROM PieceLocation;

SELECT * 
FROM Cash;

-- Show the location of the database --
show data_directory;

--Retrieve a list of all the games, ordered by date with the most recent game coming first.
SELECT *
FROM Game
ORDER BY time DESC;

--Retrieve all the games that occurred in the past week.
-- https://stackoverflow.com/questions/8732517/how-do-you-find-results-that-occurred-in-the-past-week

SELECT *
FROM Game
WHERE time > current_date - interval '7 days';

--Retrieve a list of players who have (non-NULL) names.
SELECT *
FROM Player
WHERE name IS NOT NULL;

--Do not write expression = NULL because NULL is not "equal to" NULL. 
--(The null value represents an unknown value, and it is not known whether two unknown values are equal.)

--Retrieve a list of IDs for players who have some game score larger than 2000.
SELECT gameID
FROM PlayerGame
WHERE score > 2000;

--Retrieve a list of players who have GMail accounts.
SELECT *
FROM Player
WHERE emailAddress LIKE '%gmail%';

--Retrieve all “The King”’s game scores in decreasing order.
SELECT score
FROM Player, PlayerGame
WHERE Player.ID = PlayerGame.playerID
	AND Player.name = 'The King';
	
--Retrieve the names of the players of the game played on 2006-06-28 13:20:00.
SELECT Player.name
FROM Player, Game, PlayerGame
WHERE Player.ID = PlayerGame.playerID
	AND Game.ID = PlayerGame.gameID
	AND Game.time = '2006-06-28 13:20:00';
	
--Retrieve the highest score of the game played on 2006-06-28 13:20:00.
-- http://www.postgresqltutorial.com/postgresql-max-function/
SELECT MAX (score)
FROM PlayerGame, Game
WHERE Game.time = '2006-06-28 13:20:00'
	AND Game.ID = PlayerGame.gameID;

--Retrieve the name of the winner of the game played on 2006-06-28 13:20:00. (not working as intended)
SELECT Player.name
FROM Player, Game, PlayerGame
WHERE Game.time = '2006-06-28 13:20:00'
	AND Game.ID = PlayerGame.gameID
	AND Player.ID = PlayerGame.playerID
	--Sub-query to get highest score of the game played on 2006-06-28 13:20:00.
	AND PlayerGame.score = (
		SELECT MAX (score)
		FROM PlayerGame, Player, Game
		WHERE Game.time = '2006-06-28 13:20:00'
			AND Game.ID = PlayerGame.gameID);

--The following query retrieves the names of the players who share the same name.
SELECT P1.name
FROM Player AS P1, Player AS P2
WHERE P1.name = P2.name
  AND P1.ID < P2.ID;
  
--So what does that P1.ID < P2.ID clause do in the last example query?

--My best guess....I have no idea.

--The query that joined the Player table to itself seems rather contrived. 
--Can you think of a realistic situation in which you’d want to join a table to itself?

--Resource Used: 
--https://lornajane.net/posts/2012/sql-joining-a-table-to-itself
--https://www.w3resource.com/sql/joins/perform-a-self-join.php

--Can be useful in getting two sets of information that exists in the same table without
--resorting to sub-select nested queries.