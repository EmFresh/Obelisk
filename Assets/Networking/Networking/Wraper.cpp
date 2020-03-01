#include "Wraper.h"

cstring getLastNetworkError(int)
{
	
	static std::string tmp; tmp = LastNetworkError::GetLastError();
	//strcpy_s(in,size, tmp.c_str());
	return tmp.c_str();
}

bool initNetwork()
{
	return Network::init();
}

void shutdownNetwork()
{
	Network::shutdown();
}

IPEndpointData createIPEndpointData(const char* ip, unsigned short port)
{
	return IPEndpoint::createIPEndpoint(ip, port);
}

SocketData initSocketData()
{
	return Socket::initSocket();
}

PResult createSocket(SocketData& sock, SocketType type)
{
	return Socket::create(sock, type);
}

PResult closeSocket(SocketData& soc)
{
	return Socket::close(soc);
}

PResult bindEndpointToSocket(const IPEndpointData& ep, const SocketData& soc)
{
	return Socket::bindEndpoint(ep, soc);
}

PResult listenEndpointToSocket(const IPEndpointData& ep, const SocketData& soc, int backlog)
{
	return Socket::listenEndpoint(ep, soc, backlog);
}

PResult acceptSocket(const SocketData& in, SocketData& out)
{
	return Socket::acceptSocket(in, out);
}

PResult connectEndpoint(const IPEndpointData& ep, const SocketData& soc)
{
	return Socket::listenEndpoint(ep, soc);
}

PResult sendPacketData(const SocketData& soc, void* data, int numberOfBytes, int& bytesSent)
{
	return Socket::sendPacket(soc, data, numberOfBytes, bytesSent);
}

PResult recvPacketData(const SocketData& soc, void* dest, int numberOfBytes, int& bytesRecv)
{
	return Socket::recvPacket(soc, dest, numberOfBytes, bytesRecv);
}

PResult sendAllPacketData(const SocketData& soc, void* data, int numberOfBytes)
{
	return Socket::sendAllPacket(soc, data, numberOfBytes);
}

PResult recvAllPacketData(const SocketData& soc, void* dest, int numberOfBytes)
{
	return Socket::recvAllPacket(soc, dest, numberOfBytes);
}



PResult recvFromPacketDataEx(const SocketData& soc, void* data, int numberOfBytes, int& bytesSent, const IPEndpointData& ep)
{
	return Socket::recvFromPacket(soc, data, numberOfBytes, bytesSent, ep);
}

PResult recvFromPacketData(const SocketData& soc, void* data, int numberOfBytes, const IPEndpointData& ep)
{
	return Socket::recvFromPacket(soc, data, numberOfBytes, ep);
}

PResult sendToPacketDataEx(const SocketData& soc, void* data, int numberOfBytes, int& bytesSent, const IPEndpointData& ep)
{
	return Socket::sendToPacket(soc, data, numberOfBytes, bytesSent, ep);
}

PResult sendToPacketData(const SocketData& soc, void* data, int numberOfBytes, const IPEndpointData& ep)
{
	return Socket::sendToPacket(soc, data, numberOfBytes, ep);
}
