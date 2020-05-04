using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Communication
{
    public class BranchResponse : BaseResponse
    {
        public Branch _branch {get; private set;}

        public BranchResponse(bool success, string message, Branch branch) : base(success, message)
        {
            _branch = branch;
        }

        public BranchResponse(Branch branch) : this(true,string.Empty, branch)
        {

        }

        public BranchResponse(string message) : this(false, message, null)
        {

        }
    }
}