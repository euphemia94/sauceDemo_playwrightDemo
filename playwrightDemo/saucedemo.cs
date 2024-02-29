using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

class Program
{
    static async Task MainAsync(string[] args)
    {
        #pragma warning disable CS8892
        // Launch the browser
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        var page = await browser.NewPageAsync();

        try
        {
            // Navigate to the Sauce Labs Sample Application
            await page.GotoAsync("https://www.saucedemo.com/");

            // Login
            await page.FillAsync("[data-test='username']", "standard_user");
            await page.FillAsync("[data-test='password']", "secret_sauce");
            await page.ClickAsync("[data-test='login-button']");

            // Select a T-shirt and add to cart
            await page.ClickAsync("[data-test='add-to-cart-sauce-labs-bolt-t-shirt']");

            // Go to cart
            await page.ClickAsync("[data-test='shopping-cart']");

            // Verify the T-shirt is added to the cart
            var cartItem = await page.QuerySelectorAsync(".cart_item");
            if (cartItem != null)
            {
                Console.WriteLine("T-shirt added to cart successfully.");
            }
            else
            {
                Console.WriteLine("Failed to add T-shirt to cart.");
                return;
            }

            // Proceed to checkout
            await page.ClickAsync("[data-test='checkout']");

            // Fill in checkout information
            await page.FillAsync("[data-test='firstName']", "John");
            await page.FillAsync("[data-test='lastName']", "Doe");
            await page.FillAsync("[data-test='postalCode']", "12345");

            // Continue to the next step
            await page.ClickAsync("[data-test='continue']");

            // Complete the purchase
            await page.ClickAsync("[data-test='finish']");

            // Verify the purchase is successful
            var completeHeader = (await page.TextContentAsync(".complete-header"))?.Trim();
            if (completeHeader != null && completeHeader.Equals("THANK YOU FOR YOUR ORDER"))
            {
                Console.WriteLine("Purchase completed successfully.");
            }
            else
            {
                Console.WriteLine("Failed to complete the purchase.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            // Close the browser
            await browser.CloseAsync();
        }
        #pragma warning restore CS8892
    }
}
