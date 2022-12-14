using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services {
    public class CouponService : ICouponService {

        private readonly HttpClient _client;
        private const string BasePath = "api/v1/coupon";

        public CouponService(HttpClient client) {
            _client = client;
        }

        public async Task<CouponViewModel?> GetCouponByCode(string code, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _client.GetAsync($"{BasePath}/{code}");
            if (response.StatusCode != HttpStatusCode.OK) return new CouponViewModel();
            return await response.ReadContentAs<CouponViewModel>();
        }
        public Task<CouponViewModel> CreateCoupon(CouponViewModel model, string token)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteCoupon(long id, string token)
        {
            throw new NotImplementedException();
        }
    }
}
