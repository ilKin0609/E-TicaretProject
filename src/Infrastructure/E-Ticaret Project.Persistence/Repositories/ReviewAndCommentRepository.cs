using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Contexts;

namespace E_Ticaret_Project.Persistence.Repositories;

public class ReviewAndCommentRepository:GenericRepository<ReviewAndComment>,IReviewAndCommentRepository
{
    public ReviewAndCommentRepository(E_TicaretProjectDbContext context) : base(context)
    {

    }
}
