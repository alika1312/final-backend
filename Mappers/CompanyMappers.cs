using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Mappers
{
    namespace api.Mappers
    {
        public static class CompanyMappers
        {
            public static CompanyDto ToCompanyDto(this Company company)
            {
                return new CompanyDto
                {
                    companyID = company.companyID,
                    companyName = company.companyName,
                    userNames = company.users
                        .Where(u => !string.IsNullOrEmpty(u.UserName))
                        .Select(u => u.UserName!)
                        .ToList()
                };
            }


            public static Company ToCompanyFromDto(this CompanyRequestDto dto)
            {
                return new Company
                {
                    companyName = dto.companyName
                };
            }
        }
    }

}