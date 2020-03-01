#include <Networking/IncludeThis.h>
#include <ctime>
#include <iostream>
#include <thread>
#include "keyinput.h"

struct vec3
{
	float x = 0, y = 0, z = 0;

	vec3 operator+(vec3 in)
	{
		return {x + in.x, y + in.y, z + in.z};
	}
	vec3 operator-(vec3 in)
	{
		return {x - in.x, y - in.y, z - in.z};
	}
	bool operator==(vec3 in)
	{
		return x == in.x && y == in.y && z == in.z;
	}

	bool operator!=(vec3 in)
	{
		return !(*this == in);
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
		}

		SocketData sock = Socket::initSocket();

		if(Socket::create(sock, UDP) == PResult::P_Success)
		{
			printf("Created Socket\n");
			if(Socket::connectEndpoint(endp, sock) == PResult::P_Success)
			{
				printf("conncted to endpoint\n");

				while(true)
				{


					static vec3 dat,last;
					if(KeyInput::press('a'))dat = dat - vec3{.001f,0,0};
					if(KeyInput::press('w'))dat = dat + vec3{0,.001f,0};
					if(KeyInput::press('s'))dat = dat - vec3{0,.001f,0};
					if(KeyInput::press('d'))dat = dat + vec3{.001f,0,0};

					int size = sizeof(dat);
					bool sending=true;
					if(last != dat)
					{
						if(Socket::sendToPacket(sock, &size, 4, endp) == P_GenericError)
							sending = false;
						if(Socket::sendToPacket(sock, &dat, size, endp)== P_GenericError)
							sending = false;
						
							last = dat;
					}

				}//delete[] str;
			}
			Socket::close(sock);
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
		Socket::sendPacket(sock, &size, 4, dump);
		Socket::sendPacket(sock, (char*)msg.data(), size, dump);


	}
}

void recvMesages(SocketData sock)
{

	int size = 0, dump = 0;
	std::string msg;
	while(true)
	{
		Socket::recvPacket(sock, &size, 4, dump);
		msg.resize(size);
		Socket::recvPacket(sock, (char*)msg.data(), size, dump);
		printf("Message Recieved: %s\n", msg.data());
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

			SocketData sock = Socket::initSocket();

			if(Socket::create(sock, TCP) == PResult::P_Success)
			{
				printf("Created Socket\n");
				while(true)
				{
					if(Socket::connectEndpoint(endp, sock) == PResult::P_Success)
					{
						puts("Connect Endpoint");
						auto result = P_Success;
						std::thread snd(sendMesages, sock), rec(recvMesages, sock);
						while(true);
					}
					printf("%s\n", LastNetworkError::GetLastError().c_str());
					Socket::close(sock);
				}
			}
		}

		Network::shutdown();
		system("pause");
	}
}
#endif // TCPIMP
