namespace UserManagement.Common.Helpers
{
    public static class QueryValidator
    {
        public static bool IsValidFilterExpression<T>(string filterExpression)
        {
            if (string.IsNullOrWhiteSpace(filterExpression))
                return false;

            var match = System.Text.RegularExpressions.Regex.Match(filterExpression,
                @"^[A-Za-z_][A-Za-z0-9_]*");

            if (!match.Success)
                return false;

            var propertyName = match.Value;

            return typeof(T).GetProperties().Any(p => p.Name.Equals(
                    propertyName, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsValidProperty<T>(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return false;

            var propertyName = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(propertyName))
                return false;

            return typeof(T).GetProperties()
                .Any(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
