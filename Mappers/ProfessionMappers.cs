using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class ProfessionMappers
    {
        public static ProfessionDto ToProfessionDto(this Profession profession)
        {
            return new ProfessionDto
            {
                professionID = profession.professionID,
                professionName = profession.professionName
            };
        }

        public static Profession ToProfession(this CreateProfessionDto createProfessionDto)
        {
            return new Profession
            {
                professionName = createProfessionDto.professionName
            };
        }
    }
}