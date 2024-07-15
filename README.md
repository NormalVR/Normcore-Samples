# TODO
This file should explain how to install (ex specific Unity version) and open the sample projects.

# TODO
* Update obsolete Normcore API
	* Ex Realtime.Instantiate warning in multiplayer drawing app sample

# TODO
* OculusSetFloorTrackingOriginWithoutPlugin
	* Can be done for any XR device using Unity's API: https://github.com/NormalVR/Normcore/blob/6a515dbed77d171f6b4fa76cc56d29ef48cbf3f5/Internal/Normcore%202022/Assets/XRRig/XRTrackingOriginMode.cs
	* Since we're using OpenXR atm instead of the Oculus plugin, OVRPlugin and OculusSetFloorTrackingOriginWithoutPlugin wouldn't work

# TODO
* Migrate from XRNodeState to Tracked Pose Driver in the Brush samples?
	* These are the only samples that use XRNodeState in their code for tracking
	* A few samples use the old input system in their code, but I don't think we need to blanket migrate to the new input system.
		* Because the new input system is harder to understand for beginners.

# TODO
* Update AR Core components in AR Spectator
	* There's 1-2 obsolete components, should be easy to update
	* Tested an Android mobile build. It works well!

# TODO
* OpenXR works out of the box
* But you need to specify your input controller types (they call it an interaction profile) in Project Settings
* I've enabled the Quest controller interaction profiles
* Non-Quest users will need to do this step or they won't receive input
	* Document this in this file?

# Versions
* Unity LTS - 2022.3.37f1
* Normcore 2.10.4
* OpenXR plugin
* Built-in renderer (not URP or HDRP)
