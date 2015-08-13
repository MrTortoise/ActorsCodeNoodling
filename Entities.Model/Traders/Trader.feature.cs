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
namespace Entities.Model.Traders
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Trader")]
    [NUnit.Framework.CategoryAttribute("actorSystem")]
    public partial class TraderFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Trader.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Trader", "In order to profit\r\nAs a Trader\r\nI want to be able to post invitations to trade, " +
                    "make offers, accept, reject and exchange goods", ProgrammingLanguage.CSharp, new string[] {
                        "actorSystem"});
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a trader and give it some reources")]
        [NUnit.Framework.CategoryAttribute("actorSystem")]
        [NUnit.Framework.CategoryAttribute("resourceManager")]
        public virtual void CreateATraderAndGiveItSomeReources()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a trader and give it some reources", new string[] {
                        "actorSystem",
                        "resourceManager"});
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
 testRunner.Given("I have created a Trader called \"Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "name"});
            table1.AddRow(new string[] {
                        "metal"});
            table1.AddRow(new string[] {
                        "rock"});
#line 11
 testRunner.And("I add the following resources to the Resource Manager", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "quantity"});
            table2.AddRow(new string[] {
                        "metal",
                        "3"});
            table2.AddRow(new string[] {
                        "rock",
                        "4"});
#line 15
 testRunner.When("I post the folowing resources to the Trader \"Test\"", ((string)(null)), table2, "When ");
#line 19
 testRunner.And("I ask What resources Trader \"Test\" has storing them in the context as \"TestResour" +
                    "ces\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "quantity"});
            table3.AddRow(new string[] {
                        "metal",
                        "3"});
            table3.AddRow(new string[] {
                        "rock",
                        "4"});
#line 20
 testRunner.Then("I expect the following resources in context \"TestResources\"", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion