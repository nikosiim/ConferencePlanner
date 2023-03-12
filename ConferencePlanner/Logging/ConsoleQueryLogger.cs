using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution;
using System.Diagnostics;
using System.Text;

namespace ConferencePlanner.Logging
{
    public class ConsoleQueryLogger : ExecutionDiagnosticEventListener
    {
     private readonly ILogger<ConsoleQueryLogger> _logger;

        public ConsoleQueryLogger(ILogger<ConsoleQueryLogger> logger)
        {
            _logger = logger;
        }

        public override IDisposable ExecuteRequest(IRequestContext context)
        {
            return new RequestScope(_logger, context);
        }

        private class RequestScope : IDisposable
        {
            private readonly IRequestContext _context;
            private readonly ILogger<ConsoleQueryLogger> _logger;
            private readonly Stopwatch? _queryTimer;

            public RequestScope(ILogger<ConsoleQueryLogger> logger, IRequestContext context)
            {
                _logger = logger;
                _context = context;
                _queryTimer = new Stopwatch();
                _queryTimer.Start();
            }

            public void Dispose()
            {
                if (_context.Document is null) return;

                StringBuilder sb = new(_context.Document.ToString(true));
                sb.AppendLine();

                if (_context.Variables != null)
                {
                    List<VariableValue> variables = _context.Variables!.ToList();
                    if (variables.Count > 0)
                    {
                        sb.AppendFormat($"Variables {Environment.NewLine}");
                        try
                        {
                            foreach (var variableValue in _context.Variables!)
                            {
                                string PadRightHelper(string existingString, int lengthToPadTo)
                                {
                                    if (string.IsNullOrWhiteSpace(existingString))
                                        return "".PadRight(lengthToPadTo);
                                    if (existingString.Length > lengthToPadTo)
                                        return existingString.Substring(0, lengthToPadTo);
                                    return existingString + " ".PadRight(lengthToPadTo - existingString.Length);
                                }
                                sb.AppendFormat($"  {PadRightHelper(variableValue.Name, 20)} :  {PadRightHelper(variableValue.Value.ToString(), 20)}: {variableValue.Type}");
                                sb.AppendFormat($"{Environment.NewLine}");
                            }
                        }
                        catch
                        {
                            // all input type records will land here.
                            sb.Append("  Formatting Variables Error. Continuing...");
                            sb.AppendFormat($"{Environment.NewLine}");
                        }
                    }
                }
                _queryTimer?.Stop();

                sb.AppendFormat($"Elapsed time for query is {_queryTimer?.Elapsed.TotalMilliseconds:0.#} milliseconds.");
                _logger.LogInformation(sb.ToString());
            }
        }
    }
}