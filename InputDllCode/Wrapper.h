#pragma once
#include "XinputManager.h"

#ifdef __cplusplus
extern "C"
{
#endif
	// Put your functions here
	PLUGIN_API void controllerUpdate();
	PLUGIN_API bool controllerConnected(int m_index);
	PLUGIN_API int  getControllerType(int m_index);
	PLUGIN_API void setStickDeadZone(int index, float dz);
	PLUGIN_API void setTriggerDeadZone(int index, float dz);
	PLUGIN_API void getStickDeadZone(int index, float& dz);
	PLUGIN_API void getTriggerDeadZone(int index, float& dz);


	PLUGIN_API void setVibration(int index, float L,float R);
	PLUGIN_API void setVibrationL(int index, float L);
	PLUGIN_API void setVibrationR(int index, float R);
	PLUGIN_API void getVibration(int index, float& L, float& R);
	PLUGIN_API void getVibrationL(int index, float& L);
	PLUGIN_API void getVibrationR(int index, float& R);

	PLUGIN_API void resetVibration(int index);
	PLUGIN_API bool isButtonPressed(int index, int bitmask);
	PLUGIN_API bool isButtonReleased(int index, int bitmask);
	PLUGIN_API bool isButtonDown(int index, int bitmask);
	PLUGIN_API bool isButtonUp(int index, int bitmask);

	PLUGIN_API void getSticks(int index, Stick sticks[2]);
	PLUGIN_API void getTriggers(int index, Triggers& trig);

#ifdef __cplusplus
}
#endif

