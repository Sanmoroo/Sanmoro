#pragma once

#include "tobii_gameintegration.h"
#include <iostream>
#include "windows.h"
#include <thread>
#include <vector>
#include <algorithm>
#include <functional>
#include <string>
#include <iomanip>

using namespace TobiiGameIntegration;

// This class represents a hypothetical in-game setting that the user can control.
struct Slider
{
    std::string m_name;
    const float m_minValue;
    const float m_maxValue;
    float& m_curValue;
    bool m_curValueChanged = false;

    void ChangeValue(bool increase)
    {
        float stepSize = (m_maxValue - m_minValue) / 10.f;
        float newValue = increase ? m_curValue + stepSize : m_curValue - stepSize;
        newValue = std::min<float>(m_maxValue, std::max<float>(m_minValue, newValue));
        if (newValue != m_curValue)
        {
            m_curValueChanged = true;
            m_curValue = newValue;
        }
    }

    void Draw(bool isSelected) const
    {
        std::cout << "[" << (isSelected ? "*" : " ") << "] " << m_name << " : " << m_curValue << "                       " << std::endl;
    }
};

// Forward declaration: This function is responsible for printing a list of settings and also querying user input to select a setting
void PrintExtendedViewSettingsControl(std::vector<Slider>& allSettings, bool forceDraw = false);

void ExtendedViewSettingsSample()
{
    ExtendedViewSettings s;
    
    float gazePlusHeadPitchLimitDegrees = 70.0f;
    float eyeToHeadLimitsRatio = 0.85f;
    float headCenterStabilisation = 0.f;
    float headRotationSensitivity = 1.0f;

    std::vector<Slider> settingsSliders =
    {
        // These are examples of detailed settings sliders, which are bound here directly to single Setting members of the ExtendedViewSettings struct:
        { "GazePitchUpLimitDegrees",
            s.GazePitchUpLimitDegrees.Metadata.MinMaxRange.Min,
            s.GazePitchUpLimitDegrees.Metadata.MinMaxRange.Max,
            s.GazePitchUpLimitDegrees },
        { "HeadPitchUpDegrees.Limit",
            s.HeadPitchUpDegrees.Limit.Metadata.MinMaxRange.Min,
            s.HeadPitchUpDegrees.Limit.Metadata.MinMaxRange.Max,
            s.HeadPitchUpDegrees.Limit },
        { "HeadPitchUpDegrees.SensitivityScaling",
            s.HeadPitchUpDegrees.SensitivityScaling.Metadata.MinMaxRange.Min,
            s.HeadPitchUpDegrees.SensitivityScaling.Metadata.MinMaxRange.Max,
            s.HeadPitchUpDegrees.SensitivityScaling },
        { "HeadPitchUpDegrees.DeadZoneNorm",
            s.HeadPitchUpDegrees.DeadZoneNorm.Metadata.MinMaxRange.Min,
            s.HeadPitchUpDegrees.DeadZoneNorm.Metadata.MinMaxRange.Max,
            s.HeadPitchUpDegrees.DeadZoneNorm },
        { "HeadPitchUpDegrees.SCurveStrengthNorm",
            s.HeadPitchUpDegrees.SCurveStrengthNorm.Metadata.MinMaxRange.Min,
            s.HeadPitchUpDegrees.SCurveStrengthNorm.Metadata.MinMaxRange.Max,
            s.HeadPitchUpDegrees.SCurveStrengthNorm },
        
        { "GazePitchDownLimitDegrees",
            s.GazePitchDownLimitDegrees.Metadata.MinMaxRange.Min,
            s.GazePitchDownLimitDegrees.Metadata.MinMaxRange.Max,
            s.GazePitchDownLimitDegrees },
        { "HeadPitchDownDegrees.Limit",
            s.HeadPitchDownDegrees.Limit.Metadata.MinMaxRange.Min,
            s.HeadPitchDownDegrees.Limit.Metadata.MinMaxRange.Max,
            s.HeadPitchDownDegrees.Limit },
        { "HeadPitchDownDegrees.SensitivityScaling",
            s.HeadPitchDownDegrees.SensitivityScaling.Metadata.MinMaxRange.Min,
            s.HeadPitchDownDegrees.SensitivityScaling.Metadata.MinMaxRange.Max,
            s.HeadPitchDownDegrees.SensitivityScaling },
        { "HeadPitchDownDegrees.DeadZoneNorm",
            s.HeadPitchDownDegrees.DeadZoneNorm.Metadata.MinMaxRange.Min,
            s.HeadPitchDownDegrees.DeadZoneNorm.Metadata.MinMaxRange.Max,
            s.HeadPitchDownDegrees.DeadZoneNorm },
        { "HeadPitchDownDegrees.SCurveStrengthNorm",
            s.HeadPitchDownDegrees.SCurveStrengthNorm.Metadata.MinMaxRange.Min,
            s.HeadPitchDownDegrees.SCurveStrengthNorm.Metadata.MinMaxRange.Max,
            s.HeadPitchDownDegrees.SCurveStrengthNorm },

        // These are examples of compund settings sliders, which are bound here to local float variables that will be used to set multiple members of the ExtendedViewSettings struct via helper member-functions
        { "GazePlusHeadPitchLimitDegrees",
            0.0f,
            90.0f,
            gazePlusHeadPitchLimitDegrees },
        { "EyeToHeadLimitsRatio",
            0.0f,
            1.0f,
            eyeToHeadLimitsRatio },
        { "HeadCenterStabilisation",
            0.0f,
            1.0f,
            headCenterStabilisation },
        { "HeadRotationSensitivity",
            0.0f,
            5.0f,
            headRotationSensitivity }
    };

    ITobiiGameIntegrationApi* api = GetApi("Extended View Settings Sample");
    IExtendedView* extendedView = api->GetFeatures()->GetExtendedView();

    api->GetTrackerController()->TrackWindow(GetConsoleWindow());

    system("cls");
    std::cout << std::fixed << std::setprecision(3);
    PrintExtendedViewSettingsControl(settingsSliders, true); // true = force printing of settings even though no input changed

    while ((GetAsyncKeyState(VK_ESCAPE) & 0x8000) == 0)
    {
        api->Update();
        extendedView->GetSettings(s);

        PrintExtendedViewSettingsControl(settingsSliders);

        bool anySettingChanged = false;
        for (auto& slider : settingsSliders)
        {
            if (slider.m_curValueChanged)
            {
                slider.m_curValueChanged = false;
                anySettingChanged = true;

                // We handle changes to the compound settings Sliders by calling helper-functions which set multiple ExtendedViewSettings members
                // Note that these helpers-functions overwrite some of the individual Setting members which are used in the detailed settings above for illustrative purposes
                if (slider.m_name == "GazePlusHeadPitchLimitDegrees")
                {
                    s.SetCameraMaxAnglePitchUp(slider.m_curValue);
                    s.SetCameraMaxAnglePitchDown(-slider.m_curValue); // Pitch down is negative
                }
                else if (slider.m_name == "EyeToHeadLimitsRatio")
                {
                    s.SetEyeHeadTrackingRatio(slider.m_curValue);
                }
                else if (slider.m_name == "HeadCenterStabilisation")
                {
                    s.SetCenterStabilization(slider.m_curValue);
                }
                else if (slider.m_name == "HeadRotationSensitivity")
                {
                    s.SetHeadAllRotationAxisSettingsSensitivity(slider.m_curValue);
                }
            }
        }
        if (anySettingChanged)
        {
            extendedView->UpdateSettings(s);
            PrintExtendedViewSettingsControl(settingsSliders, true);
        }

        const Transformation extendedViewTransformation = extendedView->GetTransformation();
        std::cout << "Extended View (deg): [ Yaw: " << extendedViewTransformation.Rotation.YawDegrees << " , Pitch: " << extendedViewTransformation.Rotation.PitchDegrees << " ]" << "               \r";

        Sleep(1000 / 60);
    }

    api->Shutdown();
}


