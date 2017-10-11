using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NHibernate.Envers.Tests.Entities.CustomType
{
	public class ParametrizedTestUserType : IUserType, IParameterizedType
	{
		private static readonly SqlType[] TYPES = { new SqlType(DbType.String) };
		private string param1;
		private string param2;

		public new bool Equals(object x, object y)
		{
			//noinspection ObjectEquality
			if (x == y)
			{
				return true;
			}

			if (x == null || y == null)
			{
				return false;
			}

			return x.Equals(y);
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public object NullSafeGet(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
		{
			return NHibernateUtil.String.NullSafeGet(dr, names[0], session);
		}

		public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			if (value != null)
			{
				var v = (string)value;
				if (!v.StartsWith(param1))
				{
					v = param1 + v;
				}
				if (!v.EndsWith(param2))
				{
					v = v + param2;
				}
				NHibernateUtil.String.NullSafeSet(cmd, v, index, session);
			}
			else
			{
				NHibernateUtil.String.NullSafeSet(cmd, value, index, session);
			}
		}

		public object DeepCopy(object value)
		{
			return value;
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}

		public object Assemble(object cached, object owner)
		{
			return cached;
		}

		public object Disassemble(object value)
		{
			return value;
		}

		public SqlType[] SqlTypes
		{
			get { return TYPES; }
		}

		public System.Type ReturnedType
		{
			get { return typeof (string); }
		}

		public bool IsMutable
		{
			get { return false; }
		}

		public void SetParameterValues(IDictionary<string, string> parameters)
		{
			param1 = parameters["param1"];
			param2 = parameters["param2"];
		}
	}
}