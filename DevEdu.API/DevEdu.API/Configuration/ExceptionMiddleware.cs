﻿using DevEdu.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DevEdu.API.Configuration
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private const string MessageAuthorization = "Authorization exception";
        private const string MessageValidation = "Validation exception";
        private const string MessageEntity = "Entity not found exception";
        private const int AuthorizationCode = 1000;
        private const int ValidationCode = 1001;
        private const int EntityCode = 1002;
        private readonly int _forbiden = (int)HttpStatusCode.Forbidden;
        private readonly int _notFound = (int)HttpStatusCode.NotFound;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (AuthorizationException ex)
            {
                await HandlerExceptionMessageAsync(context, ex, AuthorizationCode, MessageAuthorization, _forbiden);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionMessageAsync(context, ex, ValidationCode, MessageValidation);
            }
            catch (EntityNotFoundException ex)
            {
                await HandlerExceptionMessageAsync(context, ex, EntityCode, MessageEntity, _notFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private static Task HandlerExceptionMessageAsync(HttpContext context, Exception exception, int code, string message, int httpStatusCode)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(
                new ExceptionResponse
                {
                    Code = code,
                    Message = message,
                    Description = exception.Message
                }
            );
            context.Response.StatusCode = httpStatusCode;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleValidationExceptionMessageAsync(HttpContext context, ValidationException exception, int code, string message)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new ValidationExceptionResponse(exception)
            {
                Code = code,
                Message = message
            });
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new
            {
                code = 1003,
                message = "Unknown error",
                description = exception.Message
            });
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}