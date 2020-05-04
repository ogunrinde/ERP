using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Communication
{
    public class DepartmentResponse : BaseResponse
    {
        public Department _dept {get; private set;}

        public DepartmentResponse(bool success, string message, Department dept) : base(success, message)
        {
            _dept = dept;
        }

        public DepartmentResponse(Department dept) : this(true,string.Empty, dept)
        {

        }

        public DepartmentResponse(string message) : this(false, message, null)
        {

        }
    }
}