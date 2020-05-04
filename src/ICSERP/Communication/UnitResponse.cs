using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Communication
{
    public class UnitResponse : BaseResponse
    {
        public Unit _unit {get; private set;}

        public UnitResponse(bool success, string message, Unit unit) : base(success, message)
        {
            _unit = unit;
        }

        public UnitResponse(Unit unit) : this(true,string.Empty, unit)
        {

        }

        public UnitResponse(string message) : this(false, message, null)
        {

        }
    }
}