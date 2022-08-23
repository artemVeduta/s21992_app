# GameLiftRealtimeNative

This is the native module for GameLift realtime client SDK. It provides support for DTLS using
[MbedTLS](https://github.com/ARMmbed/mbedtls).

`DTLSConnection` class is based on [node-mbed-dtls-client](https://github.com/particle-iot/node-mbed-dtls-client).
To enable event-driven message receipt, it does not manage a UDP socket. In the SDK, a `UdpClient` is used in conjunction
with this class (`DtlsClient.cs`).

## SWIG
C# wrapper for accessing the native functionality have been generated using [SWIG](http://www.swig.org/):
These files have been further modified to support the RealTime Client SDK use case.

## Build
The Native library can be built by using CMAKE. Below are instructions for use on different systems
### Mac
1. Build the shared library (`.dylib`)
    ```
    cd build
    cmake ..
    make
    ```
1. Configure build system to find the library
    1. If using Unity: Drag, and drop the Native Library into the project. Verify Platform settings are correct, and select `Load on startup`
    1. Using VS for Mac: Set the environment variable `DYLD_LIBRARY_PATH` to include `build` directory (https://www.mono-project.com/docs/advanced/pinvoke/)

### Windows
1. Build the dynamic library (`.dll`)
    
   ```
   cd build
   cmake -G "Visual Studio 15 2017 Win64" ..
   msbuild ALL_BUILD.vcxproj /p:Configuration=Release
   ```
1. Configure build system to find the library
    1. Using Unity: Drag, and drop the Native Library into the project. Verify Platform settings are correct, and select `Load on startup`  

### Android
1. Locate an Android CMake Toolchain file. 
    Android NDK provides a toolchain file that should be used in most cases - https://developer.android.com/ndk/guides/cmake.

1. Build the shared library (`.so`) for the desired ABI. For example:
   ```
   cd build
   cmake -DCMAKE_TOOLCHAIN_FILE={AndroidToolchainFile} -DANDROID_ABI=arm64-v8a ..
   make
   ```
   
1. Configure build system to find the library
    1. Using Unity: Drag, and drop the Native Library into the project. Verify Platform settings are correct, and select `Load on startup`  

### iOS
Building the Native library for iOS is different because iOS requires the library to be formatted as a Framework.
When building the Client SDK for iOS make sure to build the "GameLiftRealtimeClientSdkForFrameworkNet45" solution.

1. Locate an iOS CMAKE toolchain file

    Similarly to the Android case above, an iOS toolchain file can be used to build the Native library for your desired platform.
    One option for an iOS toolchain file can be found here: https://github.com/leetal/ios-cmake

1. Build the iOS framework (`.framework`) for the desired platform:

    1. Generate XCode Project Files
        ``` 
        cd build
        cmake -G Xcode -DBUILD_FRAMEWORK=ON -DCMAKE_TOOLCHAIN_FILE=<pathToToolChain>/ios.toolchain.cmake -DPLATFORM=OS64
        ```
    1. Modify Build Settings as necessary
        1. Open the XCode project generated in the previous step
        1. Change Project Deployment target to the desired IOS Version
        1. Change the Scheme to `ALL_BUILD` and build configuration of the scheme to `Release`
        1. Modify the Signing settings for the framework to be the same as the iOS project.
    1. Build the Framework using XCode

1. Import the framework into the iOS project (Instructions for use with Unity)
    1. After creating the iOS Xcode project through Unity, open it up and drop the .framework file into the Frameworks folder (Add to target UnityFramework).
    1. Navigate to UnityFramework Target Build Phases menu, and validate that the framework is present in the Link Binary with Libraries section there. Change Platform to macOS + iOS if necessary.
    1. Navigate to the Unity-Iphone Target Build Phases menu, and add the framework file to the Copy Files step. Make sure the destination is set to Frameworks, and Code Sign on Copy is checked.

    