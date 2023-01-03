#include "tobii_gameintegration.h"
#include "tobii_gameintegration_internal.h"
#include <iostream>
#include "windows.h"
#include <thread>

using namespace TobiiGameIntegration;

enum EyetrackingFeature
{
    ExtendedView = 1, // Actual number doesn't matter. It is not logged anywhere
    AimAtGaze = 2,
    HackAtGaze = 3
};

void StatisticsSample()
{
    ITobiiGameIntegrationApi* api = GetApi("Statistics Sample Application");
    IStatistics* statistics = api->GetStatistics();

    // Starting tracking is needed for statistics logging
    api->GetTrackerController()->TrackWindow(GetConsoleWindow());

    // Create a list of all features with names
    Feature features[3] = {
        // For known features like ExtendedView use predefined Literals with GetLiteral
        { EyetrackingFeature::ExtendedView, statistics->GetLiteral(Literal::ExtendedView), true },
        { EyetrackingFeature::AimAtGaze, statistics->GetLiteral(Literal::AimAtGaze), true },
        { EyetrackingFeature::HackAtGaze, "Hack At Gaze", true } // Come up with the names for the specific features
    };

    // Set the list of the features to statistics
    statistics->SetFeatureList(features, 3);

    // Recorded initial state of features. Do it only once in the beginning.
    statistics->SendFeaturesState();

    std::cout << "Press Space to toggle settings" << std::endl;

    bool hackAtGazeEnabled = false; // Bind to the in-game settings
    bool dirtySettings = false;     // Set to true when settings are updated
    while (!GetAsyncKeyState(VK_ESCAPE))
    {
        api->Update();

        // Emulate settings toggle by pressing space
        if (GetKeyState(VK_SPACE) & 0x8000)
        {
            hackAtGazeEnabled = !hackAtGazeEnabled;
            dirtySettings = true;
        }

        // If user turned on hack at gaze in the settings
        if (dirtySettings && hackAtGazeEnabled)
        {
            statistics->SendFeatureEnabled(EyetrackingFeature::HackAtGaze);
            dirtySettings = false;
        }

        // If user turned off hack at gaze in the settings
        if (dirtySettings && !hackAtGazeEnabled)
        {
            statistics->SendFeatureDisabled(EyetrackingFeature::HackAtGaze);
            dirtySettings = false;
        }
        
        Sleep(1000 / 60);
    }

    api->Shutdown();
}
