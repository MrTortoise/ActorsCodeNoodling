﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Entities.Model.ContractActors
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("ExchangeContractFeature")]
    public partial class ExchangeContractFeatureFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ExchangeContractFeature.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ExchangeContractFeature", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
                    "wo numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line 7
   testRunner.Given("I create a test actor system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
   testRunner.And("I create a Resource Manager", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "name"});
            table1.AddRow(new string[] {
                        "metal"});
            table1.AddRow(new string[] {
                        "rock"});
#line 9
   testRunner.Given("I add the following resources to the Resource Manager", ((string)(null)), table1, "Given ");
#line 13
   testRunner.And("I have created a Trader called \"seller\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
   testRunner.And("I have created a Trader called \"buyer\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
   testRunner.And("I have configured the DateTime provider to return \"2015/1/1 15:00:00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get an instance of ExchangeContractActor and verify it is uninitialised")]
        public virtual void GetAnInstanceOfExchangeContractActorAndVerifyItIsUninitialised()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get an instance of ExchangeContractActor and verify it is uninitialised", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 19
 testRunner.When("I create an ExchangeContractActor called \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
 testRunner.Then("I expect the state of the ExchangeContractActor \"test\" to be \"Uninitialised\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Post invitation to Exchange contract, verify its current state is posted and that" +
            " it can be queried")]
        public virtual void PostInvitationToExchangeContractVerifyItsCurrentStateIsPostedAndThatItCanBeQueried()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post invitation to Exchange contract, verify its current state is posted and that" +
                    " it can be queried", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 24
 testRunner.Given("I create an ExchangeContractActor called \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "ExchangeType",
                        "Purchase"});
            table2.AddRow(new string[] {
                        "SellResourceName",
                        "metal"});
            table2.AddRow(new string[] {
                        "SellResourceQuantity",
                        "10"});
            table2.AddRow(new string[] {
                        "SellResourceTimePeriod",
                        "Hour"});
            table2.AddRow(new string[] {
                        "SellResourceTimePeriodQuantity",
                        "1"});
            table2.AddRow(new string[] {
                        "SuggestedOfferResourceName",
                        "rock"});
            table2.AddRow(new string[] {
                        "SuggestedOfferResourceQuantity",
                        "2"});
            table2.AddRow(new string[] {
                        "LiabilityResourceName",
                        "metal"});
            table2.AddRow(new string[] {
                        "LiabilityResourceQuantity",
                        "2"});
            table2.AddRow(new string[] {
                        "ContractOwner",
                        "Test"});
#line 25
 testRunner.When("I post to the ExchangeContract \"test\" the following invitation", ((string)(null)), table2, "When ");
#line 37
 testRunner.Then("I expect the state of the ExchangeContractActor \"test\" to be \"InvitationPosted\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 38
   testRunner.And("I expect the creator of ExchangeContractActor \"test\" to be \"Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "ExchangeType",
                        "Purchase"});
            table3.AddRow(new string[] {
                        "Resource",
                        "metal"});
            table3.AddRow(new string[] {
                        "Quantity",
                        "10"});
            table3.AddRow(new string[] {
                        "CompletionTime",
                        "2015/1/1 16:00:00"});
            table3.AddRow(new string[] {
                        "SuggestedOfferResourceName",
                        "rock"});
            table3.AddRow(new string[] {
                        "SuggestedOfferResourceQuantity",
                        "2"});
            table3.AddRow(new string[] {
                        "LiabilityResourceName",
                        "metal"});
            table3.AddRow(new string[] {
                        "LiabilityResourceQuantity",
                        "2"});
#line 39
   testRunner.And("I expect the ExchangeContractActor \"test\" to have the following for offer", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Take an invitation and make an offer, verify state is OfferMade, offer is queryab" +
            "le, and owner gets notified")]
        public virtual void TakeAnInvitationAndMakeAnOfferVerifyStateIsOfferMadeOfferIsQueryableAndOwnerGetsNotified()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Take an invitation and make an offer, verify state is OfferMade, offer is queryab" +
                    "le, and owner gets notified", ((string[])(null)));
#line 50
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 51
 testRunner.Given("I create an ExchangeContractActor called \"exchangeContract\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table4.AddRow(new string[] {
                        "ExchangeType",
                        "Purchase"});
            table4.AddRow(new string[] {
                        "SellResourceName",
                        "metal"});
            table4.AddRow(new string[] {
                        "SellResourceQuantity",
                        "10"});
            table4.AddRow(new string[] {
                        "SellResourceTimePeriod",
                        "Hour"});
            table4.AddRow(new string[] {
                        "SellResourceTimePeriodQuantity",
                        "1"});
            table4.AddRow(new string[] {
                        "SuggestedOfferResourceName",
                        "rock"});
            table4.AddRow(new string[] {
                        "SuggestedOfferResourceQuantity",
                        "2"});
            table4.AddRow(new string[] {
                        "LiabilityResourceName",
                        "metal"});
            table4.AddRow(new string[] {
                        "LiabilityResourceQuantity",
                        "2"});
            table4.AddRow(new string[] {
                        "ContractOwner",
                        "testActor"});
