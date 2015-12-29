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
namespace Entities.Model.FactoryTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("FactoryCoordinatorActor")]
    public partial class FactoryCoordinatorActorFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "FactoryCoordinatorActor.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FactoryCoordinatorActor", "In order to perform bulk operations on factories\r\nAs a user\r\nI want to be able to" +
                    " add factories and factory types", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "updatePeriod",
                        "factoryUpdatePeriod"});
            table1.AddRow(new string[] {
                        "100",
                        "1000"});
#line 8
 testRunner.And("I have configured the heartBeat actor to update with the following configuration", ((string)(null)), table1, "And ");
#line 11
 testRunner.And("I create the heirachy of coordinators", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "name"});
            table2.AddRow(new string[] {
                        "Metal"});
            table2.AddRow(new string[] {
                        "Rock"});
            table2.AddRow(new string[] {
                        "Punk"});
            table2.AddRow(new string[] {
                        "Hydrogen"});
            table2.AddRow(new string[] {
                        "geddit yet?"});
#line 12
 testRunner.And("I add the following resources to the Resource Manager", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table3.AddRow(new string[] {
                        "Metal",
                        "0.1"});
            table3.AddRow(new string[] {
                        "Rock",
                        "0.9"});
#line 19
 testRunner.And("I have created the following Material called \"The Moon\"", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table4.AddRow(new string[] {
                        "Metal",
                        "0.1"});
            table4.AddRow(new string[] {
                        "Rock",
                        "0.9"});
#line 23
 testRunner.And("I have created the following Material called \"Some Planet\"", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table5.AddRow(new string[] {
                        "Hydrogen",
                        "0.1"});
#line 27
 testRunner.And("I have created the following Material called \"Mellow Yellow\"", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "radius",
                        "orbitDistance",
                        "orbitalAngularVelocity",
                        "rotatationalAngularVelocity",
                        "initialOrbitalAngularPositionOffset",
                        "currentAngularPosition",
                        "bodyType",
                        "material",
                        "satellites"});
            table6.AddRow(new string[] {
                        "The Moon",
                        "10",
                        "100",
                        "0.1",
                        "0",
                        "0",
                        "0",
                        "Moon",
                        "The Moon",
                        ""});
            table6.AddRow(new string[] {
                        "Another Moon",
                        "10",
                        "100",
                        "0.1",
                        "0",
                        "0",
                        "0",
                        "Moon",
                        "The Moon",
                        ""});
            table6.AddRow(new string[] {
                        "The Planet",
                        "10",
                        "100",
                        "0.1",
                        "0",
                        "0",
                        "0",
                        "Planet",
                        "Some Planet",
                        "\"The Moon\",\"Another Moon\""});
            table6.AddRow(new string[] {
                        "Other Planet",
                        "10",
                        "300",
                        "0.1",
                        "0",
                        "0",
                        "0",
                        "Planet",
                        "Some Planet",
                        ""});
            table6.AddRow(new string[] {
                        "The Sun",
                        "1",
                        "2",
                        "0.2",
                        "0",
                        "0",
                        "0",
                        "Star",
                        "Mellow Yellow",
                        ""});
            table6.AddRow(new string[] {
                        "The Second Sun",
                        "1",
                        "2",
                        "0.2",
                        "0",
                        "3.14",
                        "3.14",
                        "Star",
                        "Mellow Yellow",
                        ""});
#line 30
 testRunner.And("I have created the following Celestial Bodies", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "stars",
                        "planets"});
            table7.AddRow(new string[] {
                        "Solar System",
                        "\"The Sun\",\"The Second Sun\"",
                        "\"The Planet\",\"Other Planet\""});
#line 38
 testRunner.And("I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with" +
                    " arguments", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Capacity",
                        "CargoSize"});
            table8.AddRow(new string[] {
                        "smallFactoryInventory",
                        "1000",
                        "Small"});
#line 41
 testRunner.And("I have created the following inventory types", ((string)(null)), table8, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add factory types and query them")]
        public virtual void AddFactoryTypesAndQueryThem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add factory types and query them", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "In",
                        "resource",
                        "quantity",
                        "periods"});
            table9.AddRow(new string[] {
                        "false",
                        "Metal",
                        "10",
                        "1"});
#line 46
 testRunner.Given("I have created a Factory Type called \"FuckPhysics\" with the following properties", ((string)(null)), table9, "Given ");
#line 49
 testRunner.When("I query the factory types and store result in context as \"FactoryTypes\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "In",
                        "resource",
                        "quantity",
                        "periods"});
            table10.AddRow(new string[] {
                        "false",
                        "Metal",
                        "10",
                        "1"});
#line 50
 testRunner.Then("I expect the factory type \"FuckPhysics\" with the following properties in context " +
                    "\"FactoryTypes\"", ((string)(null)), table10, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create a factory")]
        public virtual void CreateAFactory()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a factory", ((string[])(null)));
#line 54
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "In",
                        "resource",
                        "quantity",
                        "periods"});
            table11.AddRow(new string[] {
                        "false",
                        "Metal",
                        "10",
                        "1"});
#line 55
 testRunner.Given("I have created a Factory Type called \"FuckPhysics\" with the following properties", ((string)(null)), table11, "Given ");
#line 58
 testRunner.And("I have created a Trader called \"factoryCreator\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "factoryType",
                        "centerOfMass",
                        "celestialBody",
                        "inventoryType"});
            table12.AddRow(new string[] {
                        "somethingFromNothingFactory",
                        "FuckPhysics",
                        "Solar System",
                        "Other Planet",
                        "smallFactoryInventory"});
#line 59
 testRunner.When("I create the following Factories using actor \"factoryCreator\"", ((string)(null)), table12, "When ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "factoryType",
                        "centerOfMass",
                        "celestialBody",
                        "inventoryType"});
            table13.AddRow(new string[] {
                        "somethingFromNothingFactory",
                        "FuckPhysics",
                        "Solar System",
                        "Other Planet",
                        "smallFactoryInventory"});
#line 62
 testRunner.Then("I expect the FactoryCoordinator to contain the following factories", ((string)(null)), table13, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "factoryType",
                        "centerOfMass",
                        "celestialBody",
                        "inventoryType"});
            table14.AddRow(new string[] {
                        "somethingFromNothingFactory",
                        "FuckPhysics",
                        "Solar System",
                        "Other Planet",
                        "smallFactoryInventory"});
#line 65
 testRunner.Then("I expect the results of querying the trader \"factoryCreator\" for its factories to" +
                    " be", ((string)(null)), table14, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
