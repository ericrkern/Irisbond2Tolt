using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IrisbondAPI
{
    #region enums

    /// <summary>
    /// Result of the API initialization returned by start()
    /// </summary>
    public enum START_STATUS { START_OK, NO_CAMERA, CAMERA_IN_USE, CAMERA_ERROR, QT_GUI_ERROR, INVALID_LICENSE, INVALID_LICENSE_NO_INTERNET, HIRU_NEEDS_UPDATE };

    /// <summary>
    /// Result of the calibration process returned by waitForCalibrationToEnd()
    /// </summary>
    public enum CALIBRATION_STATUS { CALIBRATION_TIMEOUT, CALIBRATION_ENDED_ALT_F4, CALIBRATION_ENDED_ESCAPE, CALIBRATION_FINISHED, CALIBRATION_CANCELLED, CALIBRATION_FAILED_NO_CAMERA };

    /// <summary>
    /// Eye codification used by setUserEyeControlMode()
    /// </summary>
    public enum CONTROLLING_EYE { CONTROL_LEFT, CONTROL_RIGHT, CONTROL_ANY, CONTROL_BOTH_ALWAYS };

    /// <summary>
    /// Consumption mode for enumerator
    /// </summary>
    public enum CONSUMPTION_MODE { CONSUMPTION_LOW, CONSUMPTION_NORMAL };

    /// <summary>
    /// Consumption mode for enumerator
    /// </summary>
    public enum POSITIONING_LED_BEHAVIOUR
    {
        ALWAYS_OFF,                 /// Positioning led will always be off (will turn on on startup)
        ALWAYS_ON,                  /// Positioning led will always be on
        TURN_ON_WHEN_DETECTION      /// Positioning led will turn on when there is valid detection of the users eyes
    };

    #endregion


    #region delegates

    /// <summary>
    /// Function to be called when a new image is processed and a new gaze point is received.
    /// </summary>
    /// <param name="timestamp">The timestamp that identifies the current gaze point. In milliseconds, from epoch.</param>
    /// <param name="mouseX">X coordinate of the gaze point. In pixels, where 0 is the left side of the image.</param>
    /// <param name="mouseY">Y coordinate of the gaze point. In pixels, where 0 is the top of the image.</param>
    /// <param name="mouseRawX">X coordinate of the raw gaze point, this means, without any filtering. In pixels. For research only.</param>
    /// <param name="mouseRawY">Y coordinate of the raw gaze point, this means, without any filtering. In pixels. For research only.</param>
    /// <param name="screenWidth">Screen width considered by the API to estimate the gaze point. In pixels.</param>
    /// <param name="screenHeight">Screen height considered by the API to estimate the gaze point. In pixels.</param>
    /// <param name="leftEyeDetected">True when the left eye is detected in the current gaze point. False when it is not.</param>
    /// <param name="rightEyeDetected">True when the right eye is detected in the current gaze point. False when it is not.</param>
    /// <param name="imageWidth">Camera image width. In pixels.</param>
    /// <param name="imageHeight">Camera image height. In pixels.</param>
    /// <param name="leftEyeX">X coordinate of the left eye in the camera image. Normalized between 0 and 1, where 0 is the left side of the image, and 1 is the right side.</param>
    /// <param name="leftEyeY">Y coordinate of the left eye in the camera image. Normalized between 0 and 1, where 0 is the top of the image, and 1 is the bottom side.</param>
    /// <param name="leftEyeSize">Left eye pupil size. In pixels.</param>
    /// <param name="rightEyeX">X coordinate of the right eye in the camera image. Normalized between 0 and 1, where 0 is the left side of the image, and 1 is the right side.</param>
    /// <param name="rightEyeY">Y coordinate of the right eye in the camera image. Normalized between 0 and 1, where 0 is the top of the image, and 1 is the bottom side.</param>
    /// <param name="rightEyeSize">Right eye pupil size. In pixels.</param>
    /// <param name="distanceFactor">Tells the distance between the eye tracker and the user. Between -1 and 1, where -1 means too far, 1 is too close, and 0 is the ideal distance. Recommended [-0.5, 0.5]</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DATA_CALLBACK(
        long timestamp,
        float mouseX,
        float mouseY,
        float mouseRawX,
        float pogRawY,
        int screenWidth,
        int screenHeight,
        [MarshalAs(UnmanagedType.U1)] bool leftEyeDetected,
        [MarshalAs(UnmanagedType.U1)] bool rightEyeDetected,
        int imageWidth,
        int imageHeight,
        float leftEyeX,
        float leftEyeY,
        float leftEyeSize,
        float rightEyeX,
        float rightEyeY,
        float rightEyeSize,
        float distanceFactor);

    /// <summary>
    /// Function to be called when the calibration finishes, and the global calibration results are received.
    /// </summary>
    /// <param name="leftPrecisionError">Left eye precision, in pixels</param>
    /// <param name="leftAccuracyError">Left eye accuracy, in pixels</param>
    /// <param name="rightPrecisionError">Right eye precision, in pixels</param>
    /// <param name="rightAccuracyError">Right eye accuracy, in pixels</param>
    /// <param name="combinedPrecisionError">Both eyes precision, in pixels</param>
    /// <param name="combinedAccuracyError">Both eyes accuracy, in pixels</param>
    /// <param name="cancelled">True when the calibration is cancelled. Results must be ignored when cancelled is true.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CALIBRATION_RESULTS_CALLBACK(
        double leftPrecisionError,
        double leftAccuracyError,
        double rightPrecisionError,
        double rightAccuracyError,
        double combinedPrecisionError,
        double combinedAccuracyError,
        [MarshalAs(UnmanagedType.U1)] bool cancelled);

    /// <summary>
    /// Function to be called when the calibration finishes, and the detailed calibration results (point by point) are received.
    /// </summary>
    /// <param name="nPoints">The number of calibration points</param>
    /// <param name="xCoords">A vector with the X coordinates of the calibration points. Normalized between 0 (left of the screen) and 1 (right of the screen)</param>
    /// <param name="yCoords">A vector with the Y coordinates of the calibration points. Normalized between 0 (top of the screen) and 1 (bottom of the screen)</param>
    /// <param name="leftErrorsPx">A vector with the left eye errors for all the points. In pixels.</param>
    /// <param name="rightErrorsPx">A vector with the right eye errors for all the points. In pixels.</param>
    /// <param name="combinedErrorsPx">A vector with the combined eye errors for all the points. In pixels.</param>
    /// <param name="leftErrorsNorm">A vector with the left eye errors for all the points. Normalized between 0 (perfect point) and 1 (should be improved)</param>
    /// <param name="rightErrorsNorm">A vector with the right eye errors for all the points. Normalized between 0 (perfect point) and 1 (should be improved)</param>
    /// <param name="combinedErrorsNorm">A vector with the combined eye errors for all the points. Normalized between 0 (perfect point) and 1 (should be improved)</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CALIBRATION_RESULTS_POINTS_CALLBACK(
        int nPoints,
        IntPtr xCoords,
        IntPtr yCoords,
        IntPtr leftErrorsPx,
        IntPtr rightErrorsPx,
        IntPtr combinedErrorsPx,
        IntPtr leftErrorsNorm,
        IntPtr rightErrorsNorm,
        IntPtr combinedErrorsNorm);

    /// <summary>
    /// Function to be called when the calibration is cancelled.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CALIBRATION_CANCELLED_CALLBACK();

    /// <summary>
    /// Function to be called during the calibration to know the calibration target position and state
    /// </summary>
    /// <param name="pointX">X coordinate of the target. In pixels.</param>
    /// <param name="pointY">Y coordinate of the target. In pixels.</param>
    /// <param name="screenWidth">Screen width considered by the API to locate the target. In pixels.</param>
    /// <param name="screenHeight">Screen heigth considered by the API to locate the target. In pixels.</param>
    /// <param name="fixated">True when the target is fixated in the point (pointX, pointY), false when it is travelling towards that point.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CALIBRATION_TARGET_CALLBACK(double pointX, double pointY, int screenWidth, int screenHeight, [MarshalAs(UnmanagedType.U1)] bool fixated);

    /// <summary>
    /// Function to be called during the calibration when the user is not detected in one calibration point and decide to retry that point or cancel calibration
    /// </summary>
    /// <returns>True to try again, false to cancel calibration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool CALIBRATION_POINT_ERROR_CALLBACK();

    /// <summary>
    /// Function to be called during the calibration when a new frame is acquired from the camera and the image is received.
    /// </summary>
    /// <param name="data">A pointer to the image raw data</param>
    /// <param name="rows">The number of rows in the image</param>
    /// <param name="cols">The number of columns in the image</param>
    /// <param name="channels">The number of channels in the image. Typically 1.</param>
    /// <param name="timestamp">The timestamp that identifies the current gaze point. In milliseconds, from epoch.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void IMAGE_CALLBACK(IntPtr data, int rows, int cols, int channels, long timestamp);

    /// <summary>
    /// Function to be called during the calibration when a blink is done by the user
    /// </summary>
    /// <param name="mouseX">X coordinate of the blink event. In pixels.</param>
    /// <param name="mouseY">Y coordinate of the blink event. In pixels.</param>
    /// <param name="screenWidth">Screen width considered by the API to locate the target. In pixels.</param>
    /// <param name="screenHeight">Screen heigth considered by the API to locate the target. In pixels.</param>        
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void BLINK_CALLBACK(int x, int y, int screenWidth, int screenHeight);

    /// <summary>
    /// Function to be called during the calibration when a dwell is done by the user
    /// </summary>
    /// <param name="mouseX">X coordinate of the dwell event. In pixels.</param>
    /// <param name="mouseY">Y coordinate of the dwell event. In pixels.</param>
    /// <param name="screenWidth">Screen width considered by the API to locate the target. In pixels.</param>
    /// <param name="screenHeight">Screen heigth considered by the API to locate the target. In pixels.</param>   
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DWELL_CALLBACK(int mouseX, int mouseY, int screenWidth, int screenHeight);

    /// <summary>
    /// Function to be called when the button is pressed in Hiru
    /// </summary>  
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void BUTTON_CALLBACK();

    #endregion


    /// <summary>
    /// The main Irisbond Hiru eye tracker class
    /// For a detailed documentation please go to Doc
    /// </summary>
    interface ITracker
    {
        bool iTrackerIsPresent();
        int iCheckTrackerConnection();
        IntPtr iGetHardwareNumber();
        string iGetHWNumber();
        IntPtr iGetCameraKeyword();
        string iGetHWKeyword();
        int iGetCameraFocusDistance();
        int iImage_streaming(bool stream);
        START_STATUS iStart();
        void iStop();
        void iShowPositioningWindow();
        void iHidePositioningWindow();
        void iStartTesting(int userDistance);
        void iSetTestingRecording(bool record);
        void iSetCalibrationParameters(double travelTime, double fixationTime);
        void iSetCalibrationColors(uint targetColor, uint backgroundColor);
        void iSetTargetSize(int targetSize);
        void iSetLocalTargetImage(int index);
        bool iLoadTargetImage(string pathToImage);
        void iSetLocalTargetAnimation();
        bool iLoadTargetAnimationSpriteSheet(string pathToAnimation, int frames, float loopTime);
        void iStartCalibration(int numCalibPoints);
        void iCancelCalibration();
        void iStartImproveCalibration();
        void iStartCalibrationRectification();
        CALIBRATION_STATUS iWaitForCalibrationToEnd(int timeoutInMinutes);
        void iShowCalibrationGUI(bool show);
        void iShowCalibrationPointErrorGUI(bool show);
        void iShowCalibrationResultsGUI(bool show);
        void iSetStepByStepCalibration(bool step);
        void iResumeCalibration();
        IntPtr iGetDefaultCalibrationChar();
        string iGetDefaultCalibrationString();
        IntPtr iGetCalibrationChar();
        string iGetCalibrationString();
        void iLoadCalibrationChar(string calibrationChar);
        void iLoadDefaultCalibrationChar();
        bool iSetUserEyeControlMode(CONTROLLING_EYE controlMode);
        void iSetSmoothValue(int smooth);
        void iSetBlinkConfiguration(double singleTime, double cancelTime, bool bothEyesRequired);
        void iSetDwellConfiguration(int areaPixels, double time, bool bothEyesRequired);
        void iSetHighPerformanceMode(bool enable);
        bool iIsHighPerformanceMode();
        void iSetLowConsumptionMode(bool enable);
        CONSUMPTION_MODE iGetConsumptionMode();
        void iSetPositioningLedBehaviour(POSITIONING_LED_BEHAVIOUR mode);
        void iSetDataCallback(DATA_CALLBACK theCallback);
        void iSetCalibrationResultsCallback(CALIBRATION_RESULTS_CALLBACK theCallback);
        void iSetCalibrationResultsPointsCallback(CALIBRATION_RESULTS_POINTS_CALLBACK theCallback);
        void iSetCalibrationCancelledCallback(CALIBRATION_CANCELLED_CALLBACK theCallback);
        void iSetCalibrationTargetCallback(CALIBRATION_TARGET_CALLBACK theCallback);
        void iSetCalibrationPointErrorCallback(CALIBRATION_POINT_ERROR_CALLBACK theCallback);
        void iSetImageCallback(IMAGE_CALLBACK theCallback);
        void iSetBlinkCallback(BLINK_CALLBACK theCallback);
        void iSetDwellCallback(DWELL_CALLBACK theCallback);
        void iSetButtonPressedCallback(BUTTON_CALLBACK theCallback);
        bool iIsApplicationEnded();
        bool iSwitchOnLedLights();
        bool iSwitchOffLedLights();
        bool iAreLedLightsOn();
        void iSetLicense(string license);
    }
}

