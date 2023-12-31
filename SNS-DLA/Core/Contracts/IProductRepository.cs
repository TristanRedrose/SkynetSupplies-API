﻿using SNS_DLA.Core.Generics.IGenericRepository;
using SNS_DLA.Models.DTO_s.Business;
using SNS_DLA.Models.DTO_s.Response;
using SNS_DLA.Models.Entities;
using SNS_DLA.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNS_DLA.Core.Contracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<ProductsWithCount> GetProductsWithCategory(Filters filters);
    }
}
