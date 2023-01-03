#include "tobii_gameintegration_internal.h"
#include <iostream>
#include "windows.h"

using namespace TobiiGameIntegration;

void HeadMountedDisplaySample()
{
    ITobiiGameIntegrationApi* api = GetApi("Head mounted display sample");
    IStreamsProvider* streamsProvider = api->GetStreamsProvider();

    api->GetTrackerController()->TrackHMD();

    while (!GetAsyncKeyState(VK_ESCAPE))
    {
        api->Update();

        HMDGaze hmdGaze;
        if (streamsProvider->GetLatestHMDGaze(hmdGaze))
        {
            std::cout << "////////////////////////////////////////////////////////////////////" << std::endl << std::endl;
            std::cout << "Validity " << static_cast<int>(hmdGaze.Validity) << std::endl;
            //std::cout << "Counter " << hmdGaze.Counter << std::endl;
            //std::cout << "LedMode " << hmdGaze.LedMode << std::endl;
            std::cout << "Timestamp " << hmdGaze.Timestamp << std::endl;

            std::cout << "LeftEye.EyeOpenness " << hmdGaze.LeftEyeInfo.EyeOpenness << std::endl;
            std::cout << "LeftEye.Direction " << hmdGaze.LeftEyeInfo.GazeDirection.X << ", " << hmdGaze.LeftEyeInfo.GazeDirection.Y << ", " << hmdGaze.LeftEyeInfo.GazeDirection.Z << std::endl;
            std::cout << "LeftEye.GazeOriginMM " << hmdGaze.LeftEyeInfo.GazeOriginMM.X << ", " << hmdGaze.LeftEyeInfo.GazeOriginMM.Y << ", " << hmdGaze.LeftEyeInfo.GazeOriginMM.Z << std::endl;
            std::cout << "LeftEye.PupilPosition " << hmdGaze.LeftEyeInfo.PupilPosition.X << ", " << hmdGaze.LeftEyeInfo.PupilPosition.Y << std::endl;

            std::cout << "RightEye.EyeOpenness " << hmdGaze.RightEyeInfo.EyeOpenness << std::endl;
            std::cout << "RightEye.Direction " << hmdGaze.RightEyeInfo.GazeDirection.X << ", " << hmdGaze.RightEyeInfo.GazeDirection.Y << ", " << hmdGaze.RightEyeInfo.GazeDirection.Z << std::endl;
            std::cout << "RightEye.GazeOriginMM " << hmdGaze.RightEyeInfo.GazeOriginMM.X << ", " << hmdGaze.RightEyeInfo.GazeOriginMM.Y << ", " << hmdGaze.RightEyeInfo.GazeOriginMM.Z << std::endl;
            std::cout << "RightEye.PupilPosition " << hmdGaze.RightEyeInfo.PupilPosition.X << ", " << hmdGaze.RightEyeInfo.PupilPosition.Y << std::endl;

            std::cout << "Timestamp" << hmdGaze.Timestamp << std::endl;
        }

        Sleep(1000 / 60);
    }

    api->Shutdown();
}
