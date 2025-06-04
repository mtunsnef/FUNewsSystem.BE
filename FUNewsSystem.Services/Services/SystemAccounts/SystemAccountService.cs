using AutoMapper;
using FUNewsSystem.Domain.Exceptions.Http;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.SystemAccounts;
using FUNewsSystem.Services.DTOs.Request.SystemAccountDto;
using FUNewsSystem.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.SystemAccounts
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public SystemAccountService(ISystemAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public IQueryable<SystemAccount> GetAll()
        {
            return _accountRepository.GetAll();
        }

        public SystemAccount GetById(short id)
        {
            var account = _accountRepository.GetById(id);
            if (account == null)
                throw new NotFoundException($"SystemAccount with Id {id} not found.");

            return account;
        }

        public async Task<ApiResponseDto<string>> CreateAsync(CreateUpdateSystemAccountDto dto)
        {
            var exists = _accountRepository.GetAll().Any(a => a.AccountEmail == dto.Email);
            if (exists)
                throw new ConflictException("Email is already in use.");

            var account = _mapper.Map<SystemAccount>(dto);
            _accountRepository.Add(account);
            await _accountRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("Account created successfully.");
        }

        public async Task<ApiResponseDto<string>> UpdateAsync(short id, CreateUpdateSystemAccountDto dto)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new NotFoundException($"SystemAccount with Id {id} not found.");

            _mapper.Map(dto, account);
            _accountRepository.Update(account);
            await _accountRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("Account updated successfully.");
        }

        public async Task<ApiResponseDto<string>> DeleteAsync(short id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new NotFoundException($"SystemAccount with Id {id} not found.");

            _accountRepository.Delete(account);
            await _accountRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("Account deleted successfully.");
        }

        public async Task<SystemAccount?> GetUserByIdAsync(short id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new NotFoundException($"SystemAccount with Id {id} not found.");

            return account;
        }
        public async Task<SystemAccount?> GetAccountByEmail(string email)
        {
           return await _accountRepository.GetAccountByEmail(email);
        }
    }
}
