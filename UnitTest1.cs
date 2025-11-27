using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;
using System;
using SeleniumExtras.WaitHelpers;

namespace CloudQATests
{
    public class AutomationPracticeFormPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public AutomationPracticeFormPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        private By iframe => By.Id("iframeId");
        private By firstNameField => By.XPath("//input[@name='First Name']");
        private By countryField => By.XPath("//input[@name='Country']");
        private By stateDropdown => By.XPath("//select[@name='State']");

        public void SwitchToFormFrame()
        {
            var frame = wait.Until(ExpectedConditions.ElementIsVisible(iframe));
            driver.SwitchTo().Frame(frame);
        }

        public void SwitchBack()
        {
            driver.SwitchTo().DefaultContent();
        }

        public void TypeFirstName(string name)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(firstNameField)).SendKeys(name);
        }

        public void TypeCountry(string country)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(countryField)).SendKeys(country);
        }

        public void PickState(string state)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(stateDropdown));
            new SelectElement(element).SelectByText(state);
        }

        public string GetSelectedState()
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(stateDropdown));
            return new SelectElement(element).SelectedOption.Text;
        }
    }

    [TestFixture]
    public class AutomationPracticeFormTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private AutomationPracticeFormPage page;
        private string url = "https://app.cloudqa.io/home/AutomationPracticeForm";

        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl(url);
            page = new AutomationPracticeFormPage(driver, wait);
        }

        [TearDown]
        public void Cleanup()
        {
            driver.Quit();
        }

        [Test]
        public void FirstNameInput_Test()
        {
            string expected = "Viraj";

            page.SwitchToFormFrame();
            page.TypeFirstName(expected);

            var val = driver.FindElement(By.XPath("//input[@name='First Name']")).GetAttribute("value");
            Assert.AreEqual(expected, val);

            page.SwitchBack();
        }

        [Test]
        public void CountryInput_Test()
        {
            string country = "India";

            page.SwitchToFormFrame();
            page.TypeCountry(country);

            var actual = driver.FindElement(By.XPath("//input[@name='Country']")).GetAttribute("value");
            Assert.AreEqual(country, actual);

            page.SwitchBack();
        }

        [Test]
        public void StateDropdown_Test()
        {
            string option = "-- Select Country --";

            page.SwitchToFormFrame();
            page.PickState(option);

            var selected = page.GetSelectedState();
            Assert.AreEqual(option, selected);

            page.SwitchBack();
        }
    }
}
