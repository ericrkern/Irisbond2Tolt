![Scott-Morgan Foundation Logo](src/Irisbond2Tolt.Demo/ScottMorganFoundationLogo.png)

# Irisbond2Tolt

**Version:** 1.0.0 (Build 3)

Adds support for the Irisbond Hiro eye tracker to the Tolt Ability Drive system.

## Overview

**Irisbond2Tolt** integrates the Irisbond Hiro eye tracker with the Tolt Ability Drive system, enabling users to control the system using advanced eye-tracking technology. This project aims to improve accessibility and user experience for individuals relying on assistive technology.

## Features

- Seamless integration of Irisbond Hiro eye tracker
- Enhanced accessibility for Tolt Ability Drive users
- Open for community contributions

## Getting Started

### Prerequisites

- [Tolt Ability Drive system](https://www.tolt.com/) installed and configured
- [Irisbond Hiro eye tracker](https://www.irisbond.com/) hardware and drivers
- Python 3.7+ (or specify your language/environment)
- Any other dependencies (list here)

### Installation

1. Clone this repository:
   ```sh
   git clone https://github.com/yourusername/Irisbond2Tolt.git
   cd Irisbond2Tolt
   ```

2. Install dependencies:
   ```sh
   # Example for Python
   pip install -r requirements.txt
   ```

3. Follow the setup instructions in the `docs/` directory for integration details.

### Usage

1. Connect your Irisbond Hiro eye tracker.
2. Launch the Tolt Ability Drive system.
3. Run the integration tool:
   ```sh
   # Example command
   python main.py
   ```
4. Follow on-screen instructions to calibrate and start using the system.

## Documentation

- See the `docs/` directory for detailed integration and API documentation.

## Contributing

Contributions are welcome! Please see `CONTRIBUTING.md` for guidelines.

## Support

For questions or support, open an issue on GitHub or contact the maintainers.

## License

[MIT License](LICENSE) (or your chosen license)

## EasyClick Integration with Tolt Ability Drive

### What is EasyClick?
EasyClick is Irisbond's eye gaze access software for Windows. It allows users to control the computer mouse, click, scroll, and use the keyboard with their eyes, using the Hiru (or DUO) eye tracker. It is designed for both beginner and pro users, offering a toolbar for basic functions and a desktop bar for advanced access.

### Integration Possibilities

1. **Direct API/SDK Integration**
   - Irisbond provides an SDK (in C) for developers, which allows direct access to gaze data and device control. This is the recommended way to integrate eye tracking into custom applications, such as a Tolt Ability Drive interface.
   - The SDK is available for Windows and iPadOS, and can be requested/downloaded from [Irisbond's SDK page](https://www.irisbond.com/en/aac-products/sdk/).

2. **Using EasyClick as a Mouse Emulator**
   - EasyClick acts as a mouse emulator, translating gaze into mouse movements and clicks.
   - If the Tolt Ability Drive software can be controlled via standard mouse input (e.g., virtual buttons, on-screen controls), then EasyClick can be used out-of-the-box to provide eye gaze control for Tolt, with no further integration required.

3. **Advanced Integration (Custom Communication)**
   - For more advanced integration (e.g., sending specific commands to Tolt Ability Drive based on gaze zones, or integrating with Tolt's own API), use the Irisbond SDK to get real-time gaze data and implement custom logic in your application.
   - This allows you to map gaze points or dwell events to specific Tolt Ability Drive commands, rather than relying on mouse emulation.

### Recommended Integration Path
- **For basic use:** Use EasyClick to control the Tolt Ability Drive software if Tolt can be operated with a mouse. No coding required.
- **For custom/advanced integration:** Use the Irisbond SDK to access gaze data and implement a C# interface that translates gaze events into Tolt Ability Drive API calls.

### Example Integration Flow
1. Install EasyClick and Hiru drivers.
2. Calibrate the eye tracker using EasyClick.
3. Launch Tolt Ability Drive software.
4. Operate Tolt using gaze as a mouse, or...
5. (Advanced) Use the Irisbond SDK in your C# app to get gaze data and send commands to Tolt via your custom API interface.

### References
- [Irisbond SDK for Developers](https://www.irisbond.com/en/aac-products/sdk/)
- [EasyClick User Manual (PDF)](https://www.irisbond.com/wp-content/uploads/2024/08/2024-User-Manual-Hiru-ENG.pdf)
- [Irisbond EasyClick FAQ](https://www.irisbond.com/en/frequently-asked-questions/)

## Credits

This project is supported by and gives special thanks to [The Scott-Morgan Foundation](https://www.scottmorganfoundation.org/) for their vision, support, and contributions to assistive technology and inclusive innovation.

[https://www.scottmorganfoundation.org/](https://www.scottmorganfoundation.org/) 