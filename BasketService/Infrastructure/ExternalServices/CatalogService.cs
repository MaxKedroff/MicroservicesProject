using Core.HttpLogic;
using Core.HttpLogic.Services;
using CoreLib.HttpLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLib;

namespace Infrastructure.ExternalServices
{
    public class CatalogService : ICatalogService
    {
        private readonly IHttpRequestService _httpRequestService;
        private readonly ITraceIdAccessor _traceIdAccessor;

        public CatalogService(IHttpRequestService httpRequestService, ITraceIdAccessor traceIdAccessor, string catalogServiceBaseUrl)
        {
            _httpRequestService = httpRequestService;
            _traceIdAccessor = traceIdAccessor;
        }

        public async Task<ProductInfo> GetProductInfoAsync(Guid productId)
        {
            var requestData = new HttpRequestData
            {
                Method = HttpMethod.Get,
                Uri = new Uri($"http://localhost:5000/api/products/{productId}"),

                HeaderDictionary = new Dictionary<string, string>
                {
                    ["TraceId"] = _traceIdAccessor.
                }
            };

            var connectionData = new HttpConnectionData
            {
                Timeout = TimeSpan.FromSeconds(30),
                ClientName = "CatalogService"
            };

            var response = await _httpRequestService.SendRequestAsync<ProductResponse>(requestData, connectionData);
        
            if (response.Body == null)
            {
                throw new Exception("Product not found");
            }

            return new ProductInfo
            {
                Id = response.Body.Id,
                Name = response.Body.Name,
                Price = response.Body.Price,
                ImageUrl = response.Body.PrimaryImage
            };
        }

        private class ProductResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = "";
            public decimal Price { get; set; }
            public string PrimaryImage { get; set; } = "";
        }
    }
}
