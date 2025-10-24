using Catalog.Application.DTOs;
using MediatR;


namespace Catalog.Application.Features.Categories.Queries.GetQueries;

public class GetCategoriesQuery : IRequest<List<CategoryDTO>>
{
    
}