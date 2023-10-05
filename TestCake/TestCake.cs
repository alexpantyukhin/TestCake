using System.Configuration;

namespace TestCake;

public class TestCake
{
    public static ConnectionStringSettings GetMyConfiguration(string key)
    {
        return ConfigurationManager.ConnectionStrings[key];
    }
}