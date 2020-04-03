#include "Server.h"

Server::Server()
{
	status = ServerStatus::Lobby;
	ptr = NULL;
	CreateServer();
}

Server::~Server()
{}

//takes a string and splits it using the seperator character(s)
std::vector<std::string> split(std::string str, const char* seperators)
{
	std::vector<std::string> ans;
	char* tmp = strtok_s((char*)str.c_str(), seperators, nullptr);
	ans.push_back(tmp);

	while(tmp)
	{
		tmp = strtok_s(nullptr, seperators, nullptr);
		if(tmp)	ans.push_back(tmp);
	}
	return ans;
}

void Server::CreateServer()
{
	//Initialize winsock
	WSADATA wsa;

	int error;
	error = WSAStartup(MAKEWORD(2, 2), &wsa);

	if(error != 0)
	{
		printf("Failed to initialize %d\n", error);
		return;
	}

	//Create a Server socket
	memset(&hints, 0, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_DGRAM;
	hints.ai_protocol = IPPROTO_UDP;
	hints.ai_flags = AI_PASSIVE;

	if(getaddrinfo(NULL, PORT, &hints, &ptr) != 0)
	{
		printf("Getaddrinfo failed!! %d\n", WSAGetLastError());
		WSACleanup();
		return;
	}

	server_socket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);

	if(server_socket == INVALID_SOCKET)
	{
		printf("Failed creating a socket %d\n", WSAGetLastError());
		WSACleanup();
		return;
	}

	// Bind socket
	if(bind(server_socket, ptr->ai_addr, (int)ptr->ai_addrlen) == SOCKET_ERROR)
	{
		printf("Bind failed: %d\n", WSAGetLastError());
		closesocket(server_socket);
		freeaddrinfo(ptr);
		WSACleanup();
		return;
	}

	printf("Server is now running!\n");
	isServerRunning = true;
}

void Server::UpdateRecv()
{
	while(isServerRunning)
	{
		if(status == ServerStatus::Lobby)
		{
			struct sockaddr_in fromAddr;
			int fromlen;
			fromlen = sizeof(fromAddr);
			memset(recv_buf, 0, BUF_LEN);
			
			if(recvfrom(server_socket, recv_buf, sizeof(recv_buf), 0, (struct sockaddr*) & fromAddr, &fromlen) == SOCKET_ERROR)
			{
				printf("recvfrom() failed...%d\n", WSAGetLastError());
				continue;//goes to the top of the while loop
			}

			//////For Testing
			temp = fromAddr;

			// TODO:
			std::string message = std::string(recv_buf);
			char code = message[0];

			////what you had before//
			//// Join require
			//if(code == '@')
			//{
			//	message.erase(0, 1);
			//	join(message, fromAddr);
			//}
			//// Seat require
			//else if(code == '#')
			//{
			//	message.erase(0, 1);
			//	int seatId = message[0] - '0';
			//	message.erase(0, 1);
			//	int userId = std::stoi(message);
			//	takeSeat(userId, seatId);
			//}
			//// Player leave seat
			//else if(code == '$')
			//{
			//	message.erase(0, 1);
			//	int userId = std::stoi(message);
			//	leftSeat(userId);
			//}
			//// Player pressed start
			//else if(code == '%')
			//{
			//	message.erase(0, 1);
			//	int userId = std::stoi(message);
			//	pressedStart(userId);
			//}
			//// Player leave Lobby
			//else if(code == '&')
			//{
			//	message.erase(0, 1);
			//	int userId = std::stoi(message);
			//	leftGame(userId);
			//} else
			//{
			//	printf("Received: %s\n", recv_buf);
			//}


			//alternitive method (looks a bit cleaner... thats all)//
			int userId, seatId;
			switch(code)
			{
			case '@':// Join require
				message.erase(0, 1);
				join(message, fromAddr);
				break;

			case '#':// Seat require
				message.erase(0, 1);
				seatId = message[0] - '0';
				message.erase(0, 1);
				userId = std::stoi(message);
				takeSeat(userId, seatId);
				break;

			case '$':// Player leave seat
				message.erase(0, 1);
				userId = std::stoi(message);
				leftSeat(userId);
				break;

			case '%':// Player pressed start
				message.erase(0, 1);
				userId = std::stoi(message);
				pressedStart(userId);
				break;

			case '&':// Player leave Lobby
				message.erase(0, 1);
				userId = std::stoi(message);
				leftGame(userId);
				break;

			default:
				printf("Received: %s\n", recv_buf);
			}

		}
		else if(status == ServerStatus::Game)
		{
			struct sockaddr_in fromAddr;
			int fromlen;
			fromlen = sizeof(fromAddr);
			memset(recv_buf, 0, BUF_LEN);

			if(recvfrom(server_socket, recv_buf, sizeof(recv_buf), 0, (struct sockaddr*) & fromAddr, &fromlen) == SOCKET_ERROR)
			{
				printf("recvfrom() failed...%d\n", WSAGetLastError());
				continue;
			}

			//if(fromlen > 8)//double chech that there is other data to read
			{
				MessageType msg =(MessageType)reclass(int, recv_buf);
				switch(msg)
				{
					//TODO: create movement structure here
				case MessageType::Movement:
					//if(fromlen >= reclass(int, recv_buf[4]))//check for the correct data size
					{
						puts("recived Movement message from client");
						//TODO: get player ID,Position,velocity,elapsed time and broadcast
						BroadcastMessageToAll(recv_buf);//send it to everyone
					}

					break;
				default:

					break;
				}
			}

			// TODO:
			// Player leave game
		}
		else
		{
			printf("Player status is incorrect!!!!");
		}
	}
}

