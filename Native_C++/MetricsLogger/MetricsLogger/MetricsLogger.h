#pragma once
#include <fstream>
#include "PluginSettings.h"
#include <string>


class PLUGIN_API MetricsLogger
{
public: 
	int TestFunction();

	void WriteInputToText(const char* input);

	void WriteStateToText(float time, int num, bool hasNotWritten);




};