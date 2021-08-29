using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _ctx;
        private readonly IMapper _mapper;

        public UserRepository(DataContext ctx,IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

      
        public async  Task<MemberDto> GetMemberAsync(string username)
        {
            return await _ctx.Users
                        .Where(u => u.UserName == username)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _ctx.Users
                       .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                       .ToListAsync();

        }


    

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _ctx.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _ctx.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(n => n.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _ctx.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _ctx.Entry(user).State = EntityState.Modified;
        }
    }
}
