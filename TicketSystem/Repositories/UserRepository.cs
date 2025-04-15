using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserRepository(MyDbContext context, IMapper mapper, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IdentityResult> Add(UserModel entity)
        {
            var user = new User
            {
                UserName = entity.UserName,
                Email = entity.Email,
                NormalizedEmail = entity.Email.ToUpper(),
                PhoneNumber = entity.PhoneNumber,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                Address = entity.Address,
                Avatar = entity.Avatar,
                NationalID = entity.NationalID,
                DepartmentID = entity.DepartmentID,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
            if (user.Avatar.IsNullOrEmpty()) 
            {
                user.Avatar = "https://ticketsystem.bucket.s3.ap-southeast-2.amazonaws.com/avatars/avatar-df.png";
            }
            // Create the user with the provided password
            var result = await _userManager.CreateAsync(user, entity.PasswordHash ?? "");

            if (result.Succeeded)
            {
                // If the user was created successfully, assign the role (optional)
                if (entity.RoleID.HasValue)
                {
                    var role = await _roleManager.FindByIdAsync(entity.RoleID.ToString() ?? "");
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name ?? "");
                    }
                }

                return IdentityResult.Success;
            }

            return result;
        }

        public async Task Delete(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserVM>> GetAll()
        {
            var users = await _context.Users
               .Select(user => new UserVM
               {
                   Id = user.Id,
                   UserName = user.UserName,
                   Email = user.Email,
                   PhoneNumber = user.PhoneNumber,
                   DateOfBirth = user.DateOfBirth,
                   Gender = user.Gender,
                   Address = user.Address,
                   Avatar = user.Avatar,
                   NationalID = user.NationalID,
                   DepartmentID = user.DepartmentID,
                   DepartmentName = _context.Departments
                   .Where(d=>d.DepartmentID == user.DepartmentID)
                   .Select(p => p.DepartmentName).FirstOrDefault(),
                   Status = user.Status,
                   CreatedAt = user.CreatedAt,
                   RoleID = _context.UserRoles
                       .Where(ur => ur.UserId == user.Id)
                       .Select(ur => ur.RoleId)
                       .FirstOrDefault(),
                   RoleName = _context.UserRoles
                            .Where(ur => ur.UserId == user.Id)
                            .Join(_context.Roles,
                                  ur => ur.RoleId,
                                  r => r.Id,
                                  (ur, r) => r.Name)
                            .FirstOrDefault() // Chỉ lấy role đầu tiên
               })
               .ToListAsync();
            return _mapper.Map<IEnumerable<UserVM>>(users);
        }

        public async Task<IEnumerable<UserVM>> GetByDepartmentId(int id)
        {
            var users = await _context.Users
                .Where(user=> user.DepartmentID==id)
               .Select(user => new UserVM
               {
                   Id = user.Id,
                   UserName = user.UserName,
                   Email = user.Email,
                   PhoneNumber = user.PhoneNumber,
                   DateOfBirth = user.DateOfBirth,
                   Gender = user.Gender,
                   Address = user.Address,
                   Avatar = user.Avatar,
                   NationalID = user.NationalID,
                   DepartmentID = user.DepartmentID,
                   DepartmentName = _context.Departments
                   .Where(d => d.DepartmentID == user.DepartmentID)
                   .Select(p => p.DepartmentName).FirstOrDefault(),
                   Status = user.Status,
                   CreatedAt = user.CreatedAt,
                   RoleID = _context.UserRoles
                       .Where(ur => ur.UserId == user.Id)
                       .Select(ur => ur.RoleId)
                       .FirstOrDefault(),
                   RoleName = _context.UserRoles
                            .Where(ur => ur.UserId == user.Id)
                            .Join(_context.Roles,
                                  ur => ur.RoleId,
                                  r => r.Id,
                                  (ur, r) => r.Name)
                            .FirstOrDefault() // Chỉ lấy role đầu tiên
               })
               .ToListAsync();
            return _mapper.Map<IEnumerable<UserVM>>(users);
        }

        public async Task<UserVM> GetByEmail(string email)
        {
            var users = await _context.Users
                .Where(u=> u.Email==email)
               .Select(user => new UserVM
               {
                   Id = user.Id,
                   UserName = user.UserName,
                   Email = user.Email,
                   PhoneNumber = user.PhoneNumber,
                   DateOfBirth = user.DateOfBirth,
                   Gender = user.Gender,
                   Address = user.Address,
                   Avatar = user.Avatar,
                   NationalID = user.NationalID,
                   DepartmentID = user.DepartmentID,
                   DepartmentName = _context.Departments
                   .Where(d => d.DepartmentID == user.DepartmentID)
                   .Select(p => p.DepartmentName).FirstOrDefault(),
                   Status = user.Status,
                   CreatedAt = user.CreatedAt,
                   RoleID = _context.UserRoles
                       .Where(ur => ur.UserId == user.Id)
                       .Select(ur => ur.RoleId)
                       .FirstOrDefault(),
                   RoleName = _context.UserRoles
                            .Where(ur => ur.UserId == user.Id)
                            .Join(_context.Roles,
                                  ur => ur.RoleId,
                                  r => r.Id,
                                  (ur, r) => r.Name)
                            .FirstOrDefault() // Chỉ lấy role đầu tiên
               })
               .FirstOrDefaultAsync();
            return _mapper.Map<UserVM>(users);
        }

        public async Task<UserVM?> GetById(int id)
        {
            var users = await _context.Users
                  .Where(u => u.Id == id)
                 .Select(user => new UserVM
                 {
                     Id = user.Id,
                     UserName = user.UserName,
                     Email = user.Email,
                     PhoneNumber = user.PhoneNumber,
                     DateOfBirth = user.DateOfBirth,
                     Gender = user.Gender,
                     Address = user.Address,
                     Avatar = user.Avatar,
                     NationalID = user.NationalID,
                     DepartmentID = user.DepartmentID,
                     DepartmentName = _context.Departments
                   .Where(d => d.DepartmentID == user.DepartmentID)
                   .Select(p => p.DepartmentName).FirstOrDefault(),
                     Status = user.Status,
                     CreatedAt = user.CreatedAt,
                     RoleID = _context.UserRoles
                         .Where(ur => ur.UserId == user.Id)
                         .Select(ur => ur.RoleId)
                         .FirstOrDefault(),
                     RoleName = _context.UserRoles
                              .Where(ur => ur.UserId == user.Id)
                              .Join(_context.Roles,
                                    ur => ur.RoleId,
                                    r => r.Id,
                                    (ur, r) => r.Name)
                              .FirstOrDefault() // Chỉ lấy role đầu tiên
                 })
                 .FirstOrDefaultAsync();
            return _mapper.Map<UserVM>(users);
        }

        public async Task<UserVM> GetByName(string name)
        {
            var users = await _context.Users
                  .Where(u => u.UserName != null && u.UserName.Contains(name))
                 .Select(user => new UserVM
                 {
                     Id = user.Id,
                     UserName = user.UserName,
                     Email = user.Email,
                     PhoneNumber = user.PhoneNumber,
                     DateOfBirth = user.DateOfBirth,
                     Gender = user.Gender,
                     Address = user.Address,
                     Avatar = user.Avatar,
                     NationalID = user.NationalID,
                     DepartmentID = user.DepartmentID,
                     DepartmentName = _context.Departments
                     .Where(d => d.DepartmentID == user.DepartmentID)
                     .Select(p => p.DepartmentName).FirstOrDefault(),
                     Status = user.Status,
                     CreatedAt = user.CreatedAt,
                     RoleID = _context.UserRoles
                         .Where(ur => ur.UserId == user.Id)
                         .Select(ur => ur.RoleId)
                         .FirstOrDefault(),
                     RoleName = _context.UserRoles
                              .Where(ur => ur.UserId == user.Id)
                              .Join(_context.Roles,
                                    ur => ur.RoleId,
                                    r => r.Id,
                                    (ur, r) => r.Name)
                              .FirstOrDefault() 
                 })
                 .FirstOrDefaultAsync();
            return _mapper.Map<UserVM>(users);
        }

        public async Task Update(UserVM entity)
        {
            var existingUser = await _context.Users.FindAsync(entity.Id);
            if (existingUser != null)
            {
                var user = new User
                {
                    UserName = existingUser.UserName,
                    Email = existingUser.Email,
                    PhoneNumber = existingUser.PhoneNumber,
                    DateOfBirth = existingUser.DateOfBirth,
                    Gender = existingUser.Gender,
                    Address = existingUser.Address,
                    Avatar = existingUser.Avatar,
                    NationalID = existingUser.NationalID,
                    DepartmentID = existingUser.DepartmentID,
                    Status = existingUser.Status,
                    CreatedAt = existingUser.CreatedAt,
                };
                var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == entity.Id);

                if (userRole == null)
                {
                    // Nếu User không có Role, bạn có thể thêm Role mới cho họ
                    _context.UserRoles.Add(new IdentityUserRole<int>
                    {
                        UserId = entity.Id,
                        RoleId = (int)entity.RoleID!
                    }) ;
                }
                else
                {
                    // Nếu User đã có Role, cập nhật RoleId
                    userRole.RoleId = (int)entity.RoleID!;
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy User với ID = {entity.Id}");
            }
        }
    }
}
