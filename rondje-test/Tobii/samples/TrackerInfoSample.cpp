#include "tobii_gameintegration.h"
#include <iostream>
#include "windows.h"
#include <thread>

using namespace TobiiGameIntegration;

void TrackerInfoSample()
{
    ITobiiGameIntegrationApi* api = GetApi("Tracker info sample");

    api->GetTrackerController()->TrackWindow(GetConsoleWindow());
    api->GetTrackerController()->UpdateTrackerInfos();

    const TrackerInfo* trackerInfos;
    int numberOfTrackerInfos;

    api->GetTrackerController()->UpdateTrackerInfos();
    while (!api->GetTrackerController()->GetTrackerInfos(trackerInfos, numberOfTrackerInfos))
    {
        Sleep(100);
    }

    std::cout << std::endl;
    std::cout << "Number of trackers found: " << numberOfTrackerInfos << std::endl;
    
    std::cout << std::endl; 
    std::cout << "Attached trackers when this sample started:" << std::endl;
    for (int i = 0; i < numberOfTrackerInfos; i++)
    {
        if (trackerInfos[i].IsAttached)
        {
            std::cout << trackerInfos[i].Url << ", " << trackerInfos[i].Generation << ", " << trackerInfos[i].ModelName << std::endl;
            StreamFlags cap = trackerInfos[i].Capabilities;
            std::cout << "  Capabilities: " << 
                ((cap & StreamFlags::EyeInfo) != StreamFlags::None ? "EyeInfo " : "") << 
                ((cap & StreamFlags::Foveation) != StreamFlags::None ? "Foveation " : "") <<
                ((cap & StreamFlags::Gaze) != StreamFlags::None ? "Gaze " : "") <<
                ((cap & StreamFlags::GazeOS) != StreamFlags::None ? "GazeOS " : "") <<
                ((cap & StreamFlags::Head) != StreamFlags::None ? "Head " : "") <<
                ((cap & StreamFlags::Presence) != StreamFlags::None ? "Presence " : "") <<
                ((cap & StreamFlags::UnfilteredGaze) != StreamFlags::None ? "UnfilteredGaze " : "") <<
                std::endl;
        }
    }

    std::cout << std::endl;
    std::cout << "Current tracker connection:" << std::endl;
    while (!GetAsyncKeyState(VK_ESCAPE))
    {
        api->Update();

        TrackerInfo trackerInfo;
        if(api->GetTrackerController()->IsConnected() && api->GetTrackerController()->GetTrackerInfo(trackerInfo))
        {
            std::cout << trackerInfo.Url << "\r";
        }
        else
        {
            std::cout << "None                                                                       \r";
        }

        Sleep(1000 / 60);
    }

    api->Shutdown();
}
