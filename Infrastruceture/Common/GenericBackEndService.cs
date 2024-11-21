using BookShelf.Enums;
using Infrastructure.Models.BaseModels;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common
{
    public class GenericBackEndService
    {
        protected readonly IServiceProvider _serviceProvider;

        protected GenericBackEndService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        protected BaseResponse<T> BuildError<T>(BaseResponse<T> result, ErrorTypeEnum errorType, string errorMessage)
        {
            return result.BuildError(errorType, errorMessage);
        }


        protected BaseResponse<T> BuildError<T>(BaseResponse<T> result, ErrorTypeEnum errorType, string errorMessage, params object[] msgArguments)
        {
            errorMessage = string.Format(errorMessage, msgArguments);
            return result.BuildError(errorType, errorMessage);
        }

        protected BaseResponse BuildError(BaseResponse result, ErrorTypeEnum errorType, string errorMessage)
        {
            return result.BuildError(errorType, errorMessage);
        }

        protected BaseResponse BuildError(BaseResponse result, ErrorTypeEnum errorType, string errorMessage, params object[] msgArguments)
        {
            errorMessage = string.Format(errorMessage, msgArguments);
            return result.BuildError(errorType, errorMessage);
        }

        protected BaseResponse<T> BuildResult<T>(BaseResponse<T> result, T data, string resultMessage)
        {
            return result.BuildResult(data, resultMessage);
        }

        protected BaseResponse<T> BuildResult<T>(BaseResponse<T> result, T data, string resultMessage, params object[] msgArguments)
        {
            resultMessage = string.Format(resultMessage, msgArguments);
            return result.BuildResult(data, resultMessage);
        }

        protected BaseResponse BuildResult(BaseResponse result, string resultMessage)
        {
            return result.BuildResult(resultMessage);
        }

        protected BaseResponse BuildResult(BaseResponse result, string resultMessage, params object[] msgArguments)
        {
            resultMessage = string.Format(resultMessage, msgArguments);
            return result.BuildResult(resultMessage);
        }
    }
}
