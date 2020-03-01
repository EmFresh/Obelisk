#include "Socket.h"
#include <cassert>

typedef const char* cstring;



SocketData Socket::initSocket(IPVersion version, SocketHandle hnd)
{
	assert(version == IPv4);
	SocketData data;
	data.m_hnd = hnd;
	data.m_IPVersion = version;

	return data;
}

PResult Socket::create(SocketData& data, SocketType in)
{
	switch(in)
	{
	case TCP:
		return createTCP(data);
	case UDP:
		return createUDP(data);
	default:
		return PResult::P_GenericError;
	}
}

PResult Socket::close(SocketData& data)
{

	if(data.m_hnd == INVALID_SOCKET)
		return P_GenericError;

	if(closesocket(data.m_hnd))
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("socket close error: ", error);

		return P_GenericError;
	}

	data.m_hnd = INVALID_SOCKET;
	return P_Success;
}

PResult Socket::bindEndpoint(const IPEndpointData& ep, const SocketData& data)
{
	sockaddr_in addr = IPEndpoint::getSockAddrIPv4(ep);
	int result = bind(data.m_hnd, (sockaddr*)&addr, sizeof(sockaddr_in));
	if(result == SOCKET_ERROR)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("bind endpoint error: ", error);
		return PResult::P_GenericError;
	}
	return PResult::P_Success;
}

PResult Socket::listenEndpoint(const IPEndpointData& ep, const SocketData& data, int backlog)
{

	if(bindEndpoint(ep, data) != PResult::P_Success)
		return PResult::P_GenericError;

	int result = listen(data.m_hnd, backlog);

	if(result == SOCKET_ERROR)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("listen endpoint error: ", error);

		puts("");
		return PResult::P_GenericError;
	}


	return PResult::P_Success;
}

PResult Socket::acceptSocket(const SocketData& indata, SocketData& outdata)
{
	sockaddr_in addr = {};
	int len = sizeof(sockaddr_in);
	SocketHandle acceptConditionHnd = accept(indata.m_hnd, (sockaddr*)&addr, &len);//handle to other connection

	if(acceptConditionHnd == INVALID_SOCKET)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("accept socket error: ", error);

		return PResult::P_GenericError;
	}
	
	outdata = initSocket(IPVersion::IPv4, acceptConditionHnd);
	return PResult::P_Success;
}

PResult Socket::connectEndpoint(const IPEndpointData& ep, const SocketData& data)
{
	sockaddr_in addr = IPEndpoint::getSockAddrIPv4(ep);
	int result = connect(data.m_hnd, (sockaddr*)&addr, sizeof(sockaddr_in));

	if(result)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("connect endpoint error: ", error);

		return PResult::P_GenericError;
	}
	return PResult::P_Success;
}

PResult Socket::sendPacket(const SocketData& soc, void* data, int numberOfBytes, int& bytesSent)
{
	bytesSent = send(soc.m_hnd, (const char*)data, numberOfBytes, NULL);

	if(bytesSent == SOCKET_ERROR)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("send packet error: ", error);

		return PResult::P_GenericError;
	}
	return PResult::P_Success;
}

PResult Socket::sendPacket(const SocketData& soc, Packet& data)
{
	uint16_t encodedPacketSize = (uint16_t)htonl((u_long)data.buffer.size());
	PResult result = sendAllPacket(soc, &encodedPacketSize, sizeof(uint16_t));

	if(result != PResult::P_Success)
		return PResult::P_GenericError;

	result = sendAllPacket(soc, data.buffer.data(), (int)data.buffer.size());

	if(result != PResult::P_Success)
		return PResult::P_GenericError;

	return PResult::P_Success;
}

PResult Socket::recvPacket(const SocketData& data, void* dest, int numberOfBytes, int& bytesRecv)
{
	bytesRecv = recv(data.m_hnd, (char*)dest, numberOfBytes, NULL);

	if(!bytesRecv)// if connection was gracefully closed
		return PResult::P_GenericError;


	if(bytesRecv == SOCKET_ERROR)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("recv packet error: ", error);

		return PResult::P_GenericError;
	}

	return PResult::P_Success;
}

PResult Socket::recvPacket(const SocketData& data, Packet& dest)
{
	dest.clear();

	uint16_t encodedSize = 0;
	PResult result = recvAllPacket(data, &encodedSize, sizeof(uint16_t));
	if(result != PResult::P_Success)
		return PResult::P_GenericError;

	encodedSize = (uint16_t)ntohl((u_long)encodedSize);

	if(encodedSize > MAX_PACKET_SIZE)
		return PResult::P_GenericError;

	dest.buffer.resize(encodedSize);
	result = recvAllPacket(data, dest.buffer.data(), encodedSize);

	if(result != PResult::P_Success)
		return PResult::P_GenericError;


	return PResult::P_Success;
}

