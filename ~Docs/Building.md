# How to build
This page does a short overview of the build process: https://docs.unity3d.com/Manual/PublishingBuilds.html

# Scene selection
Don't forget to double-check which scenes are included in the build. Note that the top-most scene inside the `Build Settings` window is the one that will be loaded at startup.

For example the AR Spectator sample has 2 scenes that are meant for two different platforms (VR & mobile) - make sure the scenes list inside the `Build Settings` window reflects your target platform before you build. Add or re-order the scenes as needed.

# Standalone and PCVR
This platform has the simplest build process. There's no tricky steps required, simply press the `Build` button inside the `Build Settings` window.

# Meta Quest (Android)
TODO

# Mobile (Android)
* Switch to the Android platform inside the `Build Settings` window (if it's grayed out it means you need to [install the Android module](https://docs.unity3d.com/Manual/android-sdksetup.html))
* Go to the `XR Plug-In Management` section in the Project Settings
* Switch to the Android tab
* Enable the `ARCore` XR plugin
* Apply all the suggested fixes inside the `Project Validation` tab under the `XR Plug-In Management` section
* Click the `Build` button inside the `Build Settings` window
