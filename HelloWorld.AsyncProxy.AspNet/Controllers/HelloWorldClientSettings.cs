namespace HelloWorld.AsyncProxy.Framework
{
    public class HelloWorldClientSettings : IHelloWorldClientSettings
    {
        public HelloWorldClientSettings(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}