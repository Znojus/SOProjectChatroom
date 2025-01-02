#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <fcntl.h>
#include <netinet/in.h>
#include <string.h>
#include <mysql.h>
#include <pthread.h>

	

typedef struct{
	char name[20];
	int socket;
}Connected;

typedef struct{
	Connected conectados[100];
	int num;
}ConnectedList;

typedef struct{
	char player1[20];
	int socket1;
	char player2[20];
	int socket2;
	int gameID;
	int activa;
}Partida;

typedef struct{
	Partida partidas[100];
	int n;
}ListaPartidas;

typedef struct {
	int roomID;                  // Unique ID for the chatroom
	char participants[6][20];    // Participant names (max 6)
	int numParticipants;         // Number of participants
	int status[6];               // 0 = Pending, 1 = Accepted, -1 = Declined
} ChatRoom;

typedef struct{
	ChatRoom rooms[100];
	int n;
}ListaSalas;

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER; //Variable for mutex lock and unlock
ConnectedList Connlist;
ListaPartidas misPartidas;
ListaSalas chatRooms = {.n = 0};

int sockets[2000];
int n = 0;

// CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM 
// CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM CHAT ROOM 
int CreateChatRoom(char host[20], char participants[6][20], int numParticipants) {
	pthread_mutex_lock(&mutex);
	if (chatRooms.n >= 100) {
		pthread_mutex_unlock(&mutex);
		return -1;
	}
	
	ChatRoom *newRoom = &chatRooms.rooms[chatRooms.n];
	newRoom->roomID = rand() % 100000 + 1; // Ensure roomID > 0
	
	strcpy(newRoom->participants[0], host);
	newRoom->status[0] = 1; // Host automatically accepted
	newRoom->numParticipants = 1;
	
	for (int i = 0; i < numParticipants; i++) {
		if (strcmp(participants[i], host) == 0) continue;
		strcpy(newRoom->participants[newRoom->numParticipants], participants[i]);
		newRoom->status[newRoom->numParticipants] = 0; // Pending
		newRoom->numParticipants++;
	}
	
	chatRooms.n++;
	pthread_mutex_unlock(&mutex);
	return newRoom->roomID;
}
	
void HandleInviteResponse(int roomID, char invitee[20], int response) {
	for (int i = 0; i < chatRooms.n; i++) {
		if (chatRooms.rooms[i].roomID == roomID) {
			ChatRoom *room = &chatRooms.rooms[i];
			for (int j = 0; j < room->numParticipants; j++) {
				if (strcmp(room->participants[j], invitee) == 0) {
					room->status[j] = response;
					char updateMessage[128];
					sprintf(updateMessage, "%s has %s the invite.",
							invitee, response == 1 ? "accepted" : "declined");
					BroadcastChatMessage(roomID, "System", updateMessage);
					return;
				}
			}
		}
	}
}

void BroadcastChatMessage(int roomID, char sender[20], char message[512]) {
	for (int i = 0; i < chatRooms.n; i++) {
		if (chatRooms.rooms[i].roomID == roomID) {
			for (int j = 0; j < chatRooms.rooms[i].numParticipants; j++) {
				char recipient[20];
				strcpy(recipient, chatRooms.rooms[i].participants[j]);
				int socket = GiveMeSocket(&Connlist, recipient);
				if (socket != -1) {
					char formattedMessage[1024];
					sprintf(formattedMessage, "10/%d/%s/%s", roomID, sender, message);
					write(socket, formattedMessage, strlen(formattedMessage));
				}
			}
			break;
		}
	}
}

int Add (ConnectedList *lista, char name[20], int socket){
	//Add new connected. Return 0 if ok and -1 if list is full
	if (lista -> num == 100)
		return -1;
	else{
		strcpy(lista->conectados[lista->num].name,name);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		n++;
		return 0;
	}
}

