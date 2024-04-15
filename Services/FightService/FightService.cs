namespace RPG_API.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DbContext _context;

        public FightService(DbContext context)
        {
            _context = context;
        }


    }
}
