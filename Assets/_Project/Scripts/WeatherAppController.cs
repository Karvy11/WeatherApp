using System.Collections;
using UnityEngine;
using CleverTap.SDK.NativeUI; // Using our Package
using WeatherApp.Services;
using UnityEngine.EventSystems;

namespace WeatherApp.Controllers
{
    [RequireComponent(typeof(WeatherService))]
    public class WeatherAppController : MonoBehaviour, IPointerDownHandler
    {
        [Header("Dependencies")]
        public NativeUIManager nativeUIManager; // Reference to the SDK Object
        private WeatherService _weatherService;

        private void Start()
        {
            _weatherService = GetComponent<WeatherService>();

            // Request permission immediately
            Input.location.Start();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Hello");
            OnGameObjectClicked();

        }


        public void OnGameObjectClicked()
        {
            nativeUIManager.ShowMessage("Fetching Location...");
            StartCoroutine(GetLocationAndWeather());
        }
        private IEnumerator GetLocationAndWeather()
        {

            if (!Input.location.isEnabledByUser)
            {
                nativeUIManager.ShowMessage("Location access not enabled.");
                yield break;
            }


            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
            {
                nativeUIManager.ShowMessage("Failed to determine device location.");
                yield break;
            }


            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;

            nativeUIManager.ShowMessage($"Loc: {latitude:0.00}, {longitude:0.00}. Fetching Weather...");


            _weatherService.GetWeather(latitude, longitude,
                onSuccess: (temp) =>
                {
                    string msg = $"Current Max Temp: {temp}°C";
                    Debug.Log(msg);
                    nativeUIManager.ShowMessage(msg);
                },
                onFailure: (error) =>
                {
                    nativeUIManager.ShowMessage($"API Error: {error}");
                }
            );
        }


    }
}