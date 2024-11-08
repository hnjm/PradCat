using PradCat.Domain.Entities;

namespace PradCat.Domain.Handlers.Repositories;
public interface ICatRepository
{
    Task<Cat> CreateAsync(Cat cat);
    Task<Cat> UpdateAsync(Cat cat);
    Task<bool> DeleteAsync(int id);
    Task<Cat?> GetByIdAsync(int id);
    Task<List<Cat>> GetAllAsync(int userId);

    // GetAllAsync é o unico que precisa de userId no nível de repositório
    // pois filtra a consulta para não trazer todos os registros
    // da tabela e só depois filtrar os gatos do usuario no service
}
