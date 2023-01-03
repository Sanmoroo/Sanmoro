#include <iostream>

void GazeSample();
void AllStreamsSample();
void ExtendedViewSample();
void ExtendedViewSettingsSample();
void TrackerInfoSample();
void FeatureEnabledSettingsSample();
void HeadMountedDisplaySample();
void StatisticsSample();

int main()
{
    std::cout << "Select which sample to run:" << std::endl;
    std::cout << "1: Gaze only sample" << std::endl;
    std::cout << "2: Head mounted display sample" << std::endl;
    std::cout << "3: All streams sample" << std::endl;
    std::cout << "4: Extended View sample" << std::endl;
    std::cout << "5: Extended View settings sample" << std::endl;
    std::cout << "6: Tracker info sample" << std::endl;
    std::cout << "7: Statistics logging" << std::endl;
    std::cout << "Press escape to exit samples" << std::endl;

    int answer;
    std::cin >> answer;

    switch (answer)
    {
    case 1:
        GazeSample();
        break;
    case 2:
        HeadMountedDisplaySample();
        break;
    case 3:
        AllStreamsSample();
        break;
    case 4:
        ExtendedViewSample();
        break;
    case 5:
        ExtendedViewSettingsSample();
        break;
    case 6:
        TrackerInfoSample();
        break;
    case 7:
        StatisticsSample();
        break;
    }

    return 0;
}