void PrintExtendedViewSettingsControl(std::vector<Slider>& allSettings, bool forceDraw /* = false */)
{
    const SHORT onKeyDownCode = (SHORT)0x8001;
    bool nextSettingPressed = GetAsyncKeyState(VK_DOWN) == onKeyDownCode;
    bool prevSettingPressed = GetAsyncKeyState(VK_UP) == onKeyDownCode;
    bool increasePressed = GetAsyncKeyState(VK_RIGHT) == onKeyDownCode;
    bool decreasePressed = GetAsyncKeyState(VK_LEFT) == onKeyDownCode;

    static bool useAdvancedSettingsMode = false;
    static int selectedSettingNum = 0;

    if (forceDraw || nextSettingPressed || prevSettingPressed || increasePressed || decreasePressed)
    {
        COORD coord = { 0 };
        HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleCursorPosition(h, coord);
        std::cout << "Arrows up/down: previous/next setting. Arrows left/right: decrease/increase setting." << std::endl << std::endl;

        selectedSettingNum += nextSettingPressed ? 1 : (prevSettingPressed && selectedSettingNum > 0 ? -1 : 0);

        const int numSettings = (int)allSettings.size();
        selectedSettingNum = std::min<int>(selectedSettingNum, numSettings - 1);

        if (increasePressed || decreasePressed)
        {
            allSettings[selectedSettingNum].ChangeValue(increasePressed);
        }
        for (int i = 0; i < numSettings; i++)
        {
            allSettings[i].Draw(i == selectedSettingNum);
        }
    }
}