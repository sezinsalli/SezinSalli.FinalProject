using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Concrete
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper _mapper;
        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitofWork, ICategoryRepository categoryRepository, IMapper mapper) : base(repository, unitofWork)
        {
            _mapper = mapper;
            this.productRepository = productRepository;
        }

        

    }
}
