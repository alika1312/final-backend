using api.Dtos; // Adjust namespace as needed
using api.Models;

namespace api.Mappers
{
    public static class BranchMappers
    {
        public static Branch ToBranchFromBranchDto(this BranchDto branchDto)
        {
            return new Branch
            {
                branchName = branchDto.branchName,
                ManagerID = branchDto.ManagerID
            };
        }

        public static BranchDto ToBranchDto(this Branch branch)
        {
            return new BranchDto
            {
                branchID = branch.branchID,
                branchName = branch.branchName,
                ManagerID = branch.ManagerID
            };
        }
    }
}
