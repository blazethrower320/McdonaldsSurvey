using System; // MUST BE INCLUDED FOR CONSOLE TO WORK
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class Program
{
    private static string surveyURL = "https://www.mcdvoice.com/";
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the McDonald's Survey!");
        Console.WriteLine("Survey Code: ");
        string surveyCode = Console.ReadLine();
        string[] seperatedCode = surveyCode.Split('-');
        foreach (string s in seperatedCode)
        {
            Console.WriteLine("Section: " + s);
        }

        IWebDriver driver = new ChromeDriver();
        try
        {
            driver.Navigate().GoToUrl(surveyURL);
            for(int i = 1; i <= 6; i++)
            {
                IWebElement surveyInput = driver.FindElement(By.Id("CN" + i));
                if(surveyInput == null)
                {
                    // issue...
                    return;
                }

                surveyInput.Clear();
                surveyInput.SendKeys(seperatedCode[i - 1]);
            }

            IWebElement startButton = driver.FindElement(By.Id("NextButton"));
            startButton.Click();

            Thread.Sleep(200);
            clickOption(driver, "With an employee at the restaurant", "NextButton");
            clickOption(driver, "Drive-thru", "NextButton");
            clickAllRatings(driver, 5, "NextButton");
            clickAllRatings(driver, 1, "NextButton");
            clickAllRatings(driver, 1, "NextButton");
            //
            clickAllRatings(driver, 5, "NextButton");
            clickAllRatings(driver, 5, "NextButton");
            clickAllRatings(driver, 1, "NextButton");

            clickOption(driver, "Burgers, Chicken & Fish", "NextButton");
            clickOption(driver, "Hamburger/Cheeseburger", "NextButton");
            clickAllRatings(driver, 5, "NextButton");
            clickAllRatings(driver, 2, "NextButton");
            clickAllRatings(driver, 5, "NextButton");

            //Text Box Skip
            IWebElement button = driver.FindElement(By.Id("NextButton"));
            button.Click();

            clickAllRatings(driver, 2, "NextButton");
            clickOption(driver, "Three", "NextButton");
            clickOption(driver, "McDonald’s", "NextButton");
            clickAllRatings(driver, 5, "NextButton");

            // Skip Household income
            IWebElement button2 = driver.FindElement(By.Id("NextButton"));
            button2.Click();

            IWebElement test = driver.FindElement(By.CssSelector(".ValCode"));
            string code = test.Text.Replace("Validation Code: ", "");

            Console.WriteLine("Code: " + code);


        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("Press Enter in this console window to close the browser...");
        Console.ReadLine();

        driver.Quit();
    }
    private static void clickOption(IWebDriver driver, string optionText, string buttonName)
    {
        IWebElement option = driver.FindElement(By.XPath($"//label[contains(normalize-space(.), \"{optionText}\")]"));
        option.Click();
        IWebElement button = driver.FindElement(By.Id(buttonName));
        button.Click();
        Thread.Sleep(200);
    }
    private static void clickAllRatings(IWebDriver driver, int rating, string buttonName)
    {
        IReadOnlyCollection<IWebElement> rows = driver.FindElements(By.CssSelector("table.Inputtyperbl tbody tr"));
        foreach (IWebElement row in rows)
        {
            IWebElement option = row.FindElement(By.CssSelector($".Opt{rating} label"));
            option.Click();
            Thread.Sleep(100);
        }

        driver.FindElement(By.Id(buttonName)).Click();
        Thread.Sleep(200);
    }
}
