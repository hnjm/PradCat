using PradCat.Domain.Entities;
using PradCat.Domain.Requests.Tutors;
using PradCat.Domain.Responses;

namespace PradCat.Domain.Handlers.Services;
public interface ITutorService
{
    Task<Tutor?> CreateAsync(Tutor tutor);
    Task<bool> DeleteAsync(int id);
    Task<Response<Tutor>> UpdateAsync(UpdateTutorRequest request);
    Task<Response<Tutor>> GetByIdAsync(GetTutorByIdRequest request);
    Task<PagedResponse<List<Tutor>>> GetAllAsync(GetAllTutorsRequest request);

    // Criar e deletar o tutor se tornou responsabilidade do usuario
    // Quando criado ou deletado, automaticamente gera ou deleta o tutor
}
