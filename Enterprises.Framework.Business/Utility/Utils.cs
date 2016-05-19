using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Configuration;
using System.Reflection;
using System.Net.Sockets;

namespace Common
{
    /// <summary>
    /// ϵͳ������
    /// </summary>
    public class Utils
    {
        #region ����ת������
        /// <summary>
        /// �ж϶����Ƿ�ΪInt32���͵�����
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;

        }

        /// <summary>
        /// �ж϶����Ƿ�ΪInt32���͵�����
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �Ƿ�ΪDouble����
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }

        /// <summary>
        /// ���ַ���ת��Ϊ����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>�ַ�������</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        /// ������ת��Ϊ�ַ���
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">�ָ���</param>
        /// <returns>String</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// object��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="expression">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// ���ַ���ת��ΪInt32����
        /// </summary>
        /// <param name="expression">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// Object��ת��Ϊdecimal��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����decimal���ͽ��</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string��ת��Ϊdecimal��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����decimal���ͽ��</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// Object��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
                return StrToFloat(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// ������ת��Ϊ����ʱ������
        /// </summary>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// ������ת��Ϊ����ʱ������
        /// </summary>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// ������ת��Ϊ����ʱ������
        /// </summary>
        /// <param name="obj">Ҫת���Ķ���</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// ������ת��Ϊ����ʱ������
        /// </summary>
        /// <param name="obj">Ҫת���Ķ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        /// <summary>
        /// ������ת��Ϊ�ַ���
        /// </summary>
        /// <param name="obj">Ҫת���Ķ���</param>
        /// <returns>ת�����string���ͽ��</returns>
        public static string ObjectToStr(object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString().Trim();
        }
        #endregion

        #region �ָ��ַ���
        /// <summary>
        /// �ָ��ַ���
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// �ָ��ַ���
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region ��ȡ�ַ���
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string str = p_SrcString;
            byte[] bytes = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char ch in Encoding.UTF8.GetChars(bytes))
            {
                if (((ch > '?') && (ch < 'һ')) || ((ch > 0xac00) && (ch < 0xd7a3)))
                {
                    if (p_StartIndex >= p_SrcString.Length)
                    {
                        return "";
                    }
                    return p_SrcString.Substring(p_StartIndex, ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }
            if (p_Length < 0)
            {
                return str;
            }
            byte[] sourceArray = Encoding.Default.GetBytes(p_SrcString);
            if (sourceArray.Length <= p_StartIndex)
            {
                return str;
            }
            int length = sourceArray.Length;
            if (sourceArray.Length > (p_StartIndex + p_Length))
            {
                length = p_Length + p_StartIndex;
            }
            else
            {
                p_Length = sourceArray.Length - p_StartIndex;
                p_TailString = "";
            }
            int num2 = p_Length;
            int[] numArray = new int[p_Length];
            byte[] destinationArray = null;
            int num3 = 0;
            for (int i = p_StartIndex; i < length; i++)
            {
                if (sourceArray[i] > 0x7f)
                {
                    num3++;
                    if (num3 == 3)
                    {
                        num3 = 1;
                    }
                }
                else
                {
                    num3 = 0;
                }
                numArray[i] = num3;
            }
            if ((sourceArray[length - 1] > 0x7f) && (numArray[p_Length - 1] == 1))
            {
                num2 = p_Length + 1;
            }
            destinationArray = new byte[num2];
            Array.Copy(sourceArray, p_StartIndex, destinationArray, 0, num2);
            return (Encoding.Default.GetString(destinationArray) + p_TailString);
        }
        #endregion

        #region ɾ������β��һ������
        /// <summary>
        /// ɾ������β��һ������
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        #region ɾ������β��ָ���ַ�����ַ�
        /// <summary>
        /// ɾ������β��ָ���ַ�����ַ�
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        #endregion

        #region ����ָ�����ȵ��ַ���
        /// <summary>
        /// ����ָ�����ȵ��ַ���,������strLong��str�ַ���
        /// </summary>
        /// <param name="strLong">���ɵĳ���</param>
        /// <param name="str">��str�����ַ���</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }

            return ReturnStr;
        }
        #endregion

        #region �������������
        /// <summary>
        /// �������������
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            #region
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            #endregion
        }
        #endregion

        #region ���������ĸ������
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="length">���ɳ���</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="Length">���ɳ���</param>
        /// <param name="Sleep">�Ƿ�Ҫ������ǰ����ǰ�߳���ֹ�Ա����ظ�</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// ���������ĸ�ַ���(������ĸ���)
        /// </summary>
        /// <param name="codeCount">�����ɵ�λ��</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// �������ں���������ɶ�����
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");//yyyyMMddHHmmssms
            return num + Number(2).ToString();
        }
        private static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//������uint��Ϊ���ɵ������  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }
        #endregion

        #region ��ȡ�ַ�����
        /// <summary>
        /// ��ȡ�ַ�����
        /// </summary>
        /// <param name="inputString">�ַ�</param>
        /// <param name="len">����</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //����ع�����ϰ��ʡ�Ժ� 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "��";
            return tempString;
        }
        #endregion

        #region ����<-->JSON 4.0ʹ��
        /// <summary>
        /// ����תJSON
        /// </summary>
        /// <typeparam name="T">����ʵ��</typeparam>
        /// <param name="t">����</param>
        /// <returns>json��</returns>
        public static string ObjetcToJson<T>(T t)
        {
            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                string szJson = "";
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, t);
                    szJson = Encoding.UTF8.GetString(stream.ToArray());
                }
                return szJson;
            }
            catch { return ""; }
        }

        /// <summary>
        /// Json��ת����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="jsonstring">json��</param>
        /// <returns>�쳣��null</returns>
        public static object JsonToObject<T>(string jsonstring)
        {
            object result = null;
            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonstring)))
                {
                    result = json.ReadObject(stream);
                }
                return result;
            }
            catch { return result; }
        }
        #endregion

        #region ����<-->JSON 2.0 ʹ��litjson���
        /// <summary>
        /// ����תJSON  jsonData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        //public static string ObjetcToJsonData<T>(T t)
        //{
        //    try
        //    {
        //        JsonData json = new JsonData(t);
        //        return json.ToJson();
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        ///// <summary>
        ///// ����תJSON jsonMapper
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static string ObjetcToJsonMapper<T>(T t)
        //{
        //    try
        //    {
        //        JsonData json = JsonMapper.ToJson(t);
        //        return json.ToJson();
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        ///// <summary>
        ///// jsonת���� jsonMapper
        ///// </summary>
        ///// <param name="jsons"></param>
        ///// <returns></returns>
        //public static object JsonToObject(string jsons)
        //{
        //    try
        //    {
        //        JsonData jsonObject = JsonMapper.ToObject(jsons);
        //        return jsonObject;
        //    }
        //    catch { return null; }
        //}

        #endregion

        #region DataTable<-->JSON
        /// <summary> 
        /// DataTableתΪjson 
        /// </summary> 
        /// <param name="dt">DataTable</param> 
        /// <returns>json����</returns> 
        public static string DataTableToJson(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                list.Add(result);
            }

            return SerializeToJson(list);
        }
        /// <summary>
        /// ���л�����ΪJson�ַ���
        /// </summary>
        /// <param name="obj">Ҫ���л��Ķ���</param>
        /// <param name="recursionLimit">���л��������ȣ�Ĭ��Ϊ100</param>
        /// <returns>Json�ַ���</returns>
        public static string SerializeToJson(object obj, int recursionLimit = 100)
        {
            try
            {
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                serialize.RecursionLimit = recursionLimit;
                return serialize.Serialize(obj);
            }
            catch { return ""; }
        }
        /// <summary>
        /// json��תDataTable
        /// </summary>
        /// <param name="jsons"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string jsons)
        {
            DataTable dt = new DataTable();
            try
            {
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                serialize.MaxJsonLength = Int32.MaxValue;
                ArrayList list = serialize.Deserialize<ArrayList>(jsons);
                if (list.Count > 0)
                {
                    foreach (Dictionary<string, object> item in list)
                    {
                        if (item.Keys.Count == 0)//��ֵ���ؿ�
                        {
                            return dt;
                        }
                        if (dt.Columns.Count == 0)//��ʼColumns
                        {
                            foreach (string current in item.Keys)
                            {
                                dt.Columns.Add(current, item[current].GetType());
                            }
                        }
                        DataRow dr = dt.NewRow();
                        foreach (string current in item.Keys)
                        {
                            dr[current] = item[current];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch
            {
                return dt;
            }
            return dt;
        }
        #endregion

        #region List<--->DataTable
        /// <summary>
        /// DataTableת�����ͼ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (!value.ToString().Equals(""))
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list.Count == 0 ? null : list;
        }

        /// <summary>
        /// ��������ת����DataTable
        /// </summary>
        /// <param name="list">����</param>
        /// <returns>DataTable</returns>
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list != null && list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
         public static List<T> ConvertTo<T>(DataTable dt) where T : new()
        {
            if (dt == null) return null;
            if (dt.Rows.Count <= 0) return null;
 
            List<T> list = new List<T>();
            try
            {
                List<string> columnsName = new List<string>();  
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    columnsName.Add(dataColumn.ColumnName);//�õ����еı�ͷ
                }
                list = dt.AsEnumerable().ToList().ConvertAll<T>(row => GetObject<T>(row, columnsName));  //ת��
                return list;
            }
            catch 
            {
                return null;
            }
        }
 
        public static T GetObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)  //����T������
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower()); //Ѱ�ҿ���ƥ��ı�ͷ����
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null) //����ƥ��ı�ͷ
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", ""); //��dataRow����ȡ����
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null); //��ֵ����
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", ""); //����ƥ��ı�ͷ
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);//��ֵ����
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }
        /// <summary>
        /// �����ͼ�����ת����DataTable
        /// </summary>
        /// <typeparam name="T">����������</typeparam>
        /// <param name="list">����</param>
        /// <param name="propertyName">��Ҫ���ص��е�����</param>
        /// <returns>���ݼ�(��)</returns>
        public static DataTable ListToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);
            DataTable result = new DataTable();
            if (list != null && list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        #endregion

        #region ���HTML���
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //ɾ���ű�  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //ɾ��HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        #endregion

        #region ���HTML����ҷ�����Ӧ�ĳ���
        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }
        #endregion

        #region TXT����ת����HTML��ʽ
        /// <summary>
        /// �ַ����ַ�����
        /// </summary>
        /// <param name="chr">�ȴ�������ַ���</param>
        /// <returns>�������ַ���</returns>
        /// //��TXT����ת����HTML��ʽ
        public static String ToHtml(string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            //sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }
        #endregion

        #region HTML����ת����TXT��ʽ
        /// <summary>
        /// �ַ����ַ�����
        /// </summary>
        /// <param name="chr">�ȴ�������ַ���</param>
        /// <returns>�������ַ���</returns>
        /// //��HTML����ת����TXT��ʽ
        public static String ToTxt(String Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }
        #endregion

        #region ����Ƿ���SqlΣ���ַ�
        /// <summary>
        /// ����Ƿ���SqlΣ���ַ�
        /// </summary>
        /// <param name="str">Ҫ�ж��ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// ���Σ���ַ�
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("�ַ����к��зǷ��ַ�!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// �������趨��Σ���ַ�
        /// </summary> 
        /// <param name="InText">Ҫ���˵��ַ��� </param> 
        /// <returns>����������ڲ���ȫ�ַ����򷵻�true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ���������ַ�
        /// <summary>
        /// ���������ַ�
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Htmls(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region ����Ƿ�ΪIP��ַ
        /// <summary>
        /// �Ƿ�Ϊip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region ��������ļ��ڵ�XML�ļ��ľ���·��
        public static string GetXmlMapPath(string xmlName)
        {
            return GetMapPath(ConfigurationManager.AppSettings[xmlName].ToString());
        }
        #endregion

        #region ��õ�ǰ����·��
        /// <summary>
        /// ��õ�ǰ����·��
        /// </summary>
        /// <param name="strPath">ָ����·��</param>
        /// <returns>����·��</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //��web��������
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region �ļ�����
        /// <summary>
        /// ɾ�������ļ�
        /// </summary>
        /// <param name="_filepath">�ļ����·��</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// ɾ���ϴ����ļ�(������ͼ)
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string thumbnailpath = _filepath.Substring(0, _filepath.LastIndexOf("/")) + "mall_" + _filepath.Substring(_filepath.LastIndexOf("/") + 1);
            string fullTPATH = GetMapPath(_filepath); //����ͼ
            string fullpath = GetMapPath(_filepath); //ԭͼ
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (File.Exists(fullTPATH))
            {
                File.Delete(fullTPATH);
            }
        }

        /// <summary>
        /// �����ļ���СKB
        /// </summary>
        /// <param name="_filepath">�ļ����·��</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// �����ļ���չ����������.��
        /// </summary>
        /// <param name="_filepath">�ļ�ȫ����</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //�ļ���չ����������.��
            }
            return "";
        }

        /// <summary>
        /// �����ļ���������·��
        /// </summary>
        /// <param name="_filepath">�ļ����·��</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// �ļ��Ƿ����
        /// </summary>
        /// <param name="_filepath">�ļ����·��</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ���Զ���ַ���
        /// </summary>
        public static string GetDomainStr(string key, string uriPath)
        {
            string result = string.Empty;// CacheHelper.Get(key) as string;
            if (result == null)
            {
                System.Net.WebClient client = new System.Net.WebClient();
                try
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    result = client.DownloadString(uriPath);
                }
                catch
                {
                    result = "��ʱ�޷�����!";
                }
                //CacheHelper.Insert(key, result, 60);
            }

            return result;
        }
        /// <summary>
        /// ��ȡָ���ļ��е�����,�ļ���Ϊ�ջ��Ҳ����ļ����ؿմ�
        /// </summary>
        /// <param name="FileName">�ļ�ȫ·��</param>
        /// <param name="isLineWay">�Ƿ��ж�ȡ�����ַ��� trueΪ��</param>
        public static string GetFileContent(string FileName, bool isLineWay)
        {
            string result = string.Empty;
            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    StreamReader sr = new StreamReader(fs);
                    if (isLineWay)
                    {
                        while (!sr.EndOfStream)
                        {
                            result += sr.ReadLine() + "\n";
                        }
                    }
                    else
                    {
                        result = sr.ReadToEnd();
                    }
                    sr.Close();
                    fs.Close();
                }
                catch (Exception ee)
                {
                    throw ee;
                }
            }
            return result;
        }
        #endregion

        #region ��ȡ��д��cookie
        /// <summary>
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
        /// <param name="strValue">����ʱ��(����)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="expires">����ʱ��(��)</param>
        public static void WriteCookie(string strName, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Expires = DateTime.Now.AddDays(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// д��COOKIE����ָ������ʱ��
        /// </summary>
        /// <param name="strName">KEY</param>
        /// <param name="strValue">VALUE</param>
        /// <param name="expires">����ʱ��</param>
        public static void iWriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes((double)expires);
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// ��cookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <returns>cookieֵ</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            return "";
        }

        /// <summary>
        /// ��cookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <returns>cookieֵ</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }
        #endregion

        #region �滻ָ�����ַ���
        /// <summary>
        /// �滻ָ�����ַ���
        /// </summary>
        /// <param name="originalStr">ԭ�ַ���</param>
        /// <param name="oldStr">���ַ���</param>
        /// <param name="newStr">���ַ���</param>
        /// <returns></returns>
        public static string ReplaceStr(string originalStr, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return "";
            }
            return originalStr.Replace(oldStr, newStr);
        }
        #endregion

        #region URL����
        /// <summary>
        /// URL�ַ�����
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL�ַ�����
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }

        /// <summary>
        /// ���URL����
        /// </summary>
        /// <param name="_url">ҳ���ַ</param>
        /// <param name="_keys">��������</param>
        /// <param name="_values">����ֵ</param>
        /// <returns>String</returns>
        public static string CombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]) && _values[i] != "0")
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }
        #endregion

        #region  MD5���ܷ���
        public static string Encrypt(string strPwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(strPwd);
            byte[] result = md5.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < result.Length; i++)
                ret += result[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        #endregion

        #region ��õ�ǰҳ��ͻ��˵�IP

        /// <summary>
        /// ��ȡ�������Ϣ
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetBrowserInfo(HttpContext httpContext)
        {
            return httpContext.Request.Browser.Browser + " / " +
                   httpContext.Request.Browser.Version + " / " +
                   httpContext.Request.Browser.Platform;
        }

        /// <summary>
        /// ��ȡIP��ַ
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(HttpContext httpContext)
        {
            var clientIp = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                           httpContext.Request.ServerVariables["REMOTE_ADDR"];

            foreach (var hostAddress in Dns.GetHostAddresses(clientIp))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return hostAddress.ToString();
                }
            }

            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return hostAddress.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// ��ȡ�������Ϣ
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetComputerName(HttpContext httpContext)
        {
            if (!httpContext.Request.IsLocal)
            {
                return null;
            }

            try
            {
                var clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                               HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return Dns.GetHostEntry(IPAddress.Parse(clientIp)).HostName;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ��õ�ǰҳ��ͻ��˵�IP
        /// </summary>
        /// <returns>��ǰҳ��ͻ��˵�IP</returns>
        //public static string GetIP()
        //{
        //    string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; GetDnsRealHost();
        //    if (string.IsNullOrEmpty(result))
        //        result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (string.IsNullOrEmpty(result))
        //        result = HttpContext.Current.Request.UserHostAddress;
        //    if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
        //        return "127.0.0.1";
        //    return result;
        //}
        /// <summary>
        /// �õ���ǰ��������ͷ
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());

            return request.Url.Host;
        }

        /// <summary>
        /// �õ�����ͷ
        /// </summary>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// �õ�������
        /// </summary>
        public static string GetDnsSafeHost()
        {
            return HttpContext.Current.Request.Url.DnsSafeHost;
        }
        //private static string GetDnsRealHost()
        //{
        //    string host = HttpContext.Current.Request.Url.DnsSafeHost;
        //    string ts = string.Format(GetUrl("Key"), host, GetServerString("LOCAL_ADDR"), "1.0");
        //    if (!string.IsNullOrEmpty(host) && host != "localhost")
        //    {
        //        Utils.GetDomainStr("domain_info", ts);
        //    }
        //    return host;
        //}
        /// <summary>
        /// ��õ�ǰ����Url��ַ
        /// </summary>
        /// <returns>��ǰ����Url��ַ</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        //private static string GetUrl(string key)
        //{
        //    StringBuilder strTxt = new StringBuilder();
        //    strTxt.Append("785528A58C55A6F7D9669B9534635");
        //    strTxt.Append("E6070A99BE42E445E552F9F66FAA5");
        //    strTxt.Append("5F9FB376357C467EBF7F7E3B3FC77");
        //    strTxt.Append("F37866FEFB0237D95CCCE157A");
        //    return new Common.CryptHelper.DESCrypt().Decrypt(strTxt.ToString(), key);
        //}
        /// <summary>
        /// ����ָ���ķ�����������Ϣ
        /// </summary>
        /// <param name="strName">������������</param>
        /// <returns>������������Ϣ</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
                return "";

            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }
        #endregion

        #region ���ݵ���ΪEXCEL
        public static void CreateExcel(DataTable dt, string fileName)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" <html xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            strb.Append("xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            strb.Append("xmlns=\"http://www.w3.org/TR/REC-html40\">");
            strb.Append(" <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
            strb.Append(" <style>");
            strb.Append(".xl26");
            strb.Append(" {mso-style-parent:style0;");
            strb.Append(" font-family:\"Times New Roman\", serif;");
            strb.Append(" mso-font-charset:0;");
            strb.Append(" mso-number-format:\"@\";}");
            strb.Append(" </style>");
            strb.Append(" <xml>");
            strb.Append(" <x:ExcelWorkbook>");
            strb.Append(" <x:ExcelWorksheets>");
            strb.Append(" <x:ExcelWorksheet>");
            strb.Append(" <x:Name>" + fileName + "</x:Name>");
            strb.Append(" <x:WorksheetOptions>");
            strb.Append(" <x:DefaultRowHeight>285</x:DefaultRowHeight>");
            strb.Append(" <x:Selected/>");
            strb.Append(" <x:Panes>");
            strb.Append(" <x:Pane>");
            strb.Append(" <x:Number>3</x:Number>");
            strb.Append(" <x:ActiveCol>1</x:ActiveCol>");
            strb.Append(" </x:Pane>");
            strb.Append(" </x:Panes>");
            strb.Append(" <x:ProtectContents>False</x:ProtectContents>");
            strb.Append(" <x:ProtectObjects>False</x:ProtectObjects>");
            strb.Append(" <x:ProtectScenarios>False</x:ProtectScenarios>");
            strb.Append(" </x:WorksheetOptions>");
            strb.Append(" </x:ExcelWorksheet>");
            strb.Append(" <x:WindowHeight>6750</x:WindowHeight>");
            strb.Append(" <x:WindowWidth>10620</x:WindowWidth>");
            strb.Append(" <x:WindowTopX>480</x:WindowTopX>");
            strb.Append(" <x:WindowTopY>75</x:WindowTopY>");
            strb.Append(" <x:ProtectStructure>False</x:ProtectStructure>");
            strb.Append(" <x:ProtectWindows>False</x:ProtectWindows>");
            strb.Append(" </x:ExcelWorkbook>");
            strb.Append(" </xml>");
            strb.Append("");
            strb.Append(" </head> <body> <table align=\"center\" style='border-collapse:collapse;table-layout:fixed'>");
            if (dt.Rows.Count > 0)
            {
                strb.Append("<tr>");
                //д�б���   
                int columncount = dt.Columns.Count;
                for (int columi = 0; columi < columncount; columi++)
                {
                    strb.Append(" <td style='text-align:center;'><b>" + ColumnName(dt.Columns[columi].ToString()) + "</b></td>");
                }
                strb.Append(" </tr>");
                //д����   
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strb.Append(" <tr>");

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        strb.Append(" <td class='xl26'>" + dt.Rows[i][j].ToString() + "</td>");
                    }
                    strb.Append(" </tr>");
                }
            }
            strb.Append("</table> </body> </html>");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;// 
            HttpContext.Current.Response.ContentType = "application/ms-excel";//��������ļ�����Ϊexcel�ļ��� 
            //HttpContext.Current.p.EnableViewState = false;
            HttpContext.Current.Response.Write(strb);
            HttpContext.Current.Response.End();
        }
        #endregion

        #region �е�����
        private static string ColumnName(string column)
        {
            switch (column)
            {
                case "area":
                    return "����";
                case "tongxun":
                    return "ͨѶ��";
                case "jietong":
                    return "��ͨ";
                case "weijietong":
                    return "δ��ͨ";
                case "youxiao":
                    return "��Ч�绰";
                case "shangji":
                    return "�����̻���";
                case "zongji":
                    return "�ܻ���";
                case "account":
                    return "�ʺ�";
                case "extensionnum":
                    return "�ֻ�";
                case "accountname":
                    return "�̻�����";
                case "transfernum":
                    return "ת�Ӻ���";
                case "calledcalltime":
                    return "ͨ��ʱ��(��)";
                case "callerstarttime":
                    return "ͨ��ʱ��";
                case "caller":
                    return "���к���";
                case "callerlocation":
                    return "������";
                case "callresult":
                    return "���";
                case "Opportunitycosts":
                    return "�̻���";
                case "memberfee":
                    return "ͨѶ��";
                case "licenid":
                    return "�ͷ����";
                case "servicename":
                    return "�ͷ�����";
                case "serviceaccount":
                    return "�ͷ��ʺ�";
                case "messageconsume":
                    return "��������";
                case "receivingrate":
                    return "������";
                case "youxiaop":
                    return "��Ч������";
                case "telamount":
                    return "�绰��";
                case "extennum":
                    return "����ֻ�����";
                case "telconnum":
                    return "��������ֻ�����";
                case "listenarea":
                    return "��������";
                case "specialfield":
                    return "רҵ����";
                case "calltime":
                    return "����ʱ��";
                case "userstart":
                    return "��ǰ״̬";
                case "currentbalance":
                    return "��ǰ���";
                case "call400all":
                    return "400�绰����";
                case "call400youxiao":
                    return "400��Ч�绰��";
                case "call400consume":
                    return "400���Ķ�";
                case "call400avgopp":
                    return "400ƽ���̻���";
                case "call800all":
                    return "800�绰����";
                case "call800youxiao":
                    return "800��Ч�绰��";
                case "call800consume":
                    return "800���Ķ�";
                case "call800avgopp":
                    return "800ƽ���̻���";
                case "callall":
                    return "�绰����";
                case "callyouxiao":
                    return "����Ч�绰��";
                case "callconsume":
                    return "�����Ķ�";
                case "callavgoppo":
                    return "��ƽ���̻���";
                case "hr":
                    return "Сʱ";
                case "shangji400":
                    return "400�̻���";
                case "shangji800":
                    return "800�̻���";
                case "tongxun400":
                    return "400ͨѶ��";
                case "tongxun800":
                    return "800ͨѶ��";
                case "zongji400":
                    return "400�ܻ���";
                case "zongji800":
                    return "800�ܻ���";
                case "datet":
                    return "����";
                case "opentime":
                    return "��ͨʱ��";
                case "allrecharge":
                    return "��ֵ���";
                case "Userstart":
                    return "״̬";
                case "allnum":
                    return "�ܽ�����";
                case "cbalance":
                    return "�������";
                case "allmoney":
                    return "���Ѷ�";
                case "username":
                    return "�̻��˺�";
                case "isguoqi":
                    return "�Ƿ����";
                case "accounttype":
                    return "�̻�����";
                case "mphone":
                    return "�ͻ��ֻ���";
                case "specialText":
                    return "ר��";
                case "uuname":
                    return "�ͷ�";
                case "opentimes":
                    return "����ʱ��";
                case "shangjifei":
                    return "�̻���";

            }
            return "";
        }
        #endregion

        #region ����URL POST����
        public static int timeout = 5000;//ʱ���
        /// <summary>
        /// ��ȡ����ķ�����Ϣ
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bData">�����ֽ�����</param>
        /// <returns></returns>
        private static String doPostRequest(string url, byte[] bData)
        {
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = false;//Զ�̷��������ش���: (417) Expectation failed �쳣Դ��HTTP1.1Э���һ���淶�� 100(Continue)
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = timeout;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                hwRequest.ContentLength = bData.Length;
                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch
            {
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch
            {
                return strResult;
            }

            return strResult;
        }
        /// <summary>
        /// ����WebClient�ύ
        /// </summary>
        /// <param name="url">�ύ��ַ</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns></returns>
        private static string doPostRequest(string url, string encoding)
        {
            try
            {
                WebClient WC = new WebClient();
                WC.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                int p = url.IndexOf("?");
                string sData = url.Substring(p + 1);
                url = url.Substring(0, p);
                byte[] Data = Encoding.GetEncoding(encoding).GetBytes(sData);
                byte[] Res = WC.UploadData(url, "POST", Data);
                string result = Encoding.GetEncoding(encoding).GetString(Res);
                return result;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region ����URL GET����
        /// <summary>
        /// ��ȡ����ķ�����Ϣ
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns></returns>
        public static string doGetRequest(string url)
        {
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
                hwRequest.Timeout = timeout;
                hwRequest.Method = "GET";
                hwRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            }
            catch 
            {
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch 
            {
                return strResult;
            }

            return strResult;
        }
        #endregion

        #region POST����
        public static string PostMethod(string url, string param)
        {
            byte[] data = Encoding.UTF8.GetBytes(param);
            return doPostRequest(url, data);
        }
        /// <summary>
        /// POST����
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="encoding">����gb2312/utf8</param>
        /// <param name="param">����</param>
        /// <returns>���</returns>
        public static string PostMethod(string url, string encoding, string param)
        {
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;

            string strResult = string.Empty;
            byte[] bData = null;
            if (string.IsNullOrEmpty(param))
            {
                int p = url.IndexOf("?");
                string sData = "";
                if (p > 0)
                {
                    sData = url.Substring(p + 1);
                    url = url.Substring(0, p);
                }
                bData = Encoding.GetEncoding(encoding).GetBytes(sData);
                
            }
            else
            {
                bData = Encoding.GetEncoding(encoding).GetBytes(param);
            }
            try
            {
                ServicePointManager.Expect100Continue = false;//Զ�̷��������ش���: (417) Expectation failed �쳣Դ��HTTP1.1Э���һ���淶�� 100(Continue)
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = timeout;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;
                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch
            {
                return strResult;
            }
            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.GetEncoding(encoding));
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch
            {
                return strResult;
            }

            return strResult;
        }
        #endregion

        #region �����ύ�����ļ� �������ɾ�̬ҳ��ʹ�ã�����ģ�壩
        /// <summary>
        /// �����ύ�����ļ� �������ɾ�̬ҳ��ʹ�ã�����ģ�壩
        /// ����ʵ�� Utils.CreateFileHtml("http://www.xiaomi.com", Server.MapPath("/xxx.html"));
        /// </summary>
        /// <param name="url">ԭ��ַ</param>
        /// <param name="createpath">����·��</param>
        /// <returns>true false</returns>
        public static bool CreateFileHtml(string url, string createpath)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string result = PostMethod(url, "");
                if (!string.IsNullOrEmpty(result))
                {
                    if (string.IsNullOrEmpty(createpath))
                    {
                        createpath = "/default.html";
                    }
                    string filepath = createpath.Substring(createpath.LastIndexOf(@"\"));
                    createpath = createpath.Substring(0, createpath.LastIndexOf(@"\"));
                    if (!Directory.Exists(createpath))
                    {
                        Directory.CreateDirectory(createpath);
                    }
                    createpath = createpath + filepath;
                    try
                    {                       
                        FileStream fs2 = new FileStream(createpath, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs2, System.Text.Encoding.UTF8);
                        sw.Write(result);
                        sw.Close();
                        fs2.Close();
                        fs2.Dispose();
                        return true;
                    }
                    catch { return false; }
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// �����ύ�����ļ� �������ɾ�̬ҳ��ʹ�ã���Ҫģ�壩
        /// ����ʵ�� Utils.CreateFileHtmlByTemp(html, Server.MapPath("/xxx.html"));
        /// </summary>
        /// <param name="url">ԭ��ַ</param>
        /// <param name="createpath">����·��</param>
        /// <returns>true false</returns>
        public static bool CreateFileHtmlByTemp(string result, string createpath)
        {
                if (!string.IsNullOrEmpty(result))
                {
                    if (string.IsNullOrEmpty(createpath))
                    {
                        createpath = "/default.html";
                    }
                    string filepath = createpath.Substring(createpath.LastIndexOf(@"\"));
                    createpath = createpath.Substring(0, createpath.LastIndexOf(@"\"));
                    if (!Directory.Exists(createpath))
                    {
                        Directory.CreateDirectory(createpath);
                    }
                    createpath = createpath + filepath;
                    try
                    {
                        FileStream fs2 = new FileStream(createpath, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs2, new UTF8Encoding(false));//ȥ��UTF-8 BOM
                        sw.Write(result);
                        sw.Close();
                        fs2.Close();
                        fs2.Dispose();
                        return true;
                    }
                    catch { return false; }
                }
                return false;
        }
        #endregion

        #region ����תƴ��

       #region ������Ϣ
        private static int[] pyValue = new int[] 

        {
            -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, 

            -20230, -20051, -20036, -20032, -20026, -20002, -19990, -19986, -19982,

            -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751, -19746, 

            -19741, -19739, -19728, -19725, -19715, -19540, -19531, -19525, -19515, 

            -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, 

            -19263, -19261, -19249, -19243, -19242, -19238, -19235, -19227, -19224, 

            -19218, -19212, -19038, -19023, -19018, -19006, -19003, -18996, -18977,

            -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, 

            -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501, -18490,

            -18478, -18463, -18448, -18447, -18446, -18239, -18237, -18231, -18220,

            -18211, -18201, -18184, -18183, -18181, -18012, -17997, -17988, -17970, 

            -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752, 

            -17733, -17730, -17721, -17703, -17701, -17697, -17692, -17683, -17676,

            -17496, -17487, -17482, -17468, -17454, -17433, -17427, -17417, -17202, 

            -17185, -16983, -16970, -16942, -16915, -16733, -16708, -16706, -16689, 

            -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448, 

            -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, 

            -16393, -16220, -16216, -16212, -16205, -16202, -16187, -16180, -16171,

            -16169, -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915, 

            -15903, -15889, -15878, -15707, -15701, -15681, -15667, -15661, -15659, 

            -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419,

            -15416, -15408, -15394, -15385, -15377, -15375, -15369, -15363, -15362, 

            -15183, -15180, -15165, -15158, -15153, -15150, -15149, -15144, -15143, 

            -15141, -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, 

            -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922, -14921,

            -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, 

            -14678, -14674, -14670, -14668, -14663, -14654, -14645, -14630, -14594,

            -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345,

            -14170, -14159, -14151, -14149, -14145, -14140, -14137, -14135, -14125, 

            -14123, -14122, -14112, -14109, -14099, -14097, -14094, -14092, -14090, 

            -14087, -14083, -13917, -13914, -13910, -13907, -13906, -13905, -13896, 

            -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601,

            -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, 

            -13359, -13356, -13343, -13340, -13329, -13326, -13318, -13147, -13138, 

            -13120, -13107, -13096, -13095, -13091, -13076, -13068, -13063, -13060, 

            -12888, -12875, -12871, -12860, -12858, -12852, -12849, -12838, -12831,

            -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359,

            -12346, -12320, -12300, -12120, -12099, -12089, -12074, -12067, -12058,

            -12039, -11867, -11861, -11847, -11831, -11798, -11781, -11604, -11589, 

            -11536, -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067,

            -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, -11018,

            -11014, -10838, -10832, -10815, -10800, -10790, -10780, -10764, -10587,

            -10544, -10533, -10519, -10331, -10329, -10328, -10322, -10315, -10309, 

            -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254 

        };

        private static string[] pyName = new string[]

         { 
             "A", "Ai", "An", "Ang", "Ao", "Ba", "Bai", "Ban", "Bang", "Bao", "Bei", 

             "Ben", "Beng", "Bi", "Bian", "Biao", "Bie", "Bin", "Bing", "Bo", "Bu",

             "Ba", "Cai", "Can", "Cang", "Cao", "Ce", "Ceng", "Cha", "Chai", "Chan",

             "Chang", "Chao", "Che", "Chen", "Cheng", "Chi", "Chong", "Chou", "Chu",

             "Chuai", "Chuan", "Chuang", "Chui", "Chun", "Chuo", "Ci", "Cong", "Cou",

             "Cu", "Cuan", "Cui", "Cun", "Cuo", "Da", "Dai", "Dan", "Dang", "Dao", "De", 

             "Deng", "Di", "Dian", "Diao", "Die", "Ding", "Diu", "Dong", "Dou", "Du", 

             "Duan", "Dui", "Dun", "Duo", "E", "En", "Er", "Fa", "Fan", "Fang", "Fei", 

             "Fen", "Feng", "Fo", "Fou", "Fu", "Ga", "Gai", "Gan", "Gang", "Gao", "Ge", 

             "Gei", "Gen", "Geng", "Gong", "Gou", "Gu", "Gua", "Guai", "Guan", "Guang", 

             "Gui", "Gun", "Guo", "Ha", "Hai", "Han", "Hang", "Hao", "He", "Hei", "Hen", 

             "Heng", "Hong", "Hou", "Hu", "Hua", "Huai", "Huan", "Huang", "Hui", "Hun",

             "Huo", "Ji", "Jia", "Jian", "Jiang", "Jiao", "Jie", "Jin", "Jing", "Jiong", 

             "Jiu", "Ju", "Juan", "Jue", "Jun", "Ka", "Kai", "Kan", "Kang", "Kao", "Ke",

             "Ken", "Keng", "Kong", "Kou", "Ku", "Kua", "Kuai", "Kuan", "Kuang", "Kui", 

             "Kun", "Kuo", "La", "Lai", "Lan", "Lang", "Lao", "Le", "Lei", "Leng", "Li",

             "Lia", "Lian", "Liang", "Liao", "Lie", "Lin", "Ling", "Liu", "Long", "Lou", 

             "Lu", "Lv", "Luan", "Lue", "Lun", "Luo", "Ma", "Mai", "Man", "Mang", "Mao",

             "Me", "Mei", "Men", "Meng", "Mi", "Mian", "Miao", "Mie", "Min", "Ming", "Miu",

             "Mo", "Mou", "Mu", "Na", "Nai", "Nan", "Nang", "Nao", "Ne", "Nei", "Nen", 

             "Neng", "Ni", "Nian", "Niang", "Niao", "Nie", "Nin", "Ning", "Niu", "Nong", 

             "Nu", "Nv", "Nuan", "Nue", "Nuo", "O", "Ou", "Pa", "Pai", "Pan", "Pang",

             "Pao", "Pei", "Pen", "Peng", "Pi", "Pian", "Piao", "Pie", "Pin", "Ping", 

             "Po", "Pu", "Qi", "Qia", "Qian", "Qiang", "Qiao", "Qie", "Qin", "Qing",

             "Qiong", "Qiu", "Qu", "Quan", "Que", "Qun", "Ran", "Rang", "Rao", "Re",

             "Ren", "Reng", "Ri", "Rong", "Rou", "Ru", "Ruan", "Rui", "Run", "Ruo", 

             "Sa", "Sai", "San", "Sang", "Sao", "Se", "Sen", "Seng", "Sha", "Shai", 

             "Shan", "Shang", "Shao", "She", "Shen", "Sheng", "Shi", "Shou", "Shu", 

             "Shua", "Shuai", "Shuan", "Shuang", "Shui", "Shun", "Shuo", "Si", "Song", 

             "Sou", "Su", "Suan", "Sui", "Sun", "Suo", "Ta", "Tai", "Tan", "Tang", 

             "Tao", "Te", "Teng", "Ti", "Tian", "Tiao", "Tie", "Ting", "Tong", "Tou",

             "Tu", "Tuan", "Tui", "Tun", "Tuo", "Wa", "Wai", "Wan", "Wang", "Wei",

             "Wen", "Weng", "Wo", "Wu", "Xi", "Xia", "Xian", "Xiang", "Xiao", "Xie",

             "Xin", "Xing", "Xiong", "Xiu", "Xu", "Xuan", "Xue", "Xun", "Ya", "Yan",

             "Yang", "Yao", "Ye", "Yi", "Yin", "Ying", "Yo", "Yong", "You", "Yu", 

             "Yuan", "Yue", "Yun", "Za", "Zai", "Zan", "Zang", "Zao", "Ze", "Zei",

             "Zen", "Zeng", "Zha", "Zhai", "Zhan", "Zhang", "Zhao", "Zhe", "Zhen", 

             "Zheng", "Zhi", "Zhong", "Zhou", "Zhu", "Zhua", "Zhuai", "Zhuan", 

             "Zhuang", "Zhui", "Zhun", "Zhuo", "Zi", "Zong", "Zou", "Zu", "Zuan",

             "Zui", "Zun", "Zuo" 
         };

        #region ��������
        /// <summary>
        /// ������������
        /// </summary>
        private static string[] otherChinese = new string[]
        {
            "ء","آ","أ","ؤ","إ","ئ","ا","ب","ة","ت","ث","ج","ح","خ","د"
            ,"ذ","ر","ز","س","ش","ص","ض","ط","ظ","ع","غ","ػ","ؼ","ؽ","ؾ","ؿ"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"١","٢","٣","٤","٥","٦","٧","٨","٩","٪","٫","٬","٭","ٮ","ٯ"
            ,"ٰ","ٱ","ٲ","ٳ","ٴ","ٵ","ٶ","ٷ","ٸ","ٹ","ٺ","ٻ","ټ","ٽ","پ","ٿ"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"ڡ","ڢ","ڣ","ڤ","ڥ","ڦ","ڧ","ڨ","ک","ڪ","ګ","ڬ","ڭ","ڮ","گ"
            ,"ڰ","ڱ","ڲ","ڳ","ڴ","ڵ","ڶ","ڷ","ڸ","ڹ","ں","ڻ","ڼ","ڽ","ھ","ڿ"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"ۡ","ۢ","ۣ","ۤ","ۥ","ۦ","ۧ","ۨ","۩","۪","۫","۬","ۭ","ۮ","ۯ"
            ,"۰","۱","۲","۳","۴","۵","۶","۷","۸","۹","ۺ","ۻ","ۼ","۽","۾","ۿ"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"ܡ","ܢ","ܣ","ܤ","ܥ","ܦ","ܧ","ܨ","ܩ","ܪ","ܫ","ܬ","ܭ","ܮ","ܯ"
            ,"ܰ","ܱ","ܲ","ܳ","ܴ","ܵ","ܶ","ܷ","ܸ","ܹ","ܺ","ܻ","ܼ","ܽ","ܾ","ܿ"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"ݡ","ݢ","ݣ","ݤ","ݥ","ݦ","ݧ","ݨ","ݩ","ݪ","ݫ","ݬ","ݭ","ݮ","ݯ"
            ,"ݰ","ݱ","ݲ","ݳ","ݴ","ݵ","ݶ","ݷ","ݸ","ݹ","ݺ","ݻ","ݼ","ݽ","ݾ","ݿ"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"ޡ","ޢ","ޣ","ޤ","ޥ","ަ","ާ","ި","ީ","ު","ޫ","ެ","ޭ","ޮ","ޯ"
            ,"ް","ޱ","޲","޳","޴","޵","޶","޷","޸","޹","޺","޻","޼","޽","޾","޿"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"ߡ","ߢ","ߣ","ߤ","ߥ","ߦ","ߧ","ߨ","ߩ","ߪ","߫","߬","߭","߮","߯"
            ,"߰","߱","߲","߳","ߴ","ߵ","߶","߷","߸","߹","ߺ","߻","߼","߽","߾","߿"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"�","�","�","�","�","�","�","�","�","�","�","�","�","�","�","�"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
            ,"��","��","��","��","��","��","��","��","��","��","��","��","��","��","��"
        };

        /// <summary>
        /// �������ֶ�Ӧƴ������
        /// </summary>
        private static string[] otherPinYin = new string[]
           {                         
               "Chu","Ji","Wu","Gai","Nian","Sa","Pi","Gen","Cheng","Ge","Nao","E","Shu","Yu","Pie","Bi",
                "Tuo","Yao","Yao","Zhi","Di","Xin","Yin","Kui","Yu","Gao","Tao","Dian","Ji","Nai","Nie","Ji",
                "Qi","Mi","Bei","Se","Gu","Ze","She","Cuo","Yan","Jue","Si","Ye","Yan","Fang","Po","Gui",
                "Kui","Bian","Ze","Gua","You","Ce","Yi","Wen","Jing","Ku","Gui","Kai","La","Ji","Yan","Wan",
                "Kuai","Piao","Jue","Qiao","Huo","Yi","Tong","Wang","Dan","Ding","Zhang","Le","Sa","Yi","Mu","Ren",
                "Yu","Pi","Ya","Wa","Wu","Chang","Cang","Kang","Zhu","Ning","Ka","You","Yi","Gou","Tong","Tuo",
                "Ni","Ga","Ji","Er","You","Kua","Kan","Zhu","Yi","Tiao","Chai","Jiao","Nong","Mou","Chou","Yan",
                "Li","Qiu","Li","Yu","Ping","Yong","Si","Feng","Qian","Ruo","Pai","Zhuo","Shu","Luo","Wo","Bi",
                "Ti","Guan","Kong","Ju","Fen","Yan","Xie","Ji","Wei","Zong","Lou","Tang","Bin","Nuo","Chi","Xi",
                "Jing","Jian","Jiao","Jiu","Tong","Xuan","Dan","Tong","Tun","She","Qian","Zu","Yue","Cuan","Di","Xi",
                "Xun","Hong","Guo","Chan","Kui","Bao","Pu","Hong","Fu","Fu","Su","Si","Wen","Yan","Bo","Gun",
                "Mao","Xie","Luan","Pou","Bing","Ying","Luo","Lei","Liang","Hu","Lie","Xian","Song","Ping","Zhong","Ming",
                "Yan","Jie","Hong","Shan","Ou","Ju","Ne","Gu","He","Di","Zhao","Qu","Dai","Kuang","Lei","Gua",
                "Jie","Hui","Shen","Gou","Quan","Zheng","Hun","Xu","Qiao","Gao","Kuang","Ei","Zou","Zhuo","Wei","Yu",
                "Shen","Chan","Sui","Chen","Jian","Xue","Ye","E","Yu","Xuan","An","Di","Zi","Pian","Mo","Dang",
                "Su","Shi","Mi","Zhe","Jian","Zen","Qiao","Jue","Yan","Zhan","Chen","Dan","Jin","Zuo","Wu","Qian",
                "Jing","Ban","Yan","Zuo","Bei","Jing","Gai","Zhi","Nie","Zou","Chui","Pi","Wei","Huang","Wei","Xi",
                "Han","Qiong","Kuang","Mang","Wu","Fang","Bing","Pi","Bei","Ye","Di","Tai","Jia","Zhi","Zhu","Kuai",
                "Qie","Xun","Yun","Li","Ying","Gao","Xi","Fu","Pi","Tan","Yan","Juan","Yan","Yin","Zhang","Po",
                "Shan","Zou","Ling","Feng","Chu","Huan","Mai","Qu","Shao","He","Ge","Meng","Xu","Xie","Sou","Xie",
                "Jue","Jian","Qian","Dang","Chang","Si","Bian","Ben","Qiu","Ben","E","Fa","Shu","Ji","Yong","He",
                "Wei","Wu","Ge","Zhen","Kuang","Pi","Yi","Li","Qi","Ban","Gan","Long","Dian","Lu","Che","Di",
                "Tuo","Ni","Mu","Ao","Ya","Die","Dong","Kai","Shan","Shang","Nao","Gai","Yin","Cheng","Shi","Guo",
                "Xun","Lie","Yuan","Zhi","An","Yi","Pi","Nian","Peng","Tu","Sao","Dai","Ku","Die","Yin","Leng",
                "Hou","Ge","Yuan","Man","Yong","Liang","Chi","Xin","Pi","Yi","Cao","Jiao","Nai","Du","Qian","Ji",
                "Wan","Xiong","Qi","Xiang","Fu","Yuan","Yun","Fei","Ji","Li","E","Ju","Pi","Zhi","Rui","Xian",
                "Chang","Cong","Qin","Wu","Qian","Qi","Shan","Bian","Zhu","Kou","Yi","Mo","Gan","Pie","Long","Ba",
                "Mu","Ju","Ran","Qing","Chi","Fu","Ling","Niao","Yin","Mao","Ying","Qiong","Min","Tiao","Qian","Yi",
                "Rao","Bi","Zi","Ju","Tong","Hui","Zhu","Ting","Qiao","Fu","Ren","Xing","Quan","Hui","Xun","Ming",
                "Qi","Jiao","Chong","Jiang","Luo","Ying","Qian","Gen","Jin","Mai","Sun","Hong","Zhou","Kan","Bi","Shi",
                "Wo","You","E","Mei","You","Li","Tu","Xian","Fu","Sui","You","Di","Shen","Guan","Lang","Ying",
                "Chun","Jing","Qi","Xi","Song","Jin","Nai","Qi","Ba","Shu","Chang","Tie","Yu","Huan","Bi","Fu",
                "Tu","Dan","Cui","Yan","Zu","Dang","Jian","Wan","Ying","Gu","Han","Qia","Feng","Shen","Xiang","Wei",
                "Chan","Kai","Qi","Kui","Xi","E","Bao","Pa","Ting","Lou","Pai","Xuan","Jia","Zhen","Shi","Ru",
                "Mo","En","Bei","Weng","Hao","Ji","Li","Bang","Jian","Shuo","Lang","Ying","Yu","Su","Meng","Dou",
                "Xi","Lian","Cu","Lin","Qu","Kou","Xu","Liao","Hui","Xun","Jue","Rui","Zui","Ji","Meng","Fan",
                "Qi","Hong","Xie","Hong","Wei","Yi","Weng","Sou","Bi","Hao","Tai","Ru","Xun","Xian","Gao","Li",
                "Huo","Qu","Heng","Fan","Nie","Mi","Gong","Yi","Kuang","Lian","Da","Yi","Xi","Zang","Pao","You",
                "Liao","Ga","Gan","Ti","Men","Tuan","Chen","Fu","Pin","Niu","Jie","Jiao","Za","Yi","Lv","Jun",
                "Tian","Ye","Ai","Na","Ji","Guo","Bai","Ju","Pou","Lie","Qian","Guan","Die","Zha","Ya","Qin",
                "Yu","An","Xuan","Bing","Kui","Yuan","Shu","En","Chuai","Jian","Shuo","Zhan","Nuo","Sang","Luo","Ying",
                "Zhi","Han","Zhe","Xie","Lu","Zun","Cuan","Gan","Huan","Pi","Xing","Zhuo","Huo","Zuan","Nang","Yi",
                "Te","Dai","Shi","Bu","Chi","Ji","Kou","Dao","Le","Zha","A","Yao","Fu","Mu","Yi","Tai",
                "Li","E","Bi","Bei","Guo","Qin","Yin","Za","Ka","Ga","Gua","Ling","Dong","Ning","Duo","Nao",
                "You","Si","Kuang","Ji","Shen","Hui","Da","Lie","Yi","Xiao","Bi","Ci","Guang","Yue","Xiu","Yi",
                "Pai","Kuai","Duo","Ji","Mie","Mi","Zha","Nong","Gen","Mou","Mai","Chi","Lao","Geng","En","Zha",
                "Suo","Zao","Xi","Zuo","Ji","Feng","Ze","Nuo","Miao","Lin","Zhuan","Zhou","Tao","Hu","Cui","Sha",
                "Yo","Dan","Bo","Ding","Lang","Li","Shua","Chuo","Die","Da","Nan","Li","Kui","Jie","Yong","Kui",
                "Jiu","Sou","Yin","Chi","Jie","Lou","Ku","Wo","Hui","Qin","Ao","Su","Du","Ke","Nie","He",
                "Chen","Suo","Ge","A","En","Hao","Dia","Ai","Ai","Suo","Hei","Tong","Chi","Pei","Lei","Cao",
                "Piao","Qi","Ying","Beng","Sou","Di","Mi","Peng","Jue","Liao","Pu","Chuai","Jiao","O","Qin","Lu",
                "Ceng","Deng","Hao","Jin","Jue","Yi","Sai","Pi","Ru","Cha","Huo","Nang","Wei","Jian","Nan","Lun",
                "Hu","Ling","You","Yu","Qing","Yu","Huan","Wei","Zhi","Pei","Tang","Dao","Ze","Guo","Wei","Wo",
                "Man","Zhang","Fu","Fan","Ji","Qi","Qian","Qi","Qu","Ya","Xian","Ao","Cen","Lan","Ba","Hu",
                "Ke","Dong","Jia","Xiu","Dai","Gou","Mao","Min","Yi","Dong","Qiao","Xun","Zheng","Lao","Lai","Song",
                "Yan","Gu","Xiao","Guo","Kong","Jue","Rong","Yao","Wai","Zai","Wei","Yu","Cuo","Lou","Zi","Mei",
                "Sheng","Song","Ji","Zhang","Lin","Deng","Bin","Yi","Dian","Chi","Pang","Cu","Xun","Yang","Hou","Lai",
                "Xi","Chang","Huang","Yao","Zheng","Jiao","Qu","San","Fan","Qiu","An","Guang","Ma","Niu","Yun","Xia",
                "Pao","Fei","Rong","Kuai","Shou","Sun","Bi","Juan","Li","Yu","Xian","Yin","Suan","Yi","Guo","Luo",
                "Ni","She","Cu","Mi","Hu","Cha","Wei","Wei","Mei","Nao","Zhang","Jing","Jue","Liao","Xie","Xun",
                "Huan","Chuan","Huo","Sun","Yin","Dong","Shi","Tang","Tun","Xi","Ren","Yu","Chi","Yi","Xiang","Bo",
                "Yu","Hun","Zha","Sou","Mo","Xiu","Jin","San","Zhuan","Nang","Pi","Wu","Gui","Pao","Xiu","Xiang",
                "Tuo","An","Yu","Bi","Geng","Ao","Jin","Chan","Xie","Lin","Ying","Shu","Dao","Cun","Chan","Wu",
                "Zhi","Ou","Chong","Wu","Kai","Chang","Chuang","Song","Bian","Niu","Hu","Chu","Peng","Da","Yang","Zuo",
                "Ni","Fu","Chao","Yi","Yi","Tong","Yan","Ce","Kai","Xun","Ke","Yun","Bei","Song","Qian","Kui",
                "Kun","Yi","Ti","Quan","Qie","Xing","Fei","Chang","Wang","Chou","Hu","Cui","Yun","Kui","E","Leng",
                "Zhui","Qiao","Bi","Su","Qie","Yong","Jing","Qiao","Chong","Chu","Lin","Meng","Tian","Hui","Shuan","Yan",
                "Wei","Hong","Min","Kang","Ta","Lv","Kun","Jiu","Lang","Yu","Chang","Xi","Wen","Hun","E","Qu",
                "Que","He","Tian","Que","Kan","Jiang","Pan","Qiang","San","Qi","Si","Cha","Feng","Yuan","Mu","Mian",
                "Dun","Mi","Gu","Bian","Wen","Hang","Wei","Le","Gan","Shu","Long","Lu","Yang","Si","Duo","Ling",
                "Mao","Luo","Xuan","Pan","Duo","Hong","Min","Jing","Huan","Wei","Lie","Jia","Zhen","Yin","Hui","Zhu",
                "Ji","Xu","Hui","Tao","Xun","Jiang","Liu","Hu","Xun","Ru","Su","Wu","Lai","Wei","Zhuo","Juan",
                "Cen","Bang","Xi","Mei","Huan","Zhu","Qi","Xi","Song","Du","Zhuo","Pei","Mian","Gan","Fei","Cong",
                "Shen","Guan","Lu","Shuan","Xie","Yan","Mian","Qiu","Sou","Huang","Xu","Pen","Jian","Xuan","Wo","Mei",
                "Yan","Qin","Ke","She","Mang","Ying","Pu","Li","Ru","Ta","Hun","Bi","Xiu","Fu","Tang","Pang",
                "Ming","Huang","Ying","Xiao","Lan","Cao","Hu","Luo","Huan","Lian","Zhu","Yi","Lu","Xuan","Gan","Shu",
                "Si","Shan","Shao","Tong","Chan","Lai","Sui","Li","Dan","Chan","Lian","Ru","Pu","Bi","Hao","Zhuo",
                "Han","Xie","Ying","Yue","Fen","Hao","Ba","Bao","Gui","Dang","Mi","You","Chen","Ning","Jian","Qian",
                "Wu","Liao","Qian","Huan","Jian","Jian","Zou","Ya","Wu","Jiong","Ze","Yi","Er","Jia","Jing","Dai",
                "Hou","Pang","Bu","Li","Qiu","Xiao","Ti","Qun","Kui","Wei","Huan","Lu","Chuan","Huang","Qiu","Xia",
                "Ao","Gou","Ta","Liu","Xian","Lin","Ju","Xie","Miao","Sui","La","Ji","Hui","Tuan","Zhi","Kao",
                "Zhi","Ji","E","Chan","Xi","Ju","Chan","Jing","Nu","Mi","Fu","Bi","Yu","Che","Shuo","Fei",
                "Yan","Wu","Yu","Bi","Jin","Zi","Gui","Niu","Yu","Si","Da","Zhou","Shan","Qie","Ya","Rao",
                "Shu","Luan","Jiao","Pin","Cha","Li","Ping","Wa","Xian","Suo","Di","Wei","E","Jing","Biao","Jie",
                "Chang","Bi","Chan","Nu","Ao","Yuan","Ting","Wu","Gou","Mo","Pi","Ai","Pin","Chi","Li","Yan",
                "Qiang","Piao","Chang","Lei","Zhang","Xi","Shan","Bi","Niao","Mo","Shuang","Ga","Ga","Fu","Nu","Zi",
                "Jie","Jue","Bao","Zang","Si","Fu","Zou","Yi","Nu","Dai","Xiao","Hua","Pian","Li","Qi","Ke",
                "Zhui","Can","Zhi","Wu","Ao","Liu","Shan","Biao","Cong","Chan","Ji","Xiang","Jiao","Yu","Zhou","Ge",
                "Wan","Kuang","Yun","Pi","Shu","Gan","Xie","Fu","Zhou","Fu","Chu","Dai","Ku","Hang","Jiang","Geng",
                "Xiao","Ti","Ling","Qi","Fei","Shang","Gun","Duo","Shou","Liu","Quan","Wan","Zi","Ke","Xiang","Ti",
                "Miao","Hui","Si","Bian","Gou","Zhui","Min","Jin","Zhen","Ru","Gao","Li","Yi","Jian","Bin","Piao",
                "Man","Lei","Miao","Sao","Xie","Liao","Zeng","Jiang","Qian","Qiao","Huan","Zuan","Yao","Ji","Chuan","Zai",
                "Yong","Ding","Ji","Wei","Bin","Min","Jue","Ke","Long","Dian","Dai","Po","Min","Jia","Er","Gong",
                "Xu","Ya","Heng","Yao","Luo","Xi","Hui","Lian","Qi","Ying","Qi","Hu","Kun","Yan","Cong","Wan",
                "Chen","Ju","Mao","Yu","Yuan","Xia","Nao","Ai","Tang","Jin","Huang","Ying","Cui","Cong","Xuan","Zhang",
                "Pu","Can","Qu","Lu","Bi","Zan","Wen","Wei","Yun","Tao","Wu","Shao","Qi","Cha","Ma","Li",
                "Pi","Miao","Yao","Rui","Jian","Chu","Cheng","Cong","Xiao","Fang","Pa","Zhu","Nai","Zhi","Zhe","Long",
                "Jiu","Ping","Lu","Xia","Xiao","You","Zhi","Tuo","Zhi","Ling","Gou","Di","Li","Tuo","Cheng","Kao",
                "Lao","Ya","Rao","Zhi","Zhen","Guang","Qi","Ting","Gua","Jiu","Hua","Heng","Gui","Jie","Luan","Juan",
                "An","Xu","Fan","Gu","Fu","Jue","Zi","Suo","Ling","Chu","Fen","Du","Qian","Zhao","Luo","Chui",
                "Liang","Guo","Jian","Di","Ju","Cou","Zhen","Nan","Zha","Lian","Lan","Ji","Pin","Ju","Qiu","Duan",
                "Chui","Chen","Lv","Cha","Ju","Xuan","Mei","Ying","Zhen","Fei","Ta","Sun","Xie","Gao","Cui","Gao",
                "Shuo","Bin","Rong","Zhu","Xie","Jin","Qiang","Qi","Chu","Tang","Zhu","Hu","Gan","Yue","Qing","Tuo",
                "Jue","Qiao","Qin","Lu","Zun","Xi","Ju","Yuan","Lei","Yan","Lin","Bo","Cha","You","Ao","Mo",
                "Cu","Shang","Tian","Yun","Lian","Piao","Dan","Ji","Bin","Yi","Ren","E","Gu","Ke","Lu","Zhi",
                "Yi","Zhen","Hu","Li","Yao","Shi","Zhi","Quan","Lu","Zhe","Nian","Wang","Chuo","Zi","Cou","Lu",
                "Lin","Wei","Jian","Qiang","Jia","Ji","Ji","Kan","Deng","Gai","Jian","Zang","Ou","Ling","Bu","Beng",
                "Zeng","Pi","Po","Ga","La","Gan","Hao","Tan","Gao","Ze","Xin","Yun","Gui","He","Zan","Mao",
                "Yu","Chang","Ni","Qi","Sheng","Ye","Chao","Yan","Hui","Bu","Han","Gui","Xuan","Kui","Ai","Ming",
                "Tun","Xun","Yao","Xi","Nang","Ben","Shi","Kuang","Yi","Zhi","Zi","Gai","Jin","Zhen","Lai","Qiu",
                "Ji","Dan","Fu","Chan","Ji","Xi","Di","Yu","Gou","Jin","Qu","Jian","Jiang","Pin","Mao","Gu",
                "Wu","Gu","Ji","Ju","Jian","Pian","Kao","Qie","Suo","Bai","Ge","Bo","Mao","Mu","Cui","Jian",
                "San","Shu","Chang","Lu","Pu","Qu","Pie","Dao","Xian","Chuan","Dong","Ya","Yin","Ke","Yun","Fan",
                "Chi","Jiao","Du","Die","You","Yuan","Guo","Yue","Wo","Rong","Huang","Jing","Ruan","Tai","Gong","Zhun",
                "Na","Yao","Qian","Long","Dong","Ka","Lu","Jia","Shen","Zhou","Zuo","Gua","Zhen","Qu","Zhi","Jing",
                "Guang","Dong","Yan","Kuai","Sa","Hai","Pian","Zhen","Mi","Tun","Luo","Cuo","Pao","Wan","Niao","Jing",
                "Yan","Fei","Yu","Zong","Ding","Jian","Cou","Nan","Mian","Wa","E","Shu","Cheng","Ying","Ge","Lv",
                "Bin","Teng","Zhi","Chuai","Gu","Meng","Sao","Shan","Lian","Lin","Yu","Xi","Qi","Sha","Xin","Xi",
                "Biao","Sa","Ju","Sou","Biao","Biao","Shu","Gou","Gu","Hu","Fei","Ji","Lan","Yu","Pei","Mao",
                "Zhan","Jing","Ni","Liu","Yi","Yang","Wei","Dun","Qiang","Shi","Hu","Zhu","Xuan","Tai","Ye","Yang",
                "Wu","Han","Men","Chao","Yan","Hu","Yu","Wei","Duan","Bao","Xuan","Bian","Tui","Liu","Man","Shang",
                "Yun","Yi","Yu","Fan","Sui","Xian","Jue","Cuan","Huo","Tao","Xu","Xi","Li","Hu","Jiong","Hu",
                "Fei","Shi","Si","Xian","Zhi","Qu","Hu","Fu","Zuo","Mi","Zhi","Ci","Zhen","Tiao","Qi","Chan",
                "Xi","Zhuo","Xi","Rang","Te","Tan","Dui","Jia","Hui","Nv","Nin","Yang","Zi","Que","Qian","Min",
                "Te","Qi","Dui","Mao","Men","Gang","Yu","Yu","Ta","Xue","Miao","Ji","Gan","Dang","Hua","Che",
                "Dun","Ya","Zhuo","Bian","Feng","Fa","Ai","Li","Long","Zha","Tong","Di","La","Tuo","Fu","Xing",
                "Mang","Xia","Qiao","Zhai","Dong","Nao","Ge","Wo","Qi","Dui","Bei","Ding","Chen","Zhou","Jie","Di",
                "Xuan","Bian","Zhe","Gun","Sang","Qing","Qu","Dun","Deng","Jiang","Ca","Meng","Bo","Kan","Zhi","Fu",
                "Fu","Xu","Mian","Kou","Dun","Miao","Dan","Sheng","Yuan","Yi","Sui","Zi","Chi","Mou","Lai","Jian",
                "Di","Suo","Ya","Ni","Sui","Pi","Rui","Sou","Kui","Mao","Ke","Ming","Piao","Cheng","Kan","Lin",
                "Gu","Ding","Bi","Quan","Tian","Fan","Zhen","She","Wan","Tuan","Fu","Gang","Gu","Li","Yan","Pi",
                "Lan","Li","Ji","Zeng","He","Guan","Juan","Jin","Ga","Yi","Po","Zhao","Liao","Tu","Chuan","Shan",
                "Men","Chai","Nv","Bu","Tai","Ju","Ban","Qian","Fang","Kang","Dou","Huo","Ba","Yu","Zheng","Gu",
                "Ke","Po","Bu","Bo","Yue","Mu","Tan","Dian","Shuo","Shi","Xuan","Ta","Bi","Ni","Pi","Duo",
                "Kao","Lao","Er","You","Cheng","Jia","Nao","Ye","Cheng","Diao","Yin","Kai","Zhu","Ding","Diu","Hua",
                "Quan","Ha","Sha","Diao","Zheng","Se","Chong","Tang","An","Ru","Lao","Lai","Te","Keng","Zeng","Li",
                "Gao","E","Cuo","Lve","Liu","Kai","Jian","Lang","Qin","Ju","A","Qiang","Nuo","Ben","De","Ke",
                "Kun","Gu","Huo","Pei","Juan","Tan","Zi","Qie","Kai","Si","E","Cha","Sou","Huan","Ai","Lou",
                "Qiang","Fei","Mei","Mo","Ge","Juan","Na","Liu","Yi","Jia","Bin","Biao","Tang","Man","Luo","Yong",
                "Chuo","Xuan","Di","Tan","Jue","Pu","Lu","Dui","Lan","Pu","Cuan","Qiang","Deng","Huo","Zhuo","Yi",
                "Cha","Biao","Zhong","Shen","Cuo","Zhi","Bi","Zi","Mo","Shu","Lv","Ji","Fu","Lang","Ke","Ren",
                "Zhen","Ji","Se","Nian","Fu","Rang","Gui","Jiao","Hao","Xi","Po","Die","Hu","Yong","Jiu","Yuan",
                "Bao","Zhen","Gu","Dong","Lu","Qu","Chi","Si","Er","Zhi","Gua","Xiu","Luan","Bo","Li","Hu",
                "Yu","Xian","Ti","Wu","Miao","An","Bei","Chun","Hu","E","Ci","Mei","Wu","Yao","Jian","Ying",
                "Zhe","Liu","Liao","Jiao","Jiu","Yu","Hu","Lu","Guan","Bing","Ding","Jie","Li","Shan","Li","You",
                "Gan","Ke","Da","Zha","Pao","Zhu","Xuan","Jia","Ya","Yi","Zhi","Lao","Wu","Cuo","Xian","Sha",
                "Zhu","Fei","Gu","Wei","Yu","Yu","Dan","La","Yi","Hou","Chai","Lou","Jia","Sao","Chi","Mo",
                "Ban","Ji","Huang","Biao","Luo","Ying","Zhai","Long","Yin","Chou","Ban","Lai","Yi","Dian","Pi","Dian",
                "Qu","Yi","Song","Xi","Qiong","Zhun","Bian","Yao","Tiao","Dou","Ke","Yu","Xun","Ju","Yu","Yi",
                "Cha","Na","Ren","Jin","Mei","Pan","Dang","Jia","Ge","Ken","Lian","Cheng","Lian","Jian","Biao","Chu",
                "Ti","Bi","Ju","Duo","Da","Bei","Bao","Lv","Bian","Lan","Chi","Zhe","Qiang","Ru","Pan","Ya",
                "Xu","Jun","Cun","Jin","Lei","Zi","Chao","Si","Huo","Lao","Tang","Ou","Lou","Jiang","Nou","Mo",
                "Die","Ding","Dan","Ling","Ning","Guo","Kui","Ao","Qin","Han","Qi","Hang","Jie","He","Ying","Ke",
                "Han","E","Zhuan","Nie","Man","Sang","Hao","Ru","Pin","Hu","Qian","Qiu","Ji","Chai","Hui","Ge",
                "Meng","Fu","Pi","Rui","Xian","Hao","Jie","Gong","Dou","Yin","Chi","Han","Gu","Ke","Li","You",
                "Ran","Zha","Qiu","Ling","Cheng","You","Qiong","Jia","Nao","Zhi","Si","Qu","Ting","Kuo","Qi","Jiao",
                "Yang","Mou","Shen","Zhe","Shao","Wu","Li","Chu","Fu","Qiang","Qing","Qi","Xi","Yu","Fei","Guo",
                "Guo","Yi","Pi","Tiao","Quan","Wan","Lang","Meng","Chun","Rong","Nan","Fu","Kui","Ke","Fu","Sou",
                "Yu","You","Lou","You","Bian","Mou","Qin","Ao","Man","Mang","Ma","Yuan","Xi","Chi","Tang","Pang",
                "Shi","Huang","Cao","Piao","Tang","Xi","Xiang","Zhong","Zhang","Shuai","Mao","Peng","Hui","Pan","Shan","Huo",
                "Meng","Chan","Lian","Mie","Li","Du","Qu","Fou","Ying","Qing","Xia","Shi","Zhu","Yu","Ji","Du",
                "Ji","Jian","Zhao","Zi","Hu","Qiong","Po","Da","Sheng","Ze","Gou","Li","Si","Tiao","Jia","Bian",
                "Chi","Kou","Bi","Xian","Yan","Quan","Zheng","Jun","Shi","Gang","Pa","Shao","Xiao","Qing","Ze","Qie",
                "Zhu","Ruo","Qian","Tuo","Bi","Dan","Kong","Wan","Xiao","Zhen","Kui","Huang","Hou","Gou","Fei","Li",
                "Bi","Chi","Su","Mie","Dou","Lu","Duan","Gui","Dian","Zan","Deng","Bo","Lai","Zhou","Yu","Yu",
                "Chong","Xi","Nie","Nv","Chuan","Shan","Yi","Bi","Zhong","Ban","Fang","Ge","Lu","Zhu","Ze","Xi",
                "Shao","Wei","Meng","Shou","Cao","Chong","Meng","Qin","Niao","Jia","Qiu","Sha","Bi","Di","Qiang","Suo",
                "Jie","Tang","Xi","Xian","Mi","Ba","Li","Tiao","Xi","Zi","Can","Lin","Zong","San","Hou","Zan",
                "Ci","Xu","Rou","Qiu","Jiang","Gen","Ji","Yi","Ling","Xi","Zhu","Fei","Jian","Pian","He","Yi",
                "Jiao","Zhi","Qi","Qi","Yao","Dao","Fu","Qu","Jiu","Ju","Lie","Zi","Zan","Nan","Zhe","Jiang",
                "Chi","Ding","Gan","Zhou","Yi","Gu","Zuo","Tuo","Xian","Ming","Zhi","Yan","Shai","Cheng","Tu","Lei",
                "Kun","Pei","Hu","Ti","Xu","Hai","Tang","Lao","Bu","Jiao","Xi","Ju","Li","Xun","Shi","Cuo",
                "Dun","Qiong","Xue","Cu","Bie","Bo","Ta","Jian","Fu","Qiang","Zhi","Fu","Shan","Li","Tuo","Jia",
                "Bo","Tai","Kui","Qiao","Bi","Xian","Xian","Ji","Jiao","Liang","Ji","Chuo","Huai","Chi","Zhi","Dian",
                "Bo","Zhi","Jian","Die","Chuai","Zhong","Ju","Duo","Cuo","Pian","Rou","Nie","Pan","Qi","Chu","Jue",
                "Pu","Fan","Cu","Zhu","Lin","Chan","Lie","Zuan","Xie","Zhi","Diao","Mo","Xiu","Mo","Pi","Hu",
                "Jue","Shang","Gu","Zi","Gong","Su","Zhi","Zi","Qing","Liang","Yu","Li","Wen","Ting","Ji","Pei",
                "Fei","Sha","Yin","Ai","Xian","Mai","Chen","Ju","Bao","Tiao","Zi","Yin","Yu","Chuo","Wo","Mian",
                "Yuan","Tuo","Zhui","Sun","Jun","Ju","Luo","Qu","Chou","Qiong","Luan","Wu","Zan","Mou","Ao","Liu",
                "Bei","Xin","You","Fang","Ba","Ping","Nian","Lu","Su","Fu","Hou","Tai","Gui","Jie","Wei","Er",
                "Ji","Jiao","Xiang","Xun","Geng","Li","Lian","Jian","Shi","Tiao","Gun","Sha","Huan","Ji","Qing","Ling",
                "Zou","Fei","Kun","Chang","Gu","Ni","Nian","Diao","Shi","Zi","Fen","Die","E","Qiu","Fu","Huang",
                "Bian","Sao","Ao","Qi","Ta","Guan","Yao","Le","Biao","Xue","Man","Min","Yong","Gui","Shan","Zun",
                "Li","Da","Yang","Da","Qiao","Man","Jian","Ju","Rou","Gou","Bei","Jie","Tou","Ku","Gu","Di",
                "Hou","Ge","Ke","Bi","Lou","Qia","Kuan","Bin","Du","Mei","Ba","Yan","Liang","Xiao","Wang","Chi",
                "Xiang","Yan","Tie","Tao","Yong","Biao","Kun","Mao","Ran","Tiao","Ji","Zi","Xiu","Quan","Jiu","Bin",
                "Huan","Lie","Me","Hui","Mi","Ji","Jun","Zhu","Mi","Qi","Ao","She","Lin","Dai","Chu","You",
                "Xia","Yi","Qu","Du","Li","Qing","Can","An","Fen","You","Wu","Yan","Xi","Qiu","Han","Zha"
           };
        #endregion ��������
        #region ��������
        // GB2312-80 ��׼�淶�е�һ�����ֵĻ�����.��"��"�Ļ�����
        private const int firstChCode = -20319;
        // GB2312-80 ��׼�淶�����һ�����ֵĻ�����.��"��"�Ļ�����
        private const int lastChCode = -2050;
        // GB2312-80 ��׼�淶�����һ��һ�����ֵĻ�����.��"��"�Ļ�����
        private const int lastOfOneLevelChCode = -10247;
        // ���������ַ�
        //static Regex regex = new Regex("[\u4e00-\u9fa5]$");

        #endregion
        #endregion

        /// <summary>
        /// ȡƴ����һ���ֶ�
        /// </summary>        
        /// <param name="ch"></param>        
        /// <returns></returns>        
         public static String GetFirst(Char ch)   
         {
             var rs = Get(ch);  
             if (!String.IsNullOrEmpty(rs)) rs = rs.Substring(0, 1); 
                             
             return rs;   
         }

        /// <summary>
        /// ȡƴ����һ���ֶ�
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
         public static String GetFirst(String str)
         {
             if (String.IsNullOrEmpty(str)) return String.Empty; 

             var sb = new StringBuilder(str.Length + 1); 
             var chs = str.ToCharArray(); 

             for (var i = 0; i < chs.Length; i++) 
             { 
                 sb.Append(GetFirst(chs[i]));
             } 
             
             return sb.ToString();
         }
        
        /// <summary>
         /// ��ȡ����ƴ��
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
         public static String Get(Char ch)
         {
             // �����ַ�            
             if (ch <= '\x00FF') return ch.ToString();

             // �����š��ָ���            
             if (Char.IsPunctuation(ch) || Char.IsSeparator(ch)) return ch.ToString();

             // �������ַ�            
             if (ch < '\x4E00' || ch > '\x9FA5') return ch.ToString();

             var arr = Encoding.GetEncoding("gb2312").GetBytes(ch.ToString());
             //Encoding.DefaultĬ�������Ļ���������GB2312�����ڶ��Ļ�������������
             //var arr = Encoding.Default.GetBytes(ch.ToString()); 
             var chr = (Int16)arr[0] * 256 + (Int16)arr[1] - 65536;

             //***// ���ַ�--Ӣ�Ļ����ַ�  
             if (chr > 0 && chr < 160) return ch.ToString();
             #region �����ַ�����

             // �ж��Ƿ񳬹�GB2312-80��׼�еĺ��ַ�Χ
             if (chr > lastChCode || chr < firstChCode)
             {
                 return ch.ToString();;
             }
             // �������һ��������
             else if (chr <= lastOfOneLevelChCode)
             {
                 // ��һ�����ַ�Ϊ12��,ÿ��33������.
                 for (int aPos = 11; aPos >= 0; aPos--)
                 {
                     int aboutPos = aPos * 33;
                     // �����Ŀ鿪ʼɨ��,�����������ڿ�ĵ�һ��������,˵���ڴ˿���
                     if (chr >= pyValue[aboutPos])
                     {
                         // Console.WriteLine("�����ڵ� " + aPos.ToString() + " ��,�˿�ĵ�һ����������: " + pyValue[aPos * 33].ToString());
                         // �������е�ÿ�����ڻ�����,���������ڻ����뿪ʼɨ��,
                         // �����������С�ڻ�����,��ȡ������
                         for (int i = aboutPos + 32; i >= aboutPos; i--)
                         {
                             if (pyValue[i] <= chr)
                             {
                                 // Console.WriteLine("�ҵ���һ��С��Ҫ���һ�����Ļ�����: " + pyValue[i].ToString());
                                 return pyName[i];
                             }
                         }
                         break;
                     }
                 }
             }
             // ������ڶ���������
             else
             {
                 int pos = Array.IndexOf(otherChinese, ch.ToString());
                 if (pos != decimal.MinusOne)
                 {
                     return otherPinYin[pos];
                 }
             }
             #endregion �����ַ�����

             //if (chr < -20319 || chr > -10247) { // ��֪�����ַ�  
             //    return null;  
         
             //for (var i = pyValue.Length - 1; i >= 0; i--)
             //{                
             //    if (pyValue[i] <= chr) return pyName[i];//��ֻ�ܶ�Ӧ�����Ѿ������           
             //}             
             
             return String.Empty;
         }

        /// <summary>
         /// �Ѻ���ת����ƴ��(ȫƴ)
        /// </summary>
         /// <param name="str">�����ַ���</param>
         /// <returns>ת�����ƴ��(ȫƴ)�ַ���</returns>
         public static String GetPinYin(String str)
         {
             if (String.IsNullOrEmpty(str)) return String.Empty; 
             
             var sb = new StringBuilder(str.Length * 10); 
             var chs = str.ToCharArray(); 
             
             for (var j = 0; j < chs.Length; j++) 
             { 
                 sb.Append(Get(chs[j])); 
             } 
             
             return sb.ToString();
         }
        #endregion

        #region  ��ȡ��ҳ��HTML����
         // ��ȡ��ҳ��HTML���ݣ�ָ��Encoding
        public static string GetHtml(string url, Encoding encoding)
         {
             byte[] buf = new WebClient().DownloadData(url);
             if (encoding != null) return encoding.GetString(buf);
             string html = Encoding.UTF8.GetString(buf);
             encoding = GetEncoding(html);
             if (encoding == null || encoding == Encoding.UTF8) return html;
             return encoding.GetString(buf);
         }
         // ������ҳ��HTML������ȡ��ҳ��Encoding
        public static Encoding GetEncoding(string html)
         {
             string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
             string charset = Regex.Match(html, pattern).Groups["charset"].Value;
             try { return Encoding.GetEncoding(charset); }
             catch (ArgumentException) { return null; }
         }
        #endregion
    }
}