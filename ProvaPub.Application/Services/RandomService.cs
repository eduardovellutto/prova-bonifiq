using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Contracts;
using ProvaPub.Domain.Exceptions;
using ProvaPub.Domain.Models.Entities;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.Application.Services
{
    public class RandomService : IRandonService
    {
        private readonly TestDbContext _context;
        private readonly Random _random = new Random();

        public RandomService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetRandom()
        {
            if (await _context.Numbers.CountAsync() >= 100)
            {
                throw new NoAvailableNumbersException("Todos os números possíveis já foram gerados.");
            }

            int newNumber;
            bool exists;

            do
            {
                newNumber = _random.Next(0, 100);
                exists = await _context.Numbers.AnyAsync(x => x.Number.Equals(newNumber));
            }
            while (exists);

            _context.Numbers.Add(new RandomNumber { Number = newNumber });
            await _context.SaveChangesAsync();

            return newNumber;
        }

    }
}
