using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Melville.Linq.Statistics.Tables;
using Melville.Linq.Statistics.Functional;

namespace Melville.Linq.Statistics.FileWriter
{
    public static class UnivariateSummaryFactory
    {
        public static UnivariateSummary<T> UnivariateSummary<T>(this IEnumerable<T> data, string outcomeName, Func<T, bool> outcome)=>
            new UnivariateSummary<T>(data, outcomeName, outcome);
        public static UnivariateSummary<T> UnivariateSummary<T>(this IEnumerable<T> data, Expression<Func<T, bool>> outcome)=>
            new UnivariateSummary<T>(data, ExpressionPrinter.Print(outcome), outcome.Compile());
    }
    public class UnivariateSummary<T>
    {
        private HtmlTable output;
        IList<T> data;
        Func<T, bool> outcome;

        public UnivariateSummary(IEnumerable<T> data, string outcomeName, Func<T, bool> outcome)
        {
            this.data = data.AsList();
            this.outcome = outcome;
            output = new HtmlTable();
            output.WithTitleRow("Variable", "N", outcomeName, "\x03C7\x00B2", "p");
        }
        public UnivariateSummary(IEnumerable<T> data, string outcomeName, Func<T, bool?> outcome) :
            this(data.Where(i => outcome(i).HasValue), outcomeName, i => outcome(i).Value)
        {
        }

        public UnivariateSummary<T> WithVariable(Expression<Func<T, object>> variable) =>
            WithVariable(ExpressionPrinter.Print(variable), variable.Compile());
        public UnivariateSummary<T> WithVariable(string name, Func<T, object> variable)
        {
            var values = data
                .Select(i => new { Pred = variable(i)?.ToString(), Outcome = outcome(i) })
                .Where(i => i.Pred != null)
                .ToList();

            var chiSq = values.Table().WithRows(i => i.Pred).WithColumns(i => i.Outcome).ChiSquared();
            output.WithRow(name, values.Count, "", $"{chiSq.ChiSquared:###0.00}", $"{chiSq.P:.0000}");
            var groups = values.GroupBy(i => i.Pred).OrderBy(i => i.Key);
            foreach (var group in groups)
            {
                var numerator = @group.Count(i => i.Outcome);
                var denominator = @group.Count();
                var percent = numerator * 100.0 / denominator;
                var format = denominator >= 100? "{0} ({1:##0.0} %)": "{0} ({1:##0} %)";
                output.WithRow(HtmlTable.TD(group.Key, ("align", "right")), denominator,
                    string.Format(format, numerator, percent), "", "");
            }
            return this;
        }

        private object ToDump()
        {
            return output;
        }
    }
}