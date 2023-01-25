using Core.Abstractions.Repositories;
using Core.Domain;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Database.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly Dictionary<int, Proizvod> _products;
        private int _id = 0;

        public ProductRepository()
        {
            if (File.Exists("proizvodi.json"))
                _products = JsonSerializer.Deserialize<Dictionary<int, Proizvod>>(
                    File.ReadAllText("proizvodi.json"));
            else
                _products = new Dictionary<int, Proizvod>();

            _id = _products.Count == 0 
                ? 0 
                : _products.Values.Select(p => p.Id).Max();

        }

        void SaveRepository()
        {
            File.WriteAllText("proizvodi.json", JsonSerializer.Serialize(_products));
        }

        public List<Proizvod> GetAllProducts()
        {
            return _products.Values.ToList();
        }

        public void Insert(Proizvod product)
        {

            product.Id = ++_id;
            _products.Add(product.Id, product);
            SaveRepository();
        }
    }
}