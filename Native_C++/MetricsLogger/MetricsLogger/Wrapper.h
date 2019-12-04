#pragma once

#include "PluginSettings.h"
#include "MetricsLogger.h"

#ifdef __cplusplus

extern "C"
{
#endif
	//Functions go here, Josh

	PLUGIN_API int TestFunction();
	PLUGIN_API void WriteInputToText(const char* input);
	PLUGIN_API void WriteStateToText(float time, int num, bool hasNotWritten);

#ifdef __cplusplus

}

#endif