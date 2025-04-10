namespace testing.Steps;

using Microsoft.Playwright;
using TechTalk.SpecFlow;
using Xunit;

[Binding]
public class StartShopping
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;

    [BeforeScenario]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true, SlowMo = 0 });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    [AfterScenario]
    public async Task Teardown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }
    
    // Steps

    [Given(@"I am on the homepage")]
    public async Task GivenIAmOnTheHomepage()
    {
        await _page.GotoAsync("http://localhost:5000/");
    }

    [When(@"I click on Start Shopping")]
    public async Task WhenIClickOnStartShopping()
    {
        await _page.ClickAsync("main>div>button");
    }

    [Then(@"I should see products")]
    public async Task ThenIShouldSeeProducts()
    {
        var name = await _page.InnerTextAsync("#laptop>h4");
        Assert.Equal("Laptop", name);
        name = await _page.InnerTextAsync("#smartphone>h4");
        Assert.Equal("Smartphone", name);
    }
}