int GiveMePosition(ConnectedList *lista, char name[20]){
	//Gives back socket or -1 if it is not in the list
	pthread_mutex_lock(&mutex);
	int i=0;
	int found=0;
	while ((i< lista->num) && !found){
		if (strcmp(lista->conectados[i].name, name)==0)
			found=1;
		if (!found)
			i++;
	}
	pthread_mutex_unlock(&mutex);
	return -1;
	
}
int Delete (ConnectedList *lista, char name [20]){
	
	//Returns 0 if delete and -1 if user does not exist
	//pthread_mutex_lock(&mutex);
	int pos= GiveMePosition (lista, name);
	if (pos==-1){
		//pthread_mutex_unlock(&mutex);
		return -1;
	}
	else {
		int i;
		for (i=pos; i< lista->num-1;i++){
			lista->conectados[i]= lista->conectados[i+1];
			//strcpy(lista->conectados[i].nombre,lista->conectados[i+1].nombre);
			//lista->conectados[i].socket= lista->conectados[i+1].socket
		}
		lista->num--;
		//pthread_mutex_unlock(&mutex);
		return 0;
	}
	//pthread_mutex_unlock(&mutex);
}

int NewGame (ListaPartidas *lista, char name1[20], int socket1,char name2[20], int socket2, MYSQL *conn){
	//Add new connected. Return 0 if ok and -1 if list is full
/*	char str_query[1024];*/
	
/*	MYSQL_RES *result;*/
/*	MYSQL_ROW row;*/
	if (lista -> n == 100)
		return -1;
	
	pthread_mutex_lock(&mutex);
	
	// Insert the game into the Games table
	char str_query[1024];
	sprintf(str_query, "INSERT INTO Games () VALUES ()");
	int err = mysql_query(conn, str_query);
	if (err != 0) {
		printf("Error inserting game into Games table %u %s\n", mysql_errno(conn), mysql_error(conn));
		pthread_mutex_unlock(&mutex);
		return -1;
	}
	
	int gameID = mysql_insert_id(conn); // Retrieve the auto-generated Game ID
	
	// Save game details to in-memory list
	strcpy(lista->partidas[lista->n].player1, name1);
	lista->partidas[lista->n].socket1 = socket1;
	strcpy(lista->partidas[lista->n].player2, name2);
	lista->partidas[lista->n].socket2 = socket2;
	lista->partidas[lista->n].gameID = gameID;
	lista->partidas[lista->n].activa = 1;
	
	// Insert player-game associations
	sprintf(str_query, "INSERT INTO PlayerGame (Games, Player) VALUES (%d, (SELECT Id FROM Player WHERE Name='%s'))",
			gameID, name1);
	err = mysql_query(conn, str_query);
	if (err != 0) {
		printf("Error inserting player 1 into PlayerGame table %u %s\n", mysql_errno(conn), mysql_error(conn));
		pthread_mutex_unlock(&mutex);
		return -1;
	}
	
	sprintf(str_query, "INSERT INTO PlayerGame (Games, Player) VALUES (%d, (SELECT Id FROM Player WHERE Name='%s'))",
			gameID, name2);
	err = mysql_query(conn, str_query);
	if (err != 0) {
		printf("Error inserting player 2 into PlayerGame table %u %s\n", mysql_errno(conn), mysql_error(conn));
		pthread_mutex_unlock(&mutex);
		return -1;
	}
	
	lista->n++;
	pthread_mutex_unlock(&mutex);
	
	return gameID;
}

