using System.Linq;
using AccountInfoWebApi.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;

namespace AccountInfoWebApi.Utility
{
    public static class CommonFunction
    {
        #region Variables
        private static List<ErrorException> ValidationList = new List<ErrorException>();
        #endregion

        #region ValidateListData
        public static List<ErrorException> ValidateListData<T>(List<T> requestedDataList)
        {
            if (requestedDataList != null || requestedDataList.Count > 0)
            {
                foreach (var DataContext in requestedDataList)
                {
                    var ErrorDetail = DataValidation(DataContext);
                    if (ErrorDetail != null)
                    {
                        ValidationList.Add(ErrorDetail);
                    }
                }
            }
            return ValidationList;
        }
        public static ErrorException ValidData<T>(T RequestedData)
        {
            return DataValidation(RequestedData);
        }
        #endregion

        #region Validation
        private static ErrorException DataValidation<T>(T requestedData)
        {
            ErrorException Errors = null;
            if (requestedData != null)
            {
                var context = new ValidationContext(requestedData, null, null);
                var result = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(requestedData, context, result, true);
                if (result.Count > 0)
                {
                    Errors = new ErrorException();
                    Errors.ErrorTitle = "There are certain invalid values within requested data.";
                    Errors.ExceptionMessage = result.FirstOrDefault(ex => ex.ErrorMessage != null).ErrorMessage.ToString();
                }
            }
            else
            {
                Errors = new ErrorException();
                Errors.ErrorTitle = "The requested data can't be null or empty.";
            }
            return Errors;
        }
        #endregion


        #region Convert Datatabele To List
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) {
                            throw ex;
                        }
                    }
                }
                return objT;
            }).ToList();
        }
        #endregion
    }
}
