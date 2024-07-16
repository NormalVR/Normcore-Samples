# Troubleshooting

## Realtime: The app key is null or empty. Make sure your NormcoreAppSettings object has a valid app key before connecting.
You probably forgot to copy paste your app key into the `Realtime` component.

Follow the steps in [this page](https://normcore.io/documentation/essentials/getting-started#creating-a-normcore-application) to connect successfully.

## XR controller input isn't working
OpenXR needs a list of XR input controllers. We added the Meta Quest controllers by default.

If you're building to a different XR device then you'll have to add its interaction profiles inside this menu:

TODO screenshot of interaction profiles UI

## Can't switch to the Android build target
If the Android build target is greyed out inside the Build Settings, it means that module isn't installed.

Install it by following these steps: https://docs.unity3d.com/Manual/android-sdksetup.html
