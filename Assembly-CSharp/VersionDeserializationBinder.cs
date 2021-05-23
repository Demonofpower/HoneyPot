using System;
using System.Reflection;
using System.Runtime.Serialization;

// Token: 0x02000139 RID: 313
public sealed class VersionDeserializationBinder : SerializationBinder
{
	// Token: 0x06000760 RID: 1888 RVA: 0x00037524 File Offset: 0x00035724
	public override Type BindToType(string assemblyName, string typeName)
	{
		if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
		{
			assemblyName = Assembly.GetExecutingAssembly().FullName;
			return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
		}
		return null;
	}
}
