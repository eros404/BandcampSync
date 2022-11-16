using Eros404.BandcampSync.BandcampWebsite.Extensions;
using OpenQA.Selenium;

namespace Eros404.BandcampSync.BandcampWebsite.Pages
{
    internal class LoginPage : Page<LoginPage>
    {
        private readonly string _url;
        public LoginPage(IWebDriver driver, string baseUrl) : base(driver, baseUrl)
        {
            _url = new Uri(new Uri(baseUrl), "/login").ToString();
        }

        private IWebElement UserNameInput => Driver.FindElement(By.Id("username-field"));
        private IWebElement PasswordInput => Driver.FindElement(By.Id("password-field"));
        private IWebElement SubmitButton => Driver.FindElement(By.CssSelector("button[type=submit]"));

        protected override bool EvaluateLoadedStatus()
        {
            try
            {
                return Driver.Url.StartsWith(_url) &&
                    UserNameInput.Displayed &&
                    PasswordInput.Displayed &&
                    SubmitButton.Displayed;
            }
            catch (InvalidElementStateException)
            {
                return false;
            }
        }

        protected override void ExecuteLoad()
        {
            Driver.Navigate().GoToUrl(_url);
            Driver.WaitForJsToLoad();
        }

        public ProfilePage Login(string userName, string password)
        {
            Thread.Sleep(1000);
            UserNameInput.SendKeys(userName);
            Thread.Sleep(1000);
            PasswordInput.SendKeys(password);
            SubmitButton.Click();
            return new ProfilePage(Driver, BaseUrl);
        }
    }
}
