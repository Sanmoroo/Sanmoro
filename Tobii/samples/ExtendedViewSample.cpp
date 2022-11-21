#pragma once

#include "tobii_gameintegration.h"
#include <iostream>
#include <iomanip>
#include "windows.h"
#include <thread>

using namespace TobiiGameIntegration;

void ExtendedViewSample()
{
    ITobiiGameIntegrationApi* api = GetApi("Extended View Sample");
    IExtendedView* extendedView = api->GetFeatures()->GetExtendedView();

    api->GetTrackerController()->TrackWindow(GetConsoleWindow());

    std::cout << "Right Control: reset head pose, Left Control: hold to pause extended view." << std::endl << std::endl;

    while(!GetAsyncKeyState(VK_ESCAPE))
    {
        api->Update();

        const SHORT onKeyDownCode = (SHORT)0x8001;
        const SHORT isKeyDownCode = (SHORT)0x8000;
        bool resetDefaultHeadPose = GetAsyncKeyState(VK_RCONTROL) == onKeyDownCode;
        bool pause = (GetAsyncKeyState(VK_LCONTROL) & isKeyDownCode) != 0;

        if (resetDefaultHeadPose)
        {
            extendedView->ResetDefaultHeadPose(); // this resets the default head pose, such that the user's current head pose is considered neutral
        }

        pause ? extendedView->Pause(false) : extendedView->UnPause(); // pausing and unpausing extended view via this function smoothly transitions to the new values
        
        const Transformation trans = extendedView->GetTransformation();
     
        std::cout << std::fixed << std::setprecision(3);
        std::cout << "Extended View Rot(deg) [Y: " << trans.Rotation.YawDegrees << ",P: " << trans.Rotation.PitchDegrees << ",R: " << trans.Rotation.RollDegrees << "] " <<
            "Pos(mm) [X: " << trans.Position.X << ",Y: " << trans.Position.Y << ",Z: " << trans.Position.Z << "]          \r";

        Sleep(1000 / 60);
    }

    api->Shutdown();
}
