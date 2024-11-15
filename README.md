![](~Docs/Images/Header.jpg)

# About
[Normcore](https://normcore.io/) is a multiplayer SDK and game server hosting service for Unity. The [Normcore Asset Store package](https://assetstore.unity.com/packages/tools/network/normcore-free-multiplayer-voice-chat-for-all-platforms-195224) comes with some basic Examples to get you started. This repository contains larger sample projects that demonstrate more complex multiplayer concepts.

## Guides
Each sample project in this repository is accompanied by a guide that walks you through the details:
* [Creating a Multiplayer Drawing App](https://normcore.io/documentation/guides/creating-a-multiplayer-drawing-app)
* [Using AR as a Spectator View for Your XR App](https://normcore.io/documentation/guides/using-ar-as-a-spectator-view)
* [Synchronizing Custom Data](https://normcore.io/documentation/realtime/synchronizing-custom-data)
* [Recipe: RPC Events](https://normcore.io/documentation/guides/recipes/rpc-events)
* [Recipe: Using Addressables](https://normcore.io/documentation/guides/recipes/using-addressables)

# Downloading the samples
Clone the repo using [git](https://docs.github.com/en/get-started/using-git/about-git). Or click "Download ZIP" and unzip the contained files.

![](~Docs/Images/DownloadZIP.jpg)

# Opening the samples
* Download and open the [Unity Hub](https://unity.com/unity-hub)
* Click "Add project from disk" and select the root directory of the sample you'd like to open
* If Unity Hub prompts you to install the editor, proceed with the installation

![](~Docs/Images/AddProjectFromDisk.jpg)

# Running the samples
## Preparation
* Open the main scene inside the sample project
* If you don't already have one, create a Normcore account and application as described in [this page](https://normcore.io/documentation/essentials/getting-started#creating-a-normcore-application)
* Copy your app key into the `Realtime` component and save the scene

## Running
* Enter Play mode by pressing the Play button to launch the sample inside the editor
* There's a few options to run multiple instances of the sample for testing multiplayer, ex:
  * Copy-pasting the sample directory and opening it as a separate project in another editor window
  * [Building and deploying](~Docs/Building.md) the sample to PCVR or Meta Quest
  * Using [ParrelSync](https://github.com/VeriorPies/ParrelSync) to open another editor window

# Building the samples
Some platforms (namely XR platforms) require extra configuration steps. Follow the steps in [this page](~Docs/Building.md) for more info.

# Troubleshooting
See [this page](~Docs/Troubleshooting.md) for common questions and answers.
