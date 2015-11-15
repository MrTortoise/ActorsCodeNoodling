using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Akka.Util.Internal;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    [Scope(Tag = "Persistence")]
    public sealed class PersistenceSteps
    {
        public const string SnapShotPath = @"snapshots";

        [Given(@"I have cleared out any persistence file data")]
        public void GivenIHaveClearedOutAnyPersistenceFileData()
        {

            var dir = Directory.GetCurrentDirectory();
            var snapPath = new DirectoryInfo(dir + @"\" + SnapShotPath);
            snapPath.EnumerateFileSystemInfos().ForEach(i=>i.Delete());
        }

    }
}
