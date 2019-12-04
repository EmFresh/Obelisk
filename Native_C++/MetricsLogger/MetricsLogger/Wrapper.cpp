#pragma once
#include "Wrapper.h"

MetricsLogger metricsLogger;

int TestFunction()
{
	return metricsLogger.TestFunction();
}

void WriteInputToText(const char* input)
{
	return metricsLogger.WriteInputToText(input);
}

void WriteStateToText(float time, int num, bool hasNotWritten)
{
	return metricsLogger.WriteStateToText(time, num, hasNotWritten);

	
}

