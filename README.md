# TODO
This file should explain how to install (ex specific Unity version) and open the sample projects.

# TODO
* Update obsolete Normcore API
	* Ex Realtime.Instantiate warning in multiplayer drawing app sample

# TODO
* OculusSetFloorTrackingOriginWithoutPlugin
	* Can be done for any XR device using Unity's API: https://github.com/NormalVR/Normcore/blob/6a515dbed77d171f6b4fa76cc56d29ef48cbf3f5/Internal/Normcore%202022/Assets/XRRig/XRTrackingOriginMode.cs

# TODO
* OpenXR works out of the box
* But you need to specify your input controller types (they call it an interaction profile) in Project Settings
* I've enabled the Quest controller interaction profiles
* Non-Quest users will need to do this step or they won't receive input
	* Document this in this file?

# Versions
* Unity LTS - 2022.3.37f1
* Normcore 2.10.4
* Built-in renderer (not URP or HDRP)
