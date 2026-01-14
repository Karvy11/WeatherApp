using UnityEngine;
using System.Runtime.InteropServices;

namespace CleverTap.SDK.NativeUI
{
    // Interface for cross-platform abstraction
    public interface INativeUI
    {
        void ShowToast(string message);
    }

    // Editor Implementation (for testing in Unity Editor)
    public class EditorNativeUI : INativeUI
    {
        public void ShowToast(string message)
        {
            Debug.Log($"[Mock Toast]: {message}");
        }
    }

    // Android Implementation
    public class AndroidNativeUI : INativeUI
    {
        public void ShowToast(string message)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                // Retrieve the current activity
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                // Create a Toast object on the UI thread
                currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", currentActivity, message, 0); // 0 is LENGTH_SHORT
                    toastObject.Call("show");
                }));
            }
        }
    }

    // iOS Implementation
    public class IOSNativeUI : INativeUI
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _ShowNativeAlert(string message);
#endif

        public void ShowToast(string message)
        {
#if UNITY_IOS && !UNITY_EDITOR
            _ShowNativeAlert(message);
#else
            Debug.Log($"[iOS Simulator]: {message}");
#endif
        }
    }

    // The Manager (The "Game Object" script)
    public class NativeUIManager : MonoBehaviour
    {
        public static NativeUIManager Instance { get; private set; }
        private INativeUI _nativeUI;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            InitializePlatform();
        }

        private void InitializePlatform()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            _nativeUI = new AndroidNativeUI();
#elif UNITY_IOS && !UNITY_EDITOR
            _nativeUI = new IOSNativeUI();
#else
            _nativeUI = new EditorNativeUI();
#endif
        }

        // Public API
        public void ShowMessage(string message)
        {
            _nativeUI.ShowToast(message);
        }
    }
}

