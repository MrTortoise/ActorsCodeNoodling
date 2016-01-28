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
namespace Entities.Model.Locations
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("WorldLocationGenerator")]
    public partial class WorldLocationGeneratorFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "WorldLocationGenerator.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "WorldLocationGenerator", "In order to have places to visit\r\nAs a content builder\r\nI want to be able to add " +
                    "locations", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 7
#line hidden
#line 8
 testRunner.Given("I create a test actor system using config", "akka { \r\n   loglevel=DEBUG,  loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Lo" +
                    "gger.Serilog\"]\r\n      persistence {\r\n                       publish-plugin-comma" +
                    "nds = on\r\n                       journal {\r\n                           plugin = " +
                    "\"akka.persistence.journal.sql-server\"\r\n                           sql-server {\r\n" +
                    "                               class = \"Akka.Persistence.SqlServer.Journal.SqlSe" +
                    "rverJournal, Akka.Persistence.SqlServer\"\r\n                               plugin-" +
                    "dispatcher = \"akka.actor.default-dispatcher\"\r\n                               tab" +
                    "le-name = EventJournal\r\n                               schema-name = dbo\r\n      " +
                    "                         auto-initialize = on\r\n                               co" +
                    "nnection-string = \"Data Source=localhost\\\\SQLEXPRESS;Database=AkkaPersistenceTes" +
                    "t;User Id=akkadotnet;Password=akkadotnet;\"\r\n                           }\r\n      " +
                    "                 }\r\n                       snapshot-store {\r\n                   " +
                    "        plugin = \"akka.persistence.snapshot-store.sql-server\"\r\n                 " +
                    "          sql-server {\r\n                               class = \"Akka.Persistence" +
                    ".SqlServer.Snapshot.SqlServerSnapshotStore, Akka.Persistence.SqlServer\"\r\n       " +
                    "                        plugin-dispatcher = \"akka.actor.default-dispatcher\"\r\n   " +
                    "                            table-name = SnapshotStore\r\n                        " +
                    "       schema-name = dbo\r\n                               auto-initialize = on\r\n " +
                    "                              connection-string = \"Data Source=localhost\\\\SQLEXP" +
                    "RESS;Database=AkkaPersistenceTest;User Id=akkadotnet;Password=akkadotnet;\"\r\n    " +
                    "                       }\r\n                       }\r\n                   }\r\n\t}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
 testRunner.And("I have cleared out any persistence sql data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add a location")]
        [NUnit.Framework.CategoryAttribute("Persistence")]
        [NUnit.Framework.CategoryAttribute("LocationGenerator")]
        public virtual void AddALocation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add a location", new string[] {
                        "Persistence",
                        "LocationGenerator"});
#line 42
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 43
 testRunner.Given("I have created a LocationGenerator Actor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 44
 testRunner.And("I create a TestProbe called \"LocationWatcher\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.And("I observe LocationGenerator with TestProbe \"LocationWatcher\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.When("I add a location using \"LocationWatcher\" called \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 47
 testRunner.Then("I expect that TestProbe \"LocationWatcher\" be told the following locations was add" +
                    "ed \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 48
 testRunner.When("I poison the LocationGenerator", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
 testRunner.And("I have created a LocationGenerator Actor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.Then("I expect the location \"test\" to exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add multiple locations")]
        [NUnit.Framework.CategoryAttribute("LocationGenerator")]
        public virtual void AddMultipleLocations()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add multiple locations", new string[] {
                        "LocationGenerator"});
#line 54
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 55
 testRunner.Given("I have created a LocationGenerator Actor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 56
 testRunner.And("I create a TestProbe called \"LocationWatcher\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.And("I observe LocationGenerator with TestProbe \"LocationWatcher\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "location"});
            table1.AddRow(new string[] {
                        "test1"});
            table1.AddRow(new string[] {
                        "another"});
            table1.AddRow(new string[] {
                        "yesAnother"});
            table1.AddRow(new string[] {
                        "andAgain"});
#line 58
 testRunner.When("I add the following locations using \"LocationWatcher\"", ((string)(null)), table1, "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "location"});
            table2.AddRow(new string[] {
                        "test1"});
            table2.AddRow(new string[] {
                        "another"});
            table2.AddRow(new string[] {
                        "yesAnother"});
            table2.AddRow(new string[] {
                        "andAgain"});
#line 64
 testRunner.Then("I expect that TestProbe \"LocationWatcher\" be told the following locations were ad" +
                    "ded", ((string)(null)), table2, "Then ");
#line 70
 testRunner.When("I poison the LocationGenerator", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 71
 testRunner.And("I have created a LocationGenerator Actor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "location"});
            table3.AddRow(new string[] {
                        "test1"});
            table3.AddRow(new string[] {
                        "another"});
            table3.AddRow(new string[] {
                        "yesAnother"});
            table3.AddRow(new string[] {
                        "andAgain"});
#line 72
 testRunner.Then("I expect the follwing locations to exist", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
