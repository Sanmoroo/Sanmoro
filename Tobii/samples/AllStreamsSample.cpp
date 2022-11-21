#include "tobii_gameintegration.h"
#include <iostream>
#include "windows.h"
#include <thread>

using namespace TobiiGameIntegration;

void AllStreamsSample()
{
    ITobiiGameIntegrationApi* api = GetApi("Test Application");
    IStreamsProvider* streamsProvider = api->GetStreamsProvider();

    api->GetTrackerController()->TrackWindow(GetConsoleWindow());

    while(!GetAsyncKeyState(VK_ESCAPE))
    {
        api->Update();

        GazePoint gazePoint;
        if (streamsProvider->GetLatestGazePoint(gazePoint))
        {
            std::cout << "Gaze point: [" << gazePoint.X << ", " << gazePoint.Y << "]" << std::endl;
        }

        HeadPose headPose;
        if (streamsProvider->GetLatestHeadPose(headPose))
        {
            std::cout << "Head rotation(deg): [" << headPose.Rotation.YawDegrees << ", " << headPose.Rotation.PitchDegrees
                << "," << headPose.Rotation.RollDegrees << "]" << std::endl;
        }

        bool userIsPresent = streamsProvider->IsPresent();
        std::cout << "User Presence: " << userIsPresent << std::endl;

        Sleep(1000 / 60);
    }

    api->Shutdown();
}
