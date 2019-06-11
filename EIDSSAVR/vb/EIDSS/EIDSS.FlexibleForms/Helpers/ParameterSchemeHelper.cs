using System;
using bv.common;

namespace EIDSS.FlexibleForms.Helpers
{
    /// <summary>
    /// �������� ��� ������ �� ������� ����������
    /// </summary>
    public static class ParameterSchemeHelper
    {
        public static int ConvertToInt(this FFParameterScheme ps)
        {
            return Convert.ToInt32(ps);
        }

        /// <summary>
        /// ���������, ��������� �� ������������� ����� � ��������������
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static bool IsHorizontalOrientation(this FFParameterScheme ps)
        {
            return ((ps == FFParameterScheme.Left) || (ps == FFParameterScheme.Right));
        }

        /// <summary>
        /// ���������, ��������� �� ������������� ����� � ��������������
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static bool IsHorizontalOrientation(int ps)
        {
            return IsHorizontalOrientation(ConvertToParameterScheme(ps));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static FFParameterScheme ConvertToParameterScheme(int ps)
        {
            FFParameterScheme result = FFParameterScheme.Left;

            switch (ps)
            {
                case 0:
                    result = FFParameterScheme.Left;
                    break;
                case 1:
                    result = FFParameterScheme.Right;
                    break;
                case 2:
                    result = FFParameterScheme.Top;
                    break;
                case 3:
                    result = FFParameterScheme.Bottom;
                    break;
            }

            return result;
        }
    }
}
