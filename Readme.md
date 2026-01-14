# Unity Weather App & Native Toast SDK

### 1. Library (Native SDK)

- **Pattern:** Adapter Pattern / Bridge.
- **Structure:**
  - `INativeUI`: Common interface for UI operations.
  - `AndroidNativeUI`: Uses `AndroidJavaObject` to invoke standard Android `Toast`.
  - `IOSNativeUI`: Uses `[DllImport]` to call a custom Objective-C method in `NativeUI.mm`.
  - `NativeUIManager`: A persistent MonoBehaviour singleton serving as the entry point.

### 2. Weather App

- **Pattern:** MVC (Model-View-Controller).
- **Structure:**
  - **Model:** `WeatherResponse` classes for JSON deserialization.
  - **Service:** `WeatherService` handles `UnityWebRequest` and API callbacks.
  - **Controller:** `WeatherAppController` manages the flow (Input -> Location -> Service -> NativeUI).

## How to Use

### Setup

1.  Open the project in Unity 2021.3+ (LTS).
2.  Switch Build Target to **Android** or **iOS**.
3.  Ensure `Assets/Plugins/iOS/NativeUI.mm` exists (for iOS builds).

### Running the App

1.  Open the scene `MainScene`.
2.  There is a Cube in the center (The "Game Object").
3.  **Click the Cube.**
4.  The app will:
    - Request GPS Permission.
    - Toast the detected coordinates.
    - Fetch weather from `open-meteo.com`.
    - Toast the current max temperature.

## Testing

Unit tests are located in `Assets/Tests`.

1.  Open **Window > General > Test Runner**.
2.  Click **Run All** to verify JSON parsing logic.

## API Reference

- Endpoint: `https://api.open-meteo.com/v1/forecast`
- Params: `latitude`, `longitude`, `daily=temperature_2m_max`, `timezone=IST`
