using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ValidationAttributeLib
{
    /// <summary>
    /// DataValidation的工具程式
    /// </summary>
    public class ValidationUtility
    {
        /// <summary>
        /// 是否為刪除中的資料
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        static public bool IsDeleting(object instance)
        {
            Type instanceType = instance.GetType();

            PropertyInfo pptInfo = instanceType.GetProperty("DeletedDateTime");
            if (pptInfo == null)
                return false;

            Object valueObj = pptInfo.GetValue(instance);
            if (valueObj == null)
                return false;

            return true;
        }

        /// <summary>
        /// 取得型態為int的屬性的值。若無此欄位或無值，則傳回default(int)。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        static public int GetInt(object instance, string propertyName)
        {
            Type instanceType = instance.GetType();

            PropertyInfo pptInfo = instanceType.GetProperty(propertyName);
            if (pptInfo == null)
                return default(int);

            Object idValueObj = pptInfo.GetValue(instance);
            if (idValueObj == null || !(idValueObj is int))
                return default(int);

            return (int)idValueObj;
        }

        /// <summary>
        /// 取得型態為decimal的屬性的值。若無此欄位或無值，則傳回default(decimal)。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        static public decimal GetDecimal(object instance, string propertyName)
        {
            Type instanceType = instance.GetType();

            PropertyInfo pptInfo = instanceType.GetProperty(propertyName);
            if (pptInfo == null)
                return default(decimal);

            Object idValueObj = pptInfo.GetValue(instance);
            if (idValueObj == null || !(idValueObj is decimal))
                return default(decimal);

            return (decimal)idValueObj;
        }

        /// <summary>
        /// 取得型態為bool的屬性的值。若無此欄位或無值，則傳回null。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        static public bool? GetBoolean(object instance, string propertyName)
        {
            Type instanceType = instance.GetType();

            PropertyInfo pptInfo = instanceType.GetProperty(propertyName);
            if (pptInfo == null)
                return null;

            Object idValueObj = pptInfo.GetValue(instance);
            if (idValueObj == null || !(idValueObj is bool))
                return null;

            return (bool)idValueObj;
        }

        // MetadataTypeAttribute is not supported in System.ComponentModel.Annotations 4.7.0. Wait for future update.
        /*
        /// <summary>
        /// 取得該Property的Display name。注意：該Property必須使用[Display(Name = "起始日期")]的形式，不支援[DisplayName("起始日期")]的形式。若查無資料，則傳回null。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        static public string GetDisplayName(object instance, string propertyName)
        {
            Type type = instance.GetType();
            Type metaDataType = null;

            foreach (MetadataTypeAttribute attrib in type.GetCustomAttributes(typeof(MetadataTypeAttribute), true))
            {
                metaDataType = attrib.MetadataClassType;
            }

            if (metaDataType == null)
            {
                return null;
            }

            DisplayAttribute attr = (DisplayAttribute)metaDataType.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            return (attr != null) ? attr.Name : null;
        }
        */
    }
}
