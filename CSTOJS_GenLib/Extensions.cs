using System;
using System.Collections;
using System.Reflection;

namespace CSTOJS_GenLib;

	internal static class Extensions
	{
		//https://stackoverflow.com/a/21755933
		public static string? FirstCharToLowerCase(this string? str)
		{
			if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
				return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

			return str;
		}
		public static string? FirstCharToUpperCase(this string? str)
		{
			if (!string.IsNullOrEmpty(str) && char.IsLower(str[0]))
				return str.Length == 1 ? char.ToUpper(str[0]).ToString() : char.ToUpper(str[0]) + str[1..];

			return str;
		}

		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source?.IndexOf(toCheck, comp) >= 0;
		}

		public static string Test(this Object obj) 
		{
			return "sad";
		}

		/*
		public static T Clone<T>(T source)
		{
			JsonSerializerOptions serializeOptions = new();
			var serialized = JsonSerializer.Serialize(source, serializeOptions);
			return JsonSerializer.Deserialize<T>(serialized);
		}*/

		public static object CloneObject(this object objSource)
		{
			//Get the type of source object and create a new instance of that type
			Type typeSource = objSource.GetType();
			object objTarget = Activator.CreateInstance(typeSource);
			//Get all the properties of source object type
			PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			//Assign all source property to taget object 's properties
			foreach (PropertyInfo property in propertyInfo)
			{
				//Check whether property can be written to
				if (property.CanWrite && property.Name != "Item")
				{
					//check whether property type is value type, enum or string type
					if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(string)))
					{
						property.SetValue(objTarget, property.GetValue(objSource, null), null);
					}
					//else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
					else
					{
						object objPropertyValue = null;
						if (property.GetIndexParameters().Length > 0)
						{
							int _length = (objSource as IList).Count;

							for (int i = 0; i < _length; i++)
							{
								objPropertyValue = property.GetValue(objSource, new object[] { (int)i });
								
								objTarget.GetType().GetMethod("Add").Invoke(objTarget, new object[] { null });

								if (objPropertyValue == null)
								{
									property.SetValue(objTarget, null, new object[] { (int)i });
								}
								else
								{
									property.SetValue(objTarget, objPropertyValue.CloneObject(), new object[] { (int)i });
								}
							}
						}
						else
						{
							objPropertyValue = property.GetValue(objSource, null);
						

							if (objPropertyValue == null)
							{
								property.SetValue(objTarget, null, null);
							}
							else
							{
								property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
							}
						}
					}
				}
			}
			return objTarget;
		}

		public static T DeepCloneByReflection<T>(this T source)
		{
			var type = source.GetType();

			var target = Activator.CreateInstance(type);

			foreach (var propertyInfo in type.GetProperties())
			{
				if (propertyInfo.CanWrite && propertyInfo.CanRead)
				{
					// Handle value types and string
					if (propertyInfo.PropertyType.IsValueType ||
					propertyInfo.PropertyType == typeof(string))
					{
						propertyInfo.SetValue(target, propertyInfo.GetValue(source));
					}
					// Handle delegates
					else if (propertyInfo.PropertyType.IsSubclassOf(typeof(Delegate)))
					{
						var value = (Delegate)propertyInfo.GetValue(source);

						if (value != null)
						{
							propertyInfo.SetValue(target, value.Clone());
						}
					}
					// Handle arrays
					else if (propertyInfo.PropertyType.IsSubclassOf(typeof(Array)))
					{
						var value = (Array)propertyInfo.GetValue(source);

						if (value != null)
						{
							propertyInfo.SetValue(target, value.Clone());
						}
					}
					// Handle objects
					else
					{
						var value = propertyInfo.GetValue(source);

						if (value != null)
						{
							propertyInfo.SetValue(target, value.DeepCloneByReflection());
						}
					}
				}
			}

			return (T)target;
		}

	}

	/// <summary>
	/// BaseObject class is an abstract class for you to derive from.
	/// Every class that will be dirived from this class will support the 
	/// Clone method automaticly.<br>
	/// The class implements the interface ICloneable and there 
	/// for every object that will be derived <br>
	/// from this object will support the ICloneable interface as well.
	/// </summary>

	public abstract class BaseObject : ICloneable
	{
		/// <summary>
		/// Clone the object, and returning a reference to a cloned object.
		/// </summary>
		/// <returns>Reference to the new cloned 
		/// object.</returns>
		public object Clone()
		{
			//First we create an instance of this specific type.
			object newObject = Activator.CreateInstance(this.GetType());

			//We get the array of fields for the new type instance.
			PropertyInfo[] fields = newObject.GetType().GetProperties();

			int i = 0;

			foreach (PropertyInfo fi in this.GetType().GetProperties())
			{
				//We query if the fiels support the ICloneable interface.
				Type ICloneType = fi.PropertyType.GetInterface("ICloneable", true);

				if (ICloneType != null)
				{
					//Getting the ICloneable interface from the object.
					ICloneable IClone = (ICloneable)fi.GetValue(this);

					//We use the clone method to set the new value to the field.
					fields[i].SetValue(newObject, IClone.Clone());
				}
				else
				{
					// If the field doesn't support the ICloneable 
					// interface then just set it.
					fields[i].SetValue(newObject, fi.GetValue(this));
				}

				//Now we check if the object support the 
				//IEnumerable interface, so if it does
				//we need to enumerate all its items and check if 
				//they support the ICloneable interface.
				Type IEnumerableType = fi.PropertyType.GetInterface
								("IEnumerable", true);
				if (IEnumerableType != null)
				{
					//Get the IEnumerable interface from the field.
					IEnumerable IEnum = (IEnumerable)fi.GetValue(this);

					//This version support the IList and the 
					//IDictionary interfaces to iterate on collections.
					Type IListType = fields[i].PropertyType.GetInterface
										("IList", true);
					Type IDicType = fields[i].PropertyType.GetInterface
										("IDictionary", true);

					int j = 0;
					if (IListType != null)
					{
						//Getting the IList interface.
						IList list = (IList)fields[i].GetValue(newObject);

						foreach (object obj in IEnum)
						{
							//Checking to see if the current item 
							//support the ICloneable interface.
							ICloneType = obj.GetType().
								GetInterface("ICloneable", true);

							if (ICloneType != null)
							{
								//If it does support the ICloneable interface, 
								//we use it to set the clone of
								//the object in the list.
								ICloneable clone = (ICloneable)obj;

								list[j] = clone.Clone();
							}

							//NOTE: If the item in the list is not 
							//support the ICloneable interface then in the 
							//cloned list this item will be the same 
							//item as in the original list
							//(as long as this type is a reference type).

							j++;
						}
					}
					else if (IDicType != null)
					{
						//Getting the dictionary interface.
						IDictionary dic = (IDictionary)fields[i].
											GetValue(newObject);
						j = 0;

						foreach (DictionaryEntry de in IEnum)
						{
							//Checking to see if the item 
							//support the ICloneable interface.
							ICloneType = de.Value.GetType().
								GetInterface("ICloneable", true);

							if (ICloneType != null)
							{
								ICloneable clone = (ICloneable)de.Value;

								dic[de.Key] = clone.Clone();
							}
							j++;
						}
					}
				}
				i++;
			}
			return newObject;
		}
	}

