using loconotes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Services
{
    public class BaseDataService
    {
		protected readonly LoconotesDbContext DbContext;

		public BaseDataService(
			LoconotesDbContext dbContext
		)
		{
			DbContext = dbContext;
		}
	}


}
