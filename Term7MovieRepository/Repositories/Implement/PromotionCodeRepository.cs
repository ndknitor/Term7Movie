using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class PromotionCodeRepository : IPromotionCodeRepository
    {
        private AppDbContext _context;
        public PromotionCodeRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
