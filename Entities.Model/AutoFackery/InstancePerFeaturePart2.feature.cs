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
namespace Entities.Model.AutoFackery
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("InstancePerFeaturePart2")]
    public partial class InstancePerFeaturePart2Feature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "InstancePerFeaturePart2.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "InstancePerFeaturePart2", "In order to share expensive configuration accross scenarios\nAs a lazy dev\nI want " +
                    "to confirm the iterator resets to 0 and still iterates", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("0 - Take the iterator, confirm is 0 and then iterate to a value 3")]
        public virtual void _0_TakeTheIteratorConfirmIs0AndThenIterateToAValue3()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("0 - Take the iterator, confirm is 0 and then iterate to a value 3", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.Given("I configure, build and scope with the per feature configuration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
testRunner.And("I resolve an instance of the iterator and store it in the context", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
testRunner.Then("I expect the iterators value to be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 10
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
testRunner.Then("I expect the iterators value to be 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 12
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
testRunner.Then("I expect the iterators value to be 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
testRunner.Then("I expect the iterators value to be 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("1 - Take the iterator, confirm is 3 and then iterate to a value 6")]
        public virtual void _1_TakeTheIteratorConfirmIs3AndThenIterateToAValue6()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("1 - Take the iterator, confirm is 3 and then iterate to a value 6", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
testRunner.Given("I resolve an instance of the iterator and store it in the context", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
testRunner.Then("I expect the iterators value to be 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
testRunner.Then("I expect the iterators value to be 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
testRunner.Then("I expect the iterators value to be 5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
testRunner.Then("I expect the iterators value to be 6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("2 - Take the iterator, confirm is 6 and then iterate to a value 9")]
        public virtual void _2_TakeTheIteratorConfirmIs6AndThenIterateToAValue9()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("2 - Take the iterator, confirm is 6 and then iterate to a value 9", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
testRunner.Given("I resolve an instance of the iterator and store it in the context", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
testRunner.Then("I expect the iterators value to be 6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
testRunner.Then("I expect the iterators value to be 7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 32
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 33
testRunner.Then("I expect the iterators value to be 8", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
testRunner.When("I iterate the iterators value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
testRunner.Then("I expect the iterators value to be 9", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
