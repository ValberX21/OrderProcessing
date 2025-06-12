using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.Interface
{
    public interface IRedisService
    {
        Task SetAsync(string key, string value);
        Task<string>GetAsync(string key);
    }
}
