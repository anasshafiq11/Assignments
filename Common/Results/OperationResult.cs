namespace UserManagement.Common.Results
{
    public class OperationResult
    {
        public bool Succeeded { get; init; }

        public string Message { get; init; } = string.Empty;

        public List<string> Errors { get; init; } = [];

        public static OperationResult Success(string message = "")
            => new() { Succeeded = true, Message = message };

        public static OperationResult Failure(params string[] errors)
            => new() { Succeeded = false, Errors = errors.ToList() };
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; init; }

        public static OperationResult<T> Success(T data, string message = "")
            => new() { Succeeded = true, Data = data, Message = message };

        public new static OperationResult<T> Failure(params string[] errors)
            => new() { Succeeded = false, Errors = errors.ToList() };
    }
}
