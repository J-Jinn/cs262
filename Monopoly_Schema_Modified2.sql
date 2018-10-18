--
-- This SQL script builds a monopoly database, deleting any pre-existing version.
--
-- @author kvlinden
-- @version Summer, 2015
--
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-- Drop previous versions of the tables if they they exist, in reverse order of foreign keys.
-------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS PlayerGame;

DROP TABLE IF EXISTS PlayerAttributes;
DROP TABLE IF EXISTS Estates;
DROP TABLE IF EXISTS Residences;
DROP TABLE IF EXISTS Businesses;
DROP TABLE IF EXISTS PieceLocation;
DROP TABLE IF EXISTS Cash;

DROP TABLE IF EXISTS Game;
DROP TABLE IF EXISTS Player;
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-- Create the schema.
-------------------------------------------------------------------------------------------
CREATE TABLE Residences(
	ID integer PRIMARY KEY,
	urbanResidences TEXT [],
	suburbanResidences TEXT [],
	ruralResidences TEXT []
);
-------------------------------------------------------------------------------------------
CREATE TABLE Businesses(
	ID integer PRIMARY KEY,
	smallBusinesses TEXT [],
	mediumBusinesses TEXT [],
	largeBusinesses TEXT []
);
-------------------------------------------------------------------------------------------
CREATE TABLE PieceLocation(
	ID integer PRIMARY KEY,
	currentLocation varchar(100),
	previousLocation varchar(100)
);
-------------------------------------------------------------------------------------------
CREATE TABLE Cash(
	ID integer PRIMARY KEY,
	currencyType varchar(25),
	currentAmount integer,
	lossGainLastMove boolean --true = gain and false = loss--
);
-------------------------------------------------------------------------------------------
CREATE TABLE Estates(
	ID integer PRIMARY KEY,
	playerResidences integer REFERENCES Residences(ID),
	playerBusinesses integer REFERENCES Businesses(ID)
);
-------------------------------------------------------------------------------------------	
	CREATE TABLE PlayerAttributes(
	ID integer PRIMARY KEY,
	playerCash integer REFERENCES Cash(ID),
	playerEstates integer REFERENCES Estates(ID),
	playerPieceLocation integer REFERENCES PieceLocation(ID)
);
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
CREATE TABLE Game (
	ID integer PRIMARY KEY, 
	time timestamp
	);
-------------------------------------------------------------------------------------------
CREATE TABLE Player (
	ID integer PRIMARY KEY, 
	emailAddress varchar(50) NOT NULL,
	name varchar(50)
	);
-------------------------------------------------------------------------------------------
CREATE TABLE PlayerGame (
	gameID integer REFERENCES Game(ID), 
	playerID integer REFERENCES Player(ID),
	score integer,
	playerAttributes integer REFERENCES PlayerAttributes(ID)
	);
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-- Allow users to select data from the tables.
-------------------------------------------------------------------------------------------
GRANT SELECT ON Game TO PUBLIC;
GRANT SELECT ON Player TO PUBLIC;
GRANT SELECT ON PlayerGame TO PUBLIC;

GRANT SELECT ON PlayerAttributes TO PUBLIC;
GRANT SELECT ON Estates TO PUBLIC;
GRANT SELECT ON Residences TO PUBLIC;
GRANT SELECT ON Businesses TO PUBLIC;
GRANT SELECT ON PieceLocation TO PUBLIC;
GRANT SELECT ON Cash TO PUBLIC;
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-- Add sample records.
-------------------------------------------------------------------------------------------
INSERT INTO Residences VALUES (1, '{"home"}', '{"cottage"}', '{"lakeside"}');
-------------------------------------------------------------------------------------------
INSERT INTO Businesses VALUES (1, '{"road-side stall"}', '{"restaurant"}', '{"corporation"}');
-------------------------------------------------------------------------------------------
INSERT INTO PieceLocation VALUES (1, 'dungeon','grassy plain');
-------------------------------------------------------------------------------------------
INSERT INTO Cash VALUES (1, 'USD', 100000, false);
-------------------------------------------------------------------------------------------
INSERT INTO Estates VALUES (1, 1, 1);
-------------------------------------------------------------------------------------------
INSERT INTO PlayerAttributes VALUES (1, 1, 1, 1);
-------------------------------------------------------------------------------------------
INSERT INTO Game VALUES (1, '2006-06-27 08:00:00');
INSERT INTO Game VALUES (2, '2006-06-28 13:20:00');
INSERT INTO Game VALUES (3, '2006-06-29 18:41:00');
-------------------------------------------------------------------------------------------
INSERT INTO Player(ID, emailAddress) VALUES (1, 'me@calvin.edu');
INSERT INTO Player VALUES (2, 'king@gmail.edu', 'The King');
INSERT INTO Player VALUES (3, 'dog@gmail.edu', 'Dogbreath');
-- Insert duplicate entry for Lab08
INSERT INTO Player VALUES (4, 'dog2@gmail.edu', 'Dogbreath');
-------------------------------------------------------------------------------------------
INSERT INTO PlayerGame VALUES (1, 1, 0.00, 1);
INSERT INTO PlayerGame VALUES (1, 2, 0.00, NULL);
INSERT INTO PlayerGame VALUES (1, 3, 2350.00, NULL);
INSERT INTO PlayerGame VALUES (2, 1, 1000.00, NULL);
INSERT INTO PlayerGame VALUES (2, 2, 0.00, NULL);
INSERT INTO PlayerGame VALUES (2, 3, 500.00, NULL);
INSERT INTO PlayerGame VALUES (3, 2, 0.00, NULL);
INSERT INTO PlayerGame VALUES (3, 3, 5500.00, NULL);
-------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
