﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("FactoryUpdate")]
    public partial class FactoryUpdateFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "FactoryUpdate.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FactoryUpdate", "\tIn order to produce resources\r\n\tAs a Factory\r\n\tI want to be able to update and p" +
                    "roduce stuff", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 11
 testRunner.And("I add the following resources to the Resource Manager", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table3.AddRow(new string[] {
                        "Metal",
                        "1"});
            table3.AddRow(new string[] {
                        "Rock",
                        "9"});
#line 18
 testRunner.And("I have created the following Material called \"The Moon\"", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table4.AddRow(new string[] {
                        "Metal",
                        "1"});
            table4.AddRow(new string[] {
                        "Rock",
                        "9"});
#line 22
 testRunner.And("I have created the following Material called \"Some Planet\"", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table5.AddRow(new string[] {
                        "Hydrogen",
                        "1"});
#line 26
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
#line 29
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
#line 37
 testRunner.And("I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with" +
                    " arguments", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "In",
                        "resource",
                        "quantity",
                        "periods"});
            table8.AddRow(new string[] {
                        "false",
                        "Metal",
                        "10",
                        "1"});
#line 40
 testRunner.And("I have created a Factory Type called \"FuckPhysics\" with the following properties", ((string)(null)), table8, "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "In",
                        "resource",
                        "quantity",
                        "periods"});
            table9.AddRow(new string[] {
                        "true",
                        "Rock",
                        "10",
                        "1"});
            table9.AddRow(new string[] {
                        "false",
                        "Metal",
                        "10",
                        "1"});
#line 43
 testRunner.And("I have created a Factory Type called \"Consumer\" with the following properties", ((string)(null)), table9, "And ");
#line 47
 testRunner.And("I have created a Trader called \"factoryCreator\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Capacity",
                        "CargoSize"});
            table10.AddRow(new string[] {
                        "smallFactoryInventory",
                        "1000",
                        "Small"});
#line 48
 testRunner.And("I have created the following inventory types", ((string)(null)), table10, "And ");
#line 51
 testRunner.And("I tell the heartbeat actor to start", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Setup a dummy actor, register with FactoryCoordinator can assert that updates are" +
            " called when expected")]
        public virtual void SetupADummyActorRegisterWithFactoryCoordinatorCanAssertThatUpdatesAreCalledWhenExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setup a dummy actor, register with FactoryCoordinator can assert that updates are" +
                    " called when expected", ((string[])(null)));
#line 53
this.ScenarioSetup(scenarioInfo);
#line 6
 this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "factoryType",
                        "centerOfMass",
                        "celestialBody",
                        "inventoryType"});
            table11.AddRow(new string[] {
                        "somethingFromNothingFactory",
                        "FuckPhysics",
                        "Solar System",
                        "Other Planet",
                        "smallFactoryInventory"});
#line 54
 testRunner.Given("I create the following Factories using actor \"factoryCreator\"", ((string)(null)), table11, "Given ");
#line 57
 testRunner.And("I tell the FactoryCoordinator to start the factory called \"somethingFromNothingFa" +
                    "ctory\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.When("I tell the heartbeat actor to start", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 59
 testRunner.When("I wait for 2 FactoryUpdate time periods", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table12.AddRow(new string[] {
                        "Metal",
                        "30"});
#line 60
 testRunner.Then("I expect the factory \"somethingFromNothingFactory\" to have the following resource" +
                    "s", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update a factory that consumes and produces without sufficient input - assert upd" +
            "ate fails")]
        public virtual void UpdateAFactoryThatConsumesAndProducesWithoutSufficientInput_AssertUpdateFails()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update a factory that consumes and produces without sufficient input - assert upd" +
                    "ate fails", ((string[])(null)));
#line 64
this.ScenarioSetup(scenarioInfo);
#line 6
 this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "factoryType",
                        "centerOfMass",
                        "celestialBody",
                        "inventoryType"});
            table13.AddRow(new string[] {
                        "creatorOfMetal",
                        "Consumer",
                        "Solar System",
                        "Other Planet",
                        "smallFactoryInventory"});
#line 65
 testRunner.Given("I create the following Factories using actor \"factoryCreator\"", ((string)(null)), table13, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table14.AddRow(new string[] {
                        "Rock",
                        "0"});
#line 68
 testRunner.And("I deposit into the factory \"creatorOfMetal\" the following resources", ((string)(null)), table14, "And ");
#line 71
 testRunner.When("I wait for 2 FactoryUpdate time periods", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table15.AddRow(new string[] {
                        "Metal",
                        "0"});
            table15.AddRow(new string[] {
                        "Rock",
                        "0"});
#line 72
 testRunner.Then("I expect the factory \"creatorOfMetal\" to have the following resources", ((string)(null)), table15, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update a factory that consumes and produces with sufficient input - assert update" +
            " succeeds")]
        public virtual void UpdateAFactoryThatConsumesAndProducesWithSufficientInput_AssertUpdateSucceeds()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update a factory that consumes and produces with sufficient input - assert update" +
                    " succeeds", ((string[])(null)));
#line 77
this.ScenarioSetup(scenarioInfo);
#line 6
 this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "factoryType",
                        "centerOfMass",
                        "celestialBody",
                        "inventoryType"});
            table16.AddRow(new string[] {
                        "creatorOfMetal",
                        "Consumer",
                        "Solar System",
                        "Other Planet",
                        "smallFactoryInventory"});
#line 78
 testRunner.Given("I create the following Factories using actor \"factoryCreator\"", ((string)(null)), table16, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table17.AddRow(new string[] {
                        "Rock",
                        "30"});
#line 81
 testRunner.And("I deposit into the factory \"creatorOfMetal\" the following resources", ((string)(null)), table17, "And ");
#line 84
 testRunner.When("I wait for 2 FactoryUpdate time periods", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table18.AddRow(new string[] {
                        "Metal",
                        "30"});
            table18.AddRow(new string[] {
                        "Rock",
                        "0"});
#line 85
 testRunner.Then("I expect the factory \"creatorOfMetal\" to have the following resources", ((string)(null)), table18, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update a factory that consumes and produces with constrained input - assert updat" +
            "e succeeds correct number of times")]
        public virtual void UpdateAFactoryThatConsumesAndProducesWithConstrainedInput_AssertUpdateSucceedsCorrectNumberOfTimes()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update a factory that consumes and produces with constrained input - assert updat" +
                    "e succeeds correct number of times", ((string[])(null)));
#line 90
this.ScenarioSetup(scenarioInfo);
#line 6
 this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "factoryType",
                        "centerOfMass",
                        "celestialBody",
                        "inventoryType"});
            table19.AddRow(new string[] {
                        "creatorOfMetal",
                        "Consumer",
                        "Solar System",
                        "Other Planet",
                        "smallFactoryInventory"});
#line 91
 testRunner.Given("I create the following Factories using actor \"factoryCreator\"", ((string)(null)), table19, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table20.AddRow(new string[] {
                        "Rock",
                        "20"});
#line 94
 testRunner.And("I deposit into the factory \"creatorOfMetal\" the following resources", ((string)(null)), table20, "And ");
#line 97
 testRunner.When("I wait for 2 FactoryUpdate time periods", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "Value"});
            table21.AddRow(new string[] {
                        "Metal",
                        "20"});
            table21.AddRow(new string[] {
                        "Rock",
                        "0"});
#line 98
 testRunner.Then("I expect the factory \"creatorOfMetal\" to have the following resources", ((string)(null)), table21, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
