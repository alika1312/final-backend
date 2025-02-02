using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class ShiftTypeMappers
    {
        public static ShiftTypeDto ToShiftTypeDto(this ShiftType ShiftType)
        {
            return new ShiftTypeDto
            {
                shiftTypeID = ShiftType.shiftTypeID,
                shiftTypeName = ShiftType.shiftTypeName,

            };
        }

        public static ShiftType ToShiftTypeFromDto(this ShiftTypeRequestDto dto)
        {
            return new ShiftType
            {
                shiftTypeName = dto.shiftTypeName
            };
        }
    }
}