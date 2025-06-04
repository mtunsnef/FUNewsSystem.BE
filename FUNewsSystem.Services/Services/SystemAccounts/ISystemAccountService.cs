using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.SystemAccountDto;
using FUNewsSystem.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.SystemAccounts
{
    public interface ISystemAccountService
    {
        IQueryable<SystemAccount> GetAll();
        SystemAccount GetById(short id);
        Task<SystemAccount?> GetUserByIdAsync(short id);
        Task<ApiResponseDto<string>> CreateAsync(CreateUpdateSystemAccountDto dto);
        Task<ApiResponseDto<string>> UpdateAsync(short id, CreateUpdateSystemAccountDto dto);
        Task<ApiResponseDto<string>> DeleteAsync(short id);
        Task<SystemAccount?> GetAccountByEmail(string email);
    }
}
