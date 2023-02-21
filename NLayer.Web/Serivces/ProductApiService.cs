using NLayer.Core.DTOs;

namespace NLayer.Web.Serivces
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductWithCategoryDto>> GetProductWithCategoryAsync()
        {
            //url yazılmamaktadır url base de tanımlıdır ve oradan gelmektedir. 
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("products/GetProductsWithCategory");
            return response.Data;
        }

        public async Task<ProductDto> Save(ProductDto productDto)
        {
            var response = await _httpClient.PostAsJsonAsync("products",productDto);
            if (response.IsSuccessStatusCode==false)
            {
                return null;
            }
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();
            return responseBody.Data;
        }

        public async Task<bool> Update(ProductDto productDto)
        {
            var response =await _httpClient.PutAsJsonAsync("products", productDto);
            //response içi okuyarak da hatra basılabilir
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Remove(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            //response içi okuyarak da hatra basılabilir
            return response.IsSuccessStatusCode;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");
            if (response.Errors.Any())
            {
                //loglama işlemi yapılabilir.
            }
            return response.Data;
        }
    }
}
