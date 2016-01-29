using System;
using System.Collections.Generic;
using Akka.Actor;

namespace Entities.UniverseGenerator
{
    public class DistributionGenerator : ReceiveActor
    {
        public static readonly string Name = "DistributionGenerator";
        public static Props CreateProps(IActorRef randomGenerator)
        {
            return Props.Create(() => new DistributionGenerator(randomGenerator));
        }

        private readonly Dictionary<string, ISingleVariableFunction<int, double>> _cumulativeDistributionFunctions = new Dictionary<string, ISingleVariableFunction<int, double>>();
        private readonly Dictionary<string, ISingleVariableFunction<double, int>> _probabilityDistributionFunctions = new Dictionary<string, ISingleVariableFunction<double, int>>();
        private readonly Dictionary<string, int[]> _distributions = new Dictionary<string, int[]>();

        private readonly HashSet<IActorRef> _cdfAddedSubscriptions = new HashSet<IActorRef>();
        private readonly HashSet<IActorRef> _distributionGeneratedSubscriptions = new HashSet<IActorRef>();
       
        
        private readonly HashSet<IActorRef> _pdfAddedSubscriptions = new HashSet<IActorRef>();

        private readonly IActorRef _generatePdfToCdfJob;
        private readonly IActorRef _generateDistributionJob;


        public DistributionGenerator(IActorRef randomGenerator)
        {
            _generateDistributionJob = Context.ActorOf(GenerateCdfToDistributionJob.CreateProps(randomGenerator));
            _generatePdfToCdfJob = Context.ActorOf(GeneratePdfToCdfJob.CreateProps());

            Receive<SubscribeToPdfAdded>(msg =>
            {
                _pdfAddedSubscriptions.Add(Sender);
            });

            Receive<AddProbabilityDensityFunction>(msg =>
            {
                _probabilityDistributionFunctions.Add(msg.FunctionName, msg.Function);
                var pdfFunctionAdded = new PdfFunctionAdded();
                foreach (var pdfAddedSubscription in _pdfAddedSubscriptions)
                {
                    pdfAddedSubscription.Tell(pdfFunctionAdded);
                }
            });

            Receive<SubscribeToCdfAdded>(msg =>
            {
                _cdfAddedSubscriptions.Add(Sender);
            });

            Receive<AddCumulativeDistributionFunction>(msg =>
            {
                _cumulativeDistributionFunctions.Add(msg.FunctionName, msg.Function);
                foreach (var functionAddedSubscription in _cdfAddedSubscriptions)
                {
                    functionAddedSubscription.Tell(new CdfFunctionAdded(msg.FunctionName));
                }
            });

            Receive<GenerateCdfFromPdf>(msg =>
            {
                var function = _probabilityDistributionFunctions[msg.PdfName];
                _generatePdfToCdfJob.Tell(new GeneratePdfToCdfJob.Start(function, msg.Min, msg.Max, msg.NoPointsToSample, msg.CdfName));
            });

            Receive<GeneratePdfToCdfJob.CdfGenerated>(msg =>
            {
                _cumulativeDistributionFunctions.Add(msg.CdfName,msg.CDF);
                foreach (var cdfAddedSubscription in _cdfAddedSubscriptions)
                {
                    cdfAddedSubscription.Tell(new CdfFunctionAdded(msg.CdfName));
                }
            });

            Receive<SubscribeToDistributionGenerated>(msg =>
            {
                _distributionGeneratedSubscriptions.Add(Sender);
            });

            Receive<Generate>(msg =>
            {
                var function = _cumulativeDistributionFunctions[msg.FunctionName];
                _generateDistributionJob.Tell(new GenerateCdfToDistributionJob.Start(function,msg.NoResults,msg.DistributionName));
            });

            Receive<GenerateCdfToDistributionJob.Completed>(msg =>
            {
                _distributions.Add(msg.DistributionName, msg.Result);
                foreach (var distributionGeneratedSubscription in _distributionGeneratedSubscriptions)
                {
                    distributionGeneratedSubscription.Tell(new DistributionGenerated(msg.DistributionName,msg.Result));
                }
            });
        }

        public class SubscribeToCdfAdded{}

        public class SubscribeToDistributionGenerated{}

        public class CdfFunctionAdded{
            public string FunctionName { get;  }

            public CdfFunctionAdded(string functionName)
            {
                if (String.IsNullOrWhiteSpace(functionName)) throw new ArgumentException("Argument is null or whitespace", nameof(functionName));

                FunctionName = functionName;
            }
        }

        public class DistributionGenerated{
            public DistributionGenerated(string distributionName, int[] distribution)
            {
                if (distribution == null) throw new ArgumentNullException(nameof(distribution));
                if (String.IsNullOrWhiteSpace(distributionName)) throw new ArgumentException("Argument is null or whitespace", nameof(distributionName));

                DistributionName = distributionName;
                Distribution = distribution;
            }

            public string DistributionName { get; private set; }
            public int[] Distribution { get; private set; }
        }

        public class AddCumulativeDistributionFunction
        {
            public string FunctionName { get; }
            public ISingleVariableFunction<int, double> Function { get;  }

            public AddCumulativeDistributionFunction(string functionName, ISingleVariableFunction<int, double> function)
            {
                if (function == null) throw new ArgumentNullException(nameof(function));
                if (String.IsNullOrWhiteSpace(functionName)) throw new ArgumentException("Argument is null or whitespace", nameof(functionName));

                FunctionName = functionName;
                Function = function;
            }
        }

        public class Generate
        {
            public string DistributionName { get;  }
            public string FunctionName { get;  }
            public int NoResults { get;  }

            public Generate(string distributionName, string functionName, int noResults)
            {
                if (String.IsNullOrWhiteSpace(distributionName)) throw new ArgumentException("Argument is null or whitespace", nameof(distributionName));
                if (String.IsNullOrWhiteSpace(functionName)) throw new ArgumentException("Argument is null or whitespace", nameof(functionName));
                if (noResults <= 0) throw new ArgumentOutOfRangeException(nameof(noResults));

                DistributionName = distributionName;
                FunctionName = functionName;
                NoResults = noResults;
            }
        }

        public class SubscribeToPdfAdded{}

        public class AddProbabilityDensityFunction
        {
            public string FunctionName { get;  }
            public ISingleVariableFunction<double, int> Function { get;  }

            public AddProbabilityDensityFunction(string functionName, ISingleVariableFunction<double, int> function)
            {
                if (function == null) throw new ArgumentNullException(nameof(function));
                if (String.IsNullOrWhiteSpace(functionName)) throw new ArgumentException("Argument is null or whitespace", nameof(functionName));

                FunctionName = functionName;
                Function = function;
            }
        }

        public class PdfFunctionAdded{}

        public class GenerateCdfFromPdf
        {
            public string CdfName { get;  }
            public string PdfName { get;  }
            public int Min { get;  }
            public int Max { get;  }
            public int NoPointsToSample { get;  }

            public GenerateCdfFromPdf(string cdfName, string pdfName, int min, int max, int noPointsToSample)
            {
                if (String.IsNullOrWhiteSpace(cdfName)) throw new ArgumentException("Argument is null or whitespace", nameof(cdfName));
                if (String.IsNullOrWhiteSpace(pdfName)) throw new ArgumentException("Argument is null or whitespace", nameof(pdfName));
                if (noPointsToSample <= 0) throw new ArgumentOutOfRangeException(nameof(noPointsToSample));

                CdfName = cdfName;
                PdfName = pdfName;
                Min = min;
                Max = max;
                NoPointsToSample = noPointsToSample;
            }
        }
    }
}