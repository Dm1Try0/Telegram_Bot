using Microsoft.EntityFrameworkCore;
using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.DataAccess
{
	public class BotContext_REF : DbContext
	{
		public BotContext_REF(DbContextOptions<BotContext_REF> options) : base(options)
		{
		}
		public DbSet<Users> Users_REF { get; set; }
		public DbSet<CommonRef> Common { get; set; }
		public DbSet<Referals> Referals_REF { get; set; }
	}
}
