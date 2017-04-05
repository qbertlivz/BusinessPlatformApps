using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.Deployment.Site.Test.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Microsoft.Deployment.Site.Web.Tests
{
    [TestClass]
    public class SCCMTemplateTests
    {
        private RemoteWebDriver driver;
        private string slot = "slot1";
        private string msiPath = @"C:\Program Files\Microsoft Templates\Microsoft-SCCMTemplate\Microsoft.Bpst.App.Msi.exe";

        [TestMethod]
        public void RunSCCMTests()
        {
            Credential.Load();
            DownloadAndInstallMSI();
            OpenWebBrowser();
            HelperMethods.driver = this.driver;
            HelperMethods.WaitForPage();
            try
            {
                var background = driver.FindElementByCssSelector("div[class='st-email-background st-email-wrapper au-target']");
                background.Click();
            }
            catch { /* If not found means s3 is behind s1, expected behaviour*/}
            HelperMethods.ClickButton("Next");
            HelperMethods.WaitForPage();
            Given_AlternativeWindowsCredentials_When_Validate_Then_Success();
            HelperMethods.ClickButton("Next");
            HelperMethods.WaitForPage();
            Given_CorrectSqlCredentials_When_Validate_Then_Success();
            HelperMethods.SelectSqlDatabase(Credential.Instance.SccmSql.Source);
            HelperMethods.ClickButton("Next");
            HelperMethods.WaitForPage();
            Given_CorrectSqlCredentials_When_Validate_Then_Success();
            HelperMethods.SelectSqlDatabase(Credential.Instance.SccmSql.Target);
            HelperMethods.ClickButton("Next");
            HelperMethods.WaitForPage();
            HelperMethods.ClickButton("Validate");
            HelperMethods.WaitForPage();
            HelperMethods.ClickButton("Next");
            HelperMethods.WaitForPage();
            HelperMethods.ClickButton("Run");
            Given_AllInformationCorrect_When_DeploymentFinish_Then_SuccessMessageDisplayed();
        }

        public void Given_AllInformationCorrect_When_DeploymentFinish_Then_SuccessMessageDisplayed()
        {
            var progressText = driver.FindElementsByCssSelector("span[class='semiboldFont st-progress-text']")
                                     .FirstOrDefault(e => e.Text == "All done! You can now download your Power BI report and start exploring your data.");

            int i = 0;

            while (progressText == null && i < 10)
            {
                progressText = driver.FindElementsByCssSelector("span[class='semiboldFont st-progress-text']")
                                     .FirstOrDefault(e => e.Text == "All done! You can now download your Power BI report and start exploring your data.");
                i++;
                Thread.Sleep(new TimeSpan(0, 0, 20));
            }

            Assert.IsTrue(progressText != null);
        }

        public void SelectSqlDatabase(string databaseName)
        {
            var database = driver.FindElementsByCssSelector("select[class='btn btn-default dropdown-toggle st-input au-target']")
                            .FirstOrDefault(e => !e.Text.Contains("Windows"));

            while (database == null)
            {
                database = driver.FindElementsByCssSelector("select[class='btn btn-default dropdown-toggle st-input au-target']")
                            .FirstOrDefault(e => !e.Text.Contains("Windows"));
            }
            database.SendKeys(databaseName);
        }

        public void Given_CorrectSqlCredentials_When_Validate_Then_Success()
        {
            var button = driver.FindElementsByTagName("Button").FirstOrDefault(e => e.Text == "Validate");
            while (button == null || !button.Enabled)
            {
                button = driver.FindElementsByTagName("Button").FirstOrDefault(e => e.Text == "Validate");
                Thread.Sleep(1000);
            }

            var elements = driver.FindElementsByCssSelector("input[class='st-input au-target']");
            var serverBox = elements.FirstOrDefault(e => e.GetAttribute("value.bind").Contains("sqlServer"));
            while (serverBox == null)
            {
                serverBox = elements.FirstOrDefault(e => e.GetAttribute("value.bind").Contains("sqlServer"));
            }

            serverBox.SendKeys(Credential.Instance.SccmSql.Server);

            HelperMethods.ClickButton("Validate");

            var validated = driver.FindElementByClassName("st-validated");
            Assert.IsTrue(validated.Text == "Successfully validated");
        }

        public void ValidateCurentUserCredentialsOnSourcePage()
        {
            var checkBox = driver.FindElementsByCssSelector("input[class='au-target']").First(e => e.GetAttribute("type") == "checkbox");
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click()", checkBox);

            var elements = driver.FindElementsByCssSelector("input[class='st-input au-target']");
            var passwordBox = elements.First(e => e.GetAttribute("value.bind").Contains("password"));
            passwordBox.SendKeys("");

            var validated = driver.FindElementByClassName("st-validated");
            Assert.IsTrue(validated.Text == "Successfully validated");
        }

        public void Given_AlternativeWindowsCredentials_When_Validate_Then_Success()
        {
            var elements = driver.FindElementsByCssSelector("input[class='st-input au-target']");

            var usernameBox = elements.First(e => e.GetAttribute("value.bind").Contains("username"));
            while (!usernameBox.Enabled)
            {
                Thread.Sleep(1000);
            }

            usernameBox.Clear();
            usernameBox.SendKeys($@"{Credential.Instance.ServiceAccount.Domain}\{Credential.Instance.ServiceAccount.Username.Split('@')[0]}");

            var passwordBox = elements.First(e => e.GetAttribute("value.bind").Contains("password"));
            passwordBox.SendKeys(Credential.Instance.ServiceAccount.Password);

            HelperMethods.ClickButton("Validate");

            var validated = driver.FindElementByClassName("st-validated");

            Assert.IsTrue(validated.Text == "Successfully validated");
        }

        public void OpenWebBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = msiPath;
            options.AddArgument("?name=Microsoft-SCCMTemplate");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        public void DownloadAndInstallMSI()
        {
            if (File.Exists("SCCM.exe"))
            {
                File.Delete("SCCM.exe");
            }

            var downloadUrl = $"https://bpstservice-{slot}.azurewebsites.net/bin//Apps/Microsoft/Released/Microsoft-SCCMTemplate/Microsoft-SCCMTemplate.exe";
            using (var client = new WebClient())
            {
                client.DownloadFile(downloadUrl, "SCCM.exe");
            }

            using (var p = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "SCCM.exe";
                startInfo.Arguments = "/install /quiet";


                p.StartInfo = startInfo;

                if (p.Start())
                {
                    while (!p.HasExited)
                    {
                        Thread.Sleep(1000);
                    }
                }

                var installer = Process.GetProcessesByName("Microsoft.Bpst.App.Msi");
                installer[0].Kill();
            }
        }
    }
}