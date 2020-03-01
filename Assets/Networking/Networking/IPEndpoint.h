#pragma once
#include <string>
#include <vector>
#include <cassert>
#include <WS2tcpip.h>
#include "IPVersion.h"
typedef const char* cstring;
struct IPEndpointData
{
	IPVersion m_IPVersion = IPVersion::IPUnknown;
	cstring m_hostname = nullptr;
	cstring m_ipString = nullptr;
	ULONG m_ipBytesSize = 0;
	uint8_t* m_ipBytes = nullptr;
	unsigned short m_port = 0;

	void print()
	{
		switch(m_IPVersion)
		{
		case IPVersion::IPv4:
			printf("IP Version: IPv4\n");
			break;
		case IPVersion::IPv6:
			printf("IP Version: IPv6\n");
			break;
		default:
			printf("IP Version: Unknown\n");
		}

		printf
		(
			"HostName: %s\n"
			"Port: %d\n"
			"IP: %s\n"
			"IP bytes... ",
			m_hostname, m_port, m_ipString
		);

		for(int a = 0; a < (int)m_ipBytesSize; ++a)
			printf("%d.", int(m_ipBytes[a]));
		puts("\b \b");
	}
};

class IPEndpoint
{
public:
	static IPEndpointData createIPEndpoint(const char* ip, unsigned short port);

	//NETWORK USE ONLY//

	static IPEndpointData createIPEndpoint(const sockaddr* addr);
	static sockaddr_in getSockAddrIPv4(const IPEndpointData&);
};

