using System;

namespace EIDSS.FlexibleForms.Helpers
{
    public static class ErrorHelper
    {
        /// <summary>
        /// ����������, �������� �� ���������� ��������� ����������� �������������
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsEIDSSError(string message)
        {
            //??? - ����� � ������ ������ �������������� ���������, ������� �������� ��. ����. spThrowException
            return ((message.Length > 3) && (message.Substring(0, 3).Equals("???")));
        }

        /// <summary>
        /// �������� ��������� ����� ���������, ������ ��� �� ��������� ��������
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetMessageString(string message)
        {
            string result = String.Empty;
            if (IsEIDSSError(message))
            {
                //������� ������ ��������� ���������
                int index = message.IndexOf("???",3,StringComparison.OrdinalIgnoreCase);
                result = message.Substring(3, index - 3);
            }

            return result;
        }
    }
}
