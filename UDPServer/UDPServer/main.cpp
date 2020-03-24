///// UDP Server
#include "Server.h"
#include <thread>

Server myServer;

int main()
{
	atexit([](){myServer.CloseServer(); });//cleans up server once program exits
	myServer.CreateServer();

	std::thread receiveThread = std::thread([](){myServer.UpdateRecv();	});
	std::thread sendThread = std::thread([](){myServer.UpdateSend(); });

	while(true)if(!myServer.isServerRunning)break;

	return 0;
}