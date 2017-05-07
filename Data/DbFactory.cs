using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Data
{
	public interface IDbFactory
	{
		LoconotesDbContext DbContext { get; }
	}

    public class DbFactory : IDbFactory
    {
	    public LoconotesDbContext DbContext { get; }

		public DbFactory(
			LoconotesDbContext dbContext
	    )
		{
			DbContext = dbContext;
		}
    }
}
