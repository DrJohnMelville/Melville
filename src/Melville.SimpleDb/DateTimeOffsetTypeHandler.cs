using System;
using System.Data;
using Dapper;

namespace Melville.SimpleDb;

public class DateTimeOffsetTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
{
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value) => 
        parameter.Value = value.ToUnixTimeMilliseconds();

    public override DateTimeOffset Parse(object value) => 
        DateTimeOffset.FromUnixTimeMilliseconds((long)value);
}