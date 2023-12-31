﻿using SNS_DLA.Core.Contracts;
using SNS_DLA.Core.Generics.GenericRepository;
using SNS_DLA.Data;
using SNS_DLA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNS_DLA.Core.Repositories
{
    public class SubcategoryRepository : GenericRepository<SubCategory>, ISubcategoryRepository
    {
        public SubcategoryRepository(SNSDbContext context) : base(context)
        {

        }

    }
}