void Server::UpdateSend()
{
	while(isServerRunning)
	{
		if(status == ServerStatus::Lobby)
		{
			//std::string line;
			//std::getline(std::cin, line);

			//if (line.size() > 0)
			//	//BroadcastMessageToAll(line);
			//	//////For Testing
			//	HandleSending(line, temp);

			// check for start game
			if(checkAllStartPressed())
			{
				status = ServerStatus::Game;
				//TODO:
				BroadcastMessageToAll((char*)"$");
			}
		}
		else if(status == ServerStatus::Game)
		{

		}
		else
		{
			printf("Player status is incorrect!!!!");
		}
	}
}

void Server::HandleSending(void* _message, sockaddr_in _adress)
{
	int fromlen;
	fromlen = sizeof(_adress);
	
	if(sendto(server_socket,(char*)_message, BUF_LEN, 0, (struct sockaddr*) & _adress, fromlen) == SOCKET_ERROR)
	{
		printf("sendto() failed %d\n", WSAGetLastError());
	}
}

void Server::BroadcastMessageToAll(void* _message)
{
	std::cout << "Send All: "  << std::endl;
	if(users.listUsers.size() != 0)
	{
		for(std::list <User> ::iterator it = users.listUsers.begin(); it != users.listUsers.end(); it++)
		{
			HandleSending(_message, (*it)._adress);
		};
	}
}

void Server::CloseServer()
{
	puts("Server shutdown");
	closesocket(server_socket);
	freeaddrinfo(ptr);
	WSACleanup();
}

void Server::join(std::string _name, sockaddr_in _adress)
{
	int userId = users.listUsers.size() + 1;
	std::cout << "UserID: " << std::to_string(userId) << std::endl;

	//// Notice all player in lobby
	std::string newMassage = "!" + _name + ":" + std::to_string(userId);
	BroadcastMessageToAll((char*)newMassage.data());

	//// Sendback conformation, all other online player info, id, start info and seat info
	std::string sendBack;
	sendBack += "@";
	sendBack += std::to_string(userId);
	sendBack += ":";
	// number of user in server
	sendBack += std::to_string(users.listUsers.size());
	// users info
	if(users.listUsers.size() != 0)
	{
		for(std::list <User> ::iterator it = users.listUsers.begin(); it != users.listUsers.end(); it++)
		{
			sendBack += ":";
			sendBack += (*it)._name;
			sendBack += ":";
			sendBack += std::to_string((*it)._index);
		}
	}
	// seat info 
	for(int i = 0; i < 4; i++)
	{
		sendBack += ":";
		sendBack += std::to_string(lobbySeat[i]);
	}
	// start Info
	for(int i = 0; i < 4; i++)
	{
		sendBack += ":";
		sendBack += std::to_string(startButtons[i]);
	}
	// send
	HandleSending((char*)sendBack.c_str(), _adress);

	//// Add to player list
	users.addUser(_name, _adress, userId);
}

bool Server::checkAllStartPressed()
{
	bool check = true;
	for(int i = 0; i < 4; i++)
	{
		if(startButtons[i] == 0)
		{
			check = false;
			break;
		}
	}
	return check;
}

void Server::takeSeat(int _userId, int _seatId)
{
	if(lobbySeat[_seatId] == 0)
	{
		lobbySeat[_seatId] = _userId;
		// Update seat changing to all user
		std::string seatInfo = "#";
		for(int i = 0; i < 4; i++)
		{
			seatInfo += ":";
			seatInfo += std::to_string(lobbySeat[i]);
		}
		BroadcastMessageToAll((char*)seatInfo.c_str());
	}
	else
	{
		// Seat is occupied
		HandleSending((char*)"Seat is Occupied, please choose another one.", users.getAdress(_userId));
	}
}

void Server::leftSeat(int _id)
{
	// Remove user from seat
	for(int i = 0; i < 4; i++)
	{
		if(lobbySeat[i] == _id)
		{
			lobbySeat[i] = 0;
			break;
		}
	}
	// Update seat changing to all user
	std::string seatInfo = "#";
	for(int i = 0; i < 4; i++)
	{
		seatInfo += ":";
		seatInfo += std::to_string(lobbySeat[i]);
	}
	BroadcastMessageToAll((char*)seatInfo.c_str());
}

void Server::leftLobby(int _id)
{
	// Remove user from list of user
	users.removeUser(_id);

	// Let all user know
	//TODO:
	BroadcastMessageToAll((char*)"");
}

void Server::pressedStart(int _userId)
{
	int _id = 0;
	for(int i = 0; i < 4; i++)
	{
		if(lobbySeat[i] == _userId)
		{
			_id = i;
			break;
		}
	}
	startButtons[_id] = _userId;

	// Update seat changing to all user
	std::string startInfo = "%";
	for(int i = 0; i < 4; i++)
	{
		startInfo += ":";
		startInfo += std::to_string(startButtons[i]);
	}
	BroadcastMessageToAll((char*)startInfo.c_str());
}

void Server::leftGame(int _id)
{
	// Remove user from list of user
	users.removeUser(_id);

	// Let all user know
	//TODO:
	BroadcastMessageToAll((char*)"");
}
