using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(GigasecondSolution solution) =>
            solution.DisapproveWhenInvalid() ??
            solution.ApproveWhenValid() ??
            solution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this GigasecondSolution solution)
        {
            if (solution.CreatesNewDatetime)
                solution.AddComment(DoNotCreateDateTime);

            if (solution.DoesNotUseAddSeconds)
                solution.AddComment(UseAddSeconds);

            return solution.HasComments
                ? solution.Disapprove()
                : solution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenValid(this GigasecondSolution solution)
        {
            if (solution.UsesMathPow)
                solution.AddComment(UseScientificNotationNotMathPow(solution.GigasecondValue));

            if (solution.UsesDigitsWithoutSeparator)
                solution.AddComment(UseScientificNotationOrDigitSeparators(solution.GigasecondValue));

            if (solution.AssignsToParameterAndReturns ||
                solution.AssignsToVariableAndReturns)
                solution.AddComment(DoNotAssignAndReturn);

            if (solution.UsesLocalVariable &&
                !solution.UsesLocalConstVariable)
                solution.AddComment(ConvertVariableToConst(solution.GigasecondValueVariableName));

            if (solution.UsesField &&
                !solution.UsesConstField)
                solution.AddComment(ConvertFieldToConst(solution.GigasecondValueFieldName));

            if (solution.UsesField &&
                !solution.UsesPrivateField)
                solution.AddComment(UsePrivateVisibility(solution.GigasecondValueFieldName));

            if (solution.UsesSingleLine &&
                !solution.UsesExpressionBody)
                solution.AddComment(UseExpressionBodiedMember(solution.AddMethodName));

            if (solution.UsesScientificNotation ||
                solution.UsesDigitsWithSeparator ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }
    }
}