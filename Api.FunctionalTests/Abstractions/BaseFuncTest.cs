namespace Api.FunctionalTests.Abstractions;

public class BaseFuncTest : IClassFixture<FunctionalTestFactory>
{
    protected HttpClient HttpClient { get; init; }
    public BaseFuncTest(FunctionalTestFactory factory)
    {
        HttpClient = factory.CreateClient();
    }
}