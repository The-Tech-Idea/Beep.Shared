using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Shared
{
    public static class EntityHelper
    {
        public static EntityStructure GetEntityStructureFromType<T>()
        {
            EntityStructure entity = new EntityStructure();
            Type entityType = typeof(T);

            if (entity.Fields.Count == 0)
            {
                // Iterate over the properties of the type to create fields
                foreach (PropertyInfo propInfo in entityType.GetProperties())
                {
                    EntityField field = new EntityField
                    {
                        fieldname = propInfo.Name,
                        fieldtype = propInfo.PropertyType.FullName
                    };

                    // Additional attributes like Size1, IsAutoIncrement, AllowDBNull, and IsUnique
                    // might not be directly available or applicable for every property type.
                    // You'll need to set these based on the specific needs or defaults.

                    if (field.IsKey)
                    {
                        entity.PrimaryKeys.Add(field);
                    }

                    entity.Fields.Add(field);
                }
            }

            return entity;
        }
        public static EntityStructure GetEntityStructureFromList<T>(List<T> list)
        {
            EntityStructure entity = new EntityStructure();

            // Check if the list is empty. If it's empty, we can still get the structure from the type T.
            if (list == null || !list.Any())
            {
                return GetEntityStructureFromType<T>();
            }

            // Get the type of the items in the list
            Type itemType = typeof(T);

            // Iterate over the properties of the type to create fields
            foreach (PropertyInfo propInfo in itemType.GetProperties())
            {
                EntityField field = new EntityField
                {
                    fieldname = propInfo.Name,
                    fieldtype = propInfo.PropertyType.ToString(),
                    // Additional attributes like Size1, IsAutoIncrement, AllowDBNull, and IsUnique
                    // might need to be inferred or set to default values as they are not directly available from PropertyInfo
                };

                // Additional logic to determine if the field is a key, etc.
                // This might involve custom attributes or conventions

                entity.Fields.Add(field);
            }

            return entity;
        }
        public static EntityStructure GetEntityStructureFromType(Type type)
        {
            EntityStructure entity = new EntityStructure();
            entity.EntityName = type.Name;
            if (entity.Fields.Count == 0)
            {
                // Iterate over the properties of the type to create fields
                foreach (PropertyInfo propInfo in type.GetProperties())
                {
                    entity = SetField(propInfo, entity);
                }
            }
            return entity;
        }
        private static EntityField SetField(PropertyInfo propInfo, EntityField entity)
        {
            DbFieldCategory fldcat = DbFieldCategory.String;
            if (propInfo.PropertyType == typeof(string))
            {
                fldcat = DbFieldCategory.String;
            }
            else if (propInfo.PropertyType == typeof(int) || propInfo.PropertyType == typeof(long) || propInfo.PropertyType == typeof(float) || propInfo.PropertyType == typeof(double) || propInfo.PropertyType == typeof(decimal))
            {
                fldcat = DbFieldCategory.Numeric;
            }
            else if (propInfo.PropertyType == typeof(DateTime))
            {
                fldcat = DbFieldCategory.Date;
            }
            else if (propInfo.PropertyType == typeof(bool))
            {
                fldcat = DbFieldCategory.Boolean;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(Guid))
            {
                fldcat = DbFieldCategory.Guid;
            }
            else if (propInfo.PropertyType == typeof(JsonDocument))
            {
                fldcat = DbFieldCategory.Json;
            }
            else if (propInfo.PropertyType == typeof(XmlDocument))
            {
                fldcat = DbFieldCategory.Xml;
            }
            else if (propInfo.PropertyType == typeof(decimal))
            {
                fldcat = DbFieldCategory.Currency;
            }
            else if (propInfo.PropertyType.IsEnum)
            {
                fldcat = DbFieldCategory.Enum;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            EntityField field = new EntityField
            {
                fieldname = propInfo.Name,
                fieldtype = propInfo.PropertyType.FullName,
                fieldCategory = fldcat
            };
            // Additional attributes like Size1, IsAutoIncrement, AllowDBNull, and IsUnique
            // might not be directly available or applicable for every property type.
            // You'll need to set these based on the specific needs or defaults.

            return field;
        }
        private static EntityField SetField(DataColumn col, EntityField entity)
        {
            DbFieldCategory fldcat = DbFieldCategory.String;
            if (col.DataType != null)
            {
                // set fldcat based on col.DataType using all posibilties
                if (col.DataType == typeof(string))
                {
                    fldcat = DbFieldCategory.String;
                }
                else if (col.DataType == typeof(int) || col.DataType == typeof(long) || col.DataType == typeof(float) || col.DataType == typeof(double) || col.DataType == typeof(decimal))
                {
                    fldcat = DbFieldCategory.Numeric;
                }
                else if (col.DataType == typeof(DateTime))
                {
                    fldcat = DbFieldCategory.Date;
                }
                else if (col.DataType == typeof(bool))
                {
                    fldcat = DbFieldCategory.Boolean;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(Guid))
                {
                    fldcat = DbFieldCategory.Guid;
                }
                else if (col.DataType == typeof(JsonDocument))
                {
                    fldcat = DbFieldCategory.Json;
                }
                else if (col.DataType == typeof(XmlDocument))
                {
                    fldcat = DbFieldCategory.Xml;
                }
                else if (col.DataType == typeof(decimal))
                {
                    fldcat = DbFieldCategory.Currency;
                }
                else if (col.DataType.IsEnum)
                {
                    fldcat = DbFieldCategory.Enum;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
                else if (col.DataType == typeof(byte[]))
                {
                    fldcat = DbFieldCategory.Binary;
                }
            }

            EntityField field = new EntityField
            {
                EntityName = col.Table.TableName,
                fieldname = col.ColumnName,
                fieldtype = col.DataType.ToString(),
                fieldCategory = fldcat,
                ValueRetrievedFromParent = false,
                FieldIndex = col.Ordinal

            };
            // Additional attributes like Size1, IsAutoIncrement, AllowDBNull, and IsUnique
            // might not be directly available or applicable for every property type.
            // You'll need to set these based on the specific needs or defaults.

            return field;
        }
        private static EntityStructure SetField(PropertyInfo propInfo, EntityStructure entity)
        {
            DbFieldCategory fldcat = DbFieldCategory.String;
            if (propInfo.PropertyType == typeof(string))
            {
                fldcat = DbFieldCategory.String;
            }
            else if (propInfo.PropertyType == typeof(int) || propInfo.PropertyType == typeof(long) || propInfo.PropertyType == typeof(float) || propInfo.PropertyType == typeof(double) || propInfo.PropertyType == typeof(decimal))
            {
                fldcat = DbFieldCategory.Numeric;
            }
            else if (propInfo.PropertyType == typeof(DateTime))
            {
                fldcat = DbFieldCategory.Date;
            }
            else if (propInfo.PropertyType == typeof(bool))
            {
                fldcat = DbFieldCategory.Boolean;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(Guid))
            {
                fldcat = DbFieldCategory.Guid;
            }
            else if (propInfo.PropertyType == typeof(JsonDocument))
            {
                fldcat = DbFieldCategory.Json;
            }
            else if (propInfo.PropertyType == typeof(XmlDocument))
            {
                fldcat = DbFieldCategory.Xml;
            }
            else if (propInfo.PropertyType == typeof(decimal))
            {
                fldcat = DbFieldCategory.Currency;
            }
            else if (propInfo.PropertyType.IsEnum)
            {
                fldcat = DbFieldCategory.Enum;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            else if (propInfo.PropertyType == typeof(byte[]))
            {
                fldcat = DbFieldCategory.Binary;
            }
            EntityField field = new EntityField
            {
                fieldname = propInfo.Name,
                fieldtype = propInfo.PropertyType.FullName,
                fieldCategory = fldcat
            };
            // Additional attributes like Size1, IsAutoIncrement, AllowDBNull, and IsUnique
            // might not be directly available or applicable for every property type.
            // You'll need to set these based on the specific needs or defaults.
            if (field.IsKey)
            {
                entity.PrimaryKeys.Add(field);
            }
            entity.Fields.Add(field);
            return entity;
        }
        public static EntityStructure GetEntityStructureFromListorTable(dynamic retval)
        {
            EntityStructure entity = new EntityStructure();
            Type tp = retval.GetType();
            entity.EntityName = tp.Name;
            DataTable dt;
            if (entity.Fields.Count == 0)
            {
                if (tp.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IList)))
                {
                    dt = ToDataTable((IList)retval, GetListType(tp));
                }
                else
                {
                    dt = (DataTable)retval;

                }
                foreach (DataColumn item in dt.Columns)
                {
                    EntityField x = new EntityField();
                    try
                    {
                        x.fieldname = item.ColumnName;
                        x.fieldtype = item.DataType.ToString(); //"ColumnSize"
                        DbFieldCategory fieldCategory = DbFieldCategory.String;
                        if (item.DataType == typeof(string))
                        {
                            fieldCategory = DbFieldCategory.String;
                        }
                        else if (item.DataType == typeof(int) || item.DataType == typeof(long) || item.DataType == typeof(float) || item.DataType == typeof(double) || item.DataType == typeof(decimal))
                        {
                            fieldCategory = DbFieldCategory.Numeric;
                        }
                        else if (item.DataType == typeof(DateTime))
                        {
                            fieldCategory = DbFieldCategory.Date;
                        }
                        else if (item.DataType == typeof(bool))
                        {
                            fieldCategory = DbFieldCategory.Boolean;
                        }
                        else if (item.DataType == typeof(byte[]))
                        {
                            fieldCategory = DbFieldCategory.Binary;
                        }
                        else if (item.DataType == typeof(Guid))
                        {
                            fieldCategory = DbFieldCategory.Guid;
                        }
                        else if (item.DataType == typeof(JsonDocument))
                        {
                            fieldCategory = DbFieldCategory.Json;
                        }
                        else if (item.DataType == typeof(XmlDocument))
                        {
                            fieldCategory = DbFieldCategory.Xml;
                        }
                        else if (item.DataType == typeof(decimal))
                        {
                            fieldCategory = DbFieldCategory.Currency;
                        }
                        else if (item.DataType.IsEnum)
                        {
                            fieldCategory = DbFieldCategory.Enum;
                        }
                        x.fieldCategory = fieldCategory;
                        x.Size1 = item.MaxLength;
                        try
                        {
                            x.IsAutoIncrement = item.AutoIncrement;
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            x.AllowDBNull = item.AllowDBNull;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            x.IsUnique = item.Unique;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                       Console.WriteLine("Error in Creating Field Type");
                      
                    }

                    if (x.IsKey)
                    {
                        entity.PrimaryKeys.Add(x);
                    }


                    entity.Fields.Add(x);
                }

            }
            return entity;
        }
        public static List<object> ConvertTableToList(DataTable dt, EntityStructure ent, Type enttype)
        {
            List<object> retval = new List<object>();

            foreach (DataRow dr in dt.Rows)
            {
                var entityInstance = Activator.CreateInstance(enttype);
                foreach (EntityField col in ent.Fields)
                {
                    PropertyInfo propertyInfo = entityInstance.GetType().GetProperty(col.fieldname);
                    if (propertyInfo != null && dr[col.fieldname] != DBNull.Value)
                    {
                        try
                        {
                            object value = Convert.ChangeType(dr[col.fieldname], propertyInfo.PropertyType);
                            propertyInfo.SetValue(entityInstance, value);
                        }
                        catch (Exception ex)
                        {
                            // Log the exception or handle it accordingly
                            // Consider how you want to handle the case where the conversion fails
                        }
                    }
                    else if (propertyInfo != null)
                    {
                        // If it's DBNull, but the property exists, set the property to its default value
                        var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
                        propertyInfo.SetValue(entityInstance, defaultValue);
                    }
                }
                retval.Add(entityInstance);
            }

            // If DataTable is empty, add a default instance with null properties
            if (dt.Rows.Count == 0)
            {
                var entityInstance = Activator.CreateInstance(enttype);
                foreach (EntityField col in ent.Fields)
                {
                    PropertyInfo propertyInfo = entityInstance.GetType().GetProperty(col.fieldname);
                    if (propertyInfo != null)
                    {
                        var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
                        propertyInfo.SetValue(entityInstance, defaultValue);
                    }
                }
                retval.Add(entityInstance);
            }

            return retval;
        }
        public static DataTable ToDataTable(IList list, Type tp)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(tp);
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (var item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }
        public static DataTable ToDataTable(Type tp)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(tp);
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }


            return table;
        }
        public static DataTable ToDataTable(IEntityStructure entity)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < entity.Fields.Count; i++)
            {
                EntityField prop = entity.Fields[i];
                table.Columns.Add(prop.fieldname, Type.GetType(prop.fieldtype));
            }
            return table;
        }
        public static DataTable ToDataTable(IEntityStructure entity, Type tp)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < entity.Fields.Count; i++)
            {
                EntityField prop = entity.Fields[i];
                table.Columns.Add(prop.fieldname, Type.GetType(prop.fieldtype));
            }
            return table;
        }
        public static DataTable ToDataTable(IEntityStructure entity, List<object> list)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < entity.Fields.Count; i++)
            {
                EntityField prop = entity.Fields[i];
                table.Columns.Add(prop.fieldname, Type.GetType(prop.fieldtype));
            }
            foreach (var item in list)
            {
                DataRow dr = table.NewRow();
                foreach (EntityField prop in entity.Fields)
                {
                    dr[prop.fieldname] = GetPropertyValue(item, prop.fieldname);
                }
                table.Rows.Add(dr);
            }
            return table;
        }
        public static DataTable ToDataTable(IEntityStructure entity, List<object> list, Type tp)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < entity.Fields.Count; i++)
            {
                EntityField prop = entity.Fields[i];
                table.Columns.Add(prop.fieldname, Type.GetType(prop.fieldtype));
            }
            foreach (var item in list)
            {
                DataRow dr = table.NewRow();
                foreach (EntityField prop in entity.Fields)
                {
                    dr[prop.fieldname] = GetPropertyValue(item, prop.fieldname);
                }
                table.Rows.Add(dr);
            }
            return table;
        }
        public static ObservableBindingList<T> ConvertListToObservableBindingList<T>(List<T> list)
        where T : class, INotifyPropertyChanged, new()
        {
            ObservableBindingList<T> retval = new ObservableBindingList<T>(list);
          
            return retval;
        }
        public static ObservableBindingList<T> ConvertDataTableToObservableBindingList<T>(DataTable dt)
        where T : class, INotifyPropertyChanged, new()
        {
            ObservableBindingList<T> retval = new ObservableBindingList<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T entityInstance = new T();
                foreach (DataColumn col in dt.Columns)
                {
                    PropertyInfo propertyInfo = entityInstance.GetType().GetProperty(col.ColumnName);
                    if (propertyInfo != null && dr[col.ColumnName] != DBNull.Value)
                    {
                        try
                        {
                            object value = Convert.ChangeType(dr[col.ColumnName], propertyInfo.PropertyType);
                            propertyInfo.SetValue(entityInstance, value);
                        }
                        catch (Exception ex)
                        {
                            // Log the exception or handle it accordingly
                            // Consider how you want to handle the case where the conversion fails
                        }
                    }
                    else if (propertyInfo != null)
                    {
                        // If it's DBNull, but the property exists, set the property to its default value
                        var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
                        propertyInfo.SetValue(entityInstance, defaultValue);
                    }
                }
                retval.Add(entityInstance);
            }
            // If DataTable is empty, add a default instance with null properties
            if (dt.Rows.Count == 0)
            {
                T entityInstance = new T();
                foreach (DataColumn col in dt.Columns)
                {
                    PropertyInfo propertyInfo = entityInstance.GetType().GetProperty(col.ColumnName);
                    if (propertyInfo != null)
                    {
                        var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
                        propertyInfo.SetValue(entityInstance, defaultValue);
                    }
                }
                retval.Add(entityInstance);
            }
            return retval;
        }
        public static List<T> ConvertObservableBindingListToList<T>(ObservableBindingList<T> list)
        where T : class, INotifyPropertyChanged, new()
        {
            List<T> retval = new List<T>();
            foreach (var item in list)
            {
                retval.Add(item);
            }
            return retval;
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable table = new DataTable();
            if (items.Count == 0)
            {
                return table;
            }
            PropertyInfo[] properties = items[0].GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                table.Columns.Add(pi.Name, pi.PropertyType);
            }
            foreach (T item in items)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo pi in properties)
                {
                    row[pi.Name] = pi.GetValue(item, null);
                }
                table.Rows.Add(row);
            }
            return table;
        }
        public static DataTable ToDataTable<T>(List<T> items, Type tp)
        {
            DataTable table = new DataTable();
            if (items.Count == 0)
            {
                return table;
            }
            PropertyInfo[] properties = items[0].GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                table.Columns.Add(pi.Name, pi.PropertyType);
            }
            foreach (T item in items)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo pi in properties)
                {
                    row[pi.Name] = pi.GetValue(item, null);
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static Type GetListType(object someList)
        {
            if (someList == null)
                return null;

            var type = someList.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
                return null;

            return type.GetGenericArguments()[0];
        }
        /// <summary>
        /// Retrieves all available IBeepUIComponent implementations across loaded assemblies,
        /// instantiates them, and returns a dictionary mapping their GuidID to the instance.
        /// </summary>
        /// <returns>
        /// A dictionary where the key is the component's GuidID and the value is the IBeepUIComponent instance.
        /// </returns>
        public static Dictionary<string, IBeepUIComponent> GetAllAvailableUIComponents()
        {
            var uiComponents = new Dictionary<string, IBeepUIComponent>();

            // Get all loaded assemblies in the current AppDomain
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Handle the case where some types could not be loaded
                    types = ex.Types.Where(t => t != null).ToArray();
                }
                catch
                {
                    // If any other exception occurs, skip this assembly
                    continue;
                }

                foreach (var type in types)
                {
                    // Skip abstract classes and interfaces
                    if (type.IsAbstract || type.IsInterface)
                        continue;

                    // Check if the type implements IBeepUIComponent
                    if (!typeof(IBeepUIComponent).IsAssignableFrom(type))
                        continue;

                    // Ensure the type has a parameterless constructor
                    if (type.GetConstructor(Type.EmptyTypes) == null)
                        continue;

                    IBeepUIComponent componentInstance = null;

                    try
                    {
                        // Instantiate the component
                        componentInstance = (IBeepUIComponent)Activator.CreateInstance(type);
                    }
                    catch (Exception ex)
                    {
                        // Handle instantiation exceptions (e.g., constructor throws)
                        // Log the error or handle accordingly
                        Console.WriteLine($"Error creating instance of {type.FullName}: {ex.Message}");
                        continue;
                    }

                    if (componentInstance != null)
                    {
                        // Ensure the component has a valid GuidID
                        if (string.IsNullOrEmpty(componentInstance.GuidID))
                        {
                            // Optionally, generate a new GUID if GuidID is not set
                            componentInstance.GuidID = Guid.NewGuid().ToString();
                        }

                        // Check for duplicate GuidID
                        if (uiComponents.ContainsKey(componentInstance.GuidID))
                        {
                            Console.WriteLine($"Duplicate GuidID detected: {componentInstance.GuidID} for type {type.FullName}. Skipping.");
                            continue;
                        }

                        // Add to the dictionary
                        uiComponents.Add(componentInstance.GuidID, componentInstance);
                    }
                }
            }

            return uiComponents;
        }
        public static object GetPropertyValue(dynamic entity, string propertyName)
        {
            try
            {
                var property = entity?.GetType().GetProperty(propertyName);
                return property?.GetValue(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing property '{propertyName}': {ex.Message}");
                return null;
            }

        }
        // GetPropertyValue that will return tuple with object and value
        public static (object, object) GetPropertyValueWithObject(dynamic entity, string propertyName)
        {
            try
            {
                var property = entity?.GetType().GetProperty(propertyName);
                return (property, property?.GetValue(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing property '{propertyName}': {ex.Message}");
                return (null, null);
            }
        }
        // GetPropertyValue that will return tuple with Type of property and value
        public static (Type, object) GetPropertyValueWithType(dynamic entity, string propertyName)
        {
            try
            {
                var property = entity?.GetType().GetProperty(propertyName);
                return (property?.PropertyType, property?.GetValue(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing property '{propertyName}': {ex.Message}");
                return (null, null);
            }
        }
        #region "IBeep Logic"

        // Define format strings for various MaskFormats
        private static string _currencyFormat = "C2";       // Currency with 2 decimal places
        private static string _percentageFormat = "P2";     // Percentage with 2 decimal places
        private static string _decimalFormat = "N2";        // Number with 2 decimal places
        private static string _monthYearFormat = "MM/yyyy"; // Month and Year format
        private static string _dateFormat = "MM/dd/yyyy";    // Date format
        private static string _timeFormat = "HH:mm:ss";      // Time format
        private static string _dateTimeFormat = "MM/dd/yyyy HH:mm:ss"; // DateTime format

        public static bool ValidateData(TextBoxMaskFormat MaskFormat, string text,string CustomMask, out string message)
        {
            // Implement validation logic based on MaskFormat and other properties
            switch (MaskFormat)
            {
                case TextBoxMaskFormat.Currency:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "This field is required.";
                        return false;
                    }
                    if (!IsValidCurrency(text))
                    {
                        message = $"Invalid currency format. Expected format: {_currencyFormat}.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Percentage:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "This field is required.";
                        return false;
                    }
                    if (!IsValidPercentage(text))
                    {
                        message = $"Invalid percentage format. Expected format: {_percentageFormat}.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Decimal:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "This field is required.";
                        return false;
                    }
                    if (!IsValidDecimal(text))
                    {
                        message = $"Invalid decimal format. Expected format: {_decimalFormat}.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.CreditCard:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Credit card number is required.";
                        return false;
                    }
                    if (!IsValidCreditCard(text))
                    {
                        message = "Invalid credit card number.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.IPAddress:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "IP address is required.";
                        return false;
                    }
                    if (!IsValidIPAddress(text))
                    {
                        message = "Invalid IP address format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.PhoneNumber:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Phone number is required.";
                        return false;
                    }
                    if (!IsValidPhoneNumber(text))
                    {
                        message = "Invalid phone number format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.SocialSecurityNumber:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Social Security Number is required.";
                        return false;
                    }
                    if (!IsValidSocialSecurityNumber(text))
                    {
                        message = "Invalid Social Security Number format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.ZipCode:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Zip Code is required.";
                        return false;
                    }
                    if (!IsValidZipCode(text))
                    {
                        message = "Invalid Zip Code format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Year:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Year is required.";
                        return false;
                    }
                    if (!IsValidYear(text))
                    {
                        message = "Invalid year format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.MonthYear:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Month and Year are required.";
                        return false;
                    }
                    if (!IsValidMonthYear(text))
                    {
                        message = $"Invalid Month-Year format. Expected format: {_monthYearFormat}.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Email:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Email is required.";
                        return false;
                    }
                    if (!IsValidEmail(text))
                    {
                        message = "Invalid email format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.URL:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "URL is required.";
                        return false;
                    }
                    if (!IsValidUrl(text))
                    {
                        message = "Invalid URL format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Date:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Date is required.";
                        return false;
                    }
                    if (!IsValidDate(text))
                    {
                        message = $"Invalid date format. Expected format: {_dateFormat}.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Time:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Time is required.";
                        return false;
                    }
                    if (!IsValidTime(text))
                    {
                        message = $"Invalid time format. Expected format: {_timeFormat}.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.DateTime:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Date and Time are required.";
                        return false;
                    }
                    if (!IsValidDateTime(text))
                    {
                        message = $"Invalid date and time format. Expected format: {_dateTimeFormat}.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.TimeSpan:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Time span is required.";
                        return false;
                    }
                    if (!IsValidTimeSpan(text))
                    {
                        message = "Invalid time span format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Hexadecimal:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Hexadecimal input is required.";
                        return false;
                    }
                    if (!IsValidHexadecimal(text))
                    {
                        message = "Invalid hexadecimal format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Custom:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "This field is required.";
                        return false;
                    }
                    if (!IsValidCustomMask(text,CustomMask))
                    {
                        message = "Invalid format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Alphanumeric:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "This field is required.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Numeric:
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Numeric input is required.";
                        return false;
                    }
                    if (!IsValidNumeric(text))
                    {
                        message = "Invalid numeric format.";
                        return false;
                    }
                    break;

                case TextBoxMaskFormat.Password:
                    // Implement password strength validation if necessary
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        message = "Password cannot be empty.";
                        return false;
                    }
                    // Example: Enforce minimum length
                    if (text.Length < 8)
                    {
                        message = "Password must be at least 8 characters long.";
                        return false;
                    }
                    // Additional password strength checks can be added here
                    break;

                case TextBoxMaskFormat.None:
                default:
                    // No specific validation
                    break;
            }

            message = "Valid";
            return true;
        }

        #region "Helper Validation Methods"

        private static bool IsValidCurrency(string input)
        {
            // Attempt to parse the input as currency using the specified format
            return decimal.TryParse(input, NumberStyles.Currency, CultureInfo.CurrentCulture, out _);
        }

        private static bool IsValidPercentage(string input)
        {
            // Attempt to parse the input as percentage using the specified format dont use number styles

            if (decimal.TryParse(input, out decimal value))
            {
                return value >= 0 && value <= 1;
            }else
                return false;


        }

        private static bool IsValidDecimal(string input)
        {
            // Attempt to parse the input as decimal using the specified format
            return decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out _);
        }

        private static bool IsValidCreditCard(string input)
        {
            // Remove spaces and hyphens
            input = input.Replace(" ", "").Replace("-", "");

            // Check if all characters are digits
            if (!Regex.IsMatch(input, @"^\d+$"))
                return false;

            // Implement Luhn Algorithm
            int sum = 0;
            bool alternate = false;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(input[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }

        private static bool IsValidIPAddress(string ip)
        {
            return System.Net.IPAddress.TryParse(ip, out _);
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Simple regex for phone numbers (can be enhanced based on requirements)
            // This regex allows digits, spaces, dashes, parentheses, and plus sign
            return Regex.IsMatch(phoneNumber, @"^[\d\s\-\+\(\)]+$");
        }

        private static bool IsValidSocialSecurityNumber(string ssn)
        {
            // US SSN format: "123-45-6789"
            return Regex.IsMatch(ssn, @"^\d{3}-\d{2}-\d{4}$");
        }

        private static bool IsValidZipCode(string zipCode)
        {
            // US Zip Code formats: "12345" or "12345-6789"
            return Regex.IsMatch(zipCode, @"^\d{5}(-\d{4})?$");
        }

        private static bool IsValidYear(string year)
        {
            if (int.TryParse(year, out int y))
            {
                return y >= 1900 && y <= DateTime.Now.Year + 10; // Adjust range as needed
            }
            return false;
        }

        private static bool IsValidMonthYear(string monthYear)
        {
            // Expected format: "MM/yyyy" or "MMMM yyyy" based on _monthYearFormat
            return DateTime.TryParseExact(
                monthYear,
                _monthYearFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out _);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private static bool IsValidDate(string date)
        {
            return DateTime.TryParseExact(
                date,
                _dateFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out _);
        }

        private static bool IsValidTime(string time)
        {
            return DateTime.TryParseExact(
                time,
                _timeFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out _);
        }

        private static bool IsValidDateTime(string dateTime)
        {
            return DateTime.TryParseExact(
                dateTime,
                _dateTimeFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out _);
        }

        private static bool IsValidTimeSpan(string timeSpan)
        {
            return TimeSpan.TryParse(timeSpan, out _);
        }

        private static bool IsValidHexadecimal(string hex)
        {
            return Regex.IsMatch(hex, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private static bool IsValidCustomMask(string input,string CustomMask)
        {
            if (string.IsNullOrEmpty(CustomMask))
            {
                return true; // No custom mask defined, consider it valid
            }

            // Example implementation: Use regular expressions based on CustomMask
            // Assume CustomMask contains a valid regex pattern
            try
            {
                return Regex.IsMatch(input, CustomMask);
            }
            catch
            {
                // If the CustomMask is an invalid regex, consider the input invalid
                return false;
            }
        }

        private static bool IsValidNumeric(string input)
        {
            return double.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out _);
        }

        #endregion "Helper Validation Methods"

        #endregion "IBeep Logic"

        #region "Deep Copy"
        public static object DeepCopy(object obj)
        {
            if (obj == null) return null;

            object copy = Activator.CreateInstance(obj.GetType());

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(obj);
                    prop.SetValue(copy, value);
                }
            }

            return copy;
        }
        public static T DeepCopy<T>(T obj)
        {
            if (obj == null) return default;
            T copy = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(obj);
                    prop.SetValue(copy, value);
                }
            }
            return copy;
        }
        public static T DeepCopy<T>(T obj, Dictionary<object, object> copiedObjects)
        {
            if (obj == null) return default;
            if (copiedObjects.ContainsKey(obj))
            {
                return (T)copiedObjects[obj];
            }
            T copy = Activator.CreateInstance<T>();
            copiedObjects.Add(obj, copy);
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(obj);
                    if (value != null)
                    {
                        if (value.GetType().IsClass && value.GetType() != typeof(string))
                        {
                            prop.SetValue(copy, DeepCopy(value, copiedObjects));
                        }
                        else
                        {
                            prop.SetValue(copy, value);
                        }
                    }
                }
            }
            return copy;
        }
        public static object DeepCopy(object obj, Dictionary<object, object> copiedObjects)
        {
            if (obj == null) return null;
            if (copiedObjects.ContainsKey(obj))
            {
                return copiedObjects[obj];
            }
            object copy = Activator.CreateInstance(obj.GetType());
            copiedObjects.Add(obj, copy);
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(obj);
                    if (value != null)
                    {
                        if (value.GetType().IsClass && value.GetType() != typeof(string))
                        {
                            prop.SetValue(copy, DeepCopy(value, copiedObjects));
                        }
                        else
                        {
                            prop.SetValue(copy, value);
                        }
                    }
                }
            }
            return copy;
        }
        public static T DeepCopy<T>(T obj, Func<T, T> copyAction)
        {
            if (obj == null) return default;
            return copyAction(obj);
        }
        public static object DeepCopy(object obj, Func<object, object> copyAction)
        {
            if (obj == null) return null;
            return copyAction(obj);
        }

        public static  object DeepCopyUsingSerialize(object obj)
        {
            if (obj == null) return null;

          var json = JsonSerializer.Serialize(obj);
          return JsonSerializer.Deserialize(json, obj.GetType());
        }

    #endregion "Deep Copy"
}
}
