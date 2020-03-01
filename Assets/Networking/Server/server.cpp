#pragma once
#include <../Networking/IncludeThis.h>
#include <iostream>
#include <thread>
#include "keyinput.h"

struct vec3
{
	float x = 0, y = 0, z = 0;

	void print()
	{
		printf("(%.2f, %.2f, %.2f)", x, y, z);
	}
};

#define UDPIMP
//#define TCPIMP

#ifdef UDPIMP

int main()
{
	if(Network::init())
	{
		IPEndpointData endp = IPEndpoint::createIPEndpoint("localhost", 22);


		if(endp.m_IPVersion == IPVersion::IPv4)
		{
			endp.print();
			puts("");

			SocketData sock = Socket::initSocket(), newSock;

			if(Socket::create(sock, UDP) == PResult::P_Success)
			{
				printf("Created Socket\n");
				if(Socket::bindEndpoint(endp, sock) == PResult::P_Success)
					//if(sock.listenEndpoint(endp) == PResult::P_Success)
				{
					puts("Bind Endpoint");

					auto result = P_Success;
					while(true)
					{


						static int size, dump;

						vec3 dat;
						if(Socket::recvFromPacket(sock, &size, 4, dump, endp) == P_Success)
						{
							//printf("%s", LastNetworkError::GetLastError().c_str());
							if(size > 0)
								if(Socket::recvFromPacket(sock, &dat, size, dump, endp) == P_Success)
								{
									if(dump == sizeof(vec3))
										dat.print();
									puts("\n");
								}
						}

						//printf("Size: %d\n", size);
					}

				}
				Socket::close(sock);
			}
		}
	}

	Network::shutdown();
	system("pause");
}
#endif // UDPIMP

#ifdef TCPIMP

void  sendMesages(SocketData sock)
{
	std::string msg;
	while(true)
	{
		std::getline(std::cin, msg);
		int size = ((int)msg.size()) + 1, dump = 0;
		Socket::sendAllPacket(sock, &size, 4);
		Socket::sendAllPacket(sock, (char*)msg.data(), size);
	}
}
void recvMesages(SocketData sock)
{

	int size = 0, dump = 0;
	std::string msg;
	while(true)
	{
		Socket::recvAllPacket(sock, &size, 4);
		msg.resize(size);
		Socket::recvAllPacket(sock, (char*)msg.data(), size);
		printf("Message Received: %s\n", msg.data());
	}
}
int main()
{
	if(Network::init())
	{
		IPEndpointData endp = IPEndpoint::createIPEndpoint("localhost", 22);

		if(endp.m_IPVersion == IPVersion::IPv4)
		{
			endp.print();
			puts("");

			SocketData sock = Socket::initSocket(), newSock;

			if(Socket::create(sock, TCP) == PResult::P_Success)
			{
				printf("Created Socket\n");
				if(Socket::listenEndpoint(endp, sock) == PResult::P_Success)
				{
					puts("Listen Endpoint");

					if(Socket::acceptSocket(sock, newSock) == PResult::P_Success)
					{
						puts("Accepted Socket");
						auto result = P_Success;
						std::thread snd(sendMesages, newSock), rec(recvMesages, newSock);
						while(true);
					}
					printf("%s\n", LastNetworkError::GetLastError().c_str());

				}
				printf("%s\n", LastNetworkError::GetLastError().c_str());

				Socket::close(sock);
			}
			printf("%s\n", LastNetworkError::GetLastError().c_str());
		}
	}

	Network::shutdown();
	system("pause");
}

#endif // TCPIMP
