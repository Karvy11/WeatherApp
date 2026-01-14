using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService : MonoBehaviour
    {
        private const string BASE_URL = "https://api.open-meteo.com/v1/forecast";

        public void GetWeather(double lat, double lon, Action<float> onSuccess, Action<string> onFailure)
        {
            string url = $"{BASE_URL}?latitude={lat}&longitude={lon}&timezone=IST&daily=temperature_2m_max";
            StartCoroutine(FetchWeatherRoutine(url, onSuccess, onFailure));
        }

        private IEnumerator FetchWeatherRoutine(string url, Action<float> onSuccess, Action<string> onFailure)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    onFailure?.Invoke(request.error);
                }
                else
                {
                    try
                    {
                        string json = request.downloadHandler.text;
                        WeatherResponse data = JsonUtility.FromJson<WeatherResponse>(json);

                        // Get the first available temperature (today)
                        if (data.daily != null && data.daily.temperature_2m_max.Length > 0)
                        {
                            onSuccess?.Invoke(data.daily.temperature_2m_max[0]);
                        }
                        else
                        {
                            onFailure?.Invoke("No temperature data available.");
                        }
                    }
                    catch (Exception e)
                    {
                        onFailure?.Invoke($"Parse Error: {e.Message}");
                    }
                }
            }
        }
    }
}
