#include "Wrapper.h"
#define reclass(a_class,a_val) (*(a_class*)&(a_val))

void controllerUpdate()
{
	XinputManager::update();
}

bool controllerConnected(int index)
{
	return XinputManager::controllerConnected(index);
}

int getControllerType(int index)
{
	return XinputManager::getControllerType(index);
}

void setStickDeadZone(int index, float dz)
{
	XinputManager::getController(index)->setStickDeadZone(dz);
}

void setTriggerDeadZone(int index, float dz)
{
	return XinputManager::getController(index)->setTriggerDeadZone(dz);
}

void getStickDeadZone(int index, float& dz)
{
	dz = XinputManager::getController(index)->getStickDeadZone();
}

PLUGIN_API void getTriggerDeadZone(int index, float& dz)
{
	dz = XinputManager::getController(index)->getTriggerDeadZone();
}

void setVibration(int index, float L, float R)
{
	XinputManager::getController(index)->setVibration(L, R);
}

void setVibrationL(int index, float L)
{
	XinputManager::getController(index)->setVibrationL(L);
}

void setVibrationR(int index, float R)
{
	XinputManager::getController(index)->setVibrationR(R);
}

void getVibration(int index, float& L, float& R)
{
	XinputManager::getController(index)->getVibration(L, R);
}

void getVibrationL(int index, float& L)
{
	XinputManager::getController(index)->getVibrationL(L);
}

void getVibrationR(int index, float& R)
{
	XinputManager::getController(index)->getVibrationR(R);
}

void resetVibration(int index)
{
	XinputManager::getController(index)->resetVibration();
}

bool isButtonPressed(int index, int bitmask)
{
	return XinputManager::getController(index)->isButtonPressed(bitmask);
}

bool isButtonReleased(int index, int bitmask)
{
	return XinputManager::getController(index)->isButtonReleased(bitmask);
}

bool isButtonDown(int index, int bitmask)
{
	return XinputManager::getController(index)->isButtonDown(bitmask);
}

bool isButtonUp(int index, int bitmask)
{
	return XinputManager::getController(index)->isButtonUp(bitmask);
}

void getSticks(int index, Stick sticks[2])
{
	if(XinputManager::getControllerType(index) == XINPUT_CONTROLLER)
	{
		sticks[0] = ((XinputController*)XinputManager::getController(index))->getSticks()[0];
		sticks[1] = ((XinputController*)XinputManager::getController(index))->getSticks()[1];
	}
}

void getTriggers(int index, Triggers& trig)
{
	if(XinputManager::getControllerType(index) == XINPUT_CONTROLLER)
		trig = ((XinputController*)XinputManager::getController(index))->getTriggers();
}