PResult Socket::sendAllPacket(const SocketData& soc, void* data, int numberOfBytes)
{
	int sentBytes = 0, totalBytes = 0, bitesRemaining = 0;
	//PResult result = PResult::P_Success;
	while(numberOfBytes > totalBytes)
	{
		bitesRemaining = numberOfBytes - totalBytes;
		if((sendPacket(soc, (char*)data + totalBytes, bitesRemaining, sentBytes))
		   != PResult::P_Success)
			return PResult::P_GenericError;
		totalBytes += sentBytes;
	}

	return PResult::P_Success;
}

PResult Socket::recvAllPacket(const SocketData& data, void* dest, int numberOfBytes)
{
	int recvBytes = 0, totalBytes = 0, bitesRemaining = 0;
	//PResult result = PResult::P_Success;

	while(numberOfBytes > totalBytes)
	{
		bitesRemaining = numberOfBytes - totalBytes;
		if(recvPacket(data, (char*)dest + totalBytes, bitesRemaining, recvBytes)
		   != PResult::P_Success)
			return PResult::P_GenericError;

		totalBytes += recvBytes;
	}
	return PResult::P_Success;
}

PResult Socket::recvFromPacket(const SocketData& data, void* dest, int numberOfBytes, int& bytesRecv, const IPEndpointData& ep)
{
	sockaddr_in client = IPEndpoint::getSockAddrIPv4(ep);

	static int clientLength = sizeof(client);
	bytesRecv = recvfrom(data.m_hnd, (char*)dest, numberOfBytes, NULL, (sockaddr*)&client, &clientLength);

	if(!bytesRecv)// if connection was gracefully closed
		return PResult::P_GenericError;


	if(bytesRecv == SOCKET_ERROR)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("receve from packet error: ", error);

		return PResult::P_GenericError;
	}

	return PResult::P_Success;
}

PResult Socket::recvFromPacket(const SocketData& soc, void* data, int numberOfBytes, const IPEndpointData& ep)
{
	static int dump = 0;
	return recvFromPacket(soc, data, numberOfBytes, dump, ep);
}

PResult Socket::sendToPacket(const SocketData& soc, void* data, int numberOfBytes, int& bytesSent, const IPEndpointData& ep)
{
	sockaddr_in server = IPEndpoint::getSockAddrIPv4(ep);

	static int serverLength = sizeof(server);

	bytesSent = sendto(soc.m_hnd, (const char*)data, numberOfBytes, NULL, (sockaddr*)&server, serverLength);

	if(bytesSent == SOCKET_ERROR)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("send packet error: ", error);

		return PResult::P_GenericError;
	}
	return PResult::P_Success;
}

PResult Socket::sendToPacket(const SocketData& soc, void* data, int numberOfBytes, const IPEndpointData& ep)
{
	static int dump;
	return sendToPacket(soc, data, numberOfBytes, dump, ep);
}


PResult Socket::createTCP(SocketData& data)
{
	assert(data.m_IPVersion == IPv4);

	if(data.m_hnd != INVALID_SOCKET)
		return P_GenericError;

	data.m_hnd = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if(data.m_hnd == INVALID_SOCKET)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("create TCP error: ", error);

		return P_GenericError;
	}

	BOOL tru = TRUE;
	if(setSocketOption(data, TCP_NoDelay, &tru))
		return P_GenericError;

	return P_Success;
}

PResult Socket::createUDP(SocketData& data)
{
	assert(data.m_IPVersion == IPv4);

	if(data.m_hnd != INVALID_SOCKET)
		return P_GenericError;

	data.m_hnd = socket(AF_INET, SOCK_DGRAM, IPPROTO_IP/*0*/);

	if(data.m_hnd == INVALID_SOCKET)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("create UDP error: ", error);

		return P_GenericError;
	}


	//if(setSocketOption(TCP_NoDelay, TRUE))
	//	return P_GenericError;

	return P_Success;
}

PResult Socket::setSocketOption(SocketData& data, SocketOption opt, void* val)
{
	int result = 0;
	switch(opt)
	{
	case SocketOption::TCP_NoDelay:
		result = setsockopt(data.m_hnd, IPPROTO_TCP, TCP_NODELAY, (const char*)val, sizeof(BOOL));
		break;
	default:
		//	setsockopt(m_hnd, SOL_SOCKET, SO_RCVTIMEO, (const char*)val, sizeof(unsigned));
		return P_GenericError;
	}

	if(result)
	{
		int error = WSAGetLastError();

		LastNetworkError::SetLastError("socket options error: ", error);

		return P_GenericError;
	}


	return P_Success;
}

