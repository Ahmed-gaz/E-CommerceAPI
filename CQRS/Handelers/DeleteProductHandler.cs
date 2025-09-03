using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.Repos;
using MediatR;


namespace E_CommerceAPI.CQRS.Handelers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand,Models.Product?>
    {
        private IProductRepo _repo;
        public DeleteProductHandler(IProductRepo repo)
        {
            _repo = repo;
        }

        public async Task<Models.Product?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deletedProduct = await _repo.Deleteproduct(request.productId);
            
            if (deletedProduct == null)
                return null;

           
            return deletedProduct;
        }

       
    }
}

