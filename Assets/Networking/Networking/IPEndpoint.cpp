#include "IPEndpoint.h"

IPEndpointData IPEndpoint::createIPEndpoint(const char* ip, unsigned short port)
{
	IPEndpointData data;

	data.m_port = port;

	in_addr addr;

	int result = inet_pton(AF_INET, ip, &addr);

	if(result == 1)
		if(addr.S_un.S_addr != INADDR_NONE)
		{
			unsigned length = (unsigned)strlen(ip) + 1;
			if(data.m_ipString)
				delete[] data.m_ipString;
			data.m_ipString = new char[length];
			memcpy_s((char*)data.m_ipString, length, ip, length);

			if(data.m_hostname)
				delete[] data.m_hostname;
			data.m_hostname = new char[length];
			memcpy_s((char*)data.m_hostname, length, ip, length);

			if(data.m_ipBytes)
				delete[] data.m_ipBytes;
			data.m_ipBytes = new uint8_t[sizeof(ULONG)];
			data.m_ipBytesSize = sizeof(ULONG);
			memcpy_s(data.m_ipBytes, sizeof(ULONG), &addr.S_un.S_addr, sizeof(ULONG));

			data.m_IPVersion = IPv4;
			return data;
		}

	addrinfo hints{};
	hints.ai_family = AF_INET;
	addrinfo* hostinfo = nullptr;
	result = getaddrinfo(ip, nullptr, &hints, &hostinfo);

	if(!result)//attempt to create IP from host name i.e.  
	{
		sockaddr_in* hostAddr = (sockaddr_in*)hostinfo->ai_addr;
		//hostAddr->sin_addr.S_un.S_addr;

		if(data.m_ipString)
			delete[] data.m_ipString;
		data.m_ipString = new char[16];
		inet_ntop(AF_INET, &hostAddr->sin_addr, (char*)data.m_ipString, 16);

		if(data.m_hostname)
			delete[] data.m_hostname;
		unsigned length = (unsigned)strlen(ip) + 1;
		data.m_hostname = new char[length];
		memcpy_s((char*)data.m_hostname, length, ip, length);

		ULONG ipLong = hostAddr->sin_addr.S_un.S_addr;

		if(data.m_ipBytes)
			delete[] data.m_ipBytes;
		data.m_ipBytes = new uint8_t[sizeof(ULONG)];
		data.m_ipBytesSize = sizeof(ULONG);
		memcpy_s(data.m_ipBytes, sizeof(ULONG), &ipLong, sizeof(ULONG));

		data.m_IPVersion = IPv4;

		freeaddrinfo(hostinfo);
		return data;
	}

	return data;
}

IPEndpointData IPEndpoint::createIPEndpoint(const sockaddr* addr)
{
	assert(addr->sa_family == AF_INET);

	IPEndpointData data;

	sockaddr_in* addrv4 = reinterpret_cast<sockaddr_in*>((sockaddr*)addr);

	data.m_IPVersion = IPVersion::IPv4;
	data.m_port = ntohs(addrv4->sin_port);

	if(data.m_ipBytes)
		delete[] data.m_ipBytes;
	data.m_ipBytes = new uint8_t[sizeof(ULONG)];
	data.m_ipBytesSize = sizeof(ULONG);
	memcpy_s(&data.m_ipBytes[0], sizeof(ULONG), &addrv4->sin_addr, sizeof(ULONG));

	if(data.m_ipString)
		delete[] data.m_ipString;
	data.m_ipString = new char[16];

	inet_ntop(AF_INET, &addrv4->sin_addr, (char*)data.m_ipString, 16);



	data.m_hostname = data.m_ipString;
	return data;
}

sockaddr_in IPEndpoint::getSockAddrIPv4(const IPEndpointData& data)
{
	assert(data.m_IPVersion == IPVersion::IPv4);
	sockaddr_in addr = {};
	addr.sin_family = AF_INET;

	//addr.sin_addr.S_un.S_addr = ADDR_ANY;//not sure if this is specific for UDP?NVM it's just zero???
	memcpy_s(&addr.sin_addr, sizeof(ULONG), data.m_ipBytes, sizeof(ULONG));

	addr.sin_port = htons(data.m_port);
	return addr;
}
