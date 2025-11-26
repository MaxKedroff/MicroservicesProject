using Core.HttpLogic.Services;
using CoreLib.HttpLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpLogic
{
    /// <summary>
    /// Отправка HTTP запросов и обработка ответов
    /// </summary>
    public interface IHttpRequestService
    {
        /// <summary>
        /// Отправить HTTP-запрос
        /// </summary>
        Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData, HttpConnectionData connectionData = default);
    }


}
