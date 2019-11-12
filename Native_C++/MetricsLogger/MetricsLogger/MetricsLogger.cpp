#include "MetricsLogger.h"

int MetricsLogger::TestFunction()
{
	return 1;

}

void MetricsLogger::WriteInputToText(const char* input)
{
	std::fstream inputWrite;

	inputWrite.open("MetricsText", std::fstream::out);

	inputWrite << input; 

	inputWrite.close();

}

void MetricsLogger::WriteStateToText(float time, int num, bool hasNotWritten)
{
	
	std::string state;
	std::fstream stateWrite;

	stateWrite.open("MetricsText.txt", std::fstream::app);

	if (num == 1)
		state = "learnMovementStage1";
	if (num == 2)
		state = "learnMovementStage2";
	if (num == 3)
		state = "learnPickupSmall";
	if (num == 4)
		state = "learnPickupLarge";
	if (num == 5)
		state = "learnSorting";
	if (num == 6)
		state = "learnPickupFromChest";
	if (num == 7)
		state = "learnTowerBuild";

	if (hasNotWritten)
	{
		hasNotWritten = false;
		stateWrite << std::endl << "State: " << state << std::endl << "Time: " << time << std::endl;
	}

	stateWrite.close();
}

