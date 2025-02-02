using System.Collections.Generic;
using System.Linq;
using api.Data;
using api.Dtos;
using api.Dtos.Shift;
using api.Models;


namespace api.Mappers
{
    public static class ShiftMappers
    {

        public static ShiftDto ToShiftDto(this Shift shiftModel)
        {
            return new ShiftDto
            {
                shiftID = shiftModel.shiftID,
                date = shiftModel.date,
                extra = shiftModel.extra,
                comment = shiftModel.comment,
                ShiftTypeID = shiftModel.ShiftTypeID,
                ShiftTypeName = shiftModel.ShiftType != null ? shiftModel.ShiftType.shiftTypeName : string.Empty
            };
        }

        public static Shift ToShiftFromCreateDTO(this ShiftRequestDto shiftDto, ApplicationDBContext context)
        {
            var shiftType = context.ShiftType.FirstOrDefault(st => st.shiftTypeID == shiftDto.ShiftTypeID);
            if (shiftType == null)
            {
                throw new ArgumentException("Invalid ShiftTypeID");
            }

            return new Shift
            {
                date = shiftDto.date,
                extra = shiftDto.extra,
                comment = shiftDto.comment,
                ShiftTypeID = shiftDto.ShiftTypeID,
                ShiftType = shiftType
            };
        }

    }
}
