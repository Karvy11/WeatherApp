using System;


namespace WeatherApp.Models
{
    [Serializable]
    public class WeatherResponse
    {
        public double latitude;
        public double longitude;
        public DailyData daily;
    }

    [Serializable]
    public class DailyData
    {
        public string[] time;
        public float[] temperature_2m_max;
    }
}