int username_exists(char username[80], MYSQL *conn) {
	
	MYSQL_RES *result;
	MYSQL_ROW row;
	
	char str_query[1024];
	
	sprintf(str_query, "SELECT Name FROM Player WHERE Name = '%s'", username);
	int err=mysql_query (conn, str_query);
	if (err!=0)
	{
		printf ("Error while quering data from database %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	
	result = mysql_store_result(conn);
	row = mysql_fetch_row(result);
	
	if (row == NULL)
		return 0; // Username is not in database
	else
		return 1; // Username exists
	
}

int email_exists(char email[80], MYSQL *conn) {
	
	MYSQL_RES *result;
	MYSQL_ROW row;
	
	char str_query[1024];
	
	sprintf(str_query, "SELECT Email FROM Player WHERE Email = '%s'", email);
	int err=mysql_query (conn, str_query);
	if (err!=0)
	{
		printf ("Error while quering data from database %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	
	result = mysql_store_result(conn);
	row = mysql_fetch_row(result);
	
	if (row == NULL)
		return 0; // Email is not in database
	else
		return 1; // Email exists
	
}

void PlayerGames(char output[512], MYSQL *conn)
{
	int err;
	MYSQL_RES *result;
	MYSQL_ROW row;
	char query[300];
	strcpy(output,"1/");
	strcpy(query, "SELECT Games.Id AS GameID, Player.Name AS PlayerName FROM Games JOIN PlayerGame ON Games.Id = PlayerGame.Games JOIN Player ON Player.Id = PlayerGame.Player ORDER BY Games.Id;");
	
	err = mysql_query(conn, query);
	if (err != 0)
	{
		sprintf(output, "Error querying database %u %s\n", mysql_errno(conn), mysql_error(conn));
		return;
	}
	
	result = mysql_store_result(conn);
	if (result == NULL) {
		printf("No results found for query.\n");
		sprintf(output, "No data was obtained in the query\n");
		return;
	}
	row = mysql_fetch_row(result);
	
	//player_list is not strictly necessary, but it makes the code clearer 
	int last_game_id = -1; // Track last game processed
	char player_list[256] = ""; // Buffer to hold player names
	
	if (row == NULL)
	{
		sprintf(output, "No data was obtained in the query\n");
		return;
	}
	else
	{
		while (row != NULL)
		{
			int game_id = atoi(row[0]); // Convert GameID to an integer
			
			// Check if we've switched to a new game
			if (game_id != last_game_id && last_game_id != -1)
			{
				// Print the results for the previous game to output buffer
				sprintf(output + strlen(output), "Game ID: %d Players' names: %s,", last_game_id, player_list);
				strcpy(player_list, "");  // Reset player list for the new game
			}
			// Append player names for the current game
			if (strlen(player_list) == 0)
			{
				strcpy(player_list, row[1]); // First player in list
			}		
			else
			{
				strcat(player_list, " & ");
				strcat(player_list, row[1]); // Append subsequent players
			}
			
			last_game_id = game_id; // Update to the current game ID
			row = mysql_fetch_row(result); // Move to the next row!!
		}
		// After the loop ends, print the last game's players
		if (last_game_id != -1)
		{
			sprintf(output + strlen(output), "Game ID: %d\nPlayers' names: %s", last_game_id, player_list);
		}
		
		mysql_free_result(result); 
	}
}
	
char winner(MYSQL *conn, int gameID, char output[1024])
{
	char query[300];
	
	sprintf(query, "SELECT Player.Name, PlayerGame.PointsGame "
			"FROM PlayerGame JOIN Player ON PlayerGame.Player = Player.Id "
			"WHERE PlayerGame.Games = %d "
			"ORDER BY PlayerGame.PointsGame DESC LIMIT 1", gameID);
	
	int err=mysql_query (conn, query);
	if (err!=0)
	{
		sprintf ("Error while quering data from database %u %s\n",
				 mysql_errno(conn), mysql_error(conn));
		return;
	}
	
	MYSQL_RES *result = mysql_store_result(conn);
	if (result == NULL) {
		fprintf(stderr, "Error storing result: %s\n", mysql_error(conn));
		return;
	}
	
	MYSQL_ROW row = mysql_fetch_row(result);
	if (row == NULL) {
		sprintf(output,"2/No winner found for game ID %d", gameID);
	} else {
		sprintf(output,"2/Winner: %s, Points: %s",row[0], row[1]);
	}
	
	mysql_free_result(result);
}
void GamesPlayedByPlayer(char playerName[80], char output[1024], MYSQL *conn) {
	char query[300];
	sprintf(query, "SELECT COUNT(PlayerGame.Games) AS GameCount "
			"FROM Player JOIN PlayerGame ON Player.Id = PlayerGame.Player "
			"WHERE Player.Name = '%s'", playerName);
	
	int err = mysql_query(conn, query);
	if (err != 0) {
		sprintf(output, "Error querying database %u %s\n", mysql_errno(conn), mysql_error(conn));
		return;
	}
	MYSQL_RES *result = mysql_store_result(conn);
	MYSQL_ROW row = mysql_fetch_row(result);
	
	if (row == NULL) {
		sprintf(output, "3/Player not found or no games played.");
	} else {
		sprintf(output, "3/Player: %s, Games Played: %s", playerName, row[0]);
	}
	
	mysql_free_result(result);
}

char MyGames(MYSQL *conn,char nombre[20],char output[100]){
	int err;
	MYSQL_RES *result;
	MYSQL_ROW row;
	char query[300];
	strcpy(output,"11");
	sprintf(query,"SELECT DISTINCT Player.Name FROM Player, PlayerGame "
			"WHERE PlayerGame.Games IN (SELECT PlayerGame.Games FROM PlayerGame JOIN Player ON Player.Id = PlayerGame.Player WHERE Player.Name = '%s') "
			"AND Player.Id = PlayerGame.Player "
			"AND Player.Name != '%s';", nombre);
	err = mysql_query(conn, query);
	if (err != 0)
	{
		sprintf(output, "Error querying database %u %s\n", mysql_errno(conn), mysql_error(conn));
		return;
	}
	
	result = mysql_store_result(conn);
	if (result == NULL) {
		printf("No results found for query.\n");
		sprintf(output, "No data was obtained in the query\n");
		return;
	}
	row = mysql_fetch_row(result);
	while (row!=NULL){
		sprintf(output, "%s/%s", output, row[0]);
		row=mysql_fetch_row(result);
	}
	mysql_free_result(result);
}
void GameResultsWithPlayer(char myname[80], char playerName[80], char output[1024], MYSQL *conn) {
	char query[512];
	strcpy(output,"12/");
	sprintf(query, 
			"SELECT Games.Id AS GameID, GROUP_CONCAT(Player.Name SEPARATOR ', ') AS Players "
			"FROM PlayerGame "
			"JOIN Player ON PlayerGame.Player = Player.Id "
			"JOIN Games ON PlayerGame.Games = Games.Id "
			"WHERE Player.Name IN ('%s', '%s') "
			"GROUP BY Games.Id "
			"HAVING COUNT(DISTINCT Player.Name) = 2;",  
			myname, playerName);
	

	int err = mysql_query(conn, query);
	if (err != 0) {
		sprintf(output, "Error querying database %u %s\n", mysql_errno(conn), mysql_error(conn));
		return;
	}
	MYSQL_RES *result = mysql_store_result(conn);
	if (result == NULL) {
		sprintf(output, "No results found for the query.\n");
		return;
	}
	
	MYSQL_ROW row;
	
	while ((row = mysql_fetch_row(result))) {
		int gameID = atoi(row[0]);  // Game ID
		char *players = row[1];     // List of players in the game
		sprintf(output + strlen(output), "Game ID: %s, Player: %s \n", row[0], row[1]);
	}
	mysql_free_result(result);
}
	

void NotifyAllConnected(ConnectedList *list) {
	char connected[2048];
	GiveMeConnected(list, connected);
	printf("Notifying all connected clients: %s\n", connected); // Debug message
	pthread_mutex_lock(&mutex);
	for (int i = 0; i < n; i++) {
		if (write(sockets[i], connected, strlen(connected)) < 0) {
			printf("Failed to notify client %d. Removing from list.\n", i); // Debug error
			close(sockets[i]);
			sockets[i] = sockets[--n];
		}
	}
	pthread_mutex_unlock(&mutex);
}


void SignIn( char username[80], char password[80],  char email[80],char output[80], MYSQL *conn){
	char str_query[1024];
		
	if (email_exists(email, conn)) {
		
		sprintf(output,"5/%d",0);
		return;
		
	}
	
	if (username_exists(username, conn)) {
		
		sprintf(output,"5/%d",1);
		return;
		
	}

	
	if ((strlen(username) < 3) || (strlen(username) > 80)) //The name must have a minimum and maximum length, check if it's ok.
	{
		
		sprintf(output, "5/%d",3);
		return;
	}
	
	if ((strlen(password) < 5) || (strlen(password) > 20)) {
		
		sprintf(output,"5/%d",4);
		return;
		
	}
	sprintf(str_query, "INSERT INTO Player(Email, Name, Password) VALUES ('%s','%s','%s')",email, username, password);
	
	int err=mysql_query (conn, str_query);
	if (err!=0)
	{
		printf ("Error while quering data from database %u %s\n",mysql_errno(conn), mysql_error(conn));
		sprintf(output,"5/%d",5);
		 return;
	}
	
	sprintf(output, "5/%d",6);
}

void LogIn( Connected *list, char username[80],char password[80], char output[80], MYSQL *conn, int sock_conn)
{
	char str_query[1024];
	char password_database[80];

	MYSQL_RES *result;
	MYSQL_ROW row;
	
	if (!username_exists(username, conn)){
		sprintf(output, "6/%d", 0);
		return;
	}
	
	else{
		
		int i=0;
		int encontrado=0;
		while (i<Connlist.num && !encontrado)
		{
			if (strcmp(username,Connlist.conectados[i].name)==0)
			{
				encontrado=1;
			}
			
			else
				i=i+1;
		}
		
		if (encontrado){
			sprintf(output, "6/%d", 5);
			return;
		}
		
		else{
			sprintf(str_query, "SELECT Password FROM Player WHERE Name='%s'", username);
			
			int err=mysql_query (conn, str_query);
			if (err!=0)
			{
				sprintf ("Error while quering data from database %u %s\n",
						 mysql_errno(conn), mysql_error(conn));
				
				sprintf(output,"6/%d",1);
				return;
			}
			
			result = mysql_store_result(conn);
			row = mysql_fetch_row(result);
			
			if (row == NULL)
				sprintf(output,"%6/d",2); //password not found
			else
			{	
				strcpy(password_database, row[0]);		
				if (strcmp(password, password_database) == 0) {
					//a adir lista de conectados detr s del 6/3
					sprintf(output, "6/%d/%s", 3, username);  //Successful login
					pthread_mutex_lock(&mutex);  
					Add(&Connlist, username, sock_conn);
					pthread_mutex_unlock(&mutex);
				}
				else {sprintf(output, "6/%d",4); //Wrong password
				}
			}
		}
	}
	mysql_free_result(result);
}

void GiveMeConnected (ConnectedList *lista, char conectados [512]) {
	//Pone en conectados los nombres de todos los conectados separados por /. 
	//Primero pone el nº de conectados.
	
	sprintf(conectados, "4/%d", lista->num);
	pthread_mutex_lock(&mutex); 
	int i;
	for(i=0; i< lista->num;i++){
		strcat(conectados, ",");
		strcat(conectados, lista->conectados[i].name); 
	}
	pthread_mutex_unlock(&mutex); 

}

int GiveMeSocket(ConnectedList *lista, char name[20]){
	//Gives back socket or -1 if it is not in the list
	int i=0;
	int found=0;
	while ((i< lista->num) && !found){
		if (strcmp(lista->conectados[i].name, name)==0)
			found=1;
		if (!found)
			i++;
	}
	
	if (found)
		return lista->conectados[i].socket;
	else 
		return -1;
}

void *Client(int *socket){
	int sock_conn;
	int *s;
	s=(int *) socket;
	sock_conn= *s;
	
	MYSQL *conn;
	char input[512];
	char output[512];
	int ret;	
	int terminar=0;
	
	conn = mysql_init(NULL);
	if (conn == NULL) {
		printf("Error creating MySQL connection: %u %s\n", mysql_errno(conn), mysql_error(conn));
		close(sock_conn);
		return NULL;
	}
	if (mysql_real_connect(conn, "localhost", "root", "mysql", "Game", 0, NULL, 0) == NULL) {
		printf("Error initializing MySQL connection: %u %s\n", mysql_errno(conn), mysql_error(conn));
		mysql_close(conn);
		close(sock_conn);
		return NULL;
	}
	
	while (terminar==0){
		printf("Waiting\n");
		// Ahora recibimos su nombre, que dejamos en buff
		ret=read(sock_conn,input, sizeof(input));
		if (ret <= 0) {
			printf("Error reading from client or client disconnected\n");
			break;
		}
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		input[ret]='\0';
		
		//Escribimos el nombre en la consola
		
		printf("Received: %s\n", input);
		strcpy(output," ");
		
		//Inizialize connection
		char *p = strtok(input, "/");
		int codigo = atoi(p);
		char username[80], password[80], email[80];
		char notificacion[512];
		char invites[80];
		char invited[80];
		char myname[20];
		
			
			switch (codigo) {
			case 0 :
				Delete(&Connlist, myname);
				GiveMeConnected(&Connlist, notificacion);
				for (int j = 0; j < n; j++) { // Recuerda que 'n' debe ser el nÃºmero correcto de conexiones activas
					//if(sock_conn!=sockets[j]){
					write(sockets[j], notificacion, strlen(notificacion));
					printf("n:%s\n",notificacion);}
				break;
			case 1:
				p=strtok(NULL,"/");
				strcpy(email,p);
				p=strtok(NULL,"/");
				strcpy(username,p);
				p=strtok(NULL,"/");
				strcpy(password,p);
				SignIn(username	, password, email, output, conn);
				break;
			case 2:
				p=strtok(NULL,"/");
				strcpy(myname,p);
				p=strtok(NULL,"/");
				strcpy(password,p);
				LogIn(&Connlist,myname, password, output, conn, sock_conn);
				GiveMeConnected(&Connlist, notificacion);
				char output2[512];
				sprintf(output2,"6/3/%s",myname);
				printf(output2);
				if (strcmp(output,output2)==0){
					for (int j = 0; j < n; j++) { // Recuerda que 'n' debe ser el nÃºmero correcto de conexiones activas
						if(sock_conn!=sockets[j]){
							write(sockets[j], notificacion, strlen(notificacion));
							printf("n:%s\n",notificacion);
						}
					}		
				}
				sprintf(output,"%s/%s",output,notificacion);
				printf("I have finished the request\n");
				break;
			case 3:
				PlayerGames(output, conn);
				
				break;
			case 4:
				p=strtok(NULL,"/");
				int game_id=atoi(p);
				winner(conn,game_id, output);  // gameID input_user
				
				break;
			case 5:
				GiveMeConnected(&Connlist, output);  // Agrega los jugadores conectados a la notificaci 
				break;
				
			case 6:
				p = strtok(NULL, "/");
				if (p != NULL) {
					char playerName[80];
					strcpy(playerName, p);
					GamesPlayedByPlayer(playerName, output, conn); // Make sure the server-side function works as expected
					
				} else {
					sprintf(output, "Invalid player name.");
				}
				break;
			case 7:{ //Invitation
				char participants[6][20];
				int numParticipants = 0;
				
				// Extract host
				p = strtok(NULL, "/");
				char host[20];
				strcpy(host, p);
				
				// Extract participants
				while ((p = strtok(NULL, "/")) != NULL && numParticipants < 5) {
					if (strcmp(p, host) != 0) { // Prevent self-invite
						strcpy(participants[numParticipants], p);
						numParticipants++;
					}
				}
				
				int roomID = CreateChatRoom(host, participants, numParticipants);
				if (roomID != -1) {
					char response[128];
					sprintf(response, "10/%d/Chatroom created successfully", roomID);
					write(sock_conn, response, strlen(response));
					
					for (int i = 0; i < numParticipants; i++) {
						int socket = GiveMeSocket(&Connlist, participants[i]);
						if (socket != -1) {
							char inviteMessage[128];
							sprintf(inviteMessage, "8/%d/%s", roomID, host);
							write(socket, inviteMessage, strlen(inviteMessage));
						}
					}
				
				} else {
					write(sock_conn, "10/Failed to create chatroom.", 29);
				}
				break;
			}
				
			case 8: //Invitation accepted or not
				p = strtok(NULL, "/");
				int roomID = atoi(p);
				
				p = strtok(NULL, "/");
				char invitee[20];
				strcpy(invitee, p);
				
				p = strtok(NULL, "/");
				int response = atoi(p); // 1 = Accepted, -1 = Declined
				
				HandleInviteResponse(roomID, invitee, response);
				
				break;
			case 9: // Game start
				// Game start logic here
				printf("Game is starting between %s and %s\n", invites, invited);
				break;
			case 10: {
				p = strtok(NULL, "/");
				int roomID = atoi(p); // Room ID
				
				p = strtok(NULL, "/");
				char sender[20];
				strcpy(sender, p); // Sender's name
				
				p = strtok(NULL, "/");
				char chatMessage[512];
				strcpy(chatMessage, p); // Chat message
				
				printf("Broadcasting message to roomID: %d, message: %s\n", roomID, sender, chatMessage);
				BroadcastChatMessage(roomID, sender, chatMessage);
				break;
			}
				
				break;
			case 11:
				p=strtok(NULL, "/");
				char minombre[20];
				strcpy(minombre,p);
				MyGames(conn,minombre,output);
				break;
				
			case 12: // Results with specified player
				p = strtok(NULL, "/");
				strcpy(myname, p);  // logged-in player
				p = strtok(NULL, "/");
				char playerName[80];
				strcpy(playerName, p);  // specified player
				GameResultsWithPlayer(myname, playerName, output, conn);
				break;
			case 13: //Games during the selected date
				{
				p = strtok(NULL, "/");
				char startDate[20];
				strcpy(startDate, p);
				
				p = strtok(NULL, "/");
				char endDate[20];
				strcpy(endDate, p);
				
				// adjusting the end date to include the entire day
				char adjustedEndDate[25];
				sprintf(adjustedEndDate, "%s 23:59:59", endDate);
				
				// Query the database for games in the selected date range
				char str_query[512];
				sprintf(str_query, "SELECT Id, Date FROM Games WHERE Date BETWEEN '%s' AND '%s'", startDate, adjustedEndDate);
				
				int err = mysql_query(conn, str_query);
				if (err != 0) {
					printf("Error querying games by date range: %s\n", mysql_error(conn));
					sprintf(output, "Error querying games.");
				} else {
					MYSQL_RES *result = mysql_store_result(conn);
					MYSQL_ROW row;
					strcpy(output, "13/");
					while ((row = mysql_fetch_row(result))) {
						sprintf(output + strlen(output), "Game ID: %s, Date: %s/", row[0], row[1]);
					}
					mysql_free_result(result);
				}
				break;
			}
			default:
					printf("Invalid option received\n");
					terminar = 1;
					break;
				}
			
			printf("I will send the answer\n");
			if (codigo !=0 && codigo != 7 && codigo != 8 && codigo != 10){
				printf("->%s\n", output);			
				write(sock_conn, output, strlen(output));  // Enviar respuesta al cliente			}
			}				
		
		}
	mysql_close(conn);
	close(sock_conn);			
	return NULL;
}

int main(int argc, char *argv[]) 
{
	
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;

	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket\n");
	// Fem el bind al port
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(50215);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	int i=0;
	pthread_t thread[100];
	// Atenderemos solo 5 peticiones
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		if (sock_conn < 0)
		{printf ("He recibido conexion\n");
		continue; }
		
		printf ("Recibido\n");
		//sock_conn es el socket que usaremos para este cliente
		
		pthread_mutex_lock(&mutex);
		sockets[i]= sock_conn;
		
		if(pthread_create(&thread[i], NULL, Client, &sockets[i]) != 0) {
			printf("Error creating thread\n");
		}
		i=i+1;
		pthread_mutex_unlock(&mutex);
		}
	close(sock_listen);
	
	return 0;
}
