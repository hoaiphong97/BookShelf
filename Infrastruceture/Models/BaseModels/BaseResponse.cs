using BookShelf.Enums;

namespace Infrastructure.Models.BaseModels
{
    public class BaseResponse
    {
        public BaseResponse(bool isSuccess = false, string errorMessage = "", string detail = "", ErrorTypeEnum errorType = ErrorTypeEnum.None)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }
        public ErrorTypeEnum ErrorType { get; set; }

        public BaseResponse BuildResult(string infoMessage)
        {
            IsSuccess = true;
            Detail = infoMessage;
            return this;
        }

        public BaseResponse BuildError(ErrorTypeEnum errorType, string errorMessage)
        {
            IsSuccess = false;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
            return this;
        }

        public BaseResponse BuildError(ErrorTypeEnum errorType, string errorMessage, string infoMessage)
        {
            IsSuccess = false;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
            Detail = infoMessage;
            return this;
        }
    }

    public class BaseResponse<T>
    {
        public BaseResponse(T data = default, bool isSuccess = false, string errorMessage = "", string detail = "", ErrorTypeEnum errorType = ErrorTypeEnum.None)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
            Data = data;
            Detail = detail;
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }
        public ErrorTypeEnum ErrorType { get; set; }

        public T Data { get; set; }

        public BaseResponse<T> SetInfo(bool success, string detail = default, string errorMessage = default)
        {
            IsSuccess = success;
            Detail = detail;
            ErrorMessage = errorMessage;
            return this;
        }

        public new BaseResponse<T> BuildResult(T data, string detail = null)
        {
            SetInfo(true, detail, null);
            Data = data;
            return this;
        }

        public new BaseResponse<T> BuildError(ErrorTypeEnum errorType, string error)
        {
            SetInfo(false, null, error);
            ErrorType = errorType;
            return this;
        }
        public new BaseResponse<T> BuildError(ErrorTypeEnum errorType, string error, string message)
        {
            SetInfo(false, message, error);
            ErrorType = errorType;
            return this;
        }
    }

}