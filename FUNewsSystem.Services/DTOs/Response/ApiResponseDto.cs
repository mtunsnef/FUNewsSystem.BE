using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.DTOs.Response
{
    public class ApiResponseDto<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponseDto(string message, T data, int statusCode = (int)HttpStatusCode.OK)
        {
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public static ApiResponseDto<T> Success(T data, string message = "Success")
        {
            return new ApiResponseDto<T>(message, data);
        }
    }

}