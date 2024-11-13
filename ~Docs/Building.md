# How to build
This page does a short overview of the build process: https://docs.unity3d.com/Manual/PublishingBuilds.html

# Scene selection
Don't forget to double-check which scenes are included in the build. Note that the top-most scene inside the `Build Settings` window is the one that will be loaded at startup.

For example the AR Spectator sample has two scenes that are meant for two different platforms (VR & mobile), so make sure that the scenes list inside the `Build Settings` window reflects your target platform before you build. Add or re-order the scenes as necessary.

# Standalone and PCVR
This platform has the simplest build process. There's no tricky steps required, simply press the `Build` button inside the `Build Settings` window.

# Meta Quest (Android)
Note: These instructions use the `OpenXR` XR plugin. But the `Oculus` XR plugin can also be used instead with slightly different steps.

* Switch to the Android platform inside the `Build Settings` window (if it's grayed out it means you need to [install the Android module](https://docs.unity3d.com/Manual/android-sdksetup.html))
* Go to the `XR Plug-In Management` section in the `Project Settings` window:
  * Switch to the Android tab
  * Enable the `OpenXR` XR plugin
* Go to the `OpenXR` section under `XR Plug-In Management`:
  * Enable the `Meta Quest Support` feature group
* Go to the `Project Validation` tab under the `XR Plug-In Management` section:
  * Apply all the suggested fixes
* Go to the `Player` section in the `Project Settings` window:
  * Remove incompatible `Graphics APIs` such as `OpenGLES2`
  * Bump the `Minimum API Level` to 32
  * Set `Internet Access` to `Require`

If the build isn't working correctly on the device see if there's any useful [info in the logs](Troubleshooting.md#how-to-check-logs-on-questandroid).

# Mobile (Android)
* Switch to the Android platform inside the `Build Settings` window (if it's grayed out it means you need to [install the Android module](https://docs.unity3d.com/Manual/android-sdksetup.html))
* Go to the `XR Plug-In Management` section in the `Project Settings` window
* Switch to the Android tab
* Enable the `ARCore` XR plugin
* Apply all the suggested fixes inside the `Project Validation` tab under the `XR Plug-In Management` section
* Click the `Build` button inside the `Build Settings` window

If the build isn't working correctly on the device see if there's any useful [info in the logs](Troubleshooting.md#how-to-check-logs-on-questandroid).
