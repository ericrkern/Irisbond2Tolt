using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IrisbondAPI
{
    /// <summary>
    /// The main Irisbond Hiru eye tracker class, imports functions from IrisbondHiruAPI.dll
    /// ITracker inheritance is recommended, in order to use Irisbond Duo and Irisbond Hiru with the same implementation.
    /// Use "Imported Functions" for IrisbondHiru class, and "Interface Functions" for ITracker.
    /// For a detailed documentation please go to Doc
    /// </summary>
    class IrisbondHiru: ITracker
    {
        #region Imported Functions

        /// <summary>
        /// Check that there is an eye tracker connected to the computer.
        /// </summary>
        /// <returns>True if the eye tracker is connected. False if the eye tracker is not connected or the drivers are not installed</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool trackerIsPresent();

        /// <summary>
        /// Check that there is network communication with the eye tracker (only required for Hiru).
        /// </summary>
        /// <returns>0 if the eye tracker is reachable, -1 if the eye tracker is not connected, 1 or higher if the tracker has a connection problem</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int checkTrackerConnection();


        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getHardwareNumber();

        /// <summary>
        /// Get the eye tracker serial number in the format IRIS12345678. 
        /// This ID is unique for each eye tracker. 
        /// </summary>
        /// <returns>the hardware serial number</returns>
        public static string getHWNumber() {
            string s = Marshal.PtrToStringAnsi(getHardwareNumber());
            return s;
        }


        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getCameraKeyword();

        /// <summary>
        /// Get the eye tracker model.
        /// This ID is common for all eye trackers of the same type (i.e. Primma, Duo, but not Hiru).
        /// This is only required for troubleshooting
        /// </summary>
        /// <returns>the hardware model</returns>
        public static string getHWKeyword() {
            string s = Marshal.PtrToStringAnsi(getCameraKeyword());
            return s;
        }

        /// <summary>
        /// Get the eye tracker focus distance in centimeters.
        /// </summary>
        /// <returns>the optimal distance between the tracker and the user for the best experience</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getCameraFocusDistance();

        /// <summary>
        /// Tell the tracker to send images or not.
        /// </summary>
        /// <returns>Error code for message successfully sent. 0 if the message was delivered well.</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int image_streaming(bool stream);


        /// <summary>
        /// Start the API: Start the eye tracker and the processing loop
        /// </summary>
        /// <returns>START_STATUS code that tells whether start process is correct or there is some error</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern START_STATUS start();

        /// <summary>
        /// Stop the API: Stop the eye tracker and the processing loop
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void stop();





        /// <summary>
        /// Show a fullscreen window that helps the user know the optimal location for the best tracking experience.
        /// It can be closed by clicking/tapping of by calling hidePositioningWindow()
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void showPositioningWindow();

        /// <summary>
        /// Close the positioning window.
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void hidePositioningWindow();

        /// <summary>
        /// Close the positioning window.
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setPositioningLedBehaviour(POSITIONING_LED_BEHAVIOUR mode);





        /// <summary>
        /// Start a testing process with 36 calibration points
        /// waitForCalibrationToEnd() must be called immediately after this function.
        /// </summary>
        /// <param name="userDistance">Accurate distance from the user to the screen in cm.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void startTesting(int userDistance);

        /// <summary>
        /// Enable or disable the step-by-step calibration mode.
        /// </summary>
        /// <param name="record">True when you want to perform step-by-step calibration, false when you want a calibration without any pause. False by default.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTestingRecording(bool record);





        /// <summary>
        /// Configure the calibration target travel and fixation time.
        /// </summary>
        /// <param name="travelTime">Time that takes the target to go from one calibration point to another one. In seconds. Default value is 2s</param>
        /// <param name="fixationTime">Time that takes the target to stay fixed in one calibration point. In seconds. Default value is 1s</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCalibrationParameters(double travelTime, double fixationTime);

        /// <summary>
        /// Configure the calibration target and background color.
        /// </summary>
        /// <param name="targetColor">The color of the target in 32 bit unsigned integer format (uint32_t), RGBA channels.</param>
        /// <param name="backgroundColor">The color of the background in 32 bit unsigned integer format (uint32_t), RGBA channels.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCalibrationColors(uint targetColor, uint backgroundColor);

        /// <summary>
        /// Configure the calibration target size.
        /// </summary>
        /// <param name="targetSize">The size of the target in a range of 1 to 100. The default size value of 25 corresponds to 3% of the screen height.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTargetSize(int targetSize);

        /// <summary>
        /// Configure the calibration GUI changing the image that will be shown as the target.
        /// </summary>
        /// <param name="index">Absolut path to the image.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLocalTargetImage(int index);

        /// <summary>
        /// Load a .png image to be shown as the target. If the image is not found, the standard target will be shown.
        /// </summary>
        /// <param name="pathToImage">Absolut path to the image.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool loadTargetImage(string pathToImage);

        /// <summary>
        /// Configure the calibration GUI displaying the walking duck animation.
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLocalTargetAnimation();

        /// <summary>
        /// Load a .png image to be shown as the target. If the image is not found, the standard target will be shown.
        /// </summary>
        /// <param name="pathToAnimation">Absolut path to the animation.</param>
        /// <param name="frames">Total number of frames.</param>
        /// <param name="loopTime">The time it should take the animation to complete a loop in seconds.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool loadTargetAnimationSpriteSheet(string pathToAnimation, int frames, float loopTime);

        /// <summary>
        /// Start a calibration process to adapt the eye tracker to the user's eyes.
        /// waitForCalibrationToEnd() must be called immediately after this function.
        /// </summary>
        /// <param name="numCalibPoints">Number of calibration points. Can be 2, 3, 5, 9 or 16.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void startCalibration(int numCalibPoints);

        /// <summary>
        /// Cancel the calibration process
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void cancelCalibration();

        /// <summary>
        /// Start a 3-point calibration rectification to improve the worst 3 calibration points
        /// waitForCalibrationToEnd() must be called immediately after this function.
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void startImproveCalibration();

        /// <summary>
        /// Start a 1-point calibration rectification to improve the calibration
        /// waitForCalibrationToEnd() must be called immediately after this function.
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void startCalibrationRectification();

        /// <summary>
        /// Wait during the calibration processes until they finish
        /// </summary>
        /// <param name="timeoutInMinutes">Time to force the calibration process to end in case the user can't do it properly. In minutes.</param>
        /// <returns>CALIBRATION_STATUS Code to know the reason why the calibration ends</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CALIBRATION_STATUS waitForCalibrationToEnd(int timeoutInMinutes);

        /// <summary>
        /// Enable or disable the API calibration GUI
        /// </summary>
        /// <param name="show">True when you want to show the API calibration GUI, false when you want to show your own calibration GUI. False by default.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void showCalibrationGUI(bool show);

        /// <summary>
        /// Enable or disable the API calibration error GUI when the user detection fails
        /// </summary>
        /// <param name="show">True when you want to show the API error GUI, false when you want to show your own error GUI. False by default.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void showCalibrationPointErrorGUI(bool show);

        /// <summary>
        /// Enable or disable the API results GUI when the calibration ends
        /// </summary>
        /// <param name="show">True when you want to show the API results GUI, false when you want to show your own results GUI. False by default.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void showCalibrationResultsGUI(bool show);

        /// <summary>
        /// Enable or disable the step-by-step calibration mode.
        /// </summary>
        /// <param name="step">True when you want to perform step-by-step calibration, false when you want a calibration without any pause. False by default.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setStepByStepCalibration(bool step);

        /// <summary>
        /// Start recording on a paused fixed point on step-by-step calibration mode. Then, it will automatically advance to the next point.
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void resumeCalibration();
        

        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getDefaultCalibrationChar();
        /// <summary>
        /// Get default calibration string for your screen.
        /// </summary>
        /// <returns> the default calibration string.</returns>
        public static string getDefaultCalibrationString()
        {
            string s = Marshal.PtrToStringAnsi(getDefaultCalibrationChar());
            return s;
        }

        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getCalibrationChar();
        /// <summary>
        /// Get current calibration string. If no calibration has been performed, the default one will be returned.
        /// </summary>
        /// <returns> the current calibration string.</returns>
        public static string getCalibrationString()
        {
            string s = Marshal.PtrToStringAnsi(getCalibrationChar());
            return s;
        }

        /// <summary>
        /// Load a calibration string to the system. This will overwrite the calibration data and will automatically reset the calibration.
        /// </summary>
        /// <param name="calibrationString">A string that contains the data for the calibration. This string can be obtained using either getCalibrationString() or getDefaultCalibrationString() functions.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void loadCalibrationChar(string calibrationChar);

        /// <summary>
        /// Load a calibration string to the system. This will overwrite the calibration data and will automatically reset the calibration.
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void loadDefaultCalibrationChar();





        /// <summary>
        /// Configure the eye(s) to be taken into account for the eye tracker. It can be either eyes or both
        /// </summary>
        /// <param name="controlMode">The controlling eye in CONTROLLING_EYE format. Default value is CONTROLLING_EYE::CONTROL_ANY.</param>
        /// <returns>Deprecated, always true.</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool setUserEyeControlMode(CONTROLLING_EYE controlMode);

        /// <summary>
        /// Configure the smoothness of the mouse cursor. Low smoothing implies high speed, and viceversa.
        /// </summary>
        /// <param name="smooth">Smoothing amount. Can take values between 0 and 14, where 0 means a fast mouse, and 14 a very slow but steady mouse. Default value is 2.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setSmoothValue(int smooth);

        /// <summary>
        /// Configure the blink detector. A blink is detected when one or two eyes are closed in a time interval defined between a minimmum and a maximum blink times
        /// </summary>
        /// <param name="singleTime">The minimum time to be considered a blink. In seconds. Default value is 0.3s.</param>
        /// <param name="cancelTime">The maximum time to be considered a blink. In seconds. Default value is 1.0s.</param>
        /// <param name="bothEyesRequired">True when the blink must be done by the two eyes at the same time. False when only one eye is enough. False by default.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setBlinkConfiguration(double singleTime, double cancelTime, bool bothEyesRequired);

        /// <summary>
        /// Configure the dwell detector. A dwell is detected when the mouse is located within an area for a specific time.
        /// </summary>
        /// <param name="areaPixels">The distance the mouse can move within a fixation. In pixels. Default value is 30px.</param>
        /// <param name="time">The minumum time to be considered a fixation. In seconds. Default value is 1.0s.</param>
        /// <param name="bothEyesRequired">True when the dwell must be done by the two eyes at the same time. False when only one eye is enough. False by default.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setDwellConfiguration(int areaPixels, double time, bool bothEyesRequired);





        /// <summary>
        /// Configure the tracker to run in high performance mode.
        /// </summary>
        /// <param name="enable">True to activate high performance mode, false to disable it. True by default</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setHighPerformanceMode(bool enable);

        /// <summary>
        /// Get the tracker high performance status
        /// </summary>
        /// <returns>True when the API is in high performance mode. False when it is not.</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool isHighPerformanceMode();





        /// <summary>
        /// Set the tracker consumption mode
        /// </summary>
        /// <param name="enable">Set true for low consumption mode, and false for high performance mode.</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLowConsumptionMode(bool enable);
        
        /// <summary>
        /// Get the tracker consumption mode
        /// </summary>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CONSUMPTION_MODE getConsumptionMode();





        /// <summary>
        /// Set the callback to be called when a new image is processed and a new gaze point is received.
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setDataCallback(DATA_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called when the calibration finishes, and the global calibration results are received.
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCalibrationResultsCallback(CALIBRATION_RESULTS_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called when the calibration finishes, and the detailed calibration results (point by point) are received.
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCalibrationResultsPointsCallback(CALIBRATION_RESULTS_POINTS_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called when the calibration is cancelled.
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCalibrationCancelledCallback(CALIBRATION_CANCELLED_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called during the calibration to know the calibration target position and state
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCalibrationTargetCallback(CALIBRATION_TARGET_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called during the calibration when the user is not detected in one calibration point 
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCalibrationPointErrorCallback(CALIBRATION_POINT_ERROR_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called when a new frame is acquired from the camera and the image is received.
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setImageCallback(IMAGE_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called when a blink is done by the user
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setBlinkCallback(BLINK_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called when a dwell is done by the user
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setDwellCallback(DWELL_CALLBACK theCallback);

        /// <summary>
        /// Set the callback to be called when the button is pressed in Hiru
        /// </summary>
        /// <param name="theCallback">The pointer to the function to be executed</param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setButtonPressedCallback(BUTTON_CALLBACK theCallback);





        /// <summary>
        /// Check that the API ends after calling stop()
        /// </summary>
        /// <returns>True when the API stops properly. False when the API is still stopping.</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool isApplicationEnded();

        /// <summary>
        /// Switch on the eye tracker leds
        /// </summary>
        /// <returns>True when the action is done properly. False when it's not.</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool switchOnLedLights();

        /// <summary>
        /// Switch off the eye tracker leds
        /// </summary>
        /// <returns>True when the action is done properly. False when it's not.</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool switchOffLedLights();

        /// <summary>
        /// Check the eye tracker leds status
        /// </summary>
        /// <returns>True when the leds are on, false when the leds are off.</returns>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool areLedLightsOn();

        /// <summary>
        /// Set the license code when the license system is not based on an activation file
        /// </summary>
        /// <param name="license">license code </param>
        [DllImport("IrisbondHiruAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLicense(string license);

        #endregion

        #region Interface functions

        public bool iTrackerIsPresent()
        {
            return trackerIsPresent();
        }
        public int iCheckTrackerConnection()
        {
            return checkTrackerConnection();
        }
        public IntPtr iGetHardwareNumber()
        {
            return getHardwareNumber();
        }
        public string iGetHWNumber()
        {
            return getHWNumber();
        }
        public IntPtr iGetCameraKeyword()
        {
            return getCameraKeyword();
        }
        public string iGetHWKeyword()
        {
            return getHWKeyword();
        }
        public int iGetCameraFocusDistance()
        {
            return getCameraFocusDistance();
        }
        public int iImage_streaming(bool stream)
        {
            return image_streaming(stream);
        }
        public START_STATUS iStart()
        {
            return start();
        }
        public void iStop()
        {
            stop();
        }
        public void iShowPositioningWindow()
        {
            showPositioningWindow();
        }
        public void iHidePositioningWindow()
        {
            hidePositioningWindow();
        }
        public void iStartTesting(int userDistance)
        {
            startTesting(userDistance);
        }
        public void iSetTestingRecording(bool record)
        {
            setTestingRecording(record);
        }
        public void iSetCalibrationParameters(double travelTime, double fixationTime)
        {
            setCalibrationParameters(travelTime, fixationTime);
        }
        public void iSetCalibrationColors(uint targetColor, uint backgroundColor)
        {
            setCalibrationColors(targetColor, backgroundColor);
        }
        public void iSetTargetSize(int targetSize)
        {
            setTargetSize(targetSize);
        }
        public void iSetLocalTargetImage(int index)
        {
            setLocalTargetImage(index);
        }
        public bool iLoadTargetImage(string pathToImage)
        {
            return loadTargetImage(pathToImage);
        }
        public void iSetLocalTargetAnimation()
        {
            setLocalTargetAnimation();
        }
        public bool iLoadTargetAnimationSpriteSheet(string pathToAnimation, int frames, float loopTime)
        {
            return loadTargetAnimationSpriteSheet(pathToAnimation, frames, loopTime);
        }
        public void iStartCalibration(int numCalibPoints)
        {
            startCalibration(numCalibPoints);
        }
        public void iCancelCalibration()
        {
            cancelCalibration();
        }
        public void iStartImproveCalibration()
        {
            startImproveCalibration();
        }
        public void iStartCalibrationRectification()
        {
            startCalibrationRectification();
        }
        public CALIBRATION_STATUS iWaitForCalibrationToEnd(int timeoutInMinutes)
        {
            return waitForCalibrationToEnd(timeoutInMinutes);
        }
        public void iShowCalibrationGUI(bool show)
        {
            showCalibrationGUI(show);
        }
        public void iShowCalibrationPointErrorGUI(bool show)
        {
            showCalibrationPointErrorGUI(show);
        }
        public void iShowCalibrationResultsGUI(bool show)
        {
            showCalibrationResultsGUI(show);
        }
        public void iSetStepByStepCalibration(bool step)
        {
            setStepByStepCalibration(step);
        }
        public void iResumeCalibration()
        {
            resumeCalibration();
        }
        public IntPtr iGetDefaultCalibrationChar()
        {
            return getDefaultCalibrationChar();
        }
        public string iGetDefaultCalibrationString()
        {
            return getDefaultCalibrationString();
        }
        public IntPtr iGetCalibrationChar()
        {
            return getCalibrationChar();
        }
        public string iGetCalibrationString()
        {
            return getCalibrationString();
        }
        public void iLoadCalibrationChar(string calibrationChar)
        {
            loadCalibrationChar(calibrationChar);
        }
        public void iLoadDefaultCalibrationChar()
        {
            loadDefaultCalibrationChar();
        }
        public bool iSetUserEyeControlMode(CONTROLLING_EYE controlMode)
        {
            return setUserEyeControlMode(controlMode);
        }
        public void iSetSmoothValue(int smooth)
        {
            setSmoothValue(smooth);
        }
        public void iSetBlinkConfiguration(double singleTime, double cancelTime, bool bothEyesRequired)
        {
            setBlinkConfiguration(singleTime, cancelTime, bothEyesRequired);
        }
        public void iSetDwellConfiguration(int areaPixels, double time, bool bothEyesRequired)
        {
            setDwellConfiguration(areaPixels, time, bothEyesRequired);
        }
        public void iSetHighPerformanceMode(bool enable)
        {
            setHighPerformanceMode(enable);
        }
        public bool iIsHighPerformanceMode()
        {
            return isHighPerformanceMode();
        }
        public void iSetLowConsumptionMode(bool enable)
        {
            setLowConsumptionMode(enable);
        }
        public CONSUMPTION_MODE iGetConsumptionMode()
        {
            return getConsumptionMode();
        }
        public void iSetPositioningLedBehaviour(POSITIONING_LED_BEHAVIOUR mode)
        {
            setPositioningLedBehaviour(mode);
        }
        public void iSetDataCallback(DATA_CALLBACK theCallback)
        {
            setDataCallback(theCallback);
        }
        public void iSetCalibrationResultsCallback(CALIBRATION_RESULTS_CALLBACK theCallback)
        {
            setCalibrationResultsCallback(theCallback);
        }
        public void iSetCalibrationResultsPointsCallback(CALIBRATION_RESULTS_POINTS_CALLBACK theCallback)
        {
            setCalibrationResultsPointsCallback(theCallback);
        }
        public void iSetCalibrationCancelledCallback(CALIBRATION_CANCELLED_CALLBACK theCallback)
        {
            setCalibrationCancelledCallback(theCallback);
        }
        public void iSetCalibrationTargetCallback(CALIBRATION_TARGET_CALLBACK theCallback)
        {
            setCalibrationTargetCallback(theCallback);
        }
        public void iSetCalibrationPointErrorCallback(CALIBRATION_POINT_ERROR_CALLBACK theCallback)
        {
            setCalibrationPointErrorCallback(theCallback);
        }
        public void iSetImageCallback(IMAGE_CALLBACK theCallback)
        {
            setImageCallback(theCallback);
        }
        public void iSetBlinkCallback(BLINK_CALLBACK theCallback)
        {
            setBlinkCallback(theCallback);
        }
        public void iSetDwellCallback(DWELL_CALLBACK theCallback)
        {
            setDwellCallback(theCallback);
        }
        public void iSetButtonPressedCallback(BUTTON_CALLBACK theCallback)
        {
            setButtonPressedCallback(theCallback);
        }
        public bool iIsApplicationEnded()
        {
            return isApplicationEnded();
        }
        public bool iSwitchOnLedLights()
        {
            return switchOnLedLights();
        }
        public bool iSwitchOffLedLights()
        {
            return switchOffLedLights();
        }
        public bool iAreLedLightsOn()
        {
            return areLedLightsOn();
        }
        public void iSetLicense(string license)
        {
            setLicense(license);
        }

        #endregion
    }
}
