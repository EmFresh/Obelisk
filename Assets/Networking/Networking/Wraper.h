#pragma once
#include "PluginSettings.h"
#include "IncludeThis.h"

typedef const char* cstring;
#ifdef __cplusplus
extern "C"
{
#endif
	//ERROR//

	//gets the last error that happened
	PLUGIN_API cstring getLastNetworkError(int);

	//NETWORK//

	//initializes Winsock
	PLUGIN_API bool initNetwork();
	//Shutdown Winsoc
	PLUGIN_API void shutdownNetwork();


	//ENDPOINT//

	//Creates a new IPEndpoint handle for a given IP and port. Multiple IPEndpoints can be created
	PLUGIN_API IPEndpointData createIPEndpointData(const char* ip, unsigned short port);


	//SOCKET//

	//Creates a new Socket handle for manageing IPEndpoints. 
	PLUGIN_API SocketData initSocketData();

	//initializes the socket to use UDP or TCP conection types
	PLUGIN_API PResult createSocket(SocketData&, SocketType = TCP);
	//closes socket so it can not be used unless createSocket() is called one again.
	PLUGIN_API PResult closeSocket(SocketData&);

	//Bind Endpoint to socket.
	PLUGIN_API 	PResult bindEndpointToSocket(const IPEndpointData& ep, const SocketData&);
	//Listens for endpoint connection to the socket. It will block until a new connection is found. 
	PLUGIN_API 	PResult listenEndpointToSocket(const IPEndpointData& ep, const SocketData&, int backlog = 5);
	//Attempts to accept the listened connection. 
	PLUGIN_API 	PResult acceptSocket(const SocketData& in, SocketData& out);
	//Connects endpoint to socket
	PLUGIN_API 	PResult connectEndpoint(const IPEndpointData& ep, const  SocketData&);

	// Sending Data
	// TCP

	//Send packet over TCP server. Not guaranteed to send all bytes.
	PLUGIN_API PResult sendPacketData(const SocketData&, void* data, int numberOfBytes, int& bytesSent);
	//Receive packet over TCP server. Not guaranteed to recieve all bytes.
	PLUGIN_API PResult recvPacketData(const SocketData&, void* dest, int numberOfBytes, int& bytesRecv);
	//Send entire packet over TCP server. guaranteed to send all bytes.
	PLUGIN_API PResult sendAllPacketData(const SocketData&, void* data, int numberOfBytes);
	//Receive entire packet over TCP server. guaranteed to recieve all bytes.
	PLUGIN_API PResult recvAllPacketData(const SocketData&, void* dest, int numberOfBytes);

	//UDP

	//Receive packet over UDP server. Not guaranteed to recieve all bytes.
	PLUGIN_API PResult recvFromPacketDataEx(const SocketData&, void* data, int numberOfBytes, int& bytesSent, const IPEndpointData& ep);
	//Receive packet over UDP server. Not guaranteed to recieve all bytes.
	PLUGIN_API PResult recvFromPacketData(const SocketData&, void* data, int numberOfBytes, const IPEndpointData& ep);
	//Send packet over UDP server. Not guaranteed to send all bytes.
	PLUGIN_API PResult sendToPacketDataEx(const SocketData&, void* data, int numberOfBytes, int& bytesSent, const IPEndpointData& ep);
	//Send packet over UDP server. Not guaranteed to send all bytes.
	PLUGIN_API PResult sendToPacketData(const SocketData&, void* data, int numberOfBytes, const IPEndpointData& ep);

#ifdef __cplusplus
}
#endif

