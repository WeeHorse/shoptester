using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests;

[TestClass]
public class ExampleTest : PageTest
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;

    [TestInitialize]
    public async Task Setup()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 1000 });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [TestMethod]
    public async Task HasTitle()
    {
        await _page.GotoAsync("https://playwright.dev");

        // Expect a title "to contain" a substring.
        await Expect(_page).ToHaveTitleAsync(new Regex("Playwright"));
    }

    [TestMethod]
    public async Task GetShopLink()
    {
        await _page.GotoAsync("http://localhost:5000/");

        // Click the shop link in nav.
        await _page.GetByRole(AriaRole.Link, new() { Name = "Shop" }).ClickAsync();

        // Expect page to have a button with the text 'Electronics'.
        await Expect(_page.GetByRole(AriaRole.Button, new() { Name = "Electronics" })).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task Login()
    {
        // Go to main page
        await _page.GotoAsync("http://localhost:5000/");

        // Click the login link in nav.
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        // Fill out the login form and submit.
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email:" }).FillAsync("admin@admin.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password:" }).FillAsync("admin123");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

        // Expect the page to have a welcome message and a logout button.
        await Expect(_page.Locator("#logged-in-user")).ToContainTextAsync("Welcome, Admin");
        await Expect(_page.GetByRole(AriaRole.Button, new() { Name = "Logout" })).ToBeVisibleAsync();

        // Click the shop link in nav.
        await _page.GetByRole(AriaRole.Link, new() { Name = "Shop" }).ClickAsync();

        // Expect page to have a button with the text 'Electronics'.
        await Expect(_page.GetByRole(AriaRole.Button, new() { Name = "Electronics" })).ToBeVisibleAsync();

        // Click the add to cart button for a laptop 2 times.
        await _page.Locator("#button-add-laptop").ClickAsync();
        await _page.Locator("#button-add-laptop").ClickAsync();

        // Expect the cart link in nav to have the text 'Cart (2)'.
        await Expect(_page.Locator("#nav-cart")).ToContainTextAsync("Cart (2)");

        // Click the cart link in nav.
        await _page.GetByRole(AriaRole.Link, new() { Name = "Cart (2)" }).ClickAsync();

        // Expect the cart page to have the text 'Laptop'.
        await Expect(_page.Locator("#Laptop")).ToContainTextAsync("Laptop");

        // Click the checkout button.
        await _page.GetByRole(AriaRole.Button, new() { Name = "Checkout" }).ClickAsync();

        // Expect the page to show order placed successfully message.
        await Expect(_page.GetByRole(AriaRole.Main)).ToContainTextAsync("Order placed successfully! Thank you for your purchase.");

        // Click the profile link in nav.
        await _page.GetByRole(AriaRole.Link, new() { Name = "Profile" }).ClickAsync();

        // Expect the profile page to have the text 'Order Product: Laptop'.
        await Expect(_page.Locator("[id=\"\\31 5\"]")).ToContainTextAsync("Order Product: Laptop");
    }
}
