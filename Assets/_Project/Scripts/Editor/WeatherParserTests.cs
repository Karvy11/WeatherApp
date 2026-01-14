using NUnit.Framework;
using UnityEngine;
using WeatherApp.Models;

public class WeatherParserTests
{
    [Test]
    public void Test_JsonParsing_ReturnsCorrectTemperature()
    {
        // Arrange
        string sampleJson = @"
        {
            ""daily"": {
                ""time"": [""2022-11-29""],
                ""temperature_2m_max"": [32.0]
            }
        }";

        // Act
        WeatherResponse response = JsonUtility.FromJson<WeatherResponse>(sampleJson);

        // Assert
        Assert.IsNotNull(response.daily);
        Assert.AreEqual(1, response.daily.temperature_2m_max.Length);
        Assert.AreEqual(32.0f, response.daily.temperature_2m_max[0]);
    }

    [Test]
    public void Test_JsonParsing_HandlesEmptyData()
    {

        string emptyJson = "{}";


        WeatherResponse response = JsonUtility.FromJson<WeatherResponse>(emptyJson);


        Assert.IsNotNull(response);


        if (response.daily != null)
        {
            bool isTimeEmpty = response.daily.time == null || response.daily.time.Length == 0;
            Assert.IsTrue(isTimeEmpty, "Daily data exists but should be empty.");
        }
        else
        {

            Assert.IsNull(response.daily);
        }
    }
}