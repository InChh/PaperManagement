using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.Domain.Entities;

namespace Wf.PaperManagement;

public class RefitExceptionHandlerFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not Refit.ApiException { StatusCode: HttpStatusCode.NotFound })
        {
            context.ExceptionHandled = false;
            return;
        }

        context.Exception = new EntityNotFoundException("Can not find the specified user");
        context.ExceptionHandled = false;
    }
}