using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GamesStore.Api.Extensions.Routes;

public static class HttpResponseExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int totalCount, int pageSize)
    {
        var paginationHeader = new
        {
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationHeader));
    }
}