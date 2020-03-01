#include "Network.h"
#include <cstdio>

bool Network::init()
{
	WSADATA wsaData;
	int resault = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if(resault)
	{
		printf("this doesent fuckn work\n\n");
		return false;
	}

	if(LOBYTE(wsaData.wVersion)!=2 || HIBYTE(wsaData.wVersion) != 2)
	{
		printf("this version doesent fuckn work\n\n");
		return false;
	}

	return true;
}

void Network::shutdown()
{
	WSACleanup();
}
