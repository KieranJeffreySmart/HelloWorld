using Microsoft.Extensions.Configuration;

namespace HelloWorld.AsyncProxy
{
    public class HelloWorldClientSettings: IHelloWorldClientSettings
    {
        public HelloWorldClientSettings(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}