namespace MultiShop.Cargo.Business.Abstract;

public interface IGenericService<TResultDto, TCreateDto, TUpdateDto>
{
    Task<List<TResultDto>> TGetAllAsync();
    Task<TResultDto?> TGetByIdAsync(int id);
    Task TInsertAsync(TCreateDto dto);
    Task TUpdateAsync(TUpdateDto dto);
    Task TDeleteAsync(int id);
}