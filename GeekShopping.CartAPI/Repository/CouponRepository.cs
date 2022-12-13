using GeekShopping.CartAPI.Models.ValueObjects;
using GeekShopping.CartAPI.Repository.Interface;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.CartAPI.Repository {
    public class CouponRepository : ICouponRepository {

        private readonly HttpClient _client;

        public CouponRepository(HttpClient client) {
            _client = client;
        }
        
        public async Task<CouponVo> GetCouponByCode(string couponCode, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _client.GetAsync($"api/v1/coupon/{couponCode}");
            string content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK) return new CouponVo();
            return JsonSerializer.Deserialize<CouponVo>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