#line 52
 testRunner.And("I post to the ExchangeContract \"exchangeContract\" the following invitation", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "Resource",
                        "metal"});
            table5.AddRow(new string[] {
                        "Quantity",
                        "8"});
#line 64
   testRunner.When("the Trader called \"buyer\" makes the following offer on the ExchangeContractActor " +
                    "called \"exchangeContract\"", ((string)(null)), table5, "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "Resource",
                        "metal"});
            table6.AddRow(new string[] {
                        "Quantity",
                        "8"});
            table6.AddRow(new string[] {
                        "SenderName",
                        "buyer"});
#line 68
   testRunner.Then("I expect that the TestActor will of been notified of the following offer being ma" +
                    "de", ((string)(null)), table6, "Then ");
#line 73
   testRunner.Then("I expect the state of the ExchangeContractActor \"exchangeContract\" to be \"OfferRe" +
                    "cieved\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "Resource",
                        "metal"});
            table7.AddRow(new string[] {
                        "Quantity",
                        "8"});
            table7.AddRow(new string[] {
                        "SenderName",
                        "buyer"});
#line 74
   testRunner.And("I expect an offer on the ExchangeContractActor called \"exchangeContract\" to be", ((string)(null)), table7, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Take an invitation that is under offer, reject it and make an alternate suggestio" +
            "n")]
        public virtual void TakeAnInvitationThatIsUnderOfferRejectItAndMakeAnAlternateSuggestion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Take an invitation that is under offer, reject it and make an alternate suggestio" +
                    "n", ((string[])(null)));
#line 81
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 82
 testRunner.Given("I create an ExchangeContractActor called \"exchangeContract\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "ExchangeType",
                        "Purchase"});
            table8.AddRow(new string[] {
                        "SellResourceName",
                        "metal"});
            table8.AddRow(new string[] {
                        "SellResourceQuantity",
                        "10"});
            table8.AddRow(new string[] {
                        "SellResourceTimePeriod",
                        "Hour"});
            table8.AddRow(new string[] {
                        "SellResourceTimePeriodQuantity",
                        "1"});
            table8.AddRow(new string[] {
                        "SuggestedOfferResourceName",
                        "rock"});
            table8.AddRow(new string[] {
                        "SuggestedOfferResourceQuantity",
                        "2"});
            table8.AddRow(new string[] {
                        "LiabilityResourceName",
                        "metal"});
            table8.AddRow(new string[] {
                        "LiabilityResourceQuantity",
                        "2"});
            table8.AddRow(new string[] {
                        "ContractOwner",
                        "Test"});
#line 83
 testRunner.And("I post to the ExchangeContract \"exchangeContract\" the following invitation", ((string)(null)), table8, "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table9.AddRow(new string[] {
                        "Resource",
                        "metal"});
            table9.AddRow(new string[] {
                        "Quantity",
                        "8"});
#line 95
   testRunner.And("the Trader called \"buyer\" makes the following offer on the ExchangeContractActor " +
                    "called \"exchangeContract\"", ((string)(null)), table9, "And ");
#line 99
   testRunner.And("I configure the Trader called \"seller\" to log when offer recieved as \"offers\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table10.AddRow(new string[] {
                        "Resource",
                        "metal"});
            table10.AddRow(new string[] {
                        "Quantity",
                        "10"});
#line 100
   testRunner.When("the Trader called \"seller\" rejects the offer on the ExchangeContractActor called " +
                    "\"exchangeContract\" and makes the following suggested offer", ((string)(null)), table10, "When ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Resource",
                        "Quantity"});
            table11.AddRow(new string[] {
                        "metal",
                        "10"});
#line 104
   testRunner.Then("I expect that the Trader \"seller\" will of recieved the following offers as \"offer" +
                    "s\"", ((string)(null)), table11, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table12.AddRow(new string[] {
                        "Resource",
                        "metal"});
            table12.AddRow(new string[] {
                        "Quantity",
                        "10"});
#line 107
   testRunner.And("I expect the suggested offer on the ExchangeContractActor called \"exchangeContrac" +
                    "t\" to be", ((string)(null)), table12, "And ");
#line 111
   testRunner.And("I expect the state of the ExchangeContractActor \"exchangeContract\" to be \"Counter" +
                    "Offered\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Take an invitation that is under offer, accept it")]
        public virtual void TakeAnInvitationThatIsUnderOfferAcceptIt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Take an invitation that is under offer, accept it", ((string[])(null)));
#line 113
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
