#pragma once
#define WIN32_LEAN_AND_MEAN

#include <WinSock2.h>
#include "LastNetworkError.h"
#include "IPVersion.h"
#include "IPEndpoint.h"
#include "Packet.h"


enum SocketOption
{
	TCP_NoDelay
};

enum SocketType
{
	TCP,
	UDP
};

enum PResult
{
	P_Success,
	P_GenericError
};

typedef SOCKET SocketHandle;

struct SocketData
{
	IPVersion m_IPVersion;
	SocketHandle m_hnd;
};


class Socket
{
public:
	static SocketData initSocket(IPVersion version = IPv4,
								 SocketHandle hnd = INVALID_SOCKET);

	static PResult create(SocketData&, SocketType = TCP);
	static PResult close(SocketData&);

	static PResult bindEndpoint(const IPEndpointData& ep, const SocketData&);
	static PResult listenEndpoint(const IPEndpointData& ep, const  SocketData&, int backlog = 5);
	static PResult acceptSocket(const SocketData&, SocketData&);
	static PResult connectEndpoint(const IPEndpointData& ep, const SocketData&);

	// Sending Data //
	//TCP
	static PResult sendPacket(const SocketData&, void* data, int numberOfBytes, int& bytesSent);
	static PResult sendPacket(const SocketData&, Packet& data);
	static PResult recvPacket(const SocketData&, void* dest, int numberOfBytes, int& bytesRecv);
	static PResult recvPacket(const SocketData&, Packet& dest);
	static PResult sendAllPacket(const SocketData&, void* data, int numberOfBytes);
	static PResult recvAllPacket(const SocketData&, void* dest, int numberOfBytes);

	//UDP
	static PResult recvFromPacket(const SocketData&, void* data, int numberOfBytes, int& bytesSent, const IPEndpointData& ep);
	static PResult recvFromPacket(const SocketData&, void* data, int numberOfBytes, const IPEndpointData& ep);
	static PResult sendToPacket(const SocketData&, void* data, int numberOfBytes, int& bytesSent, const IPEndpointData& ep);
	static PResult sendToPacket(const SocketData&, void* data, int numberOfBytes, const IPEndpointData& ep);

private:
	static PResult createTCP(SocketData& data);
	static PResult createUDP(SocketData& data);
	static PResult setSocketOption(SocketData&, SocketOption opt, void* val);

};