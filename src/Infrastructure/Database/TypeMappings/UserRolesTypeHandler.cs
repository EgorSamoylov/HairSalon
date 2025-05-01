using Dapper;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.TypeMappings
{
    public class UserRolesTypeHandler : SqlMapper.TypeHandler<UserRoles>
    {
        public override void SetValue(IDbDataParameter parameter, UserRoles value)
        {
            parameter.Value = value.ToString();
        }

        public override UserRoles Parse(object? value)
        {
            if (value == null || value == DBNull.Value)
            {
                return UserRoles.User;
            }

            var stringValue = value.ToString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return UserRoles.User;
            }

            return Enum.Parse<UserRoles>(stringValue);
        }
    }
}